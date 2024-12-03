namespace RentCarSystem.Reponsitories.IReponsitories
{
    public interface IResetPassWord
    {
        Task<string> CreatePasswordResetRequestAsync(string email);
        Task<string> ResetPasswordAsync(string resetToken, string newPassword);
        Task<string> ChangePasswordAsync(string newPassword, string curPassword, string userId);
    }
}
