namespace JwtSwaggerHc.API.V2.Models
{
    public class LoginResponse
    {
        public string Token { get; set; }

        public User User { get; set; }
    }
}
