using StudentManagement.Domain.Entities;
using StudentManagement.Domain.Interfaces;

namespace StudentManagement.Application.Services;

public class StudentService
{
    private readonly IStudentRepository _studentRepository;

    public StudentService(IStudentRepository studentRepository)
    {
        _studentRepository = studentRepository;
    }

    public async Task<Student> CreateStudentAsync(string name, string email, string phone, DateTime birthDate, string address)
    {
        var existingStudent = await _studentRepository.GetByEmailAsync(email);
        if (existingStudent != null)
            throw new InvalidOperationException("A student with this email already exists");

        var student = new Student(name, email, phone, birthDate, address);
        return await _studentRepository.AddAsync(student);
    }

    public async Task<Student> GetStudentByIdAsync(Guid id)
    {
        var student = await _studentRepository.GetByIdAsync(id);
        if (student == null)
            throw new InvalidOperationException("Student not found");

        return student;
    }

    public async Task<IEnumerable<Student>> GetAllStudentsAsync()
    {
        return await _studentRepository.GetAllAsync();
    }

    public async Task<IEnumerable<Student>> GetActiveStudentsAsync()
    {
        return await _studentRepository.GetActiveStudentsAsync();
    }

    public async Task<Student> UpdateStudentAsync(Guid id, string name, string email, string phone, string address)
    {
        var student = await _studentRepository.GetByIdAsync(id);
        if (student == null)
            throw new InvalidOperationException("Student not found");

        var existingStudent = await _studentRepository.GetByEmailAsync(email);
        if (existingStudent != null && existingStudent.Id != id)
            throw new InvalidOperationException("A student with this email already exists");

        student.Update(name, email, phone, address);
        return await _studentRepository.UpdateAsync(student);
    }

    public async Task DeactivateStudentAsync(Guid id)
    {
        var student = await _studentRepository.GetByIdAsync(id);
        if (student == null)
            throw new InvalidOperationException("Student not found");

        student.Deactivate();
        await _studentRepository.UpdateAsync(student);
    }

    public async Task ActivateStudentAsync(Guid id)
    {
        var student = await _studentRepository.GetByIdAsync(id);
        if (student == null)
            throw new InvalidOperationException("Student not found");

        student.Activate();
        await _studentRepository.UpdateAsync(student);
    }

    public async Task<bool> DeleteStudentAsync(Guid id)
    {
        return await _studentRepository.DeleteAsync(id);
    }
}