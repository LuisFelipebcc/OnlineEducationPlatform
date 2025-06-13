using Microsoft.EntityFrameworkCore;
using StudentManagement.Domain.Entities;
using StudentManagement.Domain.Interfaces;
using StudentManagement.Infrastructure.Context;

namespace StudentManagement.Infrastructure.Repositories;

public class StudentRepository : IStudentRepository
{
    private readonly StudentManagementDbContext _context;

    public StudentRepository(StudentManagementDbContext context)
    {
        _context = context;
    }

    public async Task<Student> GetByIdAsync(Guid id)
    {
        return await _context.Students.FindAsync(id);
    }

    public async Task<IEnumerable<Student>> GetAllAsync()
    {
        return await _context.Students.ToListAsync();
    }

    public async Task<IEnumerable<Student>> GetActiveStudentsAsync()
    {
        return await _context.Students
            .Where(s => !s.IsDeleted)
            .ToListAsync();
    }

    public async Task<Student> GetByEmailAsync(string email)
    {
        return await _context.Students
            .FirstOrDefaultAsync(s => s.Email == email);
    }

    public async Task<Student> AddAsync(Student student)
    {
        await _context.Students.AddAsync(student);
        await _context.SaveChangesAsync();
        return student;
    }

    public async Task<Student> UpdateAsync(Student student)
    {
        _context.Students.Update(student);
        await _context.SaveChangesAsync();
        return student;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var student = await _context.Students.FindAsync(id);
        if (student == null)
            return false;

        student.IsDeleted = true;
        await _context.SaveChangesAsync();
        return true;
    }
}