using RentCarSystem.Models.Domain;

namespace RentCarSystem.Reponsitories.IReponsitories
{
    public interface IRegisterReponsitory
    {
        Task<User> RegisterUser(User user, string role);

        Task<Admin> RegisterAdmin(Admin admin);
        Task<Customer> RegisterCustomer(Customer customer);
        Task<VehicleHireService> RegisterService(VehicleHireService vehicleHireService);
        Task<Individual> RegisterIndividual(Individual individual);
        Task<Business> RegisterBusiness(Business business);
        Task<Role> RegisterRole(Role role);

        Task<ApprovalRequest> RegisterApprovalRequest(ApprovalRequest approvalRequest);
        Task<Notification> CreateNotification(Notification notification);
        Task<bool> VerifyOTP(string userId, string otpInput, string userType);
    }
}
