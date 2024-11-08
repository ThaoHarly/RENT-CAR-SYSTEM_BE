using System.ComponentModel.DataAnnotations;

namespace RentCarSystem.Models.DTO
{
    public class LoginRequestDTO
    {
        [Required]
        public string email { get; set; }
        [Required]
        public string password { get; set; }
    }
}
