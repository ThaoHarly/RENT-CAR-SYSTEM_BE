using RentCarSystem.Models.Domain;

namespace RentCarSystem.Reponsitories
{
    public interface IVehicleRepository
    {
        Task<Vehicle> CreateAsync(Vehicle vehicle);
        Task<List<Vehicle>> GettAllAsync();
        Task<Vehicle?> GettByIdAsync(Guid id);
        Task<Vehicle?> DeleteAsync(Guid id);
        Task<Vehicle?> UpdateAsync(Guid guid, Vehicle vehicle);

    }
}
