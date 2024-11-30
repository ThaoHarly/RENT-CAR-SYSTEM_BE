using System.ComponentModel.DataAnnotations;

namespace RentCarSystem.Models.DTO
{
    public class UpdateReviewDTO
    {
        [Required]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
        public int Rating { get; set; }

        [MaxLength(500)]
        public string Comment { get; set; } = null!;

        public DateOnly ReviewDate { get; set; }
    }
}
