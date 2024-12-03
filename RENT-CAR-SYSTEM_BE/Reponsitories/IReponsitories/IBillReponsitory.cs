using RentCarSystem.Models.Domain;
using RentCarSystem.Models.VNPay;

namespace RentCarSystem.Reponsitories.IReponsitories
{
    public interface IBillReponsitory
    {
        Task<Bill> createBill(PaymentResponseModel paymentResponse);
    }
}
