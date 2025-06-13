using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StudentManagement.Application.DTOs;

namespace StudentManagement.Application.Interfaces
{
    public interface IAlunoAppService
    {
        Task<AlunoDto?> GetAlunoByIdAsync(Guid alunoId);
        Task<IEnumerable<AlunoDto>> GetAllAlunosAsync(); // Geralmente para Admins
        Task<AlunoDto?> CriarAlunoSincronizadoAsync(CriarAlunoSincronizadoDto dto);

        // Operações de Matrícula
        Task<MatriculaDto?> RealizarMatriculaAsync(Guid alunoId, RealizarMatriculaDto realizarMatriculaDto);
        Task ConfirmarMatriculaAposPagamentoAsync(Guid matriculaId, decimal valorPago); // Novo método
        Task CancelarMatriculaAsync(Guid alunoId, Guid matriculaId);
        Task<IEnumerable<MatriculaDto>> GetMatriculasDoAlunoAsync(Guid alunoId);
        Task<MatriculaDto?> GetMatriculaDetailsAsync(Guid alunoId, Guid matriculaId);

        // Operações de Progresso
        Task MarcarAulaComoConcluidaAsync(Guid alunoId, Guid matriculaId, Guid aulaId);
        // Task<HistoricoAprendizadoDto?> GetHistoricoAprendizadoAsync(Guid alunoId, Guid matriculaId);

        // Operações de Certificado (exemplo)
        // Task<CertificadoDto?> EmitirCertificadoAsync(Guid alunoId, Guid cursoId); // CursoId da matrícula concluída
        // Task<IEnumerable<CertificadoDto>> GetCertificadosDoAlunoAsync(Guid alunoId);
    }
}