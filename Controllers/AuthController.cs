using AutoMapper;
using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentCarSystem.Migrations.Data;
using RentCarSystem.Models.Domain;
using RentCarSystem.Models.DTO;
using RentCarSystem.Reponsitories;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using BCrypt.Net;

namespace RentCarSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ITokenReponsitory tokenReponsitory;
        private readonly RentCarSystemContext dbContext;
        private readonly IMapper mapper;
        private readonly IRegisterReponsitory registerReponsitory;
        private readonly ILoginReponsitory loginReponsitory;
        private readonly IEmailSender emailSender;
        private readonly IResetPassWord resetPassWord;
        private readonly IPasswordHasher<User> passwordHasher;

        public AuthController(ITokenReponsitory tokenReponsitory, RentCarSystemContext dbContext, IMapper mapper, IRegisterReponsitory registerReponsitory, ILoginReponsitory loginReponsitory, IEmailSender emailSender,IResetPassWord resetPassWord)
        { 
            this.tokenReponsitory = tokenReponsitory;
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.registerReponsitory = registerReponsitory;
            this.loginReponsitory = loginReponsitory;
            this.emailSender = emailSender;
            this.resetPassWord = resetPassWord;
            this.passwordHasher = passwordHasher;
        }
    

        //POST: /api/Auth/Register
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO registerRequestDTO)
        {
            var role = registerRequestDTO.Roles.ToUpper();
            if (role == "ADMIN" || role == "CUSTOMER" || role == "SERVICE")
            {


                //Map DTO  to Domain User
                var userDomain = mapper.Map<User>(registerRequestDTO); 

                await registerReponsitory.RegisterUser(userDomain,registerRequestDTO.Password, registerRequestDTO.Roles);


                // Map DTO to Role and get UserId
                var roleDomain = mapper.Map<Role>(registerRequestDTO);
                roleDomain.UserId = userDomain.UserId;
                await registerReponsitory.RegisterRole(roleDomain);

                //Map UserDTO to User and RoleDTO to Role 

                return role switch
                {
                    "ADMIN" => await RegisterAdmin(userDomain,roleDomain),
                    "CUSTOMER" => await RegisterCustomer(userDomain, registerRequestDTO,roleDomain),
                    "SERVICE" => await RegisterService(registerRequestDTO, userDomain, roleDomain),
                    _ => BadRequest("Invalid role")
                };
            }
            return BadRequest("Something went wrong!");
        }

        [HttpPost]
        [Route("VerifyOTP")]
        public async Task<IActionResult> VerifyOTP([FromBody] OTPVerificationRequestDTO request)
        {
            // Log thông tin để kiểm tra
            Console.WriteLine($"Verifying OTP for UserId: {request.UserId}, OTP: {request.OTP}, UserType: {request.UserType}");

            // Gọi repository để xác minh OTP
            var isOtpValid = await registerReponsitory.VerifyOTP(request.UserId, request.OTP, request.UserType);

            if (!isOtpValid)
            {
                return BadRequest("Invalid or expired OTP. Please try again.");
            }

            return Ok("OTP verified successfully. Registration is complete.");
        }

        //[HttpGet]
        //[Authorize]
        //public async Task<IActionResult> GetUserInfo()
        //{
        //    // Lấy UserId từ claim trong token
        //    var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)  ;
        //    if (userIdClaim == null)
        //    {
        //        return Unauthorized("Không thể xác định thông tin người dùng từ token.");
        //    }

        //    var userId = userIdClaim.Value;

        //    // Truy vấn thông tin người dùng dựa trên UserId
        //    var user = await dbContext.Users
        //                              .Include(u => u.Roles)
        //                              .FirstOrDefaultAsync(u => u.UserId.ToString() == userId);

        //    if (user == null)
        //    {
        //        return NotFound("Không tìm thấy người dùng.");
        //    }

        //    // Lấy thông tin vai trò từ đối tượng User
        //    var roles = user.Roles.Select(r => r.Type).ToList();

        //    var userInfo = mapper.Map<UserDTO>(user);

        //    // Lấy thông tin 
        //    if(roles.Any(x => x.Equals("Customer", StringComparison.OrdinalIgnoreCase)))
        //    {
        //        var customerInfor = await dbContext.Customers.FirstOrDefaultAsync(x => x.UserId == userId);
        //        if (customerInfor == null)
        //        {
        //            return NotFound("Không tìm thấy thông tin khách hàng.");
        //        }

        //        return Ok(new
        //        {
        //            User = userInfo,
        //            CustomerDTO = new
        //            {
        //                LicenseId = customerInfor.LicenseId,
        //                UserId = customerInfor.UserId,
        //                Class = customerInfor.Class,
        //                Expire = customerInfor.Expire,
        //                Image = customerInfor.Image,
        //                Bills = new List<object>(), // Giả sử có thể lấy danh sách hóa đơn từ database
        //                RentalAgreements = new List<object>(), // Giả sử có thể lấy danh sách hợp đồng cho thuê từ database
        //                Reviews = new List<object>() // Giả sử có thể lấy danh sách đánh giá từ database
        //            }
        //        });
        //    }
        //    else if(roles.Any(x => x.Equals("Service", StringComparison.OrdinalIgnoreCase)))
        //    {
        //        // Lấy thông tin ServiceType từ bảng VehicleHireServices
        //        var vehicleHireService = await dbContext.VehicleHireServices.FirstOrDefaultAsync(x => x.UserId == user.UserId);
        //        if(vehicleHireService != null)
        //        {
        //            if(vehicleHireService.ServiceType.Equals("Business",StringComparison.OrdinalIgnoreCase))
        //            {
        //                var businessInfor = await dbContext.Businesses.FirstOrDefaultAsync(x => x.UserId.ToString() == userId);
        //                return Ok(new
        //                {
        //                    User = userInfo,
        //                    BusinessDTO = new
        //                    {
        //                        BsnId = businessInfor.BsnId,
        //                        Description = businessInfor.Description,
        //                        BusinessImg = businessInfor.BusinessImg,
        //                        RegistrationDate = businessInfor.RegistrationDate,
        //                        Vat = businessInfor.Vat,
        //                        IssuingLocation = businessInfor.IssuingLocation,
        //                        DateOfIssue = businessInfor.DateOfIssue
        //                    },
        //                    VehicleHireService = new
        //                    {
        //                        UserId = vehicleHireService.UserId,
        //                        ServiceType = vehicleHireService.ServiceType
        //                    }
        //                });
        //            }
        //            else if (vehicleHireService.ServiceType.Equals("Individual", StringComparison.OrdinalIgnoreCase))
        //            {
        //                var individualInfo = await dbContext.Individuals.FirstOrDefaultAsync(i => i.UserId == user.UserId);
        //                return Ok(new
        //                {
        //                    User = userInfo,
        //                    IndividualDTO = new
        //                    {
        //                        IdvId = individualInfo.IdvId,
        //                        UserId = individualInfo.UserId,
        //                        ServiceType = vehicleHireService.ServiceType
        //                    },
        //                    VehicleHireService = new
        //                    {
        //                        UserId = vehicleHireService.UserId,
        //                        ServiceType = vehicleHireService.ServiceType
        //                    }
        //                });
        //            }
        //            else
        //            {
        //                return Ok(new { User = userInfo, Message = "Không xác định được loại dịch vụ." });
        //            }
        //        }
        //        else
        //        {
        //            return NotFound("Không tìm thấy");
        //        }
        //    }

        //    // Nếu không có cái nào khớp
        //    return Ok(new { User = userInfo });
        //}




        //POST: /api/Auth/Login
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequestDTO)
        {
            // Tìm người dùng dựa trên email
            var user = await dbContext.Users
                .Include(u => u.Roles)
                .SingleOrDefaultAsync(u => u.Email == loginRequestDTO.email);

            if (user == null)
            {
                return Unauthorized("Người dùng không tồn tại.");
            }

            // Kiểm tra mật khẩu
            var isPasswordValid = await loginReponsitory.VerifyPassword(user, loginRequestDTO.password);

            if (!isPasswordValid)
            {
                return Unauthorized("Mật khẩu không đúng.");
            }

            // Lấy vai trò của người dùng
            var roles = user.Roles.Select(r => r.Type).ToList();
            if (!roles.Any())
            {
                return Unauthorized("Người dùng không có vai trò nào được gán.");
            }

            // Tạo JWT token
            var jwtToken = tokenReponsitory.CreateJWTToken(user, roles);

            // Lấy thông tin chi tiết dựa trên vai trò
            var userInfo = mapper.Map<UserDTO>(user);

            if (roles.Any(x => x.Equals("Customer", StringComparison.OrdinalIgnoreCase)))
            {
                var customerInfo = await dbContext.Customers.FirstOrDefaultAsync(x => x.UserId == user.UserId);
                if (customerInfo == null)
                {
                    return NotFound("Không tìm thấy thông tin khách hàng.");
                }

                return Ok(new
                {
                    Token = jwtToken,
                    User = userInfo,
                    CustomerDTO = new
                    {
                        LicenseId = customerInfo.LicenseId,
                        UserId = customerInfo.UserId,
                        Class = customerInfo.Class,
                        Expire = customerInfo.Expire,
                        Image = customerInfo.Image
                    }
                });
            }
            else if (roles.Any(x => x.Equals("Service", StringComparison.OrdinalIgnoreCase)))
            {
                var vehicleHireService = await dbContext.VehicleHireServices.FirstOrDefaultAsync(x => x.UserId == user.UserId);
                if (vehicleHireService != null)
                {
                    if (vehicleHireService.ServiceType.Equals("Business", StringComparison.OrdinalIgnoreCase))
                    {
                        var businessInfo = await dbContext.Businesses.FirstOrDefaultAsync(x => x.UserId.ToString() == user.UserId.ToString());
                        return Ok(new
                        {
                            Token = jwtToken,
                            User = userInfo,
                            BusinessDTO = new
                            {
                                BsnId = businessInfo.BsnId,
                                Description = businessInfo.Description,
                                BusinessImg = businessInfo.BusinessImg,
                                RegistrationDate = businessInfo.RegistrationDate,
                                Vat = businessInfo.Vat,
                                IssuingLocation = businessInfo.IssuingLocation,
                                DateOfIssue = businessInfo.DateOfIssue
                            }
                        });
                    }
                    else if (vehicleHireService.ServiceType.Equals("Individual", StringComparison.OrdinalIgnoreCase))
                    {
                        var individualInfo = await dbContext.Individuals.FirstOrDefaultAsync(i => i.UserId == user.UserId);
                        return Ok(new
                        {
                            Token = jwtToken,
                            User = userInfo,
                            IndividualDTO = new
                            {
                                IdvId = individualInfo.IdvId,
                                UserId = individualInfo.UserId,
                                ServiceType = vehicleHireService.ServiceType
                            }
                        });
                    }
                }
                return NotFound("Không tìm thấy thông tin dịch vụ.");
            }
            else if (roles.Any(x => x.Equals("Admin", StringComparison.OrdinalIgnoreCase)))
            {
                var AdminInfo = await dbContext.Admins.FirstOrDefaultAsync(i => i.AdminId == user.UserId);
                return Ok(new
                {
                    Token = jwtToken,
                    User = userInfo,
                    AdminDTO = new
                    {
                        AdminId = user.UserId,
                        Permissions = AdminInfo.LastLogin // Hoặc thêm thông tin khác nếu cần
                    }
                });
            }

            // Nếu không khớp vai trò nào
            return Ok(new
            {
                Token = jwtToken,
                User = userInfo,
                Message = "Vai trò không được hỗ trợ."
            });
        }

        [HttpPost]
        [Route("SendLinkForgotPassword")]
        public async Task<IActionResult> SendLinkForgotPassword([FromBody] ForgotPassWordRequestDTO requestDTO)
        {
            return Ok(await resetPassWord.CreatePasswordResetRequestAsync(requestDTO.Email));        
        }

        [HttpPut]
        [Route("ResetForgotPassword")]
        public async Task<IActionResult> ResetForgotPassword([FromBody] ResetPassWordDTO resetPassWordDTO)
        {
            return Ok(await resetPassWord.ResetPasswordAsync(resetPassWordDTO.Token, resetPassWordDTO.NewPassword));
        }

        [HttpPut]
        [Route("ChangePassword")]
        public async Task<IActionResult> ChangePassword( string newPass,string curPass)
        {
            //Get userId is logining
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return BadRequest("Vui lòng đăng nhập để đổi mật khẩu");
            }

            return Ok(await resetPassWord.ChangePasswordAsync(newPass, curPass, userId));
        } 


        //[HttpGet("generate-otp")]
        //public string OTPGenerator()
        //{
        //    Random random = new Random();
        //    string otp = random.Next(100000, 999999).ToString("D6"); // otp 6 chữ số
        //    return otp;
        //}

        //[HttpPost("generate-email-body")]
        //public string GenerateEmailBody(string name, string otptext)
        //{
        //    string emailbody = string.Empty;
        //    emailbody = "<div style = 'width:100%; background-color:grey'>";
        //    emailbody += "<h1> Hi " + name + ", Thanks for registering</h1>";
        //    emailbody += "<h2> Please enter OTP text and complete the registeration<h2>";
        //    emailbody += "<h2> OTP Text is: " + otptext + "<h2>";
        //    emailbody += "<div>";

        //    return emailbody;
        //}

        //[HttpPost("send-otp-email")]
        //public async Task SendOTPMail(string useremail, string OtpText, string Name)
        //{
        //    var mailrequest = new MailRequestDTO();
        //    mailrequest.Email = useremail;
        //    mailrequest.Subject = "Thanks for registering: OTP";
        //    mailrequest.Body = GenerateEmailBody(Name, OtpText);
        //    await this.emailSender.SendOTP(mailrequest);
        //}


        private async Task<IActionResult> RegisterAdmin(User userDomain, Role roleDomain)
        {
            //Map Admin to User
            var adminDomain = mapper.Map<Admin>(userDomain);
            await registerReponsitory.RegisterAdmin(adminDomain);

            //Map RoleDomain to RoleDTO
            var roleDTO = mapper.Map<RoleDTO>(roleDomain);

            //Map userDomain to UserDTO
            var userDTO = mapper.Map<UserDTO>(userDomain);

            //Map Admin to Admin DTO
            var adminDTO = mapper.Map<AdminDTO>(adminDomain);

            //Information about the Admin
            var result = new
            {
                User = userDTO,
                Admin = adminDTO,
                Role = roleDTO
            }; 
            return Ok(result);
        }

        private async Task<IActionResult> RegisterCustomer(User userDomain, RegisterRequestDTO registerRequestDTO, Role roleDomain)
        {
            if (registerRequestDTO.LicenseId.Length == 12)
            {
                //Map DTO to Customer
                var customerDomain = mapper.Map<Customer>(registerRequestDTO);
                //get UserId
                customerDomain.UserId = userDomain.UserId;

                await registerReponsitory.RegisterCustomer(customerDomain);

                //Map UserDomain to UserDTO
                var userDTO = mapper.Map<UserDTO>(userDomain);

                //Map Role to RoleDTO
                var roleDTO = mapper.Map<RoleDTO>(roleDomain);

                //Map Customer to CustomerDTO
                var customerDTO = mapper.Map<CustomerDTO>(customerDomain);

                //Information about the Customer
                var result = new
                {
                    User = userDTO,
                    Customer = customerDTO,
                    Role = roleDTO
                };

                return Ok(result);
            }
            return BadRequest("LicenseId length is must 12 character !");
        }

        private async Task<IActionResult> RegisterService(RegisterRequestDTO registerRequestDTO, User userDomain, Role roleDomain)
        {
            // Map registerRequestDTO to VehicleHireService
            var serviceDomain = mapper.Map<VehicleHireService>(registerRequestDTO);

            // get UserId
            serviceDomain.UserId = userDomain.UserId;
            await registerReponsitory.RegisterService(serviceDomain);

            if (registerRequestDTO.ServiceType.ToUpper().Equals("INDIVIDUAL"))
            {
                // Map Individual to User get UserId
                var individualDomain = mapper.Map<Individual>(userDomain);

                await registerReponsitory.RegisterIndividual(individualDomain);

                // Map userDomain to UserDTO
                var userDTO = mapper.Map<UserDTO>(userDomain);

                // Map RoleDomain to RoleDTO
                var roleDTO = mapper.Map<RoleDTO>(roleDomain);

                // Map VehicleHireService to ServiceDTO
                var serviceDTO = mapper.Map<ServiceDTO>(serviceDomain);

                // Map IndividualDomain to IndividualDTO
                var individualDTO = mapper.Map<IndividualDTO>(individualDomain);

                // Thông tin trả về nhưng chưa gửi email
                var result = new
                {
                    User = userDTO,
                    Role = roleDTO,
                    Service = serviceDTO,
                    Individual = individualDTO
                };

                return Ok(result);
            }
            else if (registerRequestDTO.ServiceType.ToUpper().Equals("BUSINESS"))
            {
                // Map DTO to Business
                var businessDomain = mapper.Map<Business>(registerRequestDTO);
                businessDomain.UserId = userDomain.UserId;
                var business = await registerReponsitory.RegisterBusiness(businessDomain);

                // Get AdminId
                var admId = await dbContext.Users
                    .Where(u => u.Roles.Any(r => r.Type.ToUpper() == "ADMIN"))
                    .Select(u => u.UserId.ToString())
                    .FirstOrDefaultAsync();

                // Create ApprovalRequest
                var approvalRequest = new ApprovalRequest
                {
                    AdminId = admId,
                    BsnId = businessDomain.BsnId,
                    RequestDay = DateOnly.FromDateTime(DateTime.Now),
                    Status = "PENDING"
                };
                await registerReponsitory.RegisterApprovalRequest(approvalRequest);

                // Create Notification
                var notificationDefault = new Notification
                {
                    SenderId = userDomain.UserId,
                    ReceiverId = admId,
                    Message = "New Business Registration requires approval.",
                    NotificationDate = DateOnly.FromDateTime(DateTime.Now)
                };
                await registerReponsitory.CreateNotification(notificationDefault);

                // Map userDomain to UserDTO
                var userDTO = mapper.Map<UserDTO>(userDomain);

                // Map RoleDomain to RoleDTO
                var roleDTO = mapper.Map<RoleDTO>(roleDomain);

                // Map VehicleHireService to ServiceDTO
                var serviceDTO = mapper.Map<ServiceDTO>(serviceDomain);

                // Map BusinessDomain to BusinessDTO
                var businessDTO = mapper.Map<BusinessDTO>(businessDomain);

                // Map ApprovalRequestDomain to ApprovalRequestDTO
                var approvalRequestDomain = mapper.Map<ApprovalRequestDTO>(approvalRequest);

                // Map NotificationDomain to NotificationDTO
                var notificationDTO = mapper.Map<NotificationDTO>(notificationDefault);

                // Thông tin trả về nhưng chưa gửi email
                var result = new
                {
                    User = userDTO,
                    Role = roleDTO,
                    Service = serviceDTO,
                    Business = businessDTO,
                    ApprovalRequest = approvalRequestDomain,
                    Notification = notificationDTO
                };

                return Ok(result);
            }

            return BadRequest();
        }

    }
}
