using RentCarSystem.Models.Domain;
using RentCarSystem.Models.VNPay;

namespace RentCarSystem.Reponsitories
{
    public interface IBillReponsitory
    {
        Task<Bill> createBill(PaymentResponseModel paymentResponse);
    }
}
