using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RentCarSystem.CustomActionFilter;
using RentCarSystem.Models.Domain;
using RentCarSystem.Models.DTO;
using RentCarSystem.Models.UpdateRequest;
using RentCarSystem.Reponsitories;

namespace RentCarSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IVehicleRepository vehicleRepository;

        public VehicleController(IMapper mapper, IVehicleRepository vehicleRepository)
        {
            this.mapper = mapper;
            this.vehicleRepository = vehicleRepository;
        }

        // Get all
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var vehicleDomainModel = await vehicleRepository.GettAllAsync();
            // map domain to dto
            return Ok(mapper.Map<List<Vehicle>>(vehicleDomainModel));
        }

        // Get By Id
        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var VehicleDomainModel = await vehicleRepository.GettByIdAsync(id);
            if(VehicleDomainModel == null)
            {
                return NotFound();
            }
            // map domain to dto
            return Ok(mapper.Map<VehicleDTO>(VehicleDomainModel));  
        }

        // Update
        [HttpPut]
        [Route("{id:guid}")]
        [ValidateModel]
        public async Task<IActionResult> Update([FromRoute] Guid id, UpdateVehicleRequest updateVehicleRequest)
        {
            // Map Update to domain model
            var VehicleDomainModel = mapper.Map<Vehicle>(updateVehicleRequest);

            VehicleDomainModel = await vehicleRepository.UpdateAsync(id, VehicleDomainModel);

            if (VehicleDomainModel == null)
            {
                return NotFound();
            }

            //Map domain model to dto
            return Ok(mapper.Map<ServiceDTO>(VehicleDomainModel));
        }


        // Delete
        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleteVehicle = await vehicleRepository.DeleteAsync(id);
            if(deleteVehicle == null)
            {
                return NotFound();
            }
            // map domain to dto
            return Ok(mapper.Map<VehicleDTO>(deleteVehicle));
        }
    }
}
