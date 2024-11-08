using Microsoft.EntityFrameworkCore;
using RentCarSystem.Models.Domain;

namespace RentCarSystem.Reponsitories
{
    public class VehicleHireServiceRepository : IVehicleHireServiceRepository
    {
        private readonly RentCarSystemContext dbcontext;

        public VehicleHireServiceRepository(RentCarSystemContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }

        public async Task<VehicleHireService?> DelteteAsync(string id)
        {
            var existingService = await dbcontext.VehicleHireServices.FirstOrDefaultAsync(x=>x.UserId == id);
            if (existingService == null)
            {
                return null;
            }
            dbcontext.VehicleHireServices.Remove(existingService);
            await dbcontext.SaveChangesAsync();
            return existingService;
        }

        public async Task<List<VehicleHireService>> GetAllAsync()
        {
            return await dbcontext.VehicleHireServices.Include("Business").Include("Individual").ToListAsync();
        }

        public async Task<VehicleHireService?> GetByIdAsync(string id)
        {
            return await dbcontext.VehicleHireServices.Include("Business").Include("Individual").FirstOrDefaultAsync(x=>x.UserId == id);
        }

        public async Task<VehicleHireService?> UpdateAsync(string id, VehicleHireService service)
        {
            var existingService = await dbcontext.VehicleHireServices.FirstOrDefaultAsync(x=>x.UserId==id);
            if (existingService == null)
            {
                return null;
            }
            
            existingService.ServiceType = service.ServiceType;
            existingService.BankAccount = service.BankAccount;  
            existingService.BankName = service.BankName;
            await dbcontext.SaveChangesAsync();
            return existingService;
        }
    }
}
