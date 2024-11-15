using Microsoft.AspNetCore.Authorization;
using RentCarSystem.Authorization;
using System.Security.Claims;

public class BusinessApprovalHandler : AuthorizationHandler<BusinessApprovalRequirement>
{
    private readonly RentCarSystemContext _context;

    public BusinessApprovalHandler(RentCarSystemContext context)
    {
       _context = context;
    }

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, BusinessApprovalRequirement requirement)
    {
        var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId == null)
        {
            return Task.CompletedTask;
        }

        // Kiểm tra loại dịch vụ của người dùng
        var service = _context.VehicleHireServices.FirstOrDefault(s => s.UserId == userId);
        if (service != null && service.ServiceType.ToUpper() == "BUSINESS")
        {
            // Kiểm tra trạng thái chấp thuận
            var business = _context.Businesses.FirstOrDefault(b => b.UserId == userId);
            if (business != null)
            {
                var approval = _context.ApprovalRequests
                    .FirstOrDefault(a => a.BsnId == business.BsnId && a.Status.ToUpper() == "ACCEPT");

                if (approval != null)
                {
                    context.Succeed(requirement);
                }
            }
        }
        else if (service != null && service.ServiceType.ToUpper() == "INDIVIDUAL")
        {
            // Người dùng loại Individual không cần kiểm tra thêm
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}
