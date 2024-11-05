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
<<<<<<< HEAD
        private readonly ITokenReponsitory tokenReponsitory;
        private readonly RentCarSystemContext dbContext;

        public RegisterReponsitory( ITokenReponsitory tokenReponsitory, RentCarSystemContext dbContext)
        {
=======
        private readonly UserManager<IdentityUser> userManager;
        private readonly ITokenReponsitory tokenReponsitory;
        private readonly RentCarSystemContext dbContext;

        public RegisterReponsitory(UserManager<IdentityUser> userManager, ITokenReponsitory tokenReponsitory, RentCarSystemContext dbContext)
        {
            this.userManager = userManager;
>>>>>>> 245adf983c80f561f0c244ccf5e507c9b3b495e7
            this.tokenReponsitory = tokenReponsitory;
            this.dbContext = dbContext;
        }

<<<<<<< HEAD
        public async Task<User> RegisterUser(User user, string rawPassword, string role)
        {
            //Check if Admin not exist or role different admin
            if ((await checkAdminExisting() && role.ToUpper().Equals("ADMIN")))
            {
                throw new Exception("Only admin existing ....");
            }
            //user
            var identityUser = new IdentityUser
            {
                UserName = user.PhoneNumber
            };

            //Password Encryption use PBKDF2 algorithm
            var passwordHasher = new PasswordHasher<IdentityUser>();
            user.Password = passwordHasher.HashPassword(identityUser, user.Password);

            await dbContext.Users.AddAsync(user);
            await dbContext.SaveChangesAsync();
            return user;
=======
        public async Task<User> RegisterUser(User user, string rawPassword)
        {
            //Check if Admin not exist
            if (!await checkAdminExisting())
            {
                //user
                var identityUser = new IdentityUser
                {
                    UserName = user.PhoneNumber
                };

                //Password Encryption use PBKDF2 algorithm
                var passwordHasher = new PasswordHasher<IdentityUser>();
                user.Password = passwordHasher.HashPassword(identityUser, user.Password);

                await dbContext.Users.AddAsync(user);
                await dbContext.SaveChangesAsync();
                return user;
            }
            return null;
>>>>>>> 245adf983c80f561f0c244ccf5e507c9b3b495e7
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
<<<<<<< HEAD
            await dbContext.Roles.AddRangeAsync(role);
            await dbContext.SaveChangesAsync();
            return role;
=======
            //Check if Admin not exist
            if(!await checkAdminExisting())
            {
                await dbContext.Roles.AddRangeAsync(role);
                await dbContext.SaveChangesAsync();
                return role;
            }
            return null;
>>>>>>> 245adf983c80f561f0c244ccf5e507c9b3b495e7
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
<<<<<<< HEAD
            return await dbContext.Users
                            .AnyAsync(u => u.Roles.Any(r => r.Type.ToUpper() == "ADMIN"));
        }
=======
            var existingAdmin = await dbContext.Users
                    .Where(u => u.Roles.Any(r => r.Type.ToUpper() == "ADMIN"))
                    .Select(u => u.UserId.ToString())
                    .FirstOrDefaultAsync();

            if (existingAdmin != null)
            {
                throw new Exception("Only one admin is allowed");
            }
            return false;
        }

>>>>>>> 245adf983c80f561f0c244ccf5e507c9b3b495e7
    }
}
