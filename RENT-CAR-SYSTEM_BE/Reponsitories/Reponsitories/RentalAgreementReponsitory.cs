using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using RentCarSystem.Models.Domain;
using RentCarSystem.Reponsitories.IReponsitories;

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
            rentalAgreementDomain.Status = "PENDING";


            //Caculate DepositAmount
            int rentDays = (rentalAgreement.EndDate.Value.ToDateTime(TimeOnly.MinValue) - rentalAgreement.StartDate.Value.ToDateTime(TimeOnly.MinValue)).Days;
            rentalAgreementDomain.RemainingAmount = rentDays * vehicle.PricePerDay;
            rentalAgreementDomain.DepositAmount = rentDays * vehicle.PricePerDay * 0.2; // Giả sử tiền cọc là 20 %


            await dbContext.AddAsync(rentalAgreementDomain);
            await dbContext.SaveChangesAsync();
            return rentalAgreementDomain;
            //-----

        }

        public async Task<RentalAgreement> getByIdAsync(string rentalAgreementId)
        {
            return await dbContext.RentalAgreements.FirstOrDefaultAsync(x => x.AgreementId == rentalAgreementId);
        }

        public async Task<RentalAgreement> updateRentalAgreementAsync(string rentalAgreementId) //Thanh toán tiền cọc
        {
            var existingrentalAgreement = await dbContext.RentalAgreements.FirstOrDefaultAsync(x => x.AgreementId == rentalAgreementId);

            if (existingrentalAgreement == null)
            {
                return null;
            }

            //update  RentalAgreement
            existingrentalAgreement.Status = "ACTIVE";
            existingrentalAgreement.RemainingAmount -= existingrentalAgreement.DepositAmount;

            await dbContext.SaveChangesAsync();
            return existingrentalAgreement;
        }

        public async Task<RentalAgreement> updateRentalAgreementAsync2(string rentalAgreementId) //Thanh toán lần 2
        {
            var existingrentalAgreement = await dbContext.RentalAgreements.FirstOrDefaultAsync(x => x.AgreementId == rentalAgreementId);

            if (existingrentalAgreement == null)
            {
                return null;
            }

            //update  RentalAgreement
            existingrentalAgreement.Status = "COMPLETED";
            existingrentalAgreement.RemainingAmount = 0;

            await dbContext.SaveChangesAsync();
            return existingrentalAgreement;
        }

        public async Task<RentalAgreement> GetActiveRentalAgreementAsync(string userId,string vehicleId) 
        {
            return await dbContext.RentalAgreements.FirstOrDefaultAsync(x => x.CusId == userId && x.VehicleId == vehicleId && x.Status.ToUpper() == "ACTIVE");
        }
    }
}
