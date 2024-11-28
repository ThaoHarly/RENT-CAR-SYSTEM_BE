namespace RentCarSystem.Models.VNPay
{
    public class PaymentResponseModel
    {
        public string OrderDescription { get; set; }
        public string TransactionId { get; set; }
        public string OrderId { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentId { get; set; }
        public string PaymentType { get; set; } // "DEPOSIT" or "REMAINING" -- tiền cọc hoặc tiền còn lại
        public string VehicleId { get; set; }
        public bool Success { get; set; }
        public string Token { get; set; }
        public string VnPayResponseCode { get; set; }

        public string RentalAgreementId { get; set; }

    }
}
