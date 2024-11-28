using RentCarSystem.Libraries;
using RentCarSystem.Models.DTO;
using RentCarSystem.Models.VNPay;

namespace RentCarSystem.Service.VNPay
{
    public class VnPayService: IVnPayService
    {
        private readonly IConfiguration _configuration;

        public VnPayService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string CreatePaymentUrl(PaymentInformationModel model, HttpContext context)
        {
            var timeZoneId = _configuration["TimeZoneId"];
            var timeZoneById = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            var timeNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneById);

            var pay = new VnpayLibrary();
            var tick = DateTime.Now.Ticks.ToString(); // Unique transaction reference

            // Add request data from configuration and model
            pay.AddRequestData("vnp_Version", _configuration["Vnpay:Version"]);
            pay.AddRequestData("vnp_Command", _configuration["Vnpay:Command"]);
            pay.AddRequestData("vnp_TmnCode", _configuration["Vnpay:TmnCode"]);
            pay.AddRequestData("vnp_Amount", ((int)model.Amount * 100).ToString()); // Convert amount to VNPay format
            pay.AddRequestData("vnp_CreateDate", timeNow.ToString("yyyyMMddHHmmss")); // Transaction date
            pay.AddRequestData("vnp_CurrCode", _configuration["Vnpay:CurrCode"]);
            pay.AddRequestData("vnp_IpAddr", pay.GetIpAddress(context)); // Customer's IP
            pay.AddRequestData("vnp_Locale", _configuration["Vnpay:Locale"]);

            // Include RentalAgreementId and OrderDescription in vnp_OrderInfo
            var orderInfo = $"{model.OrderDescription}|{model.RentalAgreementId}|{model.PaymentType}|{model.VehicleId}";
            pay.AddRequestData("vnp_OrderInfo", orderInfo);


            pay.AddRequestData("vnp_OrderType", model.OrderType ?? "other"); // Default OrderType to "other" if null
            pay.AddRequestData("vnp_ReturnUrl", _configuration["Vnpay:vnp_ReturnUrl"]);
            pay.AddRequestData("vnp_TxnRef", tick); // Unique reference for transaction

            // Generate the payment URL with security hash
            var paymentUrl = pay.CreateRequestUrl(_configuration["Vnpay:BaseUrl"], _configuration["Vnpay:HashSecret"]);
            return paymentUrl;
        }

        public PaymentResponseModel PaymentExecute(IQueryCollection collections)
        {
            var pay = new VnpayLibrary();
            var response = pay.GetFullResponseData(collections, _configuration["Vnpay:HashSecret"]);


            // Extract RentalAgreementId and OrderDescription from vnp_OrderInfo
            var orderInfo = collections["vnp_OrderInfo"].ToString();
            if (!string.IsNullOrEmpty(orderInfo))
            {
                var parts = orderInfo.Split('|'); // Split orderInfo by '|'

                // Assign values based on split results
                response.OrderDescription = parts.Length > 0 ? parts[0] : null; // First part as OrderDescription
                response.RentalAgreementId = parts.Length > 1 ? parts[1] : null; // Second part as RentalAgreementId
                response.PaymentType = parts.Length > 2 ? parts[2] : null;      // Third part as PaymentType
                response.VehicleId = parts.Length > 3 ? parts[3] : null;      // Fourth part as VehicleId
            }
            return response;
        }

    }
}
