using RentCarSystem.Models.DTO;
using System.ComponentModel.DataAnnotations;

namespace RentCarSystem.Models.UpdateRequest
{
    public class AddCarRequest
    {
        public Guid VehicleId { get; set; }

        public string CarBrand { get; set; } = null!;

        public string FuelType { get; set; } = null!;

        public int SeatingCapacity { get; set; }

        public string CarImage { get; set; } = null!;

        public double? ChargingTime { get; set; }
    }
}
