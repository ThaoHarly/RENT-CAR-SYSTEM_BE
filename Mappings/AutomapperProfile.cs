using AutoMapper;
using RentCarSystem.Models.Domain;
using RentCarSystem.Models.DTO;

namespace RentCarSystem.Mappings
{
    public class AutomapperProfile: Profile
    {
        public AutomapperProfile()
        {
            //Mapping RegisterRequestDTO to User
            CreateMap<RegisterRequestDTO, User>()
                .ForMember(dest => dest.UserId, opt => opt.Ignore()) // Bỏ qua UserId nếu nó tự động tạo
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Nationality, opt => opt.MapFrom(src => src.Nationality))
                .ForMember(dest => dest.Roles, opt => opt.Ignore()); // Bỏ qua ánh xạ Roles vì sẽ gán sau

            // Mapping RegisterRequestDTO to Role
            CreateMap<RegisterRequestDTO, Role>()
                .ForMember(dest => dest.RoleId, opt => opt.Ignore()) // Bỏ qua RoleId nếu nó tự động tạo
                .ForMember(dest => dest.UserId, opt => opt.Ignore()) // Bỏ qua UserId ở đây, sẽ gán sau
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Roles));

            //Map Admin to User
            CreateMap<User, Admin>()
                .ForMember(dest => dest.AdminId, opt => opt.MapFrom(src => src.UserId)) //Map AdminId to UserId
                .ForMember(dest => dest.LastLogin, opt => opt.MapFrom(src => DateTime.Now));

            //Map RegisterRequestDTO to Customer
            CreateMap<RegisterRequestDTO, Customer>()
                .ForMember(dest => dest.LicenseId, opt => opt.MapFrom(src => src.LicenseId))
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.Class, opt => opt.MapFrom(src => src.Class))
                .ForMember(dest => dest.Expire, opt => opt.MapFrom(src => src.Expire))
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Image));

            //Map RegisterRequestDTO to VehicleHireService
            CreateMap<RegisterRequestDTO, VehicleHireService>()
                .ForMember(dest => dest.BankName, opt => opt.MapFrom(src => src.BankName))
                .ForMember(dest => dest.BankAccount, opt => opt.MapFrom(src => src.BankAccount))
                .ForMember(dest => dest.ServiceType, opt => opt.MapFrom(src => src.ServiceType));

            //Map Individual to User
            CreateMap<User, Individual>()
                .ForMember(dest => dest.IdvId, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId));

            //Map Business to DTO
            CreateMap<RegisterRequestDTO, Business>()
                .ForMember(dest => dest.BsnId, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.BusinessImg, opt => opt.MapFrom(src => src.BusinessImg))
                .ForMember(dest => dest.RegistrationDate, opt => opt.MapFrom(src => src.RegistrationDate))
                .ForMember(dest => dest.Vat, opt => opt.MapFrom(src => src.Vat))
                .ForMember(dest => dest.IssuingLocation, opt => opt.MapFrom(src => src.IssuingLocation))
                .ForMember(dest => dest.DateOfIssue, opt => opt.MapFrom(src => src.DateOfIssue));

            //Map Role to RoleDTO
            CreateMap<Role, RoleDTO>();

            //Map Admin to AdminDTO
            CreateMap<Admin, AdminDTO>()
            .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.AdminNavigation));

            CreateMap<Admin, AdminDTO>();

            //Map User to UserDTO
            CreateMap<User, UserDTO>();

            //Map Customer to CustomerDTO
            CreateMap<Customer, CustomerDTO>();

            //Map VehicleHireService to ServiceDTO
            CreateMap<VehicleHireService, ServiceDTO>();

            //Map Individual to IndividualDTO
            CreateMap<Individual, IndividualDTO>();

            //Map Business to BusinessDTO
            CreateMap<Business, BusinessDTO>();

            //Map ApprovalRequest to ApprovalRequestDTO

            CreateMap<ApprovalRequestDTO, ApprovalRequest>().ReverseMap(); //Map ngược


            CreateMap<ApprovalRequestDTO, ApprovalRequest>().ReverseMap(); //Map ngược


            CreateMap<ApprovalRequest, ApprovalRequestDTO>();

            //Map Notification to NotificationDTO
            CreateMap<Notification, NotificationDTO>();

        }
    }
}
