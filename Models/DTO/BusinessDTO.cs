using RentCarSystem.Models.Domain;

namespace RentCarSystem.Models.DTO
{
    public class BusinessDTO
    {
        public string BsnId { get; set; } = Guid.NewGuid().ToString();

        public string UserId { get; set; }

        public string Description { get; set; } = null!;

        public string BusinessImg { get; set; } = null!;

        public DateOnly RegistrationDate { get; set; }

        public double Vat { get; set; }

        public string IssuingLocation { get; set; } = null!;

        public DateOnly DateOfIssue { get; set; }


    }
}
