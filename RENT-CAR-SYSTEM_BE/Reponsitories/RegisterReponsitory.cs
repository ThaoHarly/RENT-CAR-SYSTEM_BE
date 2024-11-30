using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RentCarSystem.Migrations.Data;
using RentCarSystem.Models.Domain;
using RentCarSystem.Models.DTO;
using System.ComponentModel;

namespace RentCarSystem.Reponsitories
{
    public class RegisterReponsitory : IRegisterReponsitory
    {

        private readonly UserManager<IdentityUser> userManager;
        private readonly ITokenReponsitory tokenReponsitory;
        private readonly RentCarSystemContext dbContext;
        private readonly IEmailSender emailSender;

        public RegisterReponsitory(UserManager<IdentityUser> userManager, ITokenReponsitory tokenReponsitory, RentCarSystemContext dbContext, IEmailSender emailSender)
        {
            this.userManager = userManager;
            this.tokenReponsitory = tokenReponsitory;
            this.dbContext = dbContext;
            this.emailSender = emailSender;
        }

        public async Task<User> RegisterUser(User user, string role)
        {
            //Check if Admin not exist or role different admin
            if ((await checkAdminExisting() && role.ToUpper().Equals("ADMIN")))
            {
                throw new Exception("Only one admin existing ....");
            }
            //tạo 1 identify user
            var identityUser = new IdentityUser
            {
                UserName = user.Email,
            };

            //Password Encryption use PBKDF2 algorithm
            var passwordHasher = new PasswordHasher<IdentityUser>();
            user.Password = passwordHasher.HashPassword(identityUser, user.Password);

            // Lưu người dùng với trạng thái chưa xác minh
            user.IsVerified = false;

            await dbContext.Users.AddAsync(user);
            await dbContext.SaveChangesAsync();

            // Tạo OTP và lưu vào bảng OTPRequests
            string otp = OTPGenerator();
            var otpRequest = new Otprequest
            {
                UserId = user.UserId,
                OTP = otp,
                ExpiryDate = DateTime.UtcNow.AddMinutes(5) // đặt otp có thời hạn là 5p
            };
            await dbContext.Otprequests.AddAsync(otpRequest);
            await dbContext.SaveChangesAsync();


            // Gửi OTP qua email
            try
            {
                await SendOTPMail(user.Email, otp, user.Name, user.UserId);
                Console.WriteLine("OTP email sent successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send OTP email: {ex.Message}");
                throw;
            }
            return user;
        }

        public async Task<Admin> RegisterAdmin(Admin admin)
        {
            await dbContext.Admins.AddAsync(admin);
            await dbContext.SaveChangesAsync();
            return admin;
        }

        public async Task<Customer> RegisterCustomer(Customer customer)
        {
            await dbContext.Customers.AddAsync(customer);
            await dbContext.SaveChangesAsync();
            return customer;
        }

        public async Task<VehicleHireService> RegisterService(VehicleHireService vehicleHireService)
        {
            await dbContext.VehicleHireServices.AddAsync(vehicleHireService);
            await dbContext.SaveChangesAsync();
            return vehicleHireService;
        }
        public async Task<Individual> RegisterIndividual(Individual individual)
        {
            await dbContext.Individuals.AddAsync(individual);
            await dbContext.SaveChangesAsync();
            return individual;
        }

        public async Task<Business> RegisterBusiness(Business business)
        {
            await dbContext.Businesses.AddAsync(business);
            await dbContext.SaveChangesAsync();
            return business;
        }

        public async Task<Role> RegisterRole(Role role)
        {
            await dbContext.Roles.AddRangeAsync(role);
            await dbContext.SaveChangesAsync();
            return role;
        }

        public async Task<ApprovalRequest> RegisterApprovalRequest(ApprovalRequest approvalRequest)
        {
            await dbContext.ApprovalRequests.AddAsync(approvalRequest);
            await dbContext.SaveChangesAsync();
            return approvalRequest;
        }

        public async Task<Notification> CreateNotification(Notification notification)
        {
            await dbContext.Notifications.AddAsync(notification);
            await dbContext.SaveChangesAsync();
            return notification;
        }

        public async Task<bool> checkAdminExisting()
        {
            //Check if Admin exist
            return await dbContext.Users
                            .AnyAsync(u => u.Roles.Any(r => r.Type.ToUpper() == "ADMIN"));
        }

        public string OTPGenerator()
        {
            Random random = new Random();
            string otp = random.Next(100000, 999999).ToString("D6"); // otp 6 chữ số
            return otp;
        }

        public string GenerateEmailBody(string name, string otpText, string userId)
        {
            return $@"
                <div style='font-family: Arial, sans-serif; line-height: 1.5;'>
                <h2>Hello {name},</h2>
                <p>Thank you for registering with our system. Your user ID is: <strong>{userId}</strong></p>
                <p>Your OTP is:</p>
                <h1 style='color: blue;'>{otpText}</h1>
                <p>Please use this OTP to complete your registration. This OTP is valid for 5 minutes.</p>
                <p>If you did not initiate this registration, please ignore this email.</p>
                <p>Best regards,<br/>RentCarSystem Team</p>
                </div>";
        }



        public async Task SendOTPMail(string useremail, string otpText, string name, string userId)
        {
            Console.WriteLine($"Sending OTP to {useremail}, OTP: {otpText}, Name: {name}, UserId: {userId}");

            var mailRequest = new MailRequestDTO
            {
                Email = useremail,
                Subject = "Your OTP for Registration",
                Body = GenerateEmailBody(name, otpText, userId)
            };

            try
            {
                await emailSender.SendOTP(mailRequest);
                Console.WriteLine("OTP email sent successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send OTP email: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> VerifyOTP(string userId, string otpInput, string userType)
        {
            // Kiểm tra OTP tồn tại và chưa hết hạn
            var otpRequest = await dbContext.Otprequests.FirstOrDefaultAsync(r => r.UserId == userId && r.OTP == otpInput);

            if (otpRequest == null || otpRequest.ExpiryDate < DateTime.UtcNow)
            {
                return false; // OTP không hợp lệ hoặc đã hết hạn
            }

            // OTP hợp lệ, cập nhật người dùng thành đã xác minh
            var user = await dbContext.Users.FirstOrDefaultAsync(u => u.UserId == userId);
            if (user != null)
            {
                user.IsVerified = true;
                await dbContext.SaveChangesAsync();

                // Gửi thông báo đăng ký thành công
                await emailSender.SendRegistrationSuccessEmail(user.Email, userType, user.Name);
            }

            // Xóa OTP đã xác minh
            dbContext.Otprequests.Remove(otpRequest);
            await dbContext.SaveChangesAsync();

            return true;
        }

    }
}
