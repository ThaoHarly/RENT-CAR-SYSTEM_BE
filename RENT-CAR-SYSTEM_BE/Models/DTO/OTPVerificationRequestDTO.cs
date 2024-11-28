namespace RentCarSystem.Models.DTO
{
    public class OTPVerificationRequestDTO
    {
        public string UserId { get; set; }
        public string OTP { get; set; }
        public string UserType { get; set; }
    }
}
