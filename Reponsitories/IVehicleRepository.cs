using RentCarSystem.Models.Domain;

namespace RentCarSystem.Reponsitories
{
    public interface IVehicleRepository
    {
        Task<Vehicle> CreateAsync(Vehicle vehicle);
        Task<List<Vehicle>> GettAllAsync();
        Task<Vehicle?> GettByIdAsync(string id);
        Task<Vehicle?> DeleteAsync(string id);
        Task<Vehicle?> UpdateAsync(string guid, Vehicle vehicle);

    }
}
