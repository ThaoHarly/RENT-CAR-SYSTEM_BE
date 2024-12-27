
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
            // Tìm người dùng
            var userdomain = await dbcontext.Users.FirstOrDefaultAsync(x => x.UserId == userId);
            if (userdomain == null)
            {
                return "Người dùng không tồn tại.";
            }

            // Kiểm tra mật khẩu hiện tại
            var check_result = await loginReponsitory.VerifyPassword(userdomain, curPassword);
            if (!check_result)
            {
                return "Mật khẩu hiện tại của bạn không khớp";
            }

            // Kiểm tra mật khẩu mới không được null
            if (newPassword == null)
            {
                return "" + "Mật khẩu mới không được để null.";
            }

            // Kiểm tra mật khẩu mới không được để trống
            if (string.IsNullOrWhiteSpace(newPassword))
            {
                return "Mật khẩu mới không được để trống.";
            }

            // Kiểm tra độ mạnh mật khẩu mới
            if (!IsPasswordStrong(newPassword))
            {
                return "Mật khẩu mới phải có ít nhất 8 ký tự, bao gồm chữ hoa, chữ thường, số và ký tự đặc biệt.";
            }

            var identityUser = new IdentityUser
            {
                UserName = userdomain.Email
            };

            // mã hóa mật khẩu và cập nhật 
            var enpassword = new PasswordHasher<IdentityUser>();
            userdomain.Password = enpassword.HashPassword(identityUser, newPassword);
            await dbcontext.SaveChangesAsync();

            // Gửi thông báo qua email
            await emailSender.SendEmailAsync(userdomain.Email,
                "Thông báo thay đổi mật khẩu",
                "Mật khẩu tài khoản của bạn đã được thay đổi thành công. Nếu không phải bạn thực hiện, vui lòng liên hệ hỗ trợ ngay.");


            return "Đã thay đổi mật khẩu thành công! ";
        }

        public async Task<string> CreatePasswordResetRequestAsync(string email)
        {
            var user = await dbcontext.Users.SingleOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                return "Email không tồn tại.";
            }

            // Kiểm tra xem đã tồn tại yêu cầu reset mật khẩu nào chưa
            var existingRequest = await dbcontext.PasswordResetRequests
                .SingleOrDefaultAsync(r => r.UserId == user.UserId);

            // Kiểm tra hiệu lực
            if (existingRequest != null && existingRequest.ExpiryDate > DateTime.UtcNow)
            {
                return "Token reset mật khẩu hiện tại vẫn còn hiệu lực. Vui lòng kiểm tra email của bạn.";
            }

            // Nếu không có yêu cầu cũ hoặc token đã hết hạn, tạo token mới
            var resetToken = Guid.NewGuid().ToString();
            var expiryDate = DateTime.UtcNow.AddHours(1); // Token hết hạn sau 1 giờ

            if (existingRequest != null)
            {
                // Cập nhật bản ghi cũ
                existingRequest.ResetToken = resetToken;
                existingRequest.ExpiryDate = expiryDate;
                existingRequest.CreatedAt = DateTime.UtcNow;
            }
            else
            {
                // Tạo bản ghi mới nếu chưa có
                var resetRequest = new PasswordResetRequest
                {
                    UserId = user.UserId,
                    ResetToken = resetToken,
                    ExpiryDate = expiryDate,
                    CreatedAt = DateTime.UtcNow,
                    IsUsed = false
                };

                dbcontext.PasswordResetRequests.Add(resetRequest);
            }

            // Lưu thay đổi vào DB
            await dbcontext.SaveChangesAsync();

            // Gửi email với reset token
            var resetTokenForEmail = existingRequest?.ResetToken ?? Guid.NewGuid().ToString();
            var subject = "Yêu cầu thay đổi mật khẩu của bạn";
            var message = $@"
                          <!DOCTYPE html>
                          <html lang=""en"">
                            <head>
                            <meta charset=""UTF-8"">
                            <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                            <title>Yêu cầu thay đổi mật khẩu</title>
                            <style>
                            body {{
                                font-family: Arial, sans-serif;
                                background-color: #f4f4f9;
                                margin: 0;
                                padding: 0;
                                }}
                            .container {{
                                        width: 100%;
                                        max-width: 600px;
                                        margin: 0 auto;
                                        background-color: #ffffff;
                                        border: 1px solid #e0e0e0;
                                        border-radius: 8px;
                                        box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
                                        overflow: hidden;
                                    }}
                                    .header {{
                                        background-color: #4CAF50;
                                        color: #ffffff;
                                        padding: 20px;
                                        text-align: center;
                                        font-size: 1.5em;
                                    }}
                                    .content {{
                                        padding: 20px;
                                        color: #333333;
                                        line-height: 1.6;
                                    }}
                                    .token {{
                                        display: inline-block;
                                        background-color: #4CAF50;
                                        color: #ffffff;
                                        font-weight: bold;
                                        font-size: 1.2em;
                                        padding: 10px 15px;
                                        margin: 10px 0;
                                        border-radius: 5px;
                                        text-align: center;
                                    }}
                                    .footer {{
                                        background-color: #f4f4f9;
                                        color: #777777;
                                        padding: 10px;
                                        text-align: center;
                                        font-size: 0.9em;
                                    }}
                                </style>
                            </head>
                            <body>
                                <div class=""container"">
                                    <div class=""header"">
                                        Yêu Cầu Thay Đổi Mật Khẩu
                                    </div>
                                    <div class=""content"">
                                        <p>Chúng tôi đã nhận được yêu cầu thay đổi mật khẩu cho tài khoản của bạn.</p>
                                        <p>Nếu bạn đã yêu cầu thay đổi mật khẩu, vui lòng sử dụng mã token bên dưới để thay đổi mật khẩu:</p>
                                        <div class=""token"">{resetTokenForEmail}</div>
                                        <p>Nếu không phải bạn yêu cầu, vui lòng bỏ qua email này.</p>
                                    </div>
                                    <div class=""footer"">
                                        &copy; 2024 Your Company Name. All rights reserved.
                                    </div>
                                </div>
                            </body>
                            </html>";
            await emailSender.SendEmailAsync(email, subject, message);

            return "Đã gửi email reset mật khẩu.";
        }



        public async Task<string> ResetPasswordAsync(string resetToken, string newPassword)
        {
            try
            {
                // Kiểm tra mật khẩu không được bỏ trống
                if (string.IsNullOrWhiteSpace(newPassword))
                {
                    return "Mật khẩu không được để trống.";
                }

                // Kiểm tra mật khẩu có đủ mạnh hay không
                if (!IsPasswordStrong(newPassword))
                {
                    return "Mật khẩu phải có ít nhất 8 ký tự, bao gồm chữ hoa, chữ thường, số và ký tự đặc biệt.";
                }

                var resetRequest = await dbcontext.PasswordResetRequests
                .Where(r => r.ResetToken == resetToken && r.ExpiryDate > DateTime.UtcNow && r.IsUsed == false)
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

                // Đánh dấu token là đã sử dụng và ExpiryDate hết hạn
                resetRequest.IsUsed = true;
                resetRequest.ExpiryDate = DateTime.UtcNow;

                // Lưu tất cả thay đổi vào cơ sở dữ liệu
                await dbcontext.SaveChangesAsync();

                return "Mật khẩu đã được thay đổi thành công.";
            }
            catch (Exception ex)
            {
                return $"Đã xảy ra lỗi: {ex.Message}";
            }
        }

        // Hàm kiểm tra mật khẩu mạnh
        private bool IsPasswordStrong(string password)
        {
            if (password.Length < 8) return false; // Kiểm tra độ dài
            if (!password.Any(char.IsUpper)) return false; // Chứa chữ hoa
            if (!password.Any(char.IsLower)) return false; // Chứa chữ thường
            if (!password.Any(char.IsDigit)) return false; // Chứa chữ số
            if (!password.Any(ch => "!@#$%^&*()_+-=[]{}|;:',.<>?/`~".Contains(ch))) return false; // Chứa ký tự đặc biệt
            return true;
        }

    }

}
