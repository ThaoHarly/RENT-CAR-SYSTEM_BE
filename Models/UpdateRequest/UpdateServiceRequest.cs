using System.ComponentModel.DataAnnotations;

namespace RentCarSystem.Models.UpdateRequest
{
    public class UpdateServiceRequest
    {
        [Required]
        public string ServiceType { get; set; } = null!;
        [Required]
        public string BankName { get; set; } = null!;
        [Required]
        public string BankAccount { get; set; } = null!;
    }
}
