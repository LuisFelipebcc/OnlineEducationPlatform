using ContentManagement.Domain.Aggregates;
using ContentManagement.Domain.ValueObjects;

namespace ContentManagement.Infrastructure.Context;

public static class ContentManagementDbSeeder
{
    public static async Task SeedAsync(ContentManagementDbContext context)
    {
        if (!context.Cursos.Any())
        {
            var curso = new Curso(
                "Introdução ao C#",
                "Curso introdutório sobre a linguagem C# e .NET",
                99.99m
            );

            curso.AdicionarAula(
                "Introdução ao C#",
                "Primeira aula do curso",
                "Conteúdo da primeira aula",
                1,
                new ConteudoProgramatico("Variáveis e Tipos", "Aprenda sobre variáveis e tipos de dados em C#", 1)
            );

            curso.AdicionarAula(
                "Estruturas de Controle",
                "Segunda aula do curso",
                "Conteúdo da segunda aula",
                2,
                new ConteudoProgramatico("If, Else e Switch", "Aprenda sobre estruturas de controle em C#", 2)
            );

            await context.Cursos.AddAsync(curso);
            await context.SaveChangesAsync();
        }
    }
}