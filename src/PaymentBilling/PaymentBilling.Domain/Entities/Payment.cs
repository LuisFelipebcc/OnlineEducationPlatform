using System;

namespace PaymentBilling.Domain.Entities;

public class Payment
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public Guid CourseId { get; private set; }
    public decimal Amount { get; private set; }
    public PaymentStatus Status { get; private set; }
    public PaymentMethod Method { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? PaidAt { get; private set; }

    private Payment() { }

    public Payment(Guid userId, Guid courseId, decimal amount, PaymentMethod method)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        CourseId = courseId;
        Amount = amount;
        Method = method;
        Status = PaymentStatus.Pending;
        CreatedAt = DateTime.UtcNow;
    }

    public void MarkAsPaid()
    {
        if (Status != PaymentStatus.Pending)
            throw new InvalidOperationException("Payment is not in pending status");

        Status = PaymentStatus.Paid;
        PaidAt = DateTime.UtcNow;
    }

    public void MarkAsFailed()
    {
        if (Status != PaymentStatus.Pending)
            throw new InvalidOperationException("Payment is not in pending status");

        Status = PaymentStatus.Failed;
    }
}

public enum PaymentStatus
{
    Pending,
    Paid,
    Failed
}

public enum PaymentMethod
{
    CreditCard,
    DebitCard,
    BankTransfer,
    PIX
}