using RentCarSystem.Models.Domain;

namespace RentCarSystem.Models.DTO
{
    public class MotorDTO
    {
        public string MotorId { get; set; } 

        public string VehicleId { get; set; }

        public string MotorImage { get; set; } = null!;

        public virtual VehicleDTO VehicleDTO { get; set; } = null!;
    }
}
