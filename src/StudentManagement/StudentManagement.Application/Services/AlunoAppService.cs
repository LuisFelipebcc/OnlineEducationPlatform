using ContentManagement.Application.Services; // Para ICursoAppService
using Microsoft.EntityFrameworkCore; // Necessário para FirstOrDefaultAsync em DbContext.Matriculas
using StudentManagement.Application.DTOs;
using StudentManagement.Application.Interfaces;
using StudentManagement.Domain.Entities;
using StudentManagement.Domain.Repositories;
using StudentManagement.Domain.ValueObjects;
using StudentManagement.Infrastructure.Persistence;

namespace StudentManagement.Application.Services
{
    public class AlunoAppService : IAlunoAppService
    {
        private readonly IAlunoRepository _alunoRepository;
        private readonly StudentManagementDbContext _dbContext;
        private readonly ICursoAppService _cursoAppService;

        public AlunoAppService(
            IAlunoRepository alunoRepository,
            StudentManagementDbContext dbContext,
            ICursoAppService cursoAppService
            )
        {
            _alunoRepository = alunoRepository ?? throw new ArgumentNullException(nameof(alunoRepository));
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _cursoAppService = cursoAppService ?? throw new ArgumentNullException(nameof(cursoAppService));
        }

        public async Task<AlunoDto?> GetAlunoByIdAsync(Guid alunoId)
        {
            var aluno = await _alunoRepository.GetByIdAsync(alunoId);
            return aluno == null ? null : await MapToAlunoDtoAsync(aluno);
        }

        public async Task<IEnumerable<AlunoDto>> GetAllAlunosAsync()
        {
            var alunos = await _alunoRepository.GetAllAsync();
            var alunoDtos = new List<AlunoDto>();
            foreach (var aluno in alunos)
            {
                alunoDtos.Add(await MapToAlunoDtoAsync(aluno));
            }
            return alunoDtos;
        }

        public async Task<AlunoDto?> CriarAlunoSincronizadoAsync(CriarAlunoSincronizadoDto dto)
        {
            if (await _alunoRepository.GetByIdAsync(dto.Id) != null)
            {
                var alunoExistente = await _alunoRepository.GetByIdAsync(dto.Id);
                return alunoExistente == null ? null : await MapToAlunoDtoAsync(alunoExistente);
            }

            var novoAluno = new Aluno(dto.Id, dto.NomeCompleto, dto.Email);
            await _alunoRepository.AddAsync(novoAluno);
            await _dbContext.SaveChangesAsync();
            return await MapToAlunoDtoAsync(novoAluno);
        }

        public async Task<MatriculaDto?> RealizarMatriculaAsync(Guid alunoId, RealizarMatriculaDto realizarMatriculaDto)
        {
            var aluno = await _alunoRepository.GetByIdAsync(alunoId);
            if (aluno == null)
            {
                throw new KeyNotFoundException($"Aluno com ID {alunoId} não encontrado no contexto de StudentManagement.");
            }

            var cursoDetails = await _cursoAppService.GetCursoByIdAsync(realizarMatriculaDto.CursoId);
            if (cursoDetails == null)
            {
                throw new KeyNotFoundException($"Curso com ID {realizarMatriculaDto.CursoId} não encontrado no ContentManagement.");
            }

            var precoDoCurso = cursoDetails.Preco;

            var matricula = aluno.RealizarMatricula(realizarMatriculaDto.CursoId, precoDoCurso);

            await _dbContext.SaveChangesAsync();
            return await MapToMatriculaDtoAsync(matricula);
        }

        public async Task CancelarMatriculaAsync(Guid alunoId, Guid matriculaId)
        {
            var aluno = await _alunoRepository.GetByIdAsync(alunoId);
            if (aluno == null)
            {
                throw new KeyNotFoundException($"Aluno com ID {alunoId} não encontrado.");
            }

            aluno.CancelarMatricula(matriculaId);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<MatriculaDto>> GetMatriculasDoAlunoAsync(Guid alunoId)
        {
            var aluno = await _alunoRepository.GetByIdAsync(alunoId);
            if (aluno == null)
            {
                return Enumerable.Empty<MatriculaDto>();
            }
            var matriculasDto = new List<MatriculaDto>();
            foreach (var matricula in aluno.Matriculas)
            {
                matriculasDto.Add(await MapToMatriculaDtoAsync(matricula));
            }
            return matriculasDto;
        }

        public async Task<MatriculaDto?> GetMatriculaDetailsAsync(Guid alunoId, Guid matriculaId)
        {
            var aluno = await _alunoRepository.GetByIdAsync(alunoId);
            var matricula = aluno?.Matriculas.FirstOrDefault(m => m.Id == matriculaId);

            if (matricula == null) return null;

            return await MapToMatriculaDtoAsync(matricula);
        }

        public async Task ConfirmarMatriculaAposPagamentoAsync(Guid matriculaId, decimal valorPago)
        {
            var matricula = await _dbContext.Matriculas.FirstOrDefaultAsync(m => m.Id == matriculaId);
            if (matricula == null)
            {
                throw new KeyNotFoundException($"Matrícula com ID {matriculaId} não encontrada para confirmação de pagamento.");
            }

            matricula.AtivarAposPagamento(valorPago);
            await _dbContext.SaveChangesAsync();
        }

        public async Task MarcarAulaComoConcluidaAsync(Guid alunoId, Guid matriculaId, Guid aulaId)
        {
            var aluno = await _alunoRepository.GetByIdAsync(alunoId);
            if (aluno == null) throw new KeyNotFoundException($"Aluno com ID {alunoId} não encontrado.");

            var matricula = aluno.Matriculas.FirstOrDefault(m => m.Id == matriculaId);
            if (matricula == null) throw new KeyNotFoundException($"Matrícula com ID {matriculaId} não encontrada para o aluno.");

            matricula.RegistrarProgressoAula(aulaId);
            await _dbContext.SaveChangesAsync();
        }

        private async Task<AlunoDto> MapToAlunoDtoAsync(Aluno aluno)
        {
            var matriculasDto = new List<MatriculaDto>();
            foreach (var m in aluno.Matriculas) matriculasDto.Add(await MapToMatriculaDtoAsync(m));

            var certificadosDto = aluno.Certificados.Select(c => MapToCertificadoDto(c)).ToList();

            return new AlunoDto
            {
                Id = aluno.Id,
                NomeCompleto = aluno.NomeCompleto,
                Email = aluno.Email,
                Matriculas = matriculasDto,
                Certificados = certificadosDto
            };
        }

        private async Task<MatriculaDto> MapToMatriculaDtoAsync(Matricula matricula)
        {
            string nomeCurso = "Curso não encontrado";
            var cursoDetails = await _cursoAppService.GetCursoByIdAsync(matricula.CursoId);
            if (cursoDetails != null)
            {
                nomeCurso = cursoDetails.Titulo;
            }
            return new MatriculaDto
            {
                Id = matricula.Id,
                AlunoId = matricula.AlunoId,
                CursoId = matricula.CursoId,
                NomeCurso = nomeCurso,
                DataMatricula = matricula.DataMatricula,
                Status = matricula.Status.ToString(),
                PrecoPago = matricula.PrecoPago,
                HistoricoAprendizado = matricula.HistoricoAprendizado != null ? MapToHistoricoAprendizadoDto(matricula.HistoricoAprendizado) : null
            };
        }

        private CertificadoDto MapToCertificadoDto(Certificado certificado) => new CertificadoDto
        {
            Id = certificado.Id,
            AlunoId = certificado.AlunoId,
            CursoId = certificado.CursoId,
            NomeCurso = certificado.NomeCurso,
            DataEmissao = certificado.DataEmissao,
            CodigoVerificacao = certificado.CodigoVerificacao
        };

        private HistoricoAprendizadoDto MapToHistoricoAprendizadoDto(HistoricoAprendizado historico) => new HistoricoAprendizadoDto
        {
            ProgressoAulas = new Dictionary<Guid, DateTime?>(historico.ProgressoAulas),
            DataConclusaoCurso = historico.DataConclusaoCurso
        };
    }
}
