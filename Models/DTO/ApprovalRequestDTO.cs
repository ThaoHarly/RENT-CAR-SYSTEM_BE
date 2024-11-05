using RentCarSystem.Models.Domain;

namespace RentCarSystem.Models.DTO
{
    public class ApprovalRequestDTO
    {
        public DateOnly? RequestDay { get; set; }

        public string Status { get; set; } = null!;
    }
}
