namespace RentCarSystem.Models.DTO
{
    public class CarDTO
    {
        public Guid CarId { get; set; }

        public VehicleDTO? Vehicle { get; set; }

        public string CarBrand { get; set; } = null!;

        public string FuelType { get; set; } = null!;

        public int SeatingCapacity { get; set; }

        public string CarImage { get; set; } = null!;

        public double? ChargingTime { get; set; }
    }
}
