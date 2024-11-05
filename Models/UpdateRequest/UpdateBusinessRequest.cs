using System.ComponentModel.DataAnnotations;

namespace RentCarSystem.Models.UpdateRequest
{
    public class UpdateBusinessRequest
    {
        public string Description { get; set; } = null!;
        [Required]
        public string BusinessImg { get; set; } = null!;
        [Required]
        public DateOnly RegistrationDate { get; set; }
        [Required]
        public double Vat { get; set; }
        [Required]
        public string IssuingLocation { get; set; } = null!;
        [Required]
        public DateOnly DateOfIssue { get; set; }
    }
}
