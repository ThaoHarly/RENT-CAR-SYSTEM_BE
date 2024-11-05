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
    public class CarController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly ICarRepository carRepository;

        public CarController(IMapper mapper, ICarRepository carRepository)
        {
            this.mapper = mapper;
            this.carRepository = carRepository;
        }

        // Get all
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? filterOn, [FromQuery] string? filterQuery,
                                                [FromQuery] string? sortBy, [FromQuery] bool? isAscending,
                                                [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 1000)
        {
            var CarDomainModel = await carRepository.GetAllAsync(filterOn, filterQuery, sortBy, isAscending??true,
                                                                 pageNumber, pageSize);

            // map domain model to dto
            return Ok(mapper.Map<List<CarDTO>>(CarDomainModel));
        }

        // Get by id
        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var CarDomainModel = await carRepository.GetByIdAsync(id);
            if (CarDomainModel == null)
            {
                return NotFound();
            }
            // map domain to dto
            return Ok(mapper.Map<CarDTO>(CarDomainModel));
        }

        // Update 
        [HttpPut]
        [Route("{id:guid}")]
        [ValidateModel]
        public async Task<IActionResult> Update([FromRoute] Guid id, UpdateCarRequest updateCarRequest)
        {
            // Map Update to domain model
            var CarDomainModel = mapper.Map<Car>(updateCarRequest);

            CarDomainModel = await carRepository.UpdateAsync(id, CarDomainModel);

            if (CarDomainModel == null)
            {
                return NotFound();
            }

            //Map domain model to dto
            return Ok(mapper.Map<CarDTO>(CarDomainModel));
        }

        // Delete
        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var deleteCar = await carRepository.DeleteAsync(id);
            if (deleteCar == null)
            {
                return NotFound();
            }
            // map domain to dto
            return Ok(mapper.Map<CarDTO>(deleteCar));
        }

    }
}
