using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RentCarSystem.Models.Domain;
using RentCarSystem.Models.DTO;
using RentCarSystem.Models.UpdateRequest;
using RentCarSystem.Reponsitories;

namespace RentCarSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MotorController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IMotorRepository motorRepository;

        public MotorController(IMapper mapper, IMotorRepository motorRepository)
        {
            this.mapper = mapper;
            this.motorRepository = motorRepository;
        }

        // Create
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddMotorRequest addMotorRequest)
        {
            var MotorDomainModel = mapper.Map<Motor>(addMotorRequest);
            MotorDomainModel = await motorRepository.CreateAsync(MotorDomainModel);

            // map domain to dto
            return Ok(mapper.Map<MotorDTO>(MotorDomainModel));
        }

        // Get all 
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var MotorDomainModel = await motorRepository.GetAllAsync();

            return Ok(mapper.Map<List<MotorDTO>>(MotorDomainModel));
        }

        // Get By id
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetByID([FromRoute] string id)
        {
            var MotorDomainModel = await motorRepository.GetByIdAsync(id);
            if(MotorDomainModel == null)
            {
                return NotFound();
            }
            // map domain to dto
            return Ok(mapper.Map<MotorDTO>(MotorDomainModel));
        }

        // Update
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromRoute] string id,UpdateMotorRequest updateMotorRequest)
        {
            // map update to domain model
            var MotorDomainModel = mapper.Map<Motor>(updateMotorRequest);
            MotorDomainModel = await motorRepository.UpdateAsync(id,MotorDomainModel);

            if(MotorDomainModel == null)
            {
                return NotFound();
            }

            // map domain model to dto
            return Ok(mapper.Map<MotorDTO>(MotorDomainModel));
        }

        // Delte
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            var deleteMotor = await motorRepository.DeleteAsync(id);
            if(deleteMotor == null)
            {
                return NotFound();
            }

            // map to dto
            return Ok(mapper.Map<MotorDTO>(deleteMotor));
        }

    }
}
