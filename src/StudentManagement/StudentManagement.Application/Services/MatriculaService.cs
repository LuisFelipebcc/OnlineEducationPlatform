using Microsoft.EntityFrameworkCore;
using StudentManagement.Application.Interfaces;
using StudentManagement.Domain.Entities;
using StudentManagement.Infrastructure.Data;

namespace StudentManagement.Application.Services
{
    public class MatriculaService : IMatriculaService
    {
        private readonly ApplicationDbContext _context;

        public MatriculaService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Matricula> ObterPorIdAsync(Guid id)
        {
            return await _context.Matriculas
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<IEnumerable<Matricula>> ObterTodasAsync()
        {
            return await _context.Matriculas
                .ToListAsync();
        }

        public async Task<IEnumerable<Matricula>> ObterPorAlunoIdAsync(Guid alunoId)
        {
            return await _context.Matriculas
                .Where(m => m.AlunoId == alunoId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Matricula>> ObterPorCursoIdAsync(Guid cursoId)
        {
            return await _context.Matriculas
                .Where(m => m.CursoId == cursoId)
                .ToListAsync();
        }

        public async Task<Matricula> CriarMatriculaAsync(Matricula matricula)
        {
            if (matricula == null)
                throw new ArgumentNullException(nameof(matricula));

            _context.Matriculas.Add(matricula);
            await _context.SaveChangesAsync();
            return matricula;
        }

        public async Task AtualizarMatriculaAsync(Matricula matricula)
        {
            if (matricula == null)
                throw new ArgumentNullException(nameof(matricula));

            // Se a entidade já está sendo rastreada, basta alterar e salvar.
            _context.Entry(matricula).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task RemoverMatriculaAsync(Guid id)
        {
            var matricula = await _context.Matriculas.FindAsync(id);
            if (matricula != null)
            {
                matricula.Cancelar(); // Soft delete/cancelamento lógico
                await _context.SaveChangesAsync();
            }
        }
    }
}