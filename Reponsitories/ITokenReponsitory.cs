using Microsoft.AspNetCore.Identity;

namespace RentCarSystem.Reponsitories
{
    public interface ITokenReponsitory
    {
        string CreateJWTToken(IdentityUser user, List<string> roles);
    }
}
