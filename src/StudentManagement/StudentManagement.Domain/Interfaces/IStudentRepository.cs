using StudentManagement.Domain.Entities;

namespace StudentManagement.Domain.Interfaces;

public interface IStudentRepository
{
    Task<Student> GetByIdAsync(Guid id);
    Task<IEnumerable<Student>> GetAllAsync();
    Task<IEnumerable<Student>> GetActiveStudentsAsync();
    Task<Student> GetByEmailAsync(string email);
    Task<Student> AddAsync(Student student);
    Task<Student> UpdateAsync(Student student);
    Task<bool> DeleteAsync(Guid id);
}