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
    public class VehicleHireServiceController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IVehicleHireServiceRepository vehicleHireServiceRepository;
        

        public VehicleHireServiceController(IMapper mapper, IVehicleHireServiceRepository vehicleHireServiceRepository)
        {
            this.mapper = mapper;
            this.vehicleHireServiceRepository = vehicleHireServiceRepository;
        }

        // Get all
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var vehicleServcDomainModel = await vehicleHireServiceRepository.GetAllAsync();
            // Map domain to dto
            return Ok(mapper.Map<List<ServiceDTO>>(vehicleServcDomainModel));
        }

        // Get by id
        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var ServiceDomainModel = await vehicleHireServiceRepository.GetByIdAsync(id);
            if(ServiceDomainModel == null)
            {
                return NotFound();
            }
            // Map domain model to dto
            return Ok(mapper.Map<ServiceDTO>(ServiceDomainModel));
        }

        // Update
        [HttpPost]
        [Route("{id:guid}")]
        [ValidateModel]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateServiceRequest updateServiceRequest)
        {
            // Map Update to domain model
            var ServiceDomainModel = mapper.Map<VehicleHireService>(updateServiceRequest);

            ServiceDomainModel = await vehicleHireServiceRepository.UpdateAsync(id, ServiceDomainModel);

            if(ServiceDomainModel == null)
            {
                return NotFound();
            }

            //Map domain model to dto
            return Ok(mapper.Map<ServiceDTO>(ServiceDomainModel));

        }

        // Delete
        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var deleteService = await vehicleHireServiceRepository.DelteteAsync(id);
            if(deleteService == null)
            {
                return NotFound();
            }

            // map domain to dto
            return Ok(mapper.Map<ServiceDTO>(deleteService));

        }

    }
}
