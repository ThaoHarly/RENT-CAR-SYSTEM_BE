using System.ComponentModel.DataAnnotations;

namespace RentCarSystem.Models.DTO
{
    public class MotorDTO
    {

        public string MotorId { get; set; }

        public string VehicleId { get; set; }

        public string MotorImage { get; set; } = null!;
    }
}
