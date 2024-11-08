using Microsoft.AspNetCore.Identity;
using RentCarSystem.Models.Domain;

namespace RentCarSystem.Reponsitories
{
    public class LoginReponsitory : ILoginReponsitory
    {
        private readonly RentCarSystemContext dbContext;
        private readonly IPasswordHasher<User> passwordHasher;

        public LoginReponsitory(RentCarSystemContext dbContext, IPasswordHasher<User>passwordHasher)
        {
            this.dbContext = dbContext;
            this.passwordHasher = passwordHasher;
        }

        public Task<bool> VerifyPassword(User user, string password)
        {
            var result = passwordHasher.VerifyHashedPassword(user, user.Password, password);
           return Task.FromResult( result == PasswordVerificationResult.Success);
        }
    }
}
