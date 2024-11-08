using System.ComponentModel.DataAnnotations;

namespace RentCarSystem.Models.UpdateRequest
{
    public class UpdateVehicleRequest
    {
        [Required]
        public string Category { get; set; } = null!;
        [Required]
        public string LicensePlate { get; set; } = null!;

        public string Status { get; set; } = null!;

        public double PricePerDay { get; set; }

        public double FuelConsumption { get; set; }

        public double Range { get; set; }

        public double EngineCapacity { get; set; }
    }
}
