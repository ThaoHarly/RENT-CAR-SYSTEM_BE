using Microsoft.EntityFrameworkCore;
using RentCarSystem.Models.Domain;

namespace RentCarSystem.Reponsitories
{
    public class BusinessRepository : IBusinessRepository
    {
        private readonly RentCarSystemContext dbcontext;

        public BusinessRepository(RentCarSystemContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }

        public async Task<Business?> DeleteAsync(Guid id)
        {
            var bnsModel = await dbcontext.Businesses.FirstOrDefaultAsync(x=>x.BsnId == id);
            if (bnsModel == null)
            {
                return null;
            }

            dbcontext.Businesses.Remove(bnsModel);
            await dbcontext.SaveChangesAsync();
            return bnsModel;
        }

        public async Task<List<Business>> GetAllAsync(string? filterOn = null, string? filterQuery = null,
                                                      string? sortBy = null, bool isAscending = true,
                                                      int pageNumber = 1, int pageSize = 1000)
        {
            var bnses = dbcontext.Businesses.AsQueryable();
            
            // filtering
            if(string.IsNullOrWhiteSpace(filterOn) == false && string.IsNullOrWhiteSpace(filterQuery) == false)
            {
                if(filterOn.Equals("IssuingLocation", StringComparison.OrdinalIgnoreCase))
                {
                    bnses = bnses.Where(x=>x.IssuingLocation.Contains(filterQuery));
                }
            }

            // sorting
            if(string.IsNullOrWhiteSpace(sortBy) == false)
            {
                if (sortBy.Equals("Vat", StringComparison.OrdinalIgnoreCase))
                {
                    bnses = isAscending ? bnses.OrderBy(x=>x.Vat) : bnses.OrderByDescending(x=>x.Vat);
                }
                else if(sortBy.Equals("DateOfIssue", StringComparison.OrdinalIgnoreCase))
                {
                    bnses = isAscending ? bnses.OrderBy(x => x.DateOfIssue) : bnses.OrderByDescending(x => x.DateOfIssue);
                }
            }

            // paginating
            var skipresult = (pageNumber - 1) * pageSize;

            // return
            return await bnses.Skip(skipresult).Take(pageSize).ToListAsync();
            
        }

        public async Task<Business?> GetByIdAsync(Guid id)
        {
            return await dbcontext.Businesses.FirstOrDefaultAsync(x=>x.BsnId == id);
        }

        public async Task<Business?> UpdateAsync(Guid id, Business business)
        {
            var Bns = await dbcontext.Businesses.FirstOrDefaultAsync(x=>x.BsnId==id);
            if(Bns == null)
            {
                return null;
            }

            Bns.Description = business.Description;
            Bns.BusinessImg = business.BusinessImg;
            Bns.RegistrationDate = business.RegistrationDate;
            Bns.Vat = business.Vat;
            Bns.IssuingLocation = business.IssuingLocation;
            Bns.DateOfIssue = business.DateOfIssue;

            await dbcontext.SaveChangesAsync();
            return Bns;

        }
    }
}
