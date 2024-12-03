using RentCarSystem.Models.Domain;

namespace RentCarSystem.Reponsitories.IReponsitories
{
    public interface IReviewRepository
    {
        Task<List<Review>> GetAllAsync();
        Task<List<Review>> GetReviewsByRatingAsync(List<int> ratings);
        Task<Review?> GetReviewById(string id);

        Task<Review?> CreateAsync(Review review);
        Task<Review?> DeleteByIdAsync(string id);
        Task<Review?> UpdateAsync(string id, Review review, string cusid);
        Task<IEnumerable<Review>> GetPagedReviewAsync(int pageNumber, int pageSize);
        Task<int> GetTotalCountAsync();
    }
}
