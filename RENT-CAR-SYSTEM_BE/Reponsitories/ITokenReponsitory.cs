using Microsoft.AspNetCore.Identity;
using RentCarSystem.Models.Domain;


namespace RentCarSystem.Reponsitories
{
    public interface ITokenReponsitory
    {
        string CreateJWTToken(User user, List<string> roles);
    }
}
