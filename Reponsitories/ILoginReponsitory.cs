using RentCarSystem.Models.Domain;

namespace RentCarSystem.Reponsitories
{
    public interface ILoginReponsitory
    {
        Task<bool> VerifyPassword(User user, string password);
    }
}
