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


            //Map Notification to NotificationDTO
            CreateMap<Notification, NotificationDTO>();


            //Map AddVehicleServiceDTO to Vehicle
            CreateMap<AddVehicleServiceDTO, Vehicle>()
                .ForMember(dest => dest.VehicleId, opt => opt.Ignore()) // tự động sinh
                .ForMember(dest => dest.UserId, opt => opt.Ignore()) // gán lúc đăng nhập
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category))
                .ForMember(dest => dest.LicensePlate, opt => opt.MapFrom(src => src.LicensePlate))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => "AVAILABILITY")) //Set default = "Availability"
                .ForMember(dest => dest.PricePerDay, opt => opt.MapFrom(src => src.PricePerDay))
                .ForMember(dest => dest.FuelConsumption, opt => opt.MapFrom(src => src.FuelConsumption))
                .ForMember(dest => dest.Range, opt => opt.MapFrom(src => src.Range))
                .ForMember(dest => dest.EngineCapacity, opt => opt.MapFrom(src => src.EngineCapacity));

            //Map AddVehicleServiceDTO to Motor
            CreateMap<AddVehicleServiceDTO, Motor>()
                .ForMember(dest => dest.MotorId, opt => opt.Ignore()) // tự động sinh
                .ForMember(dest => dest.VehicleId, opt => opt.Ignore()) // gán sau
                .ForMember(dest => dest.MotorImage, opt => opt.MapFrom(src => src.MotorImage))
                .ReverseMap();

            //Map Vehicle to VehicleDTO
            CreateMap<Vehicle, VehicleDTO>().ReverseMap();

            //Map Motor to MotorDTO
            CreateMap<Motor,MotorDTO>().ReverseMap();

            //Map AddVehicleServiceDTO to Car
            CreateMap<AddVehicleServiceDTO, Car>()
               .ForMember(dest => dest.CarId, opt => opt.Ignore()) // tự động sinh
               .ForMember(dest => dest.VehicleId, opt => opt.Ignore()) // gán sau
               .ForMember(dest => dest.CarBrand, opt => opt.MapFrom(src => src.CarBrand))
               .ForMember(dest => dest.FuelType, opt => opt.MapFrom(src => src.FuelType))
               .ForMember(dest => dest.SeatingCapacity, opt => opt.MapFrom(src => src.SeatingCapacity))
               .ForMember(dest => dest.CarImage, opt => opt.MapFrom(src => src.CarImage))
               .ForMember(dest => dest.ChargingTime, opt => opt.MapFrom(src => src.ChargingTime))
               .ReverseMap();

            //Map Car to CarDTO
            CreateMap<Car,CarDTO>().ReverseMap();


            //Map UpdateVehicleRequestDTO to Vehicle
            CreateMap<UpdateVehicleRequestDTO, Vehicle>()
                .ForMember(dest => dest.VehicleId, opt => opt.Ignore()) // Bo qua
                .ForMember(dest => dest.UserId, opt => opt.Ignore()) // Bo qua
                .ForMember(dest => dest.Category, opt => opt.Ignore()) //Bo qua
                .ForMember(dest => dest.LicensePlate, opt => opt.MapFrom(src => src.LicensePlate))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.status)) 
                .ForMember(dest => dest.PricePerDay, opt => opt.MapFrom(src => src.PricePerDay))
                .ForMember(dest => dest.FuelConsumption, opt => opt.MapFrom(src => src.FuelConsumption))
                .ForMember(dest => dest.Range, opt => opt.MapFrom(src => src.Range))
                .ForMember(dest => dest.EngineCapacity, opt => opt.MapFrom(src => src.EngineCapacity));

            //Map UpdateVehicleRequestDTO to Motor
            CreateMap<UpdateVehicleRequestDTO, Motor>()
                .ForMember(dest => dest.MotorId, opt => opt.Ignore()) // bo qua
                .ForMember(dest => dest.VehicleId, opt => opt.Ignore()) // bo qua
                .ForMember(dest => dest.MotorImage, opt => opt.MapFrom(src => src.MotorImage))
                .ReverseMap();

            //Map UpdateVehicleRequestDTO to Car
            CreateMap<UpdateVehicleRequestDTO, Car>()
               .ForMember(dest => dest.CarId, opt => opt.Ignore()) // bo qua
               .ForMember(dest => dest.VehicleId, opt => opt.Ignore()) // bo qua
               .ForMember(dest => dest.CarBrand, opt => opt.MapFrom(src => src.CarBrand))
               .ForMember(dest => dest.FuelType, opt => opt.MapFrom(src => src.FuelType))
               .ForMember(dest => dest.SeatingCapacity, opt => opt.MapFrom(src => src.SeatingCapacity))
               .ForMember(dest => dest.CarImage, opt => opt.MapFrom(src => src.CarImage))
               .ForMember(dest => dest.ChargingTime, opt => opt.MapFrom(src => src.ChargingTime))
               .ReverseMap();

        }
    }
}
