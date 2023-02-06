namespace TestAPI.Users.DTOs
{
    public class UserLoginResultDTO
    {
        public bool Succeeded { get; set; }

        public string Message { get; set; }

        public TokenDTO Token { get; set; }
    }
}
