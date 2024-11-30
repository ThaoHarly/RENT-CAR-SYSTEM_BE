namespace RentCarSystem.Models.DTO
{
    public class ReviewDTO
    {
        public string ReviewId { get; set; }

        public string CusId { get; set; } = null!;

        public string VehicleId { get; set; }

        public int Rating { get; set; }

        public string Comment { get; set; } = null!;

        public DateOnly ReviewDate { get; set; }

    }
}
