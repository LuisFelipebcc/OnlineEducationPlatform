using StudentManagement.Domain.Entities;
using StudentManagement.Domain.Interfaces;
using System.Text.RegularExpressions;

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
        ValidateEmail(email);
        ValidatePhone(phone);
        ValidateBirthDate(birthDate);

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
        ValidateEmail(email);
        ValidatePhone(phone);

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

    private void ValidateEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email is required", nameof(email));

        var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        if (!emailRegex.IsMatch(email))
            throw new ArgumentException("Invalid email format", nameof(email));
    }

    private void ValidatePhone(string phone)
    {
        if (string.IsNullOrWhiteSpace(phone))
            throw new ArgumentException("Phone is required", nameof(phone));

        var phoneRegex = new Regex(@"^\+?[1-9]\d{1,14}$");
        if (!phoneRegex.IsMatch(phone))
            throw new ArgumentException("Invalid phone format", nameof(phone));
    }

    private void ValidateBirthDate(DateTime birthDate)
    {
        if (birthDate > DateTime.UtcNow)
            throw new ArgumentException("Birth date cannot be in the future", nameof(birthDate));

        var minimumAge = 5;
        var maximumAge = 100;
        var age = DateTime.UtcNow.Year - birthDate.Year;
        if (birthDate.Date > DateTime.UtcNow.AddYears(-age))
            age--;

        if (age < minimumAge || age > maximumAge)
            throw new ArgumentException($"Age must be between {minimumAge} and {maximumAge} years", nameof(birthDate));
    }
}