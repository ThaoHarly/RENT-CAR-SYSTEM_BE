using RentCarSystem.Models.Domain;

namespace RentCarSystem.Reponsitories
{
    public interface IMotorRepository
    {
        Task<Motor> CreateAsync(Motor motor);
        Task<List<Motor>> GetAllAsync();
        Task<Motor?> GetByIdAsync(string id);
        Task<Motor?> UpdateAsync(string id, Motor motor);
        Task<Motor?> DeleteAsync(string id);
    }
}
