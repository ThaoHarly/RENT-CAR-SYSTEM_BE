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

        // Get all 
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var MotorDomainModel = await motorRepository.GetAllAsync();

            return Ok(mapper.Map<List<MotorDTO>>(MotorDomainModel));
        }

        // Get By id
        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetByID([FromRoute] Guid id)
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
        [Route("{id:guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id,UpdateMotorRequest updateMotorRequest)
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
        [Route("{id:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
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
