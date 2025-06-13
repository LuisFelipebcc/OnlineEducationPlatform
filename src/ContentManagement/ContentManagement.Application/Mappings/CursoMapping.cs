using ContentManagement.Application.Commands;
using ContentManagement.Application.Commands.Handlers;
using ContentManagement.Domain.Interfaces;
using ContentManagement.Infrastructure.Repositories;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace ContentManagement.Application.Mappings;

public static class CursoMapping
{
    public static IServiceCollection AddCursoMappings(this IServiceCollection services)
    {
        services.AddScoped<IRequestHandler<CreateCourseCommand, Guid>, CreateCourseCommandHandler>();
        services.AddScoped<ICourseRepository, CourseRepository>();
        return services;
    }
}