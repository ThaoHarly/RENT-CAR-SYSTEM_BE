using RentCarSystem.Models.Domain;

namespace RentCarSystem.Reponsitories
{
    public interface IRentalAgreementReponsitory
    {
        Task<RentalAgreement> createRentalAgreementAsync(RentalAgreement rentalAgreement, Vehicle vehicle, string customerId);
        Task<RentalAgreement> updateRentalAgreementAsync(string rentalAgreementId);
        Task<RentalAgreement> updateRentalAgreementAsync2(string rentalAgreementId);

        Task<RentalAgreement> GetActiveRentalAgreementAsync(string userId, string vehicleId);

        Task<RentalAgreement> getByIdAsync(string rentalAgreementId);
    }
}
