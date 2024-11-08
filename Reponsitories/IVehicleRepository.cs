using RentCarSystem.Models.Domain;

namespace RentCarSystem.Reponsitories
{
    public interface IVehicleRepository
    {
        Task<Vehicle> CreateAsync(Vehicle vehicle);
        Task<List<Vehicle>> GettAllAsync();
        Task<Vehicle?> GettByIdAsync(string id);
        Task<Vehicle?> DeleteByIdAsync(string id);
        Task<Vehicle?> UpdateAsync(string guid, Vehicle vehicle);
        Task<IEnumerable<Vehicle>> GetPagedVehiclesAsync(int pageNumber, int pageSize);
        Task<int> GetTotalCountAsync(); //Đếm có bao nhiêu xe trong db
    }
}