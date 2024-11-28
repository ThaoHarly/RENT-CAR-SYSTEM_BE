using System.Net.Mail; // Dành cho System.Net.Mail.SmtpClient
using MailKit.Net.Smtp; // Dành cho MailKit.Net.Smtp.SmtpClient
using System.Net;
using RentCarSystem.Models.DTO;
using RentCarSystem.Models.Domain;
using MimeKit;
using MailKit.Security;
using Microsoft.Extensions.Options;

namespace RentCarSystem.Reponsitories
{
    public class EmailRepository : IEmailSender
    {

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            try
            {
                // Kết nối tới máy chủ SMTP
                smtp.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);

                // Xác thực tài khoản Gmail
                smtp.Authenticate("demoemail031409@gmail.com", "uzvg apyz jano bciw");

                // Tạo email
                var mimeMessage = new MimeMessage();
                mimeMessage.From.Add(MailboxAddress.Parse("demoemail031409@gmail.com"));
                mimeMessage.To.Add(MailboxAddress.Parse(email));
                mimeMessage.Subject = subject;
                mimeMessage.Body = new TextPart("html") { Text = message }; // sử dụng "html" cho nội dung HTML

                // Gửi email
                await smtp.SendAsync(mimeMessage);
                smtp.Disconnect(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
                throw;
            }
        }

        public async Task SendRegistrationSuccessEmail(string email, string userType, string name)
        {
            string subject = $"Welcome to RentCarSystem, {userType}!";
            string body = string.Empty;

            // Tùy chỉnh nội dung email cho từng loại người dùng
            if (userType.Equals("customer", StringComparison.OrdinalIgnoreCase))
            {
                body = $@"
                    <div style='font-family: Arial, sans-serif; line-height: 1.5; color: #333;'>
                    <h2 style='color: #007BFF;'>Hello {name},</h2>
                    <p>We are absolutely delighted to welcome you as a <strong>Customer</strong> to RentCarSystem! Your account has been successfully created, and you are now part of our growing family of satisfied customers.</p>
                    <p>At RentCarSystem, we strive to provide the best vehicle rental services tailored to your needs. From luxurious rides to economical options, we’ve got you covered.</p>
                    <p>We encourage you to explore our system, browse through our wide range of vehicles, and take advantage of exclusive deals and discounts just for you.</p>
                    <p>If you ever have questions or need assistance, our dedicated support team is just an email or call away. We’re here to ensure your experience with RentCarSystem is nothing short of amazing.</p>
                    <p>Once again, thank you for choosing us. We look forward to serving you!</p>
                    <p style='margin-top: 20px;'>Warm regards,<br/>
                    <strong>RentCarSystem Team</strong></p>
                    </div>";
            }
            else if (userType.Equals("business", StringComparison.OrdinalIgnoreCase))
            {
                body = $@"
                    <div style='font-family: Arial, sans-serif; line-height: 1.5; color: #333;'>
                    <h2 style='color: #007BFF;'>Hello {name},</h2>
                    <p>Congratulations on successfully registering your <strong>Business</strong> with RentCarSystem! We are thrilled to have you onboard as a valued partner in our mission to make vehicle rentals seamless and efficient.</p>
                    <p>Your account is now active and ready to use. With your business account, you can manage your rentals, streamline operations, and connect with a broad network of customers seeking top-notch services.</p>
                    <p>As part of our commitment to your success, we provide tools and features designed to empower your business. From real-time analytics to marketing insights, we’re here to support your growth.</p>
                    <p>Feel free to log into your dashboard to explore all the functionalities available to you. Should you require any assistance, our dedicated support team is always ready to help.</p>
                    <p>We are excited to embark on this journey with you and look forward to seeing your business thrive with RentCarSystem.</p>
                    <p style='margin-top: 20px;'>Warm regards,<br/>
                    <strong>RentCarSystem Team</strong></p>
                    </div>";
            }
            else if (userType.Equals("individual", StringComparison.OrdinalIgnoreCase))
            {
                body = $@"
                    <div style='font-family: Arial, sans-serif; line-height: 1.5; color: #333;'>
                    <h2 style='color: #007BFF;'>Hello {name},</h2>
                    <p>We are excited to welcome you as an <strong>Individual Service Provider</strong> at RentCarSystem! Your account is now active, and you are ready to make a difference in the lives of our customers.</p>
                    <p>As part of our platform, you can showcase your services, connect with potential customers, and grow your presence in the vehicle rental industry. Your expertise and dedication are highly valued, and we are here to support you every step of the way.</p>
                    <p>To get started, log into your dashboard to manage your profile, track service requests, and ensure a smooth and enjoyable experience for all your clients.</p>
                    <p>If you have any questions or require guidance, do not hesitate to reach out to our support team. We’re here to help you succeed.</p>
                    <p>Thank you for trusting us to be part of your journey. Together, let’s create outstanding experiences for our customers!</p>
                    <p style='margin-top: 20px;'>Warm regards,<br/>
                    <strong>RentCarSystem Team</strong></p>
                    </div>";
            }

            // Gửi email
            await SendEmailAsync(email, subject, body);
        }

        public async Task SendOTP(MailRequestDTO mailRequest)
        {
            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            try
            {
                smtp.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                smtp.Authenticate("demoemail031409@gmail.com", "uzvg apyz jano bciw"); 

                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse("demoemail031409@gmail.com"));
                email.To.Add(MailboxAddress.Parse(mailRequest.Email));
                email.Subject = mailRequest.Subject;
                email.Body = new TextPart("html") { Text = mailRequest.Body };

                await smtp.SendAsync(email);
                smtp.Disconnect(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send OTP email: {ex.Message}");
                throw;
            }
        }

    }
}
