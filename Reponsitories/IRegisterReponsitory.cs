using RentCarSystem.Models.Domain;

namespace RentCarSystem.Reponsitories
{
    public interface IRegisterReponsitory
    {
<<<<<<< HEAD
        Task <User> RegisterUser (User user, string rawPassword, string role);
=======
        Task <User> RegisterUser (User user, string rawPassword);
>>>>>>> 245adf983c80f561f0c244ccf5e507c9b3b495e7
        Task<Admin> RegisterAdmin(Admin admin);
        Task<Customer> RegisterCustomer(Customer customer);
        Task<VehicleHireService> RegisterService(VehicleHireService vehicleHireService);
        Task<Individual> RegisterIndividual(Individual individual);
        Task<Business> RegisterBusiness(Business business);
        Task<Role> RegisterRole(Role role);

        Task<ApprovalRequest> RegisterApprovalRequest(ApprovalRequest approvalRequest);
        Task<Notification> CreateNotification(Notification notification);
    }
}
