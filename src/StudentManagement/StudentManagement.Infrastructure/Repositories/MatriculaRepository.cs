using Microsoft.EntityFrameworkCore;
using StudentManagement.Domain.Entities;
using StudentManagement.Domain.Enums;
using StudentManagement.Domain.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using StudentManagement.Infrastructure.Data;

namespace StudentManagement.Infrastructure.Repositories
{
    public class MatriculaRepository : IMatriculaRepository
    {
        private readonly ApplicationDbContext _context;

        public MatriculaRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Matricula> GetByIdAsync(int id)
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

        public async Task<Matricula> AddAsync(Matricula matricula)
        {
            await _context.Matriculas.AddAsync(matricula);
            await _context.SaveChangesAsync();
            return matricula;
        }

        public async Task UpdateAsync(Matricula matricula)
        {
            _context.Entry(matricula).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var matricula = await _context.Matriculas.FindAsync(id);
            if (matricula != null)
            {
                _context.Matriculas.Remove(matricula);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Matriculas.AnyAsync(m => m.Id == id);
        }

        public async Task<IEnumerable<Matricula>> GetMatriculasAtivasAsync()
        {
            return await _context.Matriculas
                .Include(m => m.Aluno)
                .Include(m => m.Curso)
                .Where(m => m.Status == StatusMatricula.Ativo)
                .ToListAsync();
        }

        public async Task<IEnumerable<Matricula>> GetMatriculasConcluidasAsync()
        {
            return await _context.Matriculas
                .Include(m => m.Aluno)
                .Include(m => m.Curso)
                .Where(m => m.Status == StatusMatricula.Concluido)
                .ToListAsync();
        }
    }
}