using RentCarSystem.Models.Domain;

namespace RentCarSystem.Models.DTO
{
    public class UserDTO
    {
        public string Name { get; set; } = null!;

        public string? Email { get; set; }

        public string PhoneNumber { get; set; } = null!;

        public string? Nationality { get; set; }
    }
}
