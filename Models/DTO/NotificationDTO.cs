using RentCarSystem.Models.Domain;

namespace RentCarSystem.Models.DTO
{
    public class NotificationDTO
    {
        public string NotificationId { get; set; } = Guid.NewGuid().ToString();

        public string SenderId { get; set; }

        public string ReceiverId { get; set; }

        public string Message { get; set; } = null!;

        public DateOnly NotificationDate { get; set; }
    }
}
