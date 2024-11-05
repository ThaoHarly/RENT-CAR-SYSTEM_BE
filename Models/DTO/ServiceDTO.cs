using RentCarSystem.Models.Domain;

namespace RentCarSystem.Models.DTO
{
    public class ServiceDTO
    {
        public Guid UserId { get; set; }

        public string ServiceType { get; set; } = null!;

        public string BankName { get; set; } = null!;

        public string BankAccount { get; set; } = null!;

        public virtual BusinessDTO? Business { get; set; }

        public virtual IndividualDTO? Individual { get; set; }

    }
}
