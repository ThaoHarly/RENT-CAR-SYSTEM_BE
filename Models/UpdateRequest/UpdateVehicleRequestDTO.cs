﻿using System.ComponentModel.DataAnnotations;

namespace RentCarSystem.Models.UpdateRequest
{
    public class UpdateVehicleRequestDTO
    {
        //Is Vehicle

        public string LicensePlate { get; set; } = null!;

        public double PricePerDay { get; set; }

        public double FuelConsumption { get; set; }

        public double Range { get; set; }

        public double EngineCapacity { get; set; }
        public string status { get; set; }


        //Is Motor
        public string? MotorImage { get; set; } = null;

        //Is Car
        public string? CarBrand { get; set; } = null;

        public string? FuelType { get; set; } = null;

        public int? SeatingCapacity { get; set; } = null;

        public string? CarImage { get; set; } = null;

        public double? ChargingTime { get; set; } = null;
    }
}