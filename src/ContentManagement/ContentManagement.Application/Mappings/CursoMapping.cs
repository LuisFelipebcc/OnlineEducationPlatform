using ContentManagement.Application.Commands;
using ContentManagement.Domain.Repositories;
using ContentManagement.Infrastructure.Repositories;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace ContentManagement.Application.Mappings;

public static class CursoMapping
{
    public static IServiceCollection AddCursoMappings(this IServiceCollection services)
    {
        services.AddScoped<IRequestHandler<CriarCursoCommand, Guid>, CriarCursoCommandHandler>();
        services.AddScoped<ICursoRepository, CursoRepository>();
        return services;
    }
}