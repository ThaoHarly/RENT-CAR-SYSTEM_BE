using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RentCarSystem.Models.Domain;
using RentCarSystem.Models.DTO;
using RentCarSystem.Reponsitories;
using System.Security.Claims;

namespace RentCarSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RentalAgreementController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IVehicleReponsitory vehicleReponsitory;
        private readonly IRentalAgreementReponsitory rentalAgreementReponsitory;

        public RentalAgreementController(IMapper mapper, IVehicleReponsitory vehicleReponsitory, IRentalAgreementReponsitory rentalAgreementReponsitory)
        {
            this.mapper = mapper;
            this.vehicleReponsitory = vehicleReponsitory;
            this.rentalAgreementReponsitory = rentalAgreementReponsitory;
        }



        //GET: api/createAgreement
        [Authorize(Policy = "Customer")]
        [HttpPost]
        public async Task<IActionResult> createAgreement(CreateRentalAgreementDTO createRentalAgreementDTO)
        {
            //Check valid vehicle status
            var vehicleDomain = await vehicleReponsitory.GettByIdAsync(createRentalAgreementDTO.VehicleId);
            if(vehicleDomain == null || vehicleDomain.Status.ToUpper() != "AVAILABILITY")
            {
                return BadRequest("Vehicle is not available for rental ...");
            }

            //Get userId's Customer is logining
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);


            //Update Vehicle status is 'Rented'
            vehicleDomain.Status = "RENTED";
            await vehicleReponsitory.UpdateAsync(createRentalAgreementDTO.VehicleId, vehicleDomain);

            //Map createRentalAgreementDTO to RentalAgreementDomain 
            var rentalAgreementDomain = mapper.Map<RentalAgreement>(createRentalAgreementDTO);

            //Save RentalAgreement into DB
            rentalAgreementDomain = await rentalAgreementReponsitory.createRentalAgreementAsync(rentalAgreementDomain, vehicleDomain, userId);

            //Map RentalAgreementDomain to RentalAgreementDTO
            return Ok(mapper.Map<RentalAgreementDTO>(rentalAgreementDomain));
        }
    }
}
