using Microsoft.AspNetCore.Mvc;
using RentCarSystem.Models.Domain;

namespace RentCarSystem.Reponsitories
{
    public interface ICarRepository
    {
        Task<Car> CreateAsync(Car car);
        Task<List<Car>> GetAllAsync(string? filterOn = null, string? filterQuery = null,
                                    string? sortBy = null, bool isAscending = true,
                                    int pageNumber = 1, int pageSize = 1000 );
        Task<Car?> GetByIdAsync(Guid id);
        Task<Car?> UpdateAsync(Guid id, Car car);
        Task<Car?> DeleteAsync(Guid id);

    }
}
