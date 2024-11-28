using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentCarSystem.Models.Domain;
using RentCarSystem.Models.DTO;
using RentCarSystem.Models.VNPay;
using RentCarSystem.Reponsitories;
using RentCarSystem.Service.VNPay;
using System.Security.Claims;

namespace RentCarSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IVnPayService _vnPayService;
        private readonly IRentalAgreementReponsitory rentalAgreementReponsitory;
        private readonly IMapper mapper;
        private readonly IVehicleReponsitory vehicleReponsitory;
        private readonly IBillReponsitory bill;

        public PaymentController(IVnPayService vnPayService, IRentalAgreementReponsitory rentalAgreementReponsitory, IMapper mapper, IVehicleReponsitory vehicleReponsitory, IBillReponsitory bill)
        {

            _vnPayService = vnPayService;
            this.rentalAgreementReponsitory = rentalAgreementReponsitory;
            this.mapper = mapper;
            this.vehicleReponsitory = vehicleReponsitory;
            this.bill = bill;
        }

        [HttpGet("PaymentCallbackVnpay/Payment")]
        public async Task<IActionResult> PaymentCallbackVnpay()
        {
            var response = _vnPayService.PaymentExecute(Request.Query);

            if(!response.Success)
            {
                return Ok("Payment failure ... ");
            }


            if(response.PaymentType.ToUpper() == "DEPOSIT")
            {
                //Create Bill
                var billDomain = await bill.createBill(response);

                //Update RentalAgreement
                var rentalAgreementDomain = await rentalAgreementReponsitory.updateRentalAgreementAsync(response.RentalAgreementId);
            }
            else if(response.PaymentType.ToUpper() == "REMAINING")
            {
                //Create Bill
                var billDomain = await bill.createBill(response);

                //Update RentalAgreement
                var rentalAgreementDomain = await rentalAgreementReponsitory.updateRentalAgreementAsync2(response.RentalAgreementId);

                //Update Vehicle status is 'AVAILABILITY'
                var vehicleDomain = await vehicleReponsitory.GettByIdAsync(response.VehicleId);
                if (vehicleDomain == null || vehicleDomain.Status.ToUpper() != "AVAILABILITY")
                {
                    vehicleDomain.Status = "AVAILABILITY";
                    await vehicleReponsitory.UpdateAsync(response.VehicleId, vehicleDomain);
                }
            }
            return Ok(response);
        }

        [Authorize(Policy = "Customer")]
        [HttpPost("CreatePaymentUrlVnpay")]
        public async Task<IActionResult> createAgreement(PaymentInformationModel model)
        {
            //Create RentalAgreement 
            //Check valid vehicle status and PaymentType 
            if (model.PaymentType.ToUpper() == "DEPOSIT")
            {
                var vehicleDomain = await vehicleReponsitory.GettByIdAsync(model.VehicleId);
                if (vehicleDomain == null || vehicleDomain.Status.ToUpper() != "AVAILABILITY")
                {
                    return BadRequest("Vehicle is not available for rental ...");
                }

                //Get userId's Customer is logining
                var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);


                //Update Vehicle status is 'Rented'
                vehicleDomain.Status = "RENTED";
                await vehicleReponsitory.UpdateAsync(model.VehicleId, vehicleDomain);

                //Map createRentalAgreementDTO to RentalAgreementDomain 
                var rentalAgreementDomain = mapper.Map<RentalAgreement>(model);

                //Save RentalAgreement into DB
                rentalAgreementDomain = await rentalAgreementReponsitory.createRentalAgreementAsync(rentalAgreementDomain, vehicleDomain, userId);

                // Create Url Payment
                model.Amount = rentalAgreementDomain.DepositAmount;
                model.RentalAgreementId = rentalAgreementDomain.AgreementId;
                var url = _vnPayService.CreatePaymentUrl(model, HttpContext);

                return Ok(url);
            }

            else if(model.PaymentType.ToUpper() == "REMAINING")
            {
                var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

                // Retrieve the existing rental agreement based on UserId and VehicleId
                var rentalAgreementDomain = await rentalAgreementReponsitory.GetActiveRentalAgreementAsync(userId, model.VehicleId);
                if (rentalAgreementDomain == null)
                {
                    return BadRequest("No active rental agreement found for this vehicle.");
                }

                // Create payment URL for the remaining amount
                model.Amount = rentalAgreementDomain.RemainingAmount;
                model.RentalAgreementId = rentalAgreementDomain.AgreementId;
                var url = _vnPayService.CreatePaymentUrl(model, HttpContext);

                return Ok(url);
            }
            return BadRequest("Invalid payment type");
        }
    }
}
