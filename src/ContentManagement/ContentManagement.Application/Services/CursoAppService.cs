using ContentManagement.Application.DTOs;
using ContentManagement.Domain.Entities;
using ContentManagement.Domain.Repositories;
using ContentManagement.Domain.ValueObjects;
using Microsoft.AspNetCore.Http;

namespace ContentManagement.Application.Services
{
    public class CursoAppService : ICursoAppService
    {
        private readonly ICursoRepository _cursoRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CursoAppService(
            ICursoRepository cursoRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _cursoRepository = cursoRepository ?? throw new ArgumentNullException(nameof(cursoRepository));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public async Task<CursoDto?> GetCursoByIdAsync(Guid id)
        {
            var curso = await _cursoRepository.GetByIdAsync(id);
            return curso == null ? null : MapToCursoDto(curso);
        }

        public async Task<IEnumerable<CursoDto>> GetAllCursosAsync()
        {
            var cursos = await _cursoRepository.GetAllAsync();
            return cursos.Select(MapToCursoDto);
        }

        public async Task<CursoDto> CreateCursoAsync(CriarCursoDto criarCursoDto, Guid instrutorId)
        {
            var conteudoProgramatico = new ConteudoProgramatico(
                criarCursoDto.ConteudoProgramatico.Topicos,
                criarCursoDto.ConteudoProgramatico.ObjetivosAprendizagem);

            var curso = new Curso(
                criarCursoDto.Titulo,
                criarCursoDto.Descricao ?? string.Empty,
                instrutorId,
                criarCursoDto.Preco,
                conteudoProgramatico);

            await _cursoRepository.AddAsync(curso);

            return MapToCursoDto(curso);
        }

        public async Task UpdateCursoAsync(Guid id, AtualizarCursoDto atualizarCursoDto)
        {
            var curso = await _cursoRepository.GetByIdAsync(id);
            if (curso == null)
                throw new KeyNotFoundException($"Curso com ID {id} não encontrado.");

            VerificarSeUsuarioEhAdmin();

            if (!string.IsNullOrWhiteSpace(atualizarCursoDto.Titulo) && curso.Titulo != atualizarCursoDto.Titulo)
                curso.AtualizarTitulo(atualizarCursoDto.Titulo);

            if (atualizarCursoDto.Descricao != null && curso.Descricao != atualizarCursoDto.Descricao)
                curso.AtualizarDescricao(atualizarCursoDto.Descricao);

            if (atualizarCursoDto.Preco.HasValue && curso.Preco != atualizarCursoDto.Preco.Value)
                curso.AtualizarPreco(atualizarCursoDto.Preco.Value);

            if (atualizarCursoDto.ConteudoProgramatico != null)
            {
                var novoConteudo = new ConteudoProgramatico(
                    atualizarCursoDto.ConteudoProgramatico.Topicos,
                    atualizarCursoDto.ConteudoProgramatico.ObjetivosAprendizagem);
                curso.AtualizarConteudoProgramatico(novoConteudo);
            }

            await _cursoRepository.UpdateAsync(curso);
        }

        public async Task DeleteCursoAsync(Guid id)
        {
            var curso = await _cursoRepository.GetByIdAsync(id);
            if (curso != null)
                await _cursoRepository.RemoveAsync(curso);
        }

        public async Task<AulaDto?> GetAulaByIdAsync(Guid cursoId, Guid aulaId)
        {
            var curso = await _cursoRepository.GetByIdAsync(cursoId);
            var aula = curso?.Aulas.FirstOrDefault(a => a.Id == aulaId);
            return aula == null ? null : MapToAulaDto(aula);
        }

        public async Task<IEnumerable<AulaDto>> GetAllAulasByCursoIdAsync(Guid cursoId)
        {
            var curso = await _cursoRepository.GetByIdAsync(cursoId);
            return curso?.Aulas.Select(MapToAulaDto) ?? Enumerable.Empty<AulaDto>();
        }

        public async Task<AulaDto> AddAulaToCursoAsync(Guid cursoId, CriarAulaDto criarAulaDto)
        {
            var curso = await _cursoRepository.GetByIdAsync(cursoId);
            if (curso == null)
                throw new KeyNotFoundException($"Curso com ID {cursoId} não encontrado.");

            curso.AdicionarAula(
                criarAulaDto.Titulo,
                criarAulaDto.Descricao ?? string.Empty,
                criarAulaDto.DuracaoEmMinutos,
                criarAulaDto.UrlVideo ?? string.Empty);

            await _cursoRepository.UpdateAsync(curso);

            var novaAula = curso.Aulas.LastOrDefault(a => a.Titulo == criarAulaDto.Titulo);
            return novaAula != null ? MapToAulaDto(novaAula) : throw new InvalidOperationException("Falha ao adicionar aula.");
        }

        public async Task UpdateAulaAsync(Guid cursoId, Guid aulaId, CriarAulaDto criarAulaDto)
        {
            var curso = await _cursoRepository.GetByIdAsync(cursoId);
            var aula = curso?.Aulas.FirstOrDefault(a => a.Id == aulaId);

            if (aula == null)
                throw new KeyNotFoundException($"Aula com ID {aulaId} no curso {cursoId} não encontrada.");

            aula.AtualizarDetalhes(
                criarAulaDto.Titulo,
                criarAulaDto.Descricao ?? string.Empty,
                criarAulaDto.DuracaoEmMinutos,
                criarAulaDto.UrlVideo ?? string.Empty);

            await _cursoRepository.UpdateAsync(curso);
        }

        public async Task RemoveAulaFromCursoAsync(Guid cursoId, Guid aulaId)
        {
            var curso = await _cursoRepository.GetByIdAsync(cursoId);
            if (curso == null)
                throw new KeyNotFoundException($"Curso com ID {cursoId} não encontrado.");

            curso.RemoverAula(aulaId);
            await _cursoRepository.UpdateAsync(curso);
        }

        // --- Métodos de Mapeamento ---
        private CursoDto MapToCursoDto(Curso curso) => new CursoDto
        {
            Id = curso.Id,
            Titulo = curso.Titulo,
            Descricao = curso.Descricao,
            InstrutorId = curso.InstrutorId,
            DataPublicacao = curso.DataPublicacao,
            Preco = curso.Preco,
            ConteudoProgramatico = MapToConteudoProgramaticoDto(curso.ConteudoProgramatico),
            Aulas = curso.Aulas.Select(MapToAulaDto).ToList()
        };

        private AulaDto MapToAulaDto(Aula aula) => new AulaDto
        {
            Id = aula.Id,
            Titulo = aula.Titulo,
            Descricao = aula.Descricao,
            DuracaoEmMinutos = aula.DuracaoEmMinutos,
            Ordem = aula.Ordem,
            UrlVideo = aula.UrlVideo
        };

        private ConteudoProgramaticoDto MapToConteudoProgramaticoDto(ConteudoProgramatico cp) => new ConteudoProgramaticoDto
        {
            Topicos = cp.Topicos.ToList(),
            ObjetivosAprendizagem = cp.ObjetivosAprendizagem.ToList()
        };

        private void VerificarSeUsuarioEhAdmin()
        {
            var user = _httpContextAccessor.HttpContext?.User;
            if (user == null || !user.IsInRole("Admin"))
                throw new UnauthorizedAccessException("Usuário não autorizado a atualizar este curso.");
        }
    }
}