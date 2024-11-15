using Microsoft.EntityFrameworkCore;
using RentCarSystem.Models.Domain;

namespace RentCarSystem.Reponsitories
{
    public class CarReponsitory :ICarReponsitory
    {
        private readonly RentCarSystemContext dbContext;

        public CarReponsitory(RentCarSystemContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Car> CreateAsync(Car car)
        {
            await dbContext.AddAsync(car);
            await dbContext.SaveChangesAsync();
            return car;
        }

        public async Task<Car?> DeleteByIdVehicleAsync(string vehicleId)
        {
            var existingCar = await dbContext.Cars.FirstOrDefaultAsync(x => x.VehicleId == vehicleId);
            if (existingCar == null)
            {
                return null;
            }
            // delete
            dbContext.Cars.Remove(existingCar);
            await dbContext.SaveChangesAsync();
            return existingCar;
        }

        public async Task<Car?> GetCarByIdVehicle(string vehicleId)
        {
            return await dbContext.Cars.FirstOrDefaultAsync(x => x.VehicleId == vehicleId);
        }

        public async Task<Car?> UpdateByIdVehicle(string vehicleId, Car car)
        {
            var existingCar = await dbContext.Cars.FirstOrDefaultAsync(x => x.VehicleId == vehicleId);
            if (existingCar == null)
            {
                return null;
            }
            // update car
            existingCar.CarBrand = car.CarBrand;
            existingCar.FuelType = car.FuelType;
            existingCar.SeatingCapacity = car.SeatingCapacity;
            existingCar.CarImage = car.CarImage;
            existingCar.ChargingTime = car.ChargingTime;

            await dbContext.SaveChangesAsync();
            return existingCar;
        }
    }
}
