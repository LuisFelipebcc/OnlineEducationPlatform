using ContentManagement.Domain.Entities;
using ContentManagement.Domain.ValueObjects;
using ContentManagement.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ContentManagement.Infrastructure.Context;

public static class ContentManagementDbSeeder
{
    public static async Task SeedAsync(ContentManagementDbContext context)
    {
        await context.Database.MigrateAsync();

        if (!await context.Cursos.AnyAsync())
        {
            var cursosASemear = new List<Curso>();

            // Exemplo de InstrutorId fixo (em produção, gere dinamicamente ou relacione com usuário real)
            var instrutorId = Guid.NewGuid();

            // Curso 1: Introdução ao C#
            var conteudoCSharp = new ConteudoProgramatico(
                new List<string> { "Variáveis e Tipos", "Estruturas de Controle" },
                new List<string> { "Compreender tipos de dados", "Utilizar estruturas condicionais" }
            );
            var cursoCSharp = new Curso(
                "Introdução ao C#",
                "Curso introdutório sobre a linguagem C# e o ecossistema .NET.",
                instrutorId,
                99.99m,
                conteudoCSharp
            );
            cursoCSharp.AdicionarAula(
                "Visão Geral do C# e .NET",
                "Primeira aula: O que é C#, .NET Platform, CLR, CTS, CLS.",
                45,
                "https://videos.plataforma.com/csharp-intro"
            );
            cursoCSharp.AdicionarAula(
                "Estruturas de Controle",
                "Segunda aula: if/else, switch, loops (for, while, do-while).",
                60,
                "https://videos.plataforma.com/csharp-controle"
            );
            cursosASemear.Add(cursoCSharp);

            // Curso 2: Desenvolvimento Web com ASP.NET Core
            var conteudoAspNet = new ConteudoProgramatico(
                new List<string> { "Primeiros Passos", "MVC Essencial" },
                new List<string> { "Configurar projeto ASP.NET Core", "Entender arquitetura MVC" }
            );
            var cursoAspNetCore = new Curso(
                "Desenvolvimento Web com ASP.NET Core",
                "Aprenda a construir aplicações web modernas com ASP.NET Core MVC e Razor Pages.",
                instrutorId,
                149.50m,
                conteudoAspNet
            );
            cursoAspNetCore.AdicionarAula(
                "Introdução ao ASP.NET Core",
                "O que é ASP.NET Core, arquitetura MVC, middleware.",
                50,
                "https://videos.plataforma.com/aspnet-intro"
            );
            cursoAspNetCore.AdicionarAula(
                "Controllers e Actions",
                "Como funcionam os Controllers, Actions, Views e Model Binding.",
                70,
                "https://videos.plataforma.com/aspnet-controllers"
            );
            cursosASemear.Add(cursoAspNetCore);

            // Curso 3: Programação Orientada a Objetos com C#
            var conteudoPOO = new ConteudoProgramatico(
                new List<string> { "Introdução à POO" },
                new List<string> { "Entender o paradigma orientado a objetos" }
            );
            var cursoPOO = new Curso(
                "Programação Orientada a Objetos com C#",
                "Domine os conceitos de POO: Classes, Objetos, Herança, Polimorfismo e Encapsulamento.",
                instrutorId,
                120.00m,
                conteudoPOO
            );
            cursoPOO.AdicionarAula(
                "Conceitos Fundamentais de POO",
                "O que são classes, objetos, abstração, encapsulamento.",
                55,
                "https://videos.plataforma.com/poo-fundamentos"
            );
            cursosASemear.Add(cursoPOO);

            await context.Cursos.AddRangeAsync(cursosASemear);
            await context.SaveChangesAsync();
        }
    }
}