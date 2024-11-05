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
    public class BusinessController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IBusinessRepository businessRepository;

        public BusinessController(IMapper mapper, IBusinessRepository businessRepository)
        {
            this.mapper = mapper;
            this.businessRepository = businessRepository;
        }

        // Get All
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? filterOn, [FromQuery] string? filterQuery,
                                                [FromQuery] string? sortBy, [FromQuery] bool? isAscending,
                                                [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 1000)
        {
            var bnsDomainModel = await businessRepository.GetAllAsync(filterOn,filterQuery,sortBy,isAscending ?? true,
                                                                      pageNumber,pageSize);

            // map domain to dto
            return Ok(mapper.Map<List<BusinessDTO>>(bnsDomainModel));
        }

        // Get by id
        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var bnsDomainModel = await businessRepository.GetByIdAsync(id);
            if (bnsDomainModel == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<BusinessDTO>(bnsDomainModel));
        }

        // Update
        [HttpPut]
        [Route("{id:guid}")]
        [ValidateModel]
        public async Task<IActionResult> Update([FromRoute] Guid id, UpdateBusinessRequest updateBusinessRequest)
        {
            // map update to domain
            var bnsDomainModel = mapper.Map<Business>(updateBusinessRequest);

            bnsDomainModel = await businessRepository.UpdateAsync(id, bnsDomainModel);
            if (bnsDomainModel == null)
            {
                return NotFound();
            }

            // map domain to dto
            return Ok(mapper.Map<BusinessDTO?>(bnsDomainModel));

        }

        // Delete
        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var deleteBns = await businessRepository.DeleteAsync(id);
            if(deleteBns == null)
            {
                return NotFound();
            }

            // map to dto
            return Ok(mapper.Map<BusinessDTO>(deleteBns));
        }
    }
}
