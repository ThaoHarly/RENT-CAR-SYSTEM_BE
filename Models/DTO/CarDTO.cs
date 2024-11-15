using RentCarSystem.Models.Domain;

namespace RentCarSystem.Models.DTO
{
    public class CarDTO
    {
        public string CarId { get; set; } 

        public string VehicleId { get; set; }

        public string CarBrand { get; set; } = null!;

        public string FuelType { get; set; } = null!;

        public int SeatingCapacity { get; set; }

        public string CarImage { get; set; } = null!;

        public double? ChargingTime { get; set; }

        public virtual VehicleDTO vehicleDTO { get; set; } = null!;
    }
}
