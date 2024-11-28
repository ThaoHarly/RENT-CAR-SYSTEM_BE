namespace RentCarSystem.Models.VNPay
{
    public class PaymentInformationModel
    {
        public string OrderType { get; set; }
        public double? Amount { get; set; }
        public string OrderDescription { get; set; }

        public string PaymentType { get; set; } // "DEPOSIT" or "REMAINING" -- tiền cọc hoặc tiền còn lại

        public string VehicleId { get; set; }
        public DateOnly? StartDate { get; set; }

        public DateOnly? EndDate { get; set; }
        public string? PaymentMethod { get; set; }

        public string? RentalAgreementId { get; set;}

    }
}
