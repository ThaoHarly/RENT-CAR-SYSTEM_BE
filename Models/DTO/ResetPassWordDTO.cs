namespace RentCarSystem.Models.DTO
{
    public class ResetPassWordDTO
    {
        public string Token { get; set; }        // Mã xác thực (token) từ email
        public string NewPassword { get; set; }  // Mật khẩu mới
    }
    
}
