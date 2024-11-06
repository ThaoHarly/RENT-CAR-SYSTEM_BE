using Microsoft.AspNetCore.Identity;
<<<<<<< HEAD
using RentCarSystem.Models.Domain;
=======
>>>>>>> 245adf983c80f561f0c244ccf5e507c9b3b495e7

namespace RentCarSystem.Reponsitories
{
    public interface ITokenReponsitory
    {
<<<<<<< HEAD
        string CreateJWTToken(User user, List<string> roles);
=======
        string CreateJWTToken(IdentityUser user, List<string> roles);
>>>>>>> 245adf983c80f561f0c244ccf5e507c9b3b495e7
    }
}
