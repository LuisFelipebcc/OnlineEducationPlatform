using PaymentBilling.Domain.Core;
using PaymentBilling.Domain.Enums;
using PaymentBilling.Domain.Events;
using System;

namespace PaymentBilling.Domain.Entities;

public class Payment
{
    public Guid Id { get; private set; }
    public Guid AlunoId { get; private set; }
    public Guid MatriculaId { get; private set; }
    public decimal Valor { get; private set; }
    public DateTime DataPagamento { get; private set; }
    public StatusPagamento Status { get; private set; }
    public MetodoPagamento MetodoPagamento { get; private set; }
    public string NumeroTransacao { get; private set; }
    public string? Observacoes { get; private set; }

    protected Payment() { }

    public Payment(Guid alunoId, Guid matriculaId, decimal valor, MetodoPagamento metodoPagamento)
    {
        Id = Guid.NewGuid();
        AlunoId = alunoId;
        MatriculaId = matriculaId;
        Valor = valor;
        DataPagamento = DateTime.UtcNow;
        Status = StatusPagamento.Pendente;
        MetodoPagamento = metodoPagamento;
        NumeroTransacao = GerarNumeroTransacao();
    }

    public void ConfirmarPagamento()
    {
        if (Status != StatusPagamento.Pendente)
            throw new InvalidOperationException("Apenas pagamentos pendentes podem ser confirmados");

        Status = StatusPagamento.Confirmado;

        // Disparar evento
        DomainEvents.Raise(new PagamentoConfirmadoEvent(Id, DateTime.UtcNow));
    }


    public void CancelarPagamento(string motivo)
    {
        if (Status != StatusPagamento.Pendente)
            throw new InvalidOperationException("Apenas pagamentos pendentes podem ser cancelados");

        Status = StatusPagamento.Cancelado;
        Observacoes = motivo;
    }

    public void ReembolsarPagamento(string motivo)
    {
        if (Status != StatusPagamento.Confirmado)
            throw new InvalidOperationException("Apenas pagamentos confirmados podem ser reembolsados");

        Status = StatusPagamento.Reembolsado;
        Observacoes = motivo;
    }

    private string GerarNumeroTransacao()
    {
        return $"TRX-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 8)}";
    }
}
