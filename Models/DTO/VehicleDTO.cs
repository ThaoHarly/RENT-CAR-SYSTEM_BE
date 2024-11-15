using RentCarSystem.Models.Domain;

namespace RentCarSystem.Models.DTO
{
    public class VehicleDTO
    {
        public string VehicleId { get; set; } 

        public string UserId { get; set; }

        public string Category { get; set; } = null!;

        public string LicensePlate { get; set; } = null!;

        public string Status { get; set; } = null!;

        public double PricePerDay { get; set; }

        public double FuelConsumption { get; set; }

        public double Range { get; set; }

        public double EngineCapacity { get; set; }

        public virtual CarDTO? CarDTO { get; set; }

        public virtual MotorDTO? MotorDTO { get; set; }
    }
}
