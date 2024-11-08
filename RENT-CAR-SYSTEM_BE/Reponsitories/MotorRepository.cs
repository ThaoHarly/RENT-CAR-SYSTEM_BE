using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using RentCarSystem.Models.Domain;

namespace RentCarSystem.Reponsitories
{
    public class MotorRepository : IMotorRepository
    {
        private readonly RentCarSystemContext dbContext;

        public MotorRepository(RentCarSystemContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Motor> CreateAsync(Motor motor)
        {
            await dbContext.AddAsync(motor);
            await dbContext.SaveChangesAsync();
            return motor;
        }

        public async Task<Motor?> DeleteByVehicleIdAsync(string vehicleId)
        {
            var existingMotor = await dbContext.Motors.FirstOrDefaultAsync(x => x.VehicleId == vehicleId);
            if (existingMotor == null)
            {
                return null;
            }
            // delete motor
            dbContext.Motors.Remove(existingMotor);
            await dbContext.SaveChangesAsync();
            return existingMotor;
        }

        public async Task<List<Motor>> GetAllAsync()
        {
            return await dbContext.Motors.Include("Vehicle").ToListAsync();
        }

        public async Task<Motor?> GetByVehicleIdAsync(string vehicleId)
        {
            return await dbContext.Motors.FirstOrDefaultAsync(x => x.VehicleId == vehicleId);
        }

        public async Task<Motor?> UpdateByIdVehicleAsync(string vehicleId, Motor motor)
        {
            var existingMotor = await dbContext.Motors.FirstOrDefaultAsync(x=>x.VehicleId == vehicleId);
            if (existingMotor == null)
            {
                return null;
            }
            // update infor
            existingMotor.MotorImage = motor.MotorImage;
            // save and return
            await dbContext.SaveChangesAsync();
            return existingMotor;
        }


    }
}
