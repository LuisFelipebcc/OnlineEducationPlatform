using System;
using Xunit;
using ContentManagement.Domain;

namespace ContentManagement.Tests.Domain
{
    public class AulaTests
    {
        [Fact]
        public void CriarAula_ComDadosValidos_DeveCriarComSucesso()
        {
            // Arrange
            var titulo = "Aula 1";
            var descricao = "Descrição da Aula 1";
            var duracao = 60;
            var ordem = 1;
            var conteudo = "Conteúdo da Aula 1";

            // Act
            var aula = new Aula(titulo, descricao, duracao, ordem, conteudo);

            // Assert
            Assert.NotEqual(Guid.Empty, aula.Id);
            Assert.Equal(titulo, aula.Titulo);
            Assert.Equal(descricao, aula.Descricao);
            Assert.Equal(duracao, aula.Duracao);
            Assert.Equal(ordem, aula.Ordem);
            Assert.Equal(conteudo, aula.Conteudo);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("   ")]
        public void AtualizarTitulo_ComTituloInvalido_DeveLancarExcecao(string tituloInvalido)
        {
            // Arrange
            var aula = CriarAulaValida();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => aula.AtualizarTitulo(tituloInvalido));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void AtualizarDuracao_ComDuracaoInvalida_DeveLancarExcecao(int duracaoInvalida)
        {
            // Arrange
            var aula = CriarAulaValida();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => aula.AtualizarDuracao(duracaoInvalida));
        }

        [Fact]
        public void AtualizarDuracao_ComDuracaoValida_DeveAtualizarComSucesso()
        {
            // Arrange
            var aula = CriarAulaValida();
            var novaDuracao = 90;

            // Act
            aula.AtualizarDuracao(novaDuracao);

            // Assert
            Assert.Equal(novaDuracao, aula.Duracao);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(-10)]
        public void AtualizarOrdem_ComOrdemInvalida_DeveLancarExcecao(int ordemInvalida)
        {
            // Arrange
            var aula = CriarAulaValida();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => aula.AtualizarOrdem(ordemInvalida));
        }

        [Fact]
        public void AtualizarOrdem_ComOrdemValida_DeveAtualizarComSucesso()
        {
            // Arrange
            var aula = CriarAulaValida();
            var novaOrdem = 2;

            // Act
            aula.AtualizarOrdem(novaOrdem);

            // Assert
            Assert.Equal(novaOrdem, aula.Ordem);
        }

        private Aula CriarAulaValida()
        {
            return new Aula(
                "Aula 1",
                "Descrição da Aula 1",
                60,
                1,
                "Conteúdo da Aula 1"
            );
        }
    }
}