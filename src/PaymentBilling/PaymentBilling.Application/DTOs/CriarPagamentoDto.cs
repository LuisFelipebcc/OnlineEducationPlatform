using System;
using System.ComponentModel.DataAnnotations;

namespace PaymentBilling.Application.DTOs
{
    public class CriarPagamentoDto
    {
        [Required]
        public Guid AlunoId { get; set; } // Virá do usuário autenticado

        [Required]
        public Guid ReferenciaPedidoId { get; set; } // Ex: MatriculaId ou CursoId

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "O valor do pagamento deve ser positivo.")]
        public decimal Valor { get; set; }

        // Remover: public DadosCartaoDto? DadosCartao { get; set; }
        [Required(ErrorMessage = "O token do método de pagamento é obrigatório.")]
        public string PaymentMethodToken { get; set; } // Token recebido do gateway de pagamento

        // Outros métodos de pagamento poderiam ser adicionados aqui (ex: Pix, Boleto)
    }
}