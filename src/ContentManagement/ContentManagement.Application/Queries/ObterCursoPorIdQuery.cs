using ContentManagement.Domain.Aggregates;
using MediatR;

namespace ContentManagement.Application.Queries
{
    public class GetCourseByIdQuery : IRequest<Course>
    {
        public Guid Id { get; set; }
    }
}