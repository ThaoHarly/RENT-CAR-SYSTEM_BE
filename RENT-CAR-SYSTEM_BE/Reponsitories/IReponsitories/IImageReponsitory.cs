using RentCarSystem.Models.Domain;

namespace RentCarSystem.Reponsitories.IReponsitories
{
    public interface IImageReponsitory
    {
        Task<Image> AddAsync(Image image);
    }
}
