using Library.Data;
using Library.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Library.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly LibraryContext _context;

        public AuthController(LibraryContext context)
        {
            _context = context;
        }

        public class LoginDto
        {
            public required string Username { get; set; }
            public required string Password { get; set; }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var passwordHasher = new PasswordHasher<User>();
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == loginDto.Username);
            if (user == null)
            {
                return Unauthorized("Invalid username or password.");
            }
            var verifyResult = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, loginDto.Password);

            if (verifyResult != PasswordVerificationResult.Success)
            {
                return Unauthorized("Invalid username or password.");
            }

            // Create user claims for authentication
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role.ToString())  // Store the user role
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true, // Keeps session active even after closing the browser
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, 
                                          new ClaimsPrincipal(claimsIdentity), 
                                          authProperties);

            return Ok("Logged in successfully.");
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok("Logged out successfully.");
        }

        [HttpGet("check-session")]
        public IActionResult CheckSession()
        {
            if (User.Identity?.IsAuthenticated ?? false)
            {
                return Ok(new { username = User.Identity.Name, role = User.FindFirst(ClaimTypes.Role)?.Value });
            }
            return Unauthorized("No active session.");
        }
    }
}