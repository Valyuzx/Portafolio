namespace BACKEND.DTOs
{
    public class ValidateTokenResponseDTO
    {
        public bool ExistInBlackList { set; get; }
        public bool Expired { set; get; }
    }
}
