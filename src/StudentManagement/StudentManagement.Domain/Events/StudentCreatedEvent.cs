using System;
using MediatR;

namespace StudentManagement.Domain.Events
{
    public class StudentCreatedEvent : INotification
    {
        public Guid StudentId { get; }
        public string Name { get; }
        public string Email { get; }
        public DateTime CreatedAt { get; }

        public StudentCreatedEvent(Guid studentId, string name, string email, DateTime createdAt)
        {
            StudentId = studentId;
            Name = name;
            Email = email;
            CreatedAt = createdAt;
        }
    }
}