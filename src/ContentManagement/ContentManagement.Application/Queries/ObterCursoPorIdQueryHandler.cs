using ContentManagement.Domain.Aggregates;
using ContentManagement.Domain.Interfaces;
using MediatR;

namespace ContentManagement.Application.Queries
{
    public class GetCourseByIdQueryHandler : IRequestHandler<GetCourseByIdQuery, Course>
    {
        private readonly ICourseRepository _courseRepository;

        public GetCourseByIdQueryHandler(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository ?? throw new ArgumentNullException(nameof(courseRepository));
        }

        public async Task<Course> Handle(GetCourseByIdQuery request, CancellationToken cancellationToken)
        {
            var course = await _courseRepository.GetByIdAsync(request.Id);
            if (course == null)
                throw new InvalidOperationException($"Course with ID {request.Id} not found");

            return course;
        }
    }
}