using RentCarSystem.Models.Domain;

namespace RentCarSystem.Models.DTO
{
    public class ImageDTO
    {
        public string? VehicleId { get; set; }

        public IFormFile? ImagePath { get; set; }

    }
}
