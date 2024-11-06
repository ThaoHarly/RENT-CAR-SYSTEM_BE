namespace RentCarSystem.Models.UpdateRequest
{
    public class AddMotorRequest
    {
        public string MotorImage { get; set; } = null!;
        public string VehicleId { get; set; }
    }
}
