using Shared.Domain;

namespace PaymentBilling.Domain.Events;

public class PaymentCancelledEvent : DomainEvent
{
    public Guid PaymentId { get; }
    public Guid UserId { get; }
    public decimal Amount { get; }
    public string Reason { get; }
    public DateTime CancelledAt { get; }

    public PaymentCancelledEvent(
        Guid paymentId,
        Guid userId,
        decimal amount,
        string reason,
        DateTime cancelledAt)
    {
        PaymentId = paymentId;
        UserId = userId;
        Amount = amount;
        Reason = reason;
        CancelledAt = cancelledAt;
    }
}