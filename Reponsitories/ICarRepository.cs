using RentCarSystem.Models.Domain;

namespace RentCarSystem.Reponsitories
{
    public interface ICarRepository
    {
        Task<Car> CreateAsync(Car car);
        Task<Car?> DeleteByIdVehicleAsync(string vehicleId);

        Task<Car?> UpdateByIdVehicle(string vehicleId, Car car);

        Task<Car?> GetCarByIdVehicle(string vehicleId);
    }
}