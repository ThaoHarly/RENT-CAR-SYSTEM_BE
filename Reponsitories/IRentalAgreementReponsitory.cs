using RentCarSystem.Models.Domain;

namespace RentCarSystem.Reponsitories
{
    public interface IRentalAgreementReponsitory
    {
        Task<RentalAgreement> createRentalAgreementAsync(RentalAgreement rentalAgreement, Vehicle vehicle, string customerId);
    }
}
