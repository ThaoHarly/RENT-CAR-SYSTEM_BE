using RentCarSystem.Models.Domain;
using System.ComponentModel.DataAnnotations;

namespace RentCarSystem.Models.DTO
{
    public class RegisterRequestDTO
    {
        //User 
        public string PhoneNumber { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string ?Email { get; set; }
        public string Name { get; set; } = null!;
        public string? Nationality { get; set; }
        public string Roles { get; set; } = null!;

        //Is Admin
        public DateTime? LastLogin { get; set; } = null;

        //Is Customer 
        public string? LicenseId { get; set; } = null;

        public string? Class { get; set; } = null;

        public DateOnly? Expire { get; set; } = null;

        public string? Image { get; set; } = null;


        //Is Service

        public string? ServiceType { get; set; } = null;

        public string? BankName { get; set; } = null;

        public string? BankAccount { get; set; } = null;


        //Is Business Service
        public string? Description { get; set; } = null;

        public string? BusinessImg { get; set; } = null;

        public DateOnly? RegistrationDate { get; set; } = null;

        public double? Vat { get; set; } = null;

        public string? IssuingLocation { get; set; } = null;

        public DateOnly? DateOfIssue { get; set; } = null;

        //Is Individual Service

    }
}
