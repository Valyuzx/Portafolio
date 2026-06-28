namespace BACKEND.DTOs
{
    public class RefreshTokenResponseDTO
    {
        public string RefreshToken { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
