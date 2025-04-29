namespace PaymentBilling.Domain.Events
{
    public class PagamentoRejeitadoEvent
    {
        public Guid PagamentoId { get; }
        public DateTime DataRejeicao { get; }
        public string Motivo { get; }

        public PagamentoRejeitadoEvent(Guid pagamentoId, DateTime dataRejeicao, string motivo)
        {
            PagamentoId = pagamentoId;
            DataRejeicao = dataRejeicao;
            Motivo = motivo;
        }
    }
}