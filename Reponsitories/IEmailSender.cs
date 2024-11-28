using RentCarSystem.Models.DTO;

namespace RentCarSystem.Reponsitories
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message); // gui mail
        Task SendOTP(MailRequestDTO mailRequestDTO);
        Task SendRegistrationSuccessEmail(string email, string userType, string name);

    }
}
