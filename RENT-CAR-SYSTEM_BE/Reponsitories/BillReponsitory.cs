using RentCarSystem.Models.Domain;
using RentCarSystem.Models.VNPay;

namespace RentCarSystem.Reponsitories
{
    public class BillReponsitory : IBillReponsitory
    {
        private readonly RentCarSystemContext dbContext;

        public BillReponsitory(RentCarSystemContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<Bill> createBill(PaymentResponseModel paymentResponse)
        {
            var billDomain = new Bill
            {
                BillId = paymentResponse.OrderId,
                AgreementId = paymentResponse.RentalAgreementId,
                PaymentMethod = paymentResponse.PaymentMethod,
                Date = DateOnly.FromDateTime(DateTime.Now),
                OrderDescription = paymentResponse.OrderDescription,
                Status = "COMPLETED"
            };

            await dbContext.AddAsync(billDomain);
            await dbContext.SaveChangesAsync();
            return billDomain;
        }
    }
}
