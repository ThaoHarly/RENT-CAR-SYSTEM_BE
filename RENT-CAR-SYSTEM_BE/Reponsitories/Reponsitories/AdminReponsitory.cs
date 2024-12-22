using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using RentCarSystem.Models.Domain;
using RentCarSystem.Models.DTO;
using RentCarSystem.Reponsitories.IReponsitories;

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

        public async Task<IEnumerable<UserDTO>> getAllUsersAsync()
        {
            var users = await dbContext.Users.Include(u => u.Roles).ToListAsync(); // Đảm bảo Role được load

            return users.Select(u => new UserDTO
            {
                Id = u.UserId,
                Name = u.Name,
                Email = u.Email,
                Nationality = u.Nationality,
                PhoneNumber = u.PhoneNumber,
                Roles = string.Join(", ", u.Roles.Select(r => r.Type))
            });
        }

        public async Task<UserDTO> GetUserByIdAsync(string id)
        {
            var users =  await dbContext.Users.Include(u => u.Roles).FirstOrDefaultAsync(x => x.UserId == id);

            return new UserDTO
            {
                Id = id,
                Name = users.Name,
                Email = users.Email,
                Nationality = users.Nationality,
                PhoneNumber = users.PhoneNumber,
                Roles = string.Join(", ", users.Roles.Select(r => r.Type))
            };
        }
    }
}
