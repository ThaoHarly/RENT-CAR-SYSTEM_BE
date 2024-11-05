using System.ComponentModel.DataAnnotations;

namespace RentCarSystem.Models.DTO
{
    public class MotorDTO
    {
        
        public Guid MotorId { get; set; }

        public VehicleDTO Vehicle{ get; set; }

        public string MotorImage { get; set; } = null!;
    }
}
