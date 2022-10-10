namespace WFM_API.Models
{
    public class UserDetailsWithTokenResponceDto
    {
        public string Username { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Role { get; set; } = null!;
        public string? Email { get; set; }
        public string JWTToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
