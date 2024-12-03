
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.Ocsp;
using RentCarSystem.Models.Domain;
using RentCarSystem.Models.DTO;
using RentCarSystem.Reponsitories.IReponsitories;
using System.Data;

namespace RentCarSystem.Reponsitories
{
    public class ResetPassWordRepository : IResetPassWord
    {
        private readonly RentCarSystemContext dbcontext;
        private readonly IEmailSender emailSender;
        private readonly IPasswordHasher<User> passwordHasher;
        private readonly ILoginReponsitory loginReponsitory;
        private readonly IConfiguration configuration;

        public ResetPassWordRepository(RentCarSystemContext dbcontext, IEmailSender emailSender, IConfiguration configurations, IPasswordHasher<User> passwordHasher, ILoginReponsitory loginReponsitory)
        {
            this.dbcontext = dbcontext;
            this.emailSender = emailSender;
            this.passwordHasher = passwordHasher;
            this.loginReponsitory = loginReponsitory;
            this.configuration = configuration;
        }

        public async Task<string> ChangePasswordAsync(string newPassword, string curPassword, string userId)
        {
            var userdomain = await dbcontext.Users.FirstOrDefaultAsync(x=>x.UserId == userId);
            if (userdomain == null)
            {
                return null;
            }

            var check_result = await loginReponsitory.VerifyPassword(userdomain, curPassword);
            if (!check_result)
            {
                return "Mật khẩu hiện tại của bạn không khớp";
            }

            var identityUser = new IdentityUser
            {
                UserName = userdomain.Email
            };

            // mã hóa mật khẩu và cập nhật 
            var enpassword = new PasswordHasher<IdentityUser>();
            userdomain.Password = enpassword.HashPassword(identityUser, newPassword);
            await dbcontext.SaveChangesAsync();

            return "Đã thay đổi mật khẩu thành công! ";
        }

        public async Task<string> CreatePasswordResetRequestAsync(string email)
        {
            var user = await dbcontext.Users.SingleOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                return "Email không tồn tại.";
            }

            var resetToken = Guid.NewGuid().ToString();
            var expiryDate = DateTime.Now.AddHours(1); // Token hết hạn sau 1 giờ

            var resetRequest = new PasswordResetRequest
            {
                UserId = user.UserId,
                ResetToken = resetToken,
                ExpiryDate = expiryDate
            };

            dbcontext.PasswordResetRequests.Add(resetRequest);
            await dbcontext.SaveChangesAsync();

            // Gửi email với reset token
            //var resetUrl = $"{configuration["AppSettings:BaseUrl"]}/reset-password?token={resetToken}";

            // Gửi email reset mật khẩu
            var subject = "Yêu cầu thay đổi mật khẩu của bạn";
            var message = $"Chúng tôi đã nhận được yêu cầu thay đổi mật khẩu cho tài khoản của bạn. Nếu bạn đã yêu cầu thay đổi mật khẩu, vui lòng nhập token vào để thay đổi mật khẩu. " +
                          $"Nếu không phải bạn yêu cầu, xin bỏ qua email này.\n\n{resetToken}";

            await emailSender.SendEmailAsync(email, subject, message);

            return "Đã gửi email reset mật khẩu.";
        }

        public async Task<string> ResetPasswordAsync(string resetToken, string newPassword)
        {
            var resetRequest = await dbcontext.PasswordResetRequests
            .Where(r => r.ResetToken == resetToken && r.ExpiryDate > DateTime.Now && r.IsUsed == false)
            .SingleOrDefaultAsync();

            if (resetRequest == null)
            {
                return "Token không hợp lệ hoặc đã hết hạn.";
            }

            var user = await dbcontext.Users.SingleOrDefaultAsync(u => u.UserId == resetRequest.UserId);
            if (user == null)
            {
                return "Người dùng không tồn tại.";
            }

            var identityUser = new IdentityUser
            {
                UserName = user.Email
            };

            // mã hóa mật khẩu và cập nhật 
            var passwordHasher = new PasswordHasher<IdentityUser>();
            user.Password = passwordHasher.HashPassword(identityUser, newPassword);
            await dbcontext.SaveChangesAsync();

            // Đánh dấu token là đã sử dụng
            resetRequest.IsUsed = true;
            await dbcontext.SaveChangesAsync();

            return "Mật khẩu đã được thay đổi thành công.";
        }

        
    }
    
}
