


using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace JwtWebApiDotNet7.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        public static User user = new User() { };
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("register")]
        public ActionResult<User> Register(UserDto request)
        {
            string passswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
            
            user.Username = request.Username;
            user.PasswordHash = passswordHash;
            
            return Ok(user);
        }

        [HttpPost("login")]
        public ActionResult<User> Login(UserDto request)
        {
            if(user.Username != request.Username)
            {
                return BadRequest("User not found.");
            }

            if(!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                return BadRequest("Invalid password.");
            }

            return Ok(user);
        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim> 
            { 
                new Claim(ClaimTypes.Name, user.Username)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials: creds
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
    }
}
