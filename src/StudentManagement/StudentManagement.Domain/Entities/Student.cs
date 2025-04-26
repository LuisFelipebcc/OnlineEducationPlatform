using System;

namespace StudentManagement.Domain.Entities;

public class Student
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Email { get; private set; }
    public string Phone { get; private set; }
    public DateTime BirthDate { get; private set; }
    public string Address { get; private set; }
    public StudentStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    private Student() { }

    public Student(string name, string email, string phone, DateTime birthDate, string address)
    {
        Id = Guid.NewGuid();
        Name = name;
        Email = email;
        Phone = phone;
        BirthDate = birthDate;
        Address = address;
        Status = StudentStatus.Active;
        CreatedAt = DateTime.UtcNow;
    }

    public void Update(string name, string email, string phone, string address)
    {
        Name = name;
        Email = email;
        Phone = phone;
        Address = address;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        if (Status == StudentStatus.Inactive)
            throw new InvalidOperationException("Student is already inactive");

        Status = StudentStatus.Inactive;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Activate()
    {
        if (Status == StudentStatus.Active)
            throw new InvalidOperationException("Student is already active");

        Status = StudentStatus.Active;
        UpdatedAt = DateTime.UtcNow;
    }
}

public enum StudentStatus
{
    Active,
    Inactive
}