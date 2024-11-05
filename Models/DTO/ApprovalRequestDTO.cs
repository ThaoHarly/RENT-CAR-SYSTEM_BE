using RentCarSystem.Models.Domain;

namespace RentCarSystem.Models.DTO
{
    public class ApprovalRequestDTO
    {
        public DateOnly? RequestDay { get; set; } = null;

        public string Status { get; set; } = null!;
    }
}
