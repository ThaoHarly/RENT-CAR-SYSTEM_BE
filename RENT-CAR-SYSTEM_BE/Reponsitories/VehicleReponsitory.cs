using Microsoft.EntityFrameworkCore;
using RentCarSystem.Models.Domain;
using System.Security.Claims;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

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

        public async Task<IEnumerable<Vehicle>> GetPagedVehiclesAsync(int pageNumber, int pageSize, string keyword)
        {
            var query = dbContext.Vehicles.AsQueryable();

            // Nếu có từ khóa tìm kiếm, lọc theo các trường liên quan
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                // Kiểm tra trong bảng Vehicle
                query = query.Where(v =>
                    EF.Functions.Like(v.Category.ToLower(), $"%{keyword.ToLower()}%") ||
                    EF.Functions.Like(v.LicensePlate.ToLower(), $"%{keyword.ToLower()}%"));
            }

            // Truy vấn dữ liệu từ bảng Car
            var carQuery = dbContext.Cars.AsQueryable();

            // Kiểm tra FuelType, CarBrand và SeatingCapacity trong bảng Car
            carQuery = carQuery.Where(c =>
                EF.Functions.Like(c.FuelType.ToLower(), $"%{keyword.ToLower()}%") ||  // Tìm theo FuelType
                EF.Functions.Like(c.CarBrand.ToLower(), $"%{keyword.ToLower()}%") ||   // Tìm theo CarBrand
                EF.Functions.Like(Convert.ToString(c.SeatingCapacity), $"%{keyword.ToLower()}%"));  // Tìm theo SeatingCapacity

            // Kết hợp dữ liệu Vehicle và Car, chỉ lấy các bản ghi từ Car có liên kết đúng
            query = from v in dbContext.Vehicles
                        join c in dbContext.Cars on v.VehicleId equals c.VehicleId into carGroup
                        from car in carGroup.DefaultIfEmpty()
                        where EF.Functions.Like(v.LicensePlate, $"%{keyword.ToUpper()}%") ||
                              (car != null && EF.Functions.Like(car.FuelType, $"%{keyword.ToUpper()}%")||
                              EF.Functions.Like(car.CarBrand, $"%{keyword.ToUpper()}%"))
                        select v;



            // Sắp xếp theo tình trạng: "Availability" lên đầu
            query = query.OrderBy(v => v.Status.ToUpper() == "RENTED");

            // Phân trang
            var vehicles = await query.Skip((pageNumber - 1) * pageSize)
                                       .Take(pageSize)
                                       .ToListAsync();

            return vehicles;
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

        public async Task<IEnumerable<Object>> SearchVehiclesAsync(string keyword, int pageNumber = 1, int pageSize = 10)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                return new List<object>();
            }

            var query = dbContext.Vehicles.AsQueryable();

            // Tìm kiếm theo các trường liên quan
            query = query.Where(v => EF.Functions.Like(v.Category.ToUpper(), $"%{keyword.ToUpper()}%"));

            // Sắp xếp theo Status: "Availability" lên đầu
            query = query.OrderBy(v => v.Status == "RENTED").ThenBy(v => v.VehicleId);

            // Phân trang
            query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            var vehicles = await query.ToListAsync();

            var resultList = new List<object>();

            foreach (var vehicle in vehicles)
            {
                if (vehicle.Category.ToUpper() == "CAR")
                {
                    var car = await dbContext.Cars.FirstOrDefaultAsync(c => c.VehicleId == vehicle.VehicleId);
                    resultList.Add(new { Vehicle = vehicle, Car = car });
                }
                else if (vehicle.Category.ToUpper() == "MOTOR")
                {
                    var motor = await dbContext.Motors.FirstOrDefaultAsync(m => m.VehicleId == vehicle.VehicleId);
                    resultList.Add(new { Vehicle = vehicle, Motor = motor });
                }
            }

            return resultList;
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
