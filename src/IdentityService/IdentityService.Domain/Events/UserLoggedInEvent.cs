using MediatR;

namespace IdentityService.Domain.Events
{
    public class UserLoggedInEvent : INotification
    {
        public string UserId { get; }
        public string Email { get; }
        public string UserType { get; }
        public DateTime LoginDate { get; }

        public UserLoggedInEvent(
            string userId,
            string email,
            string userType)
        {
            UserId = userId;
            Email = email;
            UserType = userType;
            LoginDate = DateTime.UtcNow;
        }
    }
}