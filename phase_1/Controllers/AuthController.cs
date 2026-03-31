using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using phase_1.Data;
using phase_1.DTOs;
using phase_1.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace phase_1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;
        private readonly EmailService _emailService;
        public AuthController(IConfiguration configuration, ApplicationDbContext context, EmailService emailService)
        {
            _configuration = configuration;
            _context = context;
            _emailService = emailService;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);
            if (existingUser != null)
            {
                return BadRequest("Username already exists.");
            }
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);
            Random random = new Random();
            string otp = random.Next(100000, 999999).ToString();
            var user = new phase_1.Models.Users
            {
                Username = request.Username,
                PasswordHash = hashedPassword,
                Name = request.Name,
                Email = request.Email,
                Role = "User",
                OtpCode = otp
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            _emailService.SendVerificationEmail(request.Email, otp);
            return Ok("Registration successful! Please check your email for the OTP code.");
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);
            if (user != null && BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                if (user.IsEmailVerified == false)
                {
                    return Unauthorized("Please verify your email with the OTP code before logging in.");
                }
                var token = GenerateJwtToken(user);
                return Ok(new { Token = token });
            }
            return Unauthorized("Invalid username or password.");
        }
        private string GenerateJwtToken(phase_1.Models.Users user)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var secretKey = jwtSettings["Key"];
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Name),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, user.Role)
            };
            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpRequest request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (user != null && user.OtpCode == request.OtpCode)
            {
                user.IsEmailVerified = true;
                user.OtpCode = string.Empty;
                await _context.SaveChangesAsync();

                return Ok("OTP verified successfully!");
            }
            return BadRequest("Invalid OTP or email.");
        }
    }
}