namespace RentCarSystem.Models.UpdateRequest
{
    public class AddMotorRequest
    {
        public string MotorImage { get; set; } = null!;
        public Guid VehicleId { get; set; }
    }
}
