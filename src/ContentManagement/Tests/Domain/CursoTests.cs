using System;
using System.Linq;
using Xunit;
using ContentManagement.Domain;

namespace ContentManagement.Tests.Domain
{
    public class CursoTests
    {
        [Fact]
        public void CriarCurso_ComDadosValidos_DeveCriarComSucesso()
        {
            // Arrange
            var titulo = "Curso de C#";
            var descricao = "Aprenda C# do zero ao avançado";
            var conteudoProgramatico = new ConteudoProgramatico(
                new[] { "Objetivo 1" },
                new[] { "Pré-requisito 1" },
                new[] { "Livro 1" }
            );

            // Act
            var curso = new Curso(titulo, descricao, conteudoProgramatico);

            // Assert
            Assert.NotEqual(Guid.Empty, curso.Id);
            Assert.Equal(titulo, curso.Titulo);
            Assert.Equal(descricao, curso.Descricao);
            Assert.Empty(curso.Aulas);
            Assert.Equal(conteudoProgramatico, curso.ConteudoProgramatico);
        }

        [Fact]
        public void AdicionarAula_ComAulaValida_DeveAdicionarComSucesso()
        {
            // Arrange
            var curso = CriarCursoValido();
            var aula = new Aula("Aula 1", "Descrição 1", 60, 1, "Conteúdo 1");

            // Act
            curso.AdicionarAula(aula);

            // Assert
            Assert.Single(curso.Aulas);
            Assert.Equal(aula, curso.Aulas.First());
        }

        [Fact]
        public void RemoverAula_ComAulaExistente_DeveRemoverComSucesso()
        {
            // Arrange
            var curso = CriarCursoValido();
            var aula = new Aula("Aula 1", "Descrição 1", 60, 1, "Conteúdo 1");
            curso.AdicionarAula(aula);

            // Act
            curso.RemoverAula(aula.Id);

            // Assert
            Assert.Empty(curso.Aulas);
        }

        [Fact]
        public void AtualizarTitulo_ComTituloValido_DeveAtualizarComSucesso()
        {
            // Arrange
            var curso = CriarCursoValido();
            var novoTitulo = "Novo Título";

            // Act
            curso.AtualizarTitulo(novoTitulo);

            // Assert
            Assert.Equal(novoTitulo, curso.Titulo);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("   ")]
        public void AtualizarTitulo_ComTituloInvalido_DeveLancarExcecao(string tituloInvalido)
        {
            // Arrange
            var curso = CriarCursoValido();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => curso.AtualizarTitulo(tituloInvalido));
        }

        private Curso CriarCursoValido()
        {
            return new Curso(
                "Curso de C#",
                "Aprenda C# do zero ao avançado",
                new ConteudoProgramatico(
                    new[] { "Objetivo 1" },
                    new[] { "Pré-requisito 1" },
                    new[] { "Livro 1" }
                )
            );
        }
    }
}