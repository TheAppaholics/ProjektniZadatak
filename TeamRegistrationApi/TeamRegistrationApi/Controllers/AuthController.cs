using Microsoft.AspNetCore.Mvc;
using TeamRegistrationApi.Models;

namespace TeamRegistrationApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterRequest request)
        {
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest(new { Message = "Please fill all required fields." });
            }

            if (UserStore.Users.Any(u => u.Email == request.Email))
            {
                return BadRequest(new { Message = "User with this email already exists." });
            }

            UserStore.Users.Add(new User
            {
                Email = request.Email,
                Password = request.Password  
            });

            return Ok(new RegisterResponse { Message = "Registration successful" });
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            var user = UserStore.Users.SingleOrDefault(u => u.Email == request.Email);

            if (user == null || user.Password != request.Password)
            {
                return Unauthorized(new { Message = "Invalid email or password" });
            }

            // Fake token za test
            var token = "fake-jwt-token-for-testing";

            return Ok(new LoginResponse
            {
                Token = token,
                Message = "Login successful",
                Email = user.Email
            });
        }
    }
}
