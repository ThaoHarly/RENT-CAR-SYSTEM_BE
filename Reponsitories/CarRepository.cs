using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentCarSystem.Models.Domain;

namespace RentCarSystem.Reponsitories
{
    public class CarRepository : ICarRepository
    {
        private readonly RentCarSystemContext dbcontext;

        public CarRepository(RentCarSystemContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }

        public async Task<Car?> DeleteAsync(Guid id)
        {
            var existingCar = await dbcontext.Cars.FirstOrDefaultAsync(x => x.CarId == id);
            if (existingCar == null)
            {
                return null;
            }
            // delete
            dbcontext.Cars.Remove(existingCar);
            await dbcontext.SaveChangesAsync();
            return existingCar;
        }

        public async Task<List<Car>> GetAllAsync(string? filterOn = null, string? filterQuery = null,
                                                 string? sortBy = null, bool isAscending = true,
                                                 int pageNumber = 1, int pageSize = 1000)
        { 
            var cars = dbcontext.Cars.Include("Vehicle").AsQueryable();

            // filtering
            if(string.IsNullOrWhiteSpace(filterOn) == false && string.IsNullOrWhiteSpace(filterQuery) == false)
            {
                if(filterOn.Equals("CarBrand", StringComparison.OrdinalIgnoreCase))
                {
                    cars = cars.Where(x => x.CarBrand.Contains(filterQuery));
                } 
            }

            // sorting 
            if (string.IsNullOrWhiteSpace(sortBy) == false)
            {
                if (sortBy.Equals("CarBrand", StringComparison.OrdinalIgnoreCase))
                {
                    cars = isAscending ? cars.OrderBy(x=>x.CarBrand) : cars.OrderByDescending(x=>x.CarBrand);
                }
                else if (sortBy.Equals("FuelType", StringComparison.OrdinalIgnoreCase))
                {
                    cars = isAscending ? cars.OrderBy(x=>x.FuelType) : cars.OrderByDescending(x=>x.FuelType);
                }
            }

            // pagination
            var skipResult = (pageNumber - 1) * pageSize;

            return await cars.Skip(skipResult).Take(pageSize).ToListAsync();
            //return await dbcontext.Cars.Include("Vehicle").ToListAsync();
        }

        public async Task<Car?> GetByIdAsync(Guid id)
        {
            return await dbcontext.Cars.Include("Vehicle").FirstOrDefaultAsync(x=>x.CarId == id);
        }

        public async Task<Car?> UpdateAsync(Guid id, Car car)
        {
            var existingCar = await dbcontext.Cars.FirstOrDefaultAsync(x=>x.CarId == id);
            if (existingCar == null)
            {
                return null;
            }
            existingCar.CarBrand = car.CarBrand;
            existingCar.FuelType = car.FuelType;
            existingCar.SeatingCapacity = car.SeatingCapacity;
            existingCar.CarImage = car.CarImage;
            existingCar.ChargingTime = car.ChargingTime;

            await dbcontext.SaveChangesAsync();
            return existingCar;
        }
    }
}
