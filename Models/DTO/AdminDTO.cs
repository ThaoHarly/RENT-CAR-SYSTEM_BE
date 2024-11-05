using RentCarSystem.Models.Domain;

namespace RentCarSystem.Models.DTO
{
    public class AdminDTO
    {
        public Guid AdminId { get; set; }

        public DateTime LastLogin { get; set; }
    }
}
