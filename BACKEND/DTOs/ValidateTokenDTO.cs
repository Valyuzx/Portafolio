namespace BACKEND.DTOs
{
    public class ValidateTokenDTO
    {
        public string TokenId { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiryDate { get; set; }
    }
}
