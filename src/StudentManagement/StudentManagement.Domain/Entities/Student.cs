using MediatR;
using StudentManagement.Domain.Events;
using Shared.Domain;

namespace StudentManagement.Domain.Entities
{
    public class Student : AggregateRoot
    {
        private readonly List<INotification> _events;
        public IReadOnlyCollection<INotification> Events => _events.AsReadOnly();

        private readonly List<Enrollment> _enrollments;
        public IReadOnlyCollection<Enrollment> Enrollments => _enrollments.AsReadOnly();
        private readonly List<Certificate> _certificates;
        public IReadOnlyCollection<Certificate> Certificates => _certificates.AsReadOnly();
        public LearningHistory LearningHistory { get; private set; }

        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string CPF { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }

        protected Student()
        {
            _events = new List<INotification>();
            _enrollments = new List<Enrollment>();
            _certificates = new List<Certificate>();
            Name = string.Empty;
            Email = string.Empty;
        }

        public Student(string name, string email, string cpf)
            : this()
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name is required", nameof(name));

            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email is required", nameof(email));

            Id = Guid.NewGuid();
            Name = name;
            Email = email;
            CPF = cpf;
            CreatedAt = DateTime.UtcNow;
            LearningHistory = new LearningHistory();

            AddEvent(new StudentCreatedEvent(Id, Name, Email, CreatedAt));
        }

        public void AddEnrollment(Enrollment enrollment)
        {
            if (enrollment == null)
                throw new ArgumentNullException(nameof(enrollment));

            if (_enrollments.Any(e => e.CourseId == enrollment.CourseId))
                throw new InvalidOperationException("Student is already enrolled in this course");

            _enrollments.Add(enrollment);
            AddEvent(new EnrollmentCreatedEvent(Id, enrollment.CourseId, enrollment.EnrollmentDate));
        }

        public void AddCertificate(Certificate certificate)
        {
            if (certificate == null)
                throw new ArgumentNullException(nameof(certificate));

            if (!_enrollments.Any(e => e.CourseId == certificate.Id && e.Status == Enums.EnrollmentStatus.Completed))
                throw new InvalidOperationException("Student has not completed the course to receive the certificate");

            _certificates.Add(certificate);
            AddEvent(new CertificateIssuedEvent(Id, certificate.Id));
        }

        public void UpdateName(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
                throw new ArgumentException("Name cannot be empty", nameof(newName));

            Name = newName;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateEmail(string newEmail)
        {
            if (string.IsNullOrWhiteSpace(newEmail))
                throw new ArgumentException("Email cannot be empty", nameof(newEmail));

            Email = newEmail;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateLearningHistory(LearningHistory newLearningHistory)
        {
            if (newLearningHistory == null)
                throw new ArgumentNullException(nameof(newLearningHistory));

            LearningHistory = newLearningHistory;
        }

        protected void AddEvent(INotification @event)
        {
            _events.Add(@event);
        }

        public void ClearEvents()
        {
            _events.Clear();
        }
    }
}