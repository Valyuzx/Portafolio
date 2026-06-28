namespace BACKEND.DTOs
{
    public class ValidateTokenDTO
    {
        public string TokenId { get; set; } = null!; //jti del token
        public string Token { get; set; } = null!;
        public DateTime ExpiryDate { get; set; }
    }
}
