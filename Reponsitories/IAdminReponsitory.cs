using RentCarSystem.Models.Domain;

namespace RentCarSystem.Reponsitories
{
    public interface IAdminReponsitory
    {
        Task<List<Admin>> GetAdminAsync();
        Task<ApprovalRequest> UpdateApprovalRequestAsync(string idBusiness, ApprovalRequest approvalRequest);
        Task<Notification> SendNotificationAsync(string idBusiness, string status);
        Task<Admin> DeleteAdminAsync(Admin admin);
    }
}
