using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using ContentManagement.Domain;
using ContentManagement.Domain.Repositories;
using ContentManagement.Infrastructure.Persistence;
using ContentManagement.Infrastructure.Repositories;

namespace ContentManagement.Tests.Integration
{
    public class CriarCursoTests : IDisposable
    {
        private readonly ServiceProvider _serviceProvider;
        private readonly ContentManagementDbContext _context;
        private readonly ICursoRepository _cursoRepository;

        public CriarCursoTests()
        {
            var services = new ServiceCollection();

            services.AddDbContext<ContentManagementDbContext>(options =>
                options.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()));

            services.AddScoped<ICursoRepository, CursoRepository>();

            _serviceProvider = services.BuildServiceProvider();
            _context = _serviceProvider.GetRequiredService<ContentManagementDbContext>();
            _cursoRepository = _serviceProvider.GetRequiredService<ICursoRepository>();
        }

        [Fact]
        public async Task CriarCurso_ComDadosValidos_DevePersistirComSucesso()
        {
            // Arrange
            var conteudoProgramatico = new ConteudoProgramatico(
                new[] { "Objetivo 1" },
                new[] { "Pré-requisito 1" },
                new[] { "Livro 1" }
            );

            var curso = new Curso(
                "Curso de C#",
                "Aprenda C# do zero ao avançado",
                conteudoProgramatico
            );

            // Act
            await _cursoRepository.AdicionarAsync(curso);

            // Assert
            var cursoPersistido = await _cursoRepository.ObterPorIdAsync(curso.Id);
            Assert.NotNull(cursoPersistido);
            Assert.Equal(curso.Titulo, cursoPersistido.Titulo);
            Assert.Equal(curso.Descricao, cursoPersistido.Descricao);
            Assert.Equal(curso.ConteudoProgramatico, cursoPersistido.ConteudoProgramatico);
        }

        [Fact]
        public async Task CriarCurso_ComAulas_DevePersistirAulasComSucesso()
        {
            // Arrange
            var conteudoProgramatico = new ConteudoProgramatico(
                new[] { "Objetivo 1" },
                new[] { "Pré-requisito 1" },
                new[] { "Livro 1" }
            );

            var curso = new Curso(
                "Curso de C#",
                "Aprenda C# do zero ao avançado",
                conteudoProgramatico
            );

            var aula = new Aula(
                "Aula 1",
                "Introdução ao C#",
                60,
                1,
                "Conteúdo da aula 1"
            );

            curso.AdicionarAula(aula);

            // Act
            await _cursoRepository.AdicionarAsync(curso);

            // Assert
            var cursoPersistido = await _cursoRepository.ObterPorIdAsync(curso.Id);
            Assert.NotNull(cursoPersistido);
            Assert.Single(cursoPersistido.Aulas);
            Assert.Equal(aula.Titulo, cursoPersistido.Aulas.First().Titulo);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _serviceProvider.Dispose();
        }
    }
}