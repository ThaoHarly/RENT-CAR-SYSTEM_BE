using RentCarSystem.Models.Domain;
using RentCarSystem.Models.UpdateRequest;

namespace RentCarSystem.Reponsitories
{
    public interface IVehicleHireServiceRepository
    {
        Task<List<VehicleHireService>> GetAllAsync();
        Task<VehicleHireService?> GetByIdAsync(Guid id);
        Task<VehicleHireService?> UpdateAsync(Guid id, VehicleHireService service);
        Task<VehicleHireService?> DelteteAsync(Guid id);
    }
}
