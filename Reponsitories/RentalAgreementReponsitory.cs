using RentCarSystem.Models.Domain;

namespace RentCarSystem.Reponsitories
{
    public class RentalAgreementReponsitory : IRentalAgreementReponsitory
    {
        private readonly RentCarSystemContext dbContext;

        public RentalAgreementReponsitory(RentCarSystemContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<RentalAgreement> createRentalAgreementAsync(RentalAgreement rentalAgreement, Vehicle vehicle, string customerId)
        {
            var rentalAgreementDomain = rentalAgreement;

            rentalAgreementDomain.VehicleId = vehicle.VehicleId;
            rentalAgreementDomain.CusId = customerId;
            rentalAgreementDomain.ServiceId = vehicle.UserId;
            rentalAgreementDomain.Status = "ACTIVE";


            //Caculate DepositAmount
            int rentDays = (rentalAgreement.EndDate.Value.ToDateTime(TimeOnly.MinValue) - rentalAgreement.StartDate.Value.ToDateTime(TimeOnly.MinValue)).Days;
            rentalAgreementDomain.DepositAmount = rentDays * vehicle.PricePerDay * 0.2; // Giả sử tiền cọc là 20 %


            
            //Set là đã thanh toán rồi
            await dbContext.AddAsync(rentalAgreementDomain);
            await dbContext.SaveChangesAsync();
            return rentalAgreementDomain;


            //-----





        }
    }
}
