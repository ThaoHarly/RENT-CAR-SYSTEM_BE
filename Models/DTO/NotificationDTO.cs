using RentCarSystem.Models.Domain;

namespace RentCarSystem.Models.DTO
{
    public class NotificationDTO
    {
        public Guid NotificationId { get; set; }

        public string Message { get; set; } = null!;

        public DateOnly NotificationDate { get; set; }
    }
}
