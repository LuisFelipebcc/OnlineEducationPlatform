using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ContentManagement.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Registra o AutoMapper e procura por perfis no assembly atual (ex: MappingProfile)
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        // Registra o MediatR e encontra todos os handlers e requests no assembly atual
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        // A linha 'services.AddScoped<IRequestHandler<CreateCourseCommand, Guid>, CreateCourseCommandHandler>();'
        // não é mais necessária, pois o MediatR faz isso automaticamente.

        // A linha 'services.AddScoped<ICourseRepository, CourseRepository>();' foi removida.
        // Ela deve ser movida para a configuração de injeção de dependência da camada de Infraestrutura.
        return services;
    }
}