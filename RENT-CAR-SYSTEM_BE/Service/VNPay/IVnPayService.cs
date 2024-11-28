using RentCarSystem.Models.DTO;
using RentCarSystem.Models.VNPay;

namespace RentCarSystem.Service.VNPay
{
    public interface IVnPayService
    {
        string CreatePaymentUrl(PaymentInformationModel model, HttpContext context);
        PaymentResponseModel PaymentExecute(IQueryCollection collections);

    }
}
