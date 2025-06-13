using Microsoft.EntityFrameworkCore;
using StudentManagement.Application.Interfaces;
using StudentManagement.Domain.Entities;
using StudentManagement.Infrastructure.Data;
using ContentManagement.Domain.Aggregates;

namespace StudentManagement.Application.Services
{
    public class EnrollmentService : IEnrollmentService
    {
        private readonly ApplicationDbContext _context;

        public EnrollmentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Enrollment> GetByIdAsync(Guid id)
        {
            return await _context.Enrollments
                .Include(e => e.Student)
                .Include(e => e.Course)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<IEnumerable<Enrollment>> GetAllAsync()
        {
            return await _context.Enrollments
                .Include(e => e.Student)
                .Include(e => e.Course)
                .ToListAsync();
        }

        public async Task<IEnumerable<Enrollment>> GetByStudentIdAsync(Guid studentId)
        {
            return await _context.Enrollments
                .Include(e => e.Course)
                .Where(e => e.StudentId == studentId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Enrollment>> GetByCourseIdAsync(Guid courseId)
        {
            return await _context.Enrollments
                .Include(e => e.Student)
                .Where(e => e.CourseId == courseId)
                .ToListAsync();
        }

        public async Task<Enrollment> CreateEnrollmentAsync(Enrollment enrollment)
        {
            if (enrollment == null)
                throw new ArgumentNullException(nameof(enrollment));

            enrollment.EnrollmentDate = DateTime.UtcNow;
            _context.Enrollments.Add(enrollment);
            await _context.SaveChangesAsync();
            return enrollment;
        }

        public async Task UpdateEnrollmentAsync(Enrollment enrollment)
        {
            if (enrollment == null)
                throw new ArgumentNullException(nameof(enrollment));

            _context.Entry(enrollment).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteEnrollmentAsync(Guid id)
        {
            var enrollment = await _context.Enrollments.FindAsync(id);
            if (enrollment != null)
            {
                enrollment.Cancel();
                await _context.SaveChangesAsync();
            }
        }
    }
}