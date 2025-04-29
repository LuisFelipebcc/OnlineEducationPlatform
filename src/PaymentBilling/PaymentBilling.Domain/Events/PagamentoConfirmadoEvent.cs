namespace PaymentBilling.Domain.Events
{
    public class PagamentoConfirmadoEvent
    {
        public Guid PagamentoId { get; }
        public DateTime DataConfirmacao { get; }

        public PagamentoConfirmadoEvent(Guid pagamentoId, DateTime dataConfirmacao)
        {
            PagamentoId = pagamentoId;
            DataConfirmacao = dataConfirmacao;
        }
    }
}