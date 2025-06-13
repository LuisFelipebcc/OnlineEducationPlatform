using Shared.Domain;

namespace ContentManagement.Domain.Events;

public class CourseCriadoEvent : DomainEvent
{
    public Guid CourseId { get; }
    public string Title { get; }
    public string Description { get; }
    public DateTime CriadoEm { get; }

    public CourseCriadoEvent(Guid courseId, string titulo, string descricao, DateTime criadoEm)
    {
        CourseId = courseId;
        Title = titulo;
        Description = descricao;
        CriadoEm = criadoEm;
    }
}