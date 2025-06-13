using IdentityService.Application.DTOs;
using IdentityService.Application.Events; // Adicionado
using IdentityService.Application.Interfaces;
using IdentityService.Domain.Entities;
using IdentityService.Domain.Repositories;
using IdentityService.Infrastructure.Persistence; // Para o DbContext
using MediatR; // Adicionado
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging; // Adicionado para Logging
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BCrypt.Net;

namespace IdentityService.Application.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserRepository _userRepository;
        private readonly IAlunoRepository _alunoRepository;
        private readonly IAdminRepository _adminRepository;
        private readonly IdentityDbContext _dbContext; // Para SaveChangesAsync
        private readonly IConfiguration _configuration;
        private readonly IMediator _mediator; // Adicionado
        private readonly ILogger<AuthenticationService> _logger; // Adicionado

        private const string PersonaAluno = "Aluno";
        private const string PersonaAdmin = "Admin";

        public AuthenticationService(
            IUserRepository userRepository,
            IAlunoRepository alunoRepository,
            IAdminRepository adminRepository,
            IdentityDbContext dbContext,
            IConfiguration configuration,
            IMediator mediator,
            ILogger<AuthenticationService> logger) // Adicionado
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _alunoRepository = alunoRepository ?? throw new ArgumentNullException(nameof(alunoRepository));
            _adminRepository = adminRepository ?? throw new ArgumentNullException(nameof(adminRepository));
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator)); // Adicionado
            _logger = logger ?? throw new ArgumentNullException(nameof(logger)); // Adicionado
        }

        public async Task<UsuarioDto?> RegistrarAsync(RegistrarUsuarioDto registrarUsuarioDto)
        {
            if (await _userRepository.EmailExistsAsync(registrarUsuarioDto.Email))
            {
                // Considerar lançar uma exceção específica ou retornar um resultado com erro
                _logger.LogWarning("Tentativa de registro com email já existente: {Email}", registrarUsuarioDto.Email);
                return null;
            }

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(registrarUsuarioDto.Senha);
            var user = new User(registrarUsuarioDto.Email, passwordHash);

            await _userRepository.AddAsync(user);

            string tipoPersonaNormalizado = registrarUsuarioDto.TipoPersona.Trim();
            Persona persona;

            if (string.Equals(tipoPersonaNormalizado, PersonaAluno, StringComparison.OrdinalIgnoreCase))
            {
                var alunoPersona = new Aluno(user.Id, registrarUsuarioDto.NomeCompleto, DateTime.UtcNow);
                await _alunoRepository.AddAsync(alunoPersona);
                persona = alunoPersona;
            }
            else if (string.Equals(tipoPersonaNormalizado, PersonaAdmin, StringComparison.OrdinalIgnoreCase))
            {
                // Para Admin, o campo 'Cargo' pode vir do DTO ou ser um valor padrão
                var admin = new Admin(user.Id, registrarUsuarioDto.NomeCompleto, "Administrador Padrão");
                await _adminRepository.AddAsync(admin);
                persona = admin;
            }
            else
            {
                // Tipo de persona inválido, poderia lançar exceção ou retornar erro
                // Por agora, vamos reverter a adição do usuário se a persona for inválida (ou não salvar)
                // Idealmente, a transação cuidaria disso.
                _logger.LogWarning("Tentativa de registro com tipo de persona inválido: {TipoPersona}", registrarUsuarioDto.TipoPersona);
                return null;
            }

            await _dbContext.SaveChangesAsync(); // Salva User e Persona

            // Se for um aluno, publica o evento para outros BCs (como StudentManagement) reagirem
            if (persona is Aluno alunoIdentidade)
            {
                await _mediator.Publish(new AlunoDaIdentidadeRegistradoEvent(alunoIdentidade.Id, alunoIdentidade.NomeCompleto, user.Email));
            }

            var token = GerarTokenJwt(user, persona.NomeCompleto, tipoPersonaNormalizado);

            return new UsuarioDto
            {
                Id = user.Id,
                NomeCompleto = persona.NomeCompleto,
                Email = user.Email,
                TipoPersona = tipoPersonaNormalizado,
                Token = token
            };
        }

        public async Task<UsuarioDto?> LoginAsync(LoginDto loginDto)
        {
            var user = await _userRepository.GetByEmailAsync(loginDto.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Senha, user.PasswordHash))
            {
                return null; // Credenciais inválidas
            }

            // Buscar a persona associada para obter NomeCompleto e TipoPersona
            Persona? persona = await _alunoRepository.GetByIdAsync(user.Id);
            string tipoPersona = PersonaAluno;

            if (persona == null)
            {
                persona = await _adminRepository.GetByIdAsync(user.Id);
                tipoPersona = PersonaAdmin;
            }

            if (persona == null)
            {
                // Inconsistência de dados: usuário existe mas não tem persona.
                // Logar erro e retornar falha.
                _logger.LogError("Inconsistência de dados: Usuário {UserId} existe mas não possui persona (Aluno ou Admin) associada.", user.Id);
                return null;
            }

            var token = GerarTokenJwt(user, persona.NomeCompleto, tipoPersona);

            return new UsuarioDto
            {
                Id = user.Id,
                NomeCompleto = persona.NomeCompleto,
                Email = user.Email,
                TipoPersona = tipoPersona,
                Token = token
            };
        }

        private string GerarTokenJwt(User user, string nomeCompleto, string tipoPersona)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["JwtSettings:Secret"] ?? throw new InvalidOperationException("JWT Secret not configured."));

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()), // Subject (ID do usuário)
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Name, nomeCompleto),
                new Claim(ClaimTypes.Role, tipoPersona) // Adiciona a persona como uma Role
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(Convert.ToDouble(_configuration["JwtSettings:ExpiryHours"] ?? "8")),
                Issuer = _configuration["JwtSettings:Issuer"],
                Audience = _configuration["JwtSettings:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}