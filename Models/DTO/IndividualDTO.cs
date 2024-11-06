using RentCarSystem.Models.Domain;

namespace RentCarSystem.Models.DTO
{
    public class IndividualDTO
    {
        public string IdvId { get; set; } = Guid.NewGuid().ToString();

        public string UserId { get; set; }
    }
}
