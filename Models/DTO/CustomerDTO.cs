using RentCarSystem.Models.Domain;

namespace RentCarSystem.Models.DTO
{
    public class CustomerDTO
    {
        public string LicenseId { get; set; } = null!;

        public string Class { get; set; } = null!;

        public DateOnly Expire { get; set; }

        public string Image { get; set; } = null!;
    }
}
