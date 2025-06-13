using System;
using System.Collections.Generic;
using System.Linq;

namespace PaymentBilling.Domain.ValueObjects
{
    // ATENÇÃO: Em um sistema real, NUNCA armazene dados completos de cartão.
    // Use um gateway de pagamento e armazene apenas tokens ou referências seguras.
    // Esta classe é uma simplificação para fins de modelagem do domínio.
    public class DadosCartao // Idealmente herdaria de uma classe base ValueObject
    {
        public string NumeroCartaoMascarado { get; } // Ex: "************1234"
        public string NomeTitular { get; }
        public string DataValidade { get; } // Formato MM/YY
        // O CVV NUNCA deve ser armazenado.

        // Construtor para criar DadosCartao a partir de dados brutos (usado no DTO de entrada - A SER REMOVIDO)
        // public DadosCartao(string numeroCartao, string nomeTitular, string dataValidade)
        // {
        //     if (string.IsNullOrWhiteSpace(numeroCartao) || numeroCartao.Length < 13 || numeroCartao.Length > 19)
        //         throw new ArgumentException("Número do cartão inválido.", nameof(numeroCartao));
        //     if (string.IsNullOrWhiteSpace(nomeTitular))
        //         throw new ArgumentNullException(nameof(nomeTitular));
        //     if (string.IsNullOrWhiteSpace(dataValidade) || !System.Text.RegularExpressions.Regex.IsMatch(dataValidade, @"^(0[1-9]|1[0-2])\/\d{2}$"))
        //         throw new ArgumentException("Data de validade inválida (MM/YY).", nameof(dataValidade));

        //     NumeroCartaoMascarado = $"************{numeroCartao.Substring(numeroCartao.Length - 4)}";
        //     NomeTitular = nomeTitular;
        //     DataValidade = dataValidade;
        // }

        // Construtor para criar DadosCartao a partir de dados seguros/mascarados (retornados pelo gateway)
        public DadosCartao(string numeroCartaoMascarado, string nomeTitular, string dataValidade)
        {
            if (string.IsNullOrWhiteSpace(nomeTitular))
                throw new ArgumentNullException(nameof(nomeTitular));
            if (string.IsNullOrWhiteSpace(dataValidade) || !System.Text.RegularExpressions.Regex.IsMatch(dataValidade, @"^(0[1-9]|1[0-2])\/\d{2}$"))
                throw new ArgumentException("Data de validade inválida (MM/YY).", nameof(dataValidade));
            NumeroCartaoMascarado = numeroCartaoMascarado; // Assume que já vem mascarado
            NomeTitular = nomeTitular;
            DataValidade = dataValidade;
        }

        // Construtor para EF Core ou desserialização, se necessário, com propriedades públicas para set.
        // Mas para VOs, a imutabilidade é preferível.

        protected IEnumerable<object> GetEqualityComponents()
        {
            yield return NumeroCartaoMascarado;
            yield return NomeTitular;
            yield return DataValidade;
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var other = (DadosCartao)obj;
            return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
        }

        public override int GetHashCode()
        {
            return GetEqualityComponents()
                .Select(x => x?.GetHashCode() ?? 0) // Handle potential nulls in components
                .Aggregate((x, y) => x ^ y);
        }

        public static bool operator ==(DadosCartao? left, DadosCartao? right) => Equals(left, right);
        public static bool operator !=(DadosCartao? left, DadosCartao? right) => !Equals(left, right);
    }
}