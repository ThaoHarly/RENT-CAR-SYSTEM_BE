using RentCarSystem.Models.Domain;

namespace RentCarSystem.Reponsitories
{
    public interface IMotorRepository
    {
        Task<Motor> CreateAsync(Motor motor);
        Task<List<Motor>> GetAllAsync();
        Task<Motor?> GetByVehicleIdAsync(string vehicleId);
        Task<Motor?> UpdateByIdVehicleAsync(string id, Motor motor);
        Task<Motor?> DeleteByVehicleIdAsync(string vehicleId);
    }
}
