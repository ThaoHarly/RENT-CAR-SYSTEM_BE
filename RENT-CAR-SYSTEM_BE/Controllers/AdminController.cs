using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentCarSystem.Models.Domain;
using RentCarSystem.Models.DTO;
using RentCarSystem.Reponsitories;

namespace RentCarSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Policy = "RequireAdminRole")]
    public class AdminController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IAdminReponsitory adminReponsitory;
        private readonly RentCarSystemContext dbContext;

        public AdminController(IMapper mapper, IAdminReponsitory adminReponsitory, RentCarSystemContext dbContext)
        {
            this.mapper = mapper;
            this.adminReponsitory = adminReponsitory;
            this.dbContext = dbContext;
        }



        //GET :api/Admin/
        [Authorize(Policy = "BusinessWithAcceptStatus")] //test thử authorization, nhớ xóa
        [HttpGet]
        public async Task<IActionResult> getAdmin()
        {
            var adminDomain = await adminReponsitory.GetAdminAsync();
            if(adminDomain == null)
            {
                return NotFound();
            }
            var adminDTO = mapper.Map <List<AdminDTO >> (adminDomain);

            //Map Admin Domain to Admin DTO
            return Ok(adminDTO);
        }


        //UPDATE :api/Admin/{id}
        [HttpPut]
        [Route("{businessId}")]
        public async Task<IActionResult> approvalBusiness([FromRoute] string businessId, [FromBody] ApprovalRequestDTO approvalRequestDTO)
        {
            //Map DTO to Domain Model
            var approvalDomain = mapper.Map<ApprovalRequest>(approvalRequestDTO);

            approvalDomain = await adminReponsitory.UpdateApprovalRequestAsync(businessId, approvalDomain);

            if(approvalDomain == null)
            {
                return NotFound();
            }

            //Admin send Notification 
            var notification = await adminReponsitory.SendNotificationAsync(businessId,approvalRequestDTO.Status);

            //Map notification Domain to NotificationDTO
            var notificationDTO = mapper.Map<NotificationDTO>(notification);

            //Map Approval Domain Model to DTO 
            var ApprovalDTO = mapper.Map<ApprovalRequestDTO>(approvalDomain);

            //Information about the Approval and Notification
            var result = new
            {
                Approval = ApprovalDTO,
                Notification = notificationDTO
            };
                
            return Ok(result);

        }
    }
}
