using Microsoft.EntityFrameworkCore;
using RentCarSystem.Models.Domain;
using System.Security.Claims;

namespace RentCarSystem.Reponsitories
{
    public class VehicleReponsitory : IVehicleReponsitory
    {
        private readonly RentCarSystemContext dbContext;

        public VehicleReponsitory(RentCarSystemContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Vehicle> CreateAsync(Vehicle vehicle)
        {
            await dbContext.AddAsync(vehicle);
            await dbContext.SaveChangesAsync();
            return vehicle;
        }

        public async Task<Vehicle?> DeleteByIdAsync(string vehicleId)
        {
            var existingVehicle = await dbContext.Vehicles.FirstOrDefaultAsync(x => x.VehicleId == vehicleId);
            if (existingVehicle == null)
            {
                return null;
            }
            // delete vehicle
            dbContext.Vehicles.Remove(existingVehicle);
            await dbContext.SaveChangesAsync();
            return existingVehicle;
        }

        public async Task<IEnumerable<Vehicle>> GetPagedVehiclesAsync(int pageNumber, int pageSize)
        {
            return await dbContext.Vehicles.Skip((pageNumber - 1) *pageSize) // Ví dụ nếu ở trong 3, với size là 10, thì bỏ qua 20 datas trước đó
                                            .Take(pageSize)
                                            .ToListAsync();
        }

        public Task<List<Vehicle>> GettAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Vehicle?> GettByIdAsync(string id)
        {
            return await dbContext.Vehicles.Include("User").FirstOrDefaultAsync(x => x.VehicleId == id);
        }

        public async Task<int> GetTotalCountAsync()
        {
            //Đếm coi có bao nhiêu xe trong db
            return await dbContext.Vehicles.CountAsync();
        }

        public async Task<Vehicle?> UpdateAsync(string vehicleId, Vehicle vehicle)
        {
            var existingVehicle = await dbContext.Vehicles.FirstOrDefaultAsync(x => x.VehicleId == vehicleId);
            if (existingVehicle == null)
            {
                return null;
            }

            // update vehicle
            existingVehicle.LicensePlate = vehicle.LicensePlate;
            existingVehicle.Status = vehicle.Status;
            existingVehicle.PricePerDay = vehicle.PricePerDay;
            existingVehicle.FuelConsumption = vehicle.FuelConsumption;
            existingVehicle.Range = vehicle.Range;
            existingVehicle.EngineCapacity = vehicle.EngineCapacity;


            await dbContext.SaveChangesAsync();
            return existingVehicle;
        }
    }
}
