using Shared.Domain;

namespace PaymentBilling.Domain.Events;

public class PaymentCreatedEvent : DomainEvent
{
    public Guid PaymentId { get; }
    public Guid UserId { get; }
    public decimal Amount { get; }
    public string Description { get; }
    public string MaskedCardNumber { get; }
    public DateTime CreatedAt { get; }

    public PaymentCreatedEvent(
        Guid paymentId,
        Guid userId,
        decimal amount,
        string description,
        string maskedCardNumber,
        DateTime createdAt)
    {
        PaymentId = paymentId;
        UserId = userId;
        Amount = amount;
        Description = description;
        MaskedCardNumber = maskedCardNumber;
        CreatedAt = createdAt;
    }
}