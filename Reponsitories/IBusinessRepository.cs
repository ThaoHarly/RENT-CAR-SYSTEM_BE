using RentCarSystem.Models.Domain;

namespace RentCarSystem.Reponsitories
{
    public interface IBusinessRepository
    {
        Task<List<Business>> GetAllAsync(string? filterOn = null, string? filterQuery = null,
                                         string? sortBy = null, bool isAscending = true,
                                         int pageNumber = 1, int pageSize = 1000);
        Task<Business?> GetByIdAsync(Guid id);
        Task<Business?> UpdateAsync(Guid id, Business business);
        Task<Business?> DeleteAsync(Guid id);
        
    }
}
