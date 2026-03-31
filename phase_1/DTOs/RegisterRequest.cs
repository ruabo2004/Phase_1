using System.Runtime.CompilerServices;

namespace phase_1.DTOs
{
    public class RegisterRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; }
    }
}
