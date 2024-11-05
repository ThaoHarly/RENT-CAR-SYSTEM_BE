using AutoMapper;
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

        public AuthController(ITokenReponsitory tokenReponsitory, RentCarSystemContext dbContext, IMapper mapper, IRegisterReponsitory registerReponsitory)
        {
            this.tokenReponsitory = tokenReponsitory;
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.registerReponsitory = registerReponsitory;
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
                await registerReponsitory.RegisterUser(userDomain,registerRequestDTO.Password);

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
            //Map registerRequestDTO to VehicleHireService
            var serviceDomain = mapper.Map<VehicleHireService>(registerRequestDTO);

            //get UserId
            serviceDomain.UserId = userDomain.UserId;
            await registerReponsitory.RegisterService(serviceDomain);

            if (registerRequestDTO.ServiceType.ToUpper().Equals("INDIVIDUAL"))
            {
                //Map Individual to User get UserId
                var individualDomain = mapper.Map<Individual>(userDomain);

                await registerReponsitory.RegisterIndividual(individualDomain);

                //Map userDomain to UserDTO
                var userDTO = mapper.Map<UserDTO>(userDomain);

                //Map RoleDomain to RoleDTO
                var roleDTO = mapper.Map<RoleDTO>(roleDomain);

                //Map VehicleHireService to ServiceDTO
                var serviceDTO = mapper.Map<ServiceDTO>(serviceDomain);

                //Map IndividualDomain to IndividualDTO
                var individualDTO = mapper.Map<IndividualDTO>(individualDomain);

                //Information about the Individual
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


                //Get AdminId
                var admId = await dbContext.Users
                    .Where(u => u.Roles.Any(r => r.Type.ToUpper() == "ADMIN"))
                    .Select(u => u.UserId.ToString())
                    .FirstOrDefaultAsync();

                // Create ApprovalRequest
                var approvalRequest = new ApprovalRequest
                {
                    //admin id
                    AdminId = Guid.Parse(admId), //convert string to Guid
                    BsnId = businessDomain.BsnId,
                    RequestDay = DateOnly.FromDateTime(DateTime.Now),
                    Status = "PENDING"
                };
                await registerReponsitory.RegisterApprovalRequest(approvalRequest);


                //Create Notification
                var notificationDefault = new Notification
                {
                    SenderId = userDomain.UserId,
                    ReceiverId = Guid.Parse(admId),
                    Message = "New Business Registration requires approval.",
                    NotificationDate = DateOnly.FromDateTime(DateTime.Now)
                };
                await registerReponsitory.CreateNotification(notificationDefault);


                //Map userDomain to UserDTO
                var userDTO = mapper.Map<UserDTO>(userDomain);

                //Map RoleDomain to RoleDTO
                var roleDTO = mapper.Map<RoleDTO>(roleDomain);

                //Map VehicleHireService to ServiceDTO
                var serviceDTO = mapper.Map<ServiceDTO>(serviceDomain);

                //Map BusinessDomain to BusinessDTO
                var businessDTO = mapper.Map<BusinessDTO>(businessDomain);

                //Map ApprovalRequestDomain to ApprovalRequestDTO
                var approvalRequestDomain = mapper.Map<ApprovalRequestDTO>(approvalRequest);

                //Map NotificationDomain to NotificationDTO
                var notificationDTO = mapper.Map<NotificationDTO>(notificationDefault);

                //Information about the Business
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
