using System;
using MediatR;

namespace StudentManagement.Domain.Events
{
    public class CertificateIssuedEvent : INotification
    {
        public Guid StudentId { get; }
        public Guid CourseId { get; }
        public DateTime IssueDate { get; }

        public CertificateIssuedEvent(Guid studentId, Guid courseId)
        {
            StudentId = studentId;
            CourseId = courseId;
            IssueDate = DateTime.UtcNow;
        }
    }
}