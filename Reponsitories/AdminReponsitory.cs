using Microsoft.EntityFrameworkCore;
using RentCarSystem.Models.Domain;
using RentCarSystem.Models.DTO;

namespace RentCarSystem.Reponsitories
{
    public class AdminReponsitory : IAdminReponsitory
    {
        private readonly RentCarSystemContext dbContext;
        private readonly IRegisterReponsitory registerReponsitory;

        public AdminReponsitory(RentCarSystemContext dbContext, IRegisterReponsitory registerReponsitory)
        {
            this.dbContext = dbContext;
            this.registerReponsitory = registerReponsitory;
        }
        public async Task<List<Admin>> GetAdminAsync()
        {
            return await dbContext.Admins.Include(x => x.AdminNavigation).ToListAsync();
        }

        public async Task<ApprovalRequest> UpdateApprovalRequestAsync(string idBusiness, ApprovalRequest approvalRequest)
        {
            var existingRequest = await dbContext.ApprovalRequests.FirstOrDefaultAsync(x => x.BsnId == idBusiness);

            if (existingRequest == null)
                return null;

            // Update status
            existingRequest.Status = approvalRequest.Status;

            await dbContext.SaveChangesAsync();                       

            return existingRequest;
        }

        public async Task<Notification> SendNotificationAsync(string idBusiness, string status)
        {
            //Get AdminId
            var admId = await dbContext.Users
                .Where(u => u.Roles.Any(r => r.Type.ToUpper() == "ADMIN"))
                .Select(u => u.UserId.ToString())
                .FirstOrDefaultAsync();

            //Get UserId of business
            var user = await dbContext.Businesses.FirstOrDefaultAsync(x => x.BsnId == idBusiness);

            //Create Notification
            var notificationDefault = new Notification
            {
                SenderId = admId,
                ReceiverId = user.UserId,
                Message = $"Admin {status.ToUpper()} request for this business ...",
                NotificationDate = DateOnly.FromDateTime(DateTime.Now)
            };

            var notification = await registerReponsitory.CreateNotification(notificationDefault);

            return notification;
        }

        public Task<Admin> DeleteAdminAsync(Admin admin)
        {
            throw new NotImplementedException();
        }

    }
}
