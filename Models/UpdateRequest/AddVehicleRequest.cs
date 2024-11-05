namespace RentCarSystem.Models.UpdateRequest
{
    public class AddVehicleRequest
    {
        public Guid UserId { get; set; }
        public string Category { get; set; } = null!;

        public string LicensePlate { get; set; } = null!;

        public string Status { get; set; } = null!;

        public double PricePerDay { get; set; }

        public double FuelConsumption { get; set; }

        public double Range { get; set; }

        public double EngineCapacity { get; set; }
    }
}
