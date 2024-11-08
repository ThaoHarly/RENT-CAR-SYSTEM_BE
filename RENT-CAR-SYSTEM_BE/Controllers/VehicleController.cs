using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RentCarSystem.Models.Domain;
using RentCarSystem.Models.DTO;
using RentCarSystem.Reponsitories;
using System.Data;
using System.Security.Claims;

namespace RentCarSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IVehicleReponsitory vehicleRepository;
        private readonly RentCarSystemContext dbContext;
        private readonly IMotorRepository motorRepository;
        private readonly ICarReponsitory carReponsitory;

        public VehicleController(IMapper mapper, IVehicleReponsitory vehicleRepository, RentCarSystemContext dbContext, IMotorRepository motorRepository, ICarReponsitory carReponsitory)
        {
            this.mapper = mapper;
            this.vehicleRepository = vehicleRepository;
            this.dbContext = dbContext;
            this.motorRepository = motorRepository;
            this.carReponsitory = carReponsitory;
        }

        // Create
        [Authorize(Policy = "BusinessWithAcceptStatus")]
        [HttpPost]
        [Route("AddVehicle")]
        public async Task<IActionResult> Create([FromBody] AddVehicleServiceDTO addVehicleServiceDTO)
        {
            var category = addVehicleServiceDTO.Category.ToUpper();
            if (category == "MOTOR" || category == "CAR")
            {
                //Map DTO to Vehicle domain
                var vehicleDomain = mapper.Map<Vehicle>(addVehicleServiceDTO);

                //Get userId is logining
                var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

                vehicleDomain.UserId = userId;
                vehicleDomain = await vehicleRepository.CreateAsync(vehicleDomain);

                return category switch
                {
                    "MOTOR" => await AddMotor(vehicleDomain, addVehicleServiceDTO),
                    "CAR" => await AddCar(vehicleDomain, addVehicleServiceDTO),
                    _ => BadRequest("Invalid category")
                };

            }

            return BadRequest("Something went wrong ...");
        }



        // Get all
        [HttpGet]
        [Route("GetAllVehicle")]
        public async Task<IActionResult> GetAllVehicle(int pageNumber = 1, int pageSize = 10)
        {
            // Đảm bảo pageNumber và pageSize hợp lệ
            pageNumber = pageNumber < 1 ? 1 : pageNumber;
            pageSize = pageSize < 1 ? 10 : pageSize;

            //get all vehicle sau khi phân trang
            var vehicleDomain = await vehicleRepository.GetPagedVehiclesAsync(pageNumber,pageSize);

            if(vehicleDomain == null || !vehicleDomain.Any())
            {
                return NotFound("No vehicles found ...");
            }

            //Vehicle List
            var vehicleDetailsList = new List<object>();
            
            foreach(var vehicle in vehicleDomain)
            {
                if(vehicle.Category.ToUpper() == "CAR")
                {
                    var carDomain = await carReponsitory.GetCarByIdVehicle(vehicle.VehicleId);
                    
                    //Map Vehicle Domain to VehicleDTO
                    var vehicleDTO = mapper.Map<VehicleDTO>(vehicle);

                    //Map Car Domain to Car DTO
                    var carDTO = mapper.Map<CarDTO>(carDomain);

                    //Add to vehicleDetailsList
                    vehicleDetailsList.Add(new
                    {
                        Vehicle = vehicleDTO,
                        Car = carDTO
                    });
                }
                else if(vehicle.Category.ToUpper() =="MOTOR")
                {
                    var motorDomain = await motorRepository.GetByVehicleIdAsync(vehicle.VehicleId);

                    //Map Vehicle Domain to VehicleDTO
                    var vehicleDTO = mapper.Map<VehicleDTO>(vehicle);

                    //Map Car Domain to Car DTO
                    var motorDTO = mapper.Map<MotorDTO>(motorDomain);

                    //Add to vehicleDetailsList
                    vehicleDetailsList.Add(new
                    {
                        Vehicle = vehicleDTO,
                        Moto = motorDTO
                    });
                }
            }
            return Ok(new
            {
                totalVehicleCount = await vehicleRepository.GetTotalCountAsync(),
                pageNumber = pageNumber,
                pageSize = pageSize,
                data = vehicleDetailsList
            });
        }


        //Update
        [Authorize(Policy = "BusinessWithAcceptStatus")]
        [HttpPut]
        [Route("UpdateVehicle")]
        public async Task<IActionResult> Update(string VehicleId, UpdateVehicleRequestDTO updateVehicleRequestDTO)
        {
            //Check xem phải chủ sở hữu của xe không
            var ownerShipResult = await CheckOwnerShip(VehicleId);

            if (ownerShipResult is ObjectResult result && (bool)result.Value == false)
            {
                return Unauthorized("You are not the owner of this vehicle");
            }

            //Get data from Vehicle Table
            var vehicle = await vehicleRepository.GettByIdAsync(VehicleId);

            if (vehicle.Category.ToUpper() == "CAR")
            {
                //Map carDomain to updateVehicleRequestDTO
                var carDomain = mapper.Map<Car>(updateVehicleRequestDTO);
                //Update Car
                carDomain = await carReponsitory.UpdateByIdVehicle(VehicleId, carDomain);


                //Map Vehicle Domain to updateVehicleRequestDTO
                var vehicleDomain = mapper.Map<Vehicle>(updateVehicleRequestDTO);
                //Update Vehicle
                vehicleDomain = await vehicleRepository.UpdateAsync(VehicleId, vehicleDomain);

                //Map VehicleDomain to VehicleDTO
                var vehicleDTO = mapper.Map<VehicleDTO>(vehicleDomain);

                //Map CarDomain to CarDTO
                var carDTO = mapper.Map<CarDTO>(carDomain);

                //Information about Vehicle deleted
                var resultVehicle = new
                {
                    Message = "You update success!",
                    Vehicle = vehicleDTO,
                    Car = carDTO
                };
                return Ok(resultVehicle);
            }
            else if (vehicle.Category.ToUpper() == "MOTOR")
            {
                //Map motorDomain to updateVehicleRequestDTO
                var motorDomain = mapper.Map<Motor>(updateVehicleRequestDTO);
                //Update Motor
                motorDomain = await motorRepository.UpdateByIdVehicleAsync(VehicleId, motorDomain);


                //Map Vehicle Domain to updateVehicleRequestDTO
                var vehicleDomain = mapper.Map<Vehicle>(updateVehicleRequestDTO);
                //Update Vehicle
                vehicleDomain = await vehicleRepository.UpdateAsync(VehicleId, vehicleDomain);

                //Map VehicleDomain to VehicleDTO
                var vehicleDTO = mapper.Map<VehicleDTO>(vehicleDomain);

                //Map MotorDomain to CarDTO
                var motorDTO = mapper.Map<MotorDTO>(motorDomain);

                //Information about Vehicle deleted
                var resultVehicle = new
                {
                    Message = "You update success!",
                    Vehicle = vehicleDTO,
                    Moto = motorDTO
                };
                return Ok(resultVehicle);
            }
            return BadRequest("some thing was wrong ... ");
        }

        //Delete 
        [Authorize(Policy = "BusinessWithAcceptStatus")]
        [HttpDelete]
        [Route("DeleteVehicle/{VehicleId}")]
        public async Task<IActionResult> DeleteById( string VehicleId)
        {
            //Check xem phải chủ sở hữu của xe không
            var ownerShipResult = await CheckOwnerShip(VehicleId);
            
            if(ownerShipResult is ObjectResult result && (bool)result.Value == false )
            {
                return Unauthorized("You are not the owner of this vehicle");
            }

            //Get data from Vehicle Table
            var vehicle = await vehicleRepository.GettByIdAsync(VehicleId);

            if(vehicle.Category.ToUpper() == "CAR")
            {
                //Delete Car 
                var carDomain = await carReponsitory.DeleteByIdVehicleAsync(VehicleId);

                //Delete Vehicle
                var vehicleDomain = await vehicleRepository.DeleteByIdAsync(VehicleId);

                //Map VehicleDomain to VehicleDTO
                var vehicleDTO = mapper.Map<VehicleDTO>(vehicleDomain);

                //Map CarDomain to CarDTO
                var carDTO = mapper.Map<CarDTO>(carDomain);

                //Information about Vehicle deleted
                var resultVehicle = new
                {
                    Message = "You deleted success!",
                    Vehicle = vehicleDTO,
                    Car = carDTO
                };
                return Ok(resultVehicle);
            }
            else if(vehicle.Category.ToUpper() == "MOTOR")
            {

                //Delete Motor
                var motorDomain = await motorRepository.DeleteByVehicleIdAsync(VehicleId);

                //Delete Vehicle
                var vehicleDomain = await vehicleRepository.DeleteByIdAsync(VehicleId);

                //Map VehicleDomain to VehicleDTO
                var vehicleDTO = mapper.Map<VehicleDTO>(vehicleDomain);

                //Map MotorDomain to CarDTO
                var motorDTO = mapper.Map<MotorDTO>(motorDomain);

                //Information about Vehicle deleted
                var resultVehicle = new
                {
                    Message = "You deleted success!",
                    Vehicle = vehicleDTO,
                    Moto = motorDTO
                };
                return Ok(resultVehicle);
            }
            return BadRequest("some thing was wrong ... ");
        }



        private async Task<IActionResult> AddMotor(Vehicle vehicleDomain, AddVehicleServiceDTO addVehicleServiceDTO)
        {
            //Map Motor to Vehicle Domain
            var motorDomain = mapper.Map<Motor>(addVehicleServiceDTO);
            motorDomain.VehicleId = vehicleDomain.VehicleId;
            await motorRepository.CreateAsync(motorDomain);

            //Map VehicleDomain to VehicleDTO
            var vehicleDTO = mapper.Map<VehicleDTO>(vehicleDomain);

            //Map MotorDomain to MotorDTO
            var motoDTO = mapper.Map<MotorDTO>(motorDomain);

            //Information about the Motor
            var result = new
            {
                Vehicle = vehicleDTO,
                Motor = motoDTO
            };

            return Ok(result);
        }

        private async Task<IActionResult> AddCar(Vehicle vehicleDomain, AddVehicleServiceDTO addVehicleServiceDTO)
        {
            //Map Car to Vehicle Domain
            var carDomain = mapper.Map<Car>(addVehicleServiceDTO);
            carDomain.VehicleId = vehicleDomain.VehicleId;
            await carReponsitory.CreateAsync(carDomain);


            //Map VehicleDomain to VehicleDTO
            var vehicleDTO = mapper.Map<VehicleDTO>(vehicleDomain);

            //Map CarDomain to CarDTO
            var carDTO = mapper.Map<CarDTO>(carDomain);

            //Information about the Car
            var result = new
            {
                Vehicle = vehicleDTO,
                Car = carDTO
            };

            return Ok(result);
        }

        private async Task<IActionResult> CheckOwnerShip(string VehicleId)
        {
            //Get userId is logining
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            //Find Vehicle by VehicleId
            var vehicle = await vehicleRepository.GettByIdAsync(VehicleId);
            if (vehicle == null)
            {
                return NotFound();
            }

            if(vehicle.UserId == userId)
            {
                return Ok(true); // User is the owner
            }
            return Ok(false);// User is not the owner
        }
    }
}
