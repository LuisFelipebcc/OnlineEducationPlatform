using ContentManagement.Domain.Entities;
using ContentManagement.Domain.ValueObjects;
using ContentManagement.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ContentManagement.Infrastructure.Context;

public static class ContentManagementDbSeeder
{
    public static async Task SeedAsync(ContentManagementDbContext context)
    {
        // Garante que as migrações pendentes sejam aplicadas antes de semear.
        // Opcional: pode ser feito em outro lugar, como no Program.cs.
        // if ((await context.Database.GetPendingMigrationsAsync()).Any())
        // {
        //     await context.Database.MigrateAsync();
        // }

        if (!await context.Cursos.AnyAsync()) // Assumindo que o DbSet se chama 'Cursos'
        {
            var cursosASemear = new List<Curso>();

            // Curso 1: Introdução ao C#
            var cursoCSharp = new Curso(
                "Introdução ao C#",
                "Curso introdutório sobre a linguagem C# e o ecossistema .NET.",
                99.99m
            );

            cursoCSharp.AddAula( // Supondo que o método seja AddAula
                "Visão Geral do C# e .NET",
                "Primeira aula: O que é C#, .NET Platform, CLR, CTS, CLS.",
                "Conteúdo detalhado sobre a introdução ao C# e .NET, configuração do ambiente de desenvolvimento (Visual Studio, VS Code), e primeiro programa \"Olá, Mundo!\".",
                1,
                new ConteudoProgramatico("Variáveis e Tipos", "Aprenda sobre variáveis e tipos de dados em C#", 1)
            );

            cursoCSharp.AddAula(
                "Estruturas de Controle",
                "Segunda aula: if/else, switch, loops (for, while, do-while).",
                "Conteúdo detalhado sobre como controlar o fluxo de execução do programa utilizando estruturas condicionais e de repetição.",
                2,
                new ConteudoProgramatico("If, Else e Switch", "Aprenda sobre estruturas de controle em C#", 2)
            );
            cursosASemear.Add(cursoCSharp);

            // Curso 2: Desenvolvimento Web com ASP.NET Core
            var cursoAspNetCore = new Curso(
                "Desenvolvimento Web com ASP.NET Core",
                "Aprenda a construir aplicações web modernas com ASP.NET Core MVC e Razor Pages.",
                149.50m
            );

            cursoAspNetCore.AddAula(
                "Introdução ao ASP.NET Core",
                "O que é ASP.NET Core, arquitetura MVC, middleware.",
                "Conteúdo sobre os fundamentos do ASP.NET Core, ciclo de vida da requisição e configuração de um projeto MVC.",
                1,
                new ConteudoProgramatico("Primeiros Passos", "Configuração e conceitos iniciais do ASP.NET Core.", 1)
            );

            cursoAspNetCore.AddAula(
                "Controllers e Actions",
                "Como funcionam os Controllers, Actions, Views e Model Binding.",
                "Conteúdo prático sobre a criação de controllers para lidar com requisições HTTP e retornar views ou dados.",
                2,
                new ConteudoProgramatico("MVC Essencial", "Componentes chave do padrão MVC em ASP.NET Core.", 1)
            );
            cursosASemear.Add(cursoAspNetCore);

            // Curso 3: Programação Orientada a Objetos com C#
            var cursoPOO = new Curso(
                "Programação Orientada a Objetos com C#",
                "Domine os conceitos de POO: Classes, Objetos, Herança, Polimorfismo e Encapsulamento.",
                120.00m
            );

            cursoPOO.AddAula(
                "Conceitos Fundamentais de POO",
                "O que são classes, objetos, abstração, encapsulamento.",
                "Explicação detalhada dos pilares da Programação Orientada a Objetos e como aplicá-los em C#.",
                1,
                new ConteudoProgramatico("Introdução à POO", "Entendendo o paradigma orientado a objetos.", 1)
            );
            cursosASemear.Add(cursoPOO);

            await context.Cursos.AddRangeAsync(cursosASemear);
            await context.SaveChangesAsync();
        }
    }
}