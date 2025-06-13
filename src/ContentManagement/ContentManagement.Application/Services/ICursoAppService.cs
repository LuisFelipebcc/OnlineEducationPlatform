using ContentManagement.Application.DTOs;

namespace ContentManagement.Application.Services
{
    public interface ICursoAppService
    {
        Task<CursoDto?> GetCursoByIdAsync(Guid id);
        Task<IEnumerable<CursoDto>> GetAllCursosAsync();
        Task<CursoDto> CreateCursoAsync(CriarCursoDto criarCursoDto, Guid instrutorId);
        Task UpdateCursoAsync(Guid id, AtualizarCursoDto atualizarCursoDto);
        Task DeleteCursoAsync(Guid id);

        Task<AulaDto?> GetAulaByIdAsync(Guid cursoId, Guid aulaId);
        Task<IEnumerable<AulaDto>> GetAllAulasByCursoIdAsync(Guid cursoId);
        Task<AulaDto> AddAulaToCursoAsync(Guid cursoId, CriarAulaDto criarAulaDto);
        Task UpdateAulaAsync(Guid cursoId, Guid aulaId, CriarAulaDto criarAulaDto); // CriarAulaDto pode ser reutilizado ou um AtualizarAulaDto pode ser criado
        Task RemoveAulaFromCursoAsync(Guid cursoId, Guid aulaId);
    }
}