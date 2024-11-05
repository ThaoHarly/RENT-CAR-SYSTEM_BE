namespace RentCarSystem.Models.UpdateRequest
{
    public class UpdateCarRequest
    {
        public string CarBrand { get; set; } = null!;

        public string FuelType { get; set; } = null!;

        public int SeatingCapacity { get; set; }

        public string CarImage { get; set; } = null!;

        public double? ChargingTime { get; set; }
    }
}
