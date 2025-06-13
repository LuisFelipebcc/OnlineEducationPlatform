using Shared.Domain;

namespace PaymentBilling.Domain.Events;

public class PaymentRejectedEvent : DomainEvent
{
    public Guid PaymentId { get; }
    public Guid UserId { get; }
    public decimal Amount { get; }
    public string ErrorMessage { get; }
    public DateTime RejectedAt { get; }

    public PaymentRejectedEvent(
        Guid paymentId,
        Guid userId,
        decimal amount,
        string errorMessage,
        DateTime rejectedAt)
    {
        PaymentId = paymentId;
        UserId = userId;
        Amount = amount;
        ErrorMessage = errorMessage;
        RejectedAt = rejectedAt;
    }
}