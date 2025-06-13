using PaymentBilling.Domain.Enums;
using PaymentBilling.Domain.Events;
using PaymentBilling.Domain.ValueObjects;
using Shared.Domain;

namespace PaymentBilling.Domain.Entities
{
    public class Payment : AggregateRoot
    {
        private readonly List<DomainEvent> _events = new();
        public IReadOnlyCollection<DomainEvent> Events => _events.AsReadOnly();

        public Guid Id { get; private set; }
        public Guid UserId { get; private set; }
        public decimal Amount { get; private set; }
        public string Description { get; private set; }
        public PaymentStatus Status { get; private set; }
        public CardData CardData { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }

        protected Payment()
        {
            Description = string.Empty;
            CardData = new CardData();
        }

        public Payment(Guid userId, decimal amount, string description, CardData cardData)
        {
            if (userId == Guid.Empty)
                throw new ArgumentException("User ID is required", nameof(userId));

            if (amount <= 0)
                throw new ArgumentException("Amount must be greater than zero", nameof(amount));

            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("Description is required", nameof(description));

            if (cardData == null)
                throw new ArgumentNullException(nameof(cardData));

            if (!cardData.IsValid())
                throw new ArgumentException("Invalid card data", nameof(cardData));

            Id = Guid.NewGuid();
            UserId = userId;
            Amount = amount;
            Description = description;
            CardData = cardData;
            Status = PaymentStatus.Pending;
            CreatedAt = DateTime.UtcNow;

            AddDomainEvent(new PaymentCreatedEvent(Id, UserId, Amount, Description, CardData.CardNumber, CreatedAt));
        }

        /// <summary>
        /// Confirms the payment and changes its status to Processed
        /// </summary>
        /// <param name="courseId">The ID of the course associated with this payment.</param>
        /// <param name="gatewayTransactionCode">The transaction code from the payment gateway.</param>
        /// <exception cref="InvalidOperationException">Thrown when the payment is not in Pending status</exception>
        /// <exception cref="ArgumentException">Thrown if courseId is empty or gatewayTransactionCode is null/whitespace.</exception>
        public void Confirm(Guid courseId, string gatewayTransactionCode)
        {
            if (Status != PaymentStatus.Pending)
                throw new InvalidOperationException("Only pending payments can be confirmed");

            if (courseId == Guid.Empty)
                throw new ArgumentException("Course ID is required for confirming payment.", nameof(courseId));

            if (string.IsNullOrWhiteSpace(gatewayTransactionCode))
                throw new ArgumentException("Gateway transaction code is required for confirming payment.", nameof(gatewayTransactionCode));

            Status = PaymentStatus.Processed;
            UpdatedAt = DateTime.UtcNow;

            AddDomainEvent(new PaymentConfirmedEvent(Id, UserId, courseId, gatewayTransactionCode));
        }

        /// <summary>
        /// Rejects the payment and changes its status to Rejected
        /// </summary>
        /// <param name="reason">The reason for rejection</param>
        /// <exception cref="InvalidOperationException">Thrown when the payment is not in Pending status</exception>
        /// <exception cref="ArgumentException">Thrown when the reason is null or empty</exception>
        public void Reject(string reason)
        {
            if (Status != PaymentStatus.Pending)
                throw new InvalidOperationException("Only pending payments can be rejected");

            if (string.IsNullOrWhiteSpace(reason))
                throw new ArgumentException("Reason is required", nameof(reason));

            Status = PaymentStatus.Rejected;
            UpdatedAt = DateTime.UtcNow;

            AddDomainEvent(new PaymentRejectedEvent(Id, UserId, Amount, reason, DateTime.UtcNow));
        }

        /// <summary>
        /// Cancels the payment and changes its status to Cancelled
        /// </summary>
        /// <param name="reason">The reason for cancellation</param>
        /// <exception cref="InvalidOperationException">Thrown when the payment is not in Pending status</exception>
        /// <exception cref="ArgumentException">Thrown when the reason is null or empty</exception>
        public void Cancel(string reason)
        {
            if (Status != PaymentStatus.Pending)
                throw new InvalidOperationException("Only pending payments can be cancelled");

            if (string.IsNullOrWhiteSpace(reason))
                throw new ArgumentException("Reason is required", nameof(reason));

            Status = PaymentStatus.Cancelled;
            UpdatedAt = DateTime.UtcNow;

            AddDomainEvent(new PaymentCancelledEvent(Id, UserId, Amount, reason, DateTime.UtcNow));
        }

        protected void AddDomainEvent(DomainEvent eventToAdd)
        {
            _events.Add(eventToAdd);
        }

        /// <summary>
        /// Clears all domain events from the payment
        /// </summary>
        public void ClearEvents()
        {
            _events.Clear();
        }
    }
}