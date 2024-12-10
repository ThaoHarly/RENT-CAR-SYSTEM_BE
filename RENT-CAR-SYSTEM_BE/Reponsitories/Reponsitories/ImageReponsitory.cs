using Microsoft.EntityFrameworkCore;
using RentCarSystem.Models.Domain;
using RentCarSystem.Reponsitories.IReponsitories;

namespace RentCarSystem.Reponsitories.Reponsitories
{
    public class ImageReponsitory : IImageReponsitory
    {
        private readonly RentCarSystemContext dbContext;

        public ImageReponsitory(RentCarSystemContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<Image> AddAsync(Image image)
        {
            var checkVehicleExisting = await dbContext.Vehicles.FirstOrDefaultAsync(x => x.VehicleId == image.VehicleId);
            if (checkVehicleExisting == null)
                return null;

            await dbContext.AddAsync(image);
            await dbContext.SaveChangesAsync();
            return image;
        }

        public async Task<List<Image>> GetImageByVehicleId(string vehicleId)
        {

            return await dbContext.Images.Where(x => x.VehicleId == vehicleId).ToListAsync();
        }
    }
}
