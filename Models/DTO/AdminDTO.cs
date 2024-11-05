using RentCarSystem.Models.Domain;

namespace RentCarSystem.Models.DTO
{
    public class AdminDTO
    {
        public string AdminId { get; set; }

        public DateTime LastLogin { get; set; }

        public UserDTO User { get; set; }
    }
}
