using System.ComponentModel.DataAnnotations;

namespace PaymentBilling.Application.DTOs
{
    // ATENÇÃO: Este DTO é para entrada de dados. Em um sistema real,
    // a manipulação de dados de cartão deve ser feita com extremo cuidado e,
    // idealmente, através de um iframe/SDK de um gateway de pagamento para conformidade PCI.
    public class DadosCartaoDto
    {
        [Required(ErrorMessage = "O nome do titular do cartão é obrigatório.")]
        public string NomeTitular { get; set; }

        [Required(ErrorMessage = "O número do cartão é obrigatório.")]
        [CreditCard(ErrorMessage = "Número de cartão inválido.")]
        public string NumeroCartao { get; set; } // Número completo para processamento

        [Required(ErrorMessage = "A data de validade é obrigatória (MM/YY).")]
        [RegularExpression(@"^(0[1-9]|1[0-2])\/\d{2}$", ErrorMessage = "Formato da data de validade inválido. Use MM/YY.")]
        public string DataValidade { get; set; } // MM/YY

        [Required(ErrorMessage = "O CVV é obrigatório.")]
        [RegularExpression(@"^\d{3,4}$", ErrorMessage = "CVV inválido.")]
        public string Cvv { get; set; }
    }
}