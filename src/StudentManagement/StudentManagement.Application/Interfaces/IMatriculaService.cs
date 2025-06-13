using StudentManagement.Domain.Entities;

namespace StudentManagement.Application.Interfaces
{
    public interface IEnrollmentService
    {
        Task<Enrollment> GetByIdAsync(Guid id);
        Task<IEnumerable<Enrollment>> GetAllAsync();
        Task<IEnumerable<Enrollment>> GetByStudentIdAsync(Guid studentId);
        Task<IEnumerable<Enrollment>> GetByCourseIdAsync(Guid courseId);
        Task<Enrollment> CreateEnrollmentAsync(Enrollment enrollment);
        Task UpdateEnrollmentAsync(Enrollment enrollment);
        Task DeleteEnrollmentAsync(Guid id);
    }
}