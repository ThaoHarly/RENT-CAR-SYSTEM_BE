using RentCarSystem.Models.Domain;

namespace RentCarSystem.Reponsitories.IReponsitories
{
    public interface ILoginReponsitory
    {
        Task<bool> VerifyPassword(User user, string password);
    }
}
