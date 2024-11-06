using Microsoft.EntityFrameworkCore;
using RentCarSystem.Models.Domain;

namespace RentCarSystem.Reponsitories
{
    public class VehicleRepository: IVehicleRepository
    {
        private readonly RentCarSystemContext dbcontext;

        public VehicleRepository(RentCarSystemContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }

        public async Task<Vehicle> CreateAsync(Vehicle vehicle)
        {
            await dbcontext.AddAsync(vehicle);
            await dbcontext.SaveChangesAsync();
            return vehicle;
        }

        public async Task<Vehicle?> DeleteAsync(string id)
        {
            var existingVehicle = await dbcontext.Vehicles.FirstOrDefaultAsync(x=>x.VehicleId == id);
            if (existingVehicle == null)
            {
                return null;
            }
            // delete
            dbcontext.Vehicles.Remove(existingVehicle);
            await dbcontext.SaveChangesAsync();
            return existingVehicle;
            
        }

        public async Task<List<Vehicle>> GettAllAsync()
        {
            return await dbcontext.Vehicles.Include("User").ToListAsync();
        }

        public async Task<Vehicle?> GettByIdAsync(string id)
        {
            return await dbcontext.Vehicles.Include("User").FirstOrDefaultAsync(x=>x.VehicleId == id);
        }

        public async Task<Vehicle?> UpdateAsync(string id, Vehicle vehicle)
        {
            var existingVehicle = await dbcontext.Vehicles.FirstOrDefaultAsync(x=>x.VehicleId == id);
            if (existingVehicle == null)
            {
                return null;    
            }

            existingVehicle.Category = vehicle.Category;
            existingVehicle.LicensePlate = vehicle.LicensePlate;
            existingVehicle.Status = vehicle.Status;
            existingVehicle.PricePerDay = vehicle.PricePerDay;
            existingVehicle.FuelConsumption = vehicle.FuelConsumption;
            existingVehicle.Range = vehicle.Range;
            existingVehicle.EngineCapacity = vehicle.EngineCapacity;

            await dbcontext.SaveChangesAsync();
            return existingVehicle;

        }
    }
}
