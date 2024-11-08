using RentCarSystem.Models.Domain;
using RentCarSystem.Models.UpdateRequest;

namespace RentCarSystem.Reponsitories
{
    public interface IVehicleHireServiceRepository
    {
        Task<List<VehicleHireService>> GetAllAsync();
        Task<VehicleHireService?> GetByIdAsync(string id);
        Task<VehicleHireService?> UpdateAsync(string id, VehicleHireService service);
        Task<VehicleHireService?> DelteteAsync(string id);
    }
}
