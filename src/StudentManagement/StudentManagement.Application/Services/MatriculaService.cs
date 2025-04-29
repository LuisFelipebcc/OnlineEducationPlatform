using StudentManagement.Domain.Entities;
using StudentManagement.Application.Interfaces;
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

        public async Task<Matricula> GetByIdAsync(Guid id)
        {
            return await _context.Matriculas
                .Include(m => m.Aluno)
                .Include(m => m.Curso)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<IEnumerable<Matricula>> GetAllAsync()
        {
            return await _context.Matriculas
                .Include(m => m.Aluno)
                .Include(m => m.Curso)
                .ToListAsync();
        }

        public async Task<IEnumerable<Matricula>> GetByAlunoIdAsync(int alunoId)
        {
            return await _context.Matriculas
                .Include(m => m.Curso)
                .Where(m => m.AlunoId == alunoId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Matricula>> GetByCursoIdAsync(int cursoId)
        {
            return await _context.Matriculas
                .Include(m => m.Aluno)
                .Where(m => m.CursoId == cursoId)
                .ToListAsync();
        }

        public async Task<Matricula> CreateMatriculaAsync(Matricula matricula)
        {
            if (matricula == null)
                throw new ArgumentNullException(nameof(matricula));

            matricula.DataMatricula = DateTime.Now;
            _context.Matriculas.Add(matricula);
            await _context.SaveChangesAsync();
            return matricula;
        }

        public async Task UpdateMatriculaAsync(Matricula matricula)
        {
            if (matricula == null)
                throw new ArgumentNullException(nameof(matricula));

            _context.Entry(matricula).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteMatriculaAsync(int id)
        {
            var matricula = await _context.Matriculas.FindAsync(id);
            if (matricula != null)
            {
                _context.Matriculas.Remove(matricula);
                await _context.SaveChangesAsync();
            }
        }
    }
}