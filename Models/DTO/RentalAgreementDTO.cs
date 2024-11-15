namespace RentCarSystem.Models.DTO
{
    public class RentalAgreementDTO
    {
        public string VehicleId { get; set; }
        public string CusId { get; set; }
        public string ServiceId { get; set; }
        public DateOnly? StartDate { get; set; }

        public DateOnly? EndDate { get; set; }
        public string? Status { get; set; }
        public double? DepositAmount { get; set; }

        public string? PaymentMethod { get; set; }
    }
}
