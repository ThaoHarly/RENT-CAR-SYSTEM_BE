using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using RentCarSystem.Models.Domain;

namespace RentCarSystem.Reponsitories
{
    public class MotorRepository : IMotorRepository
    {
        private readonly RentCarSystemContext dbcontext;

        public MotorRepository(RentCarSystemContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }

        public async Task<Motor?> DeleteAsync(Guid id)
        {
            var existingMotor = await dbcontext.Motors.FirstOrDefaultAsync(x=>x.MotorId == id);
            if (existingMotor == null)
            {
                return null;
            }
            // delete 
            dbcontext.Motors.Remove(existingMotor);
            await dbcontext.SaveChangesAsync();
            return existingMotor;
        }

        public async Task<List<Motor>> GetAllAsync()
        {
            return await dbcontext.Motors.Include("Vehicle").ToListAsync();
        }

        public async Task<Motor?> GetByIdAsync(Guid id)
        {
            return await dbcontext.Motors.Include("Vehicle").FirstOrDefaultAsync(x=>x.MotorId == id);   
        }

        public async Task<Motor?> UpdateAsync(Guid id, Motor motor)
        {
            var existingMotor = await dbcontext.Motors.FirstOrDefaultAsync(x=>x.MotorId == id);
            if (existingMotor == null)
            {
                return null;
            }
            // update infor
            existingMotor.MotorImage = motor.MotorImage;
            // save and return
            await dbcontext.SaveChangesAsync();
            return existingMotor;
        }


    }
}
