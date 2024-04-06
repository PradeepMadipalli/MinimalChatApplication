using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MinimalChatApplication.Model;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MinimalChatApp.Controllers
{
    [Route("api/")]
    [ApiController]
    [Authorize]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _Configuration;
        private readonly UserManager<IdentityUser> _userManager;

        public LoginController(IConfiguration configuration, UserManager<IdentityUser> userManager
            )
        {
            _Configuration = configuration;
            _userManager = userManager;
        }


        [HttpPost]
        [Route("Register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] Register register)
        {
            if (ModelState.IsValid)
            {


                var userExist = await _userManager.FindByEmailAsync(register.Email);
                if (userExist != null)
                {
                    return StatusCode(StatusCodes.Status409Conflict, new Response { Status = "Error", Message = "User Already Exist" });
                }
                IdentityUser user = new()
                {
                    Email = register.Email,
                    UserName = register.Name,
                    SecurityStamp = Guid.NewGuid().ToString(),
                };
                var result = await _userManager.CreateAsync(user, register.Password);
                if (result.Succeeded)
                {
                    string usid = await _userManager.GetUserIdAsync(user);

                    ResponseRegister responseRegister = new ResponseRegister()
                    {
                        userid = usid,
                        username = register.Name,
                        Email = register.Email,
                    };
                    return Ok(responseRegister);

                }
                else
                {
                    return BadRequest("Registration failed due to validation errors");
                    

                }
            }
            else
            {
                return BadRequest("Registration failed due to validation errors");

            }

        }
        [HttpPost]
        [Route("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] Users users)
        {
            var user = await _userManager.FindByEmailAsync(users.Email);
            if (user != null && await _userManager.CheckPasswordAsync(user, users.Password))
            {
                Users users1 = new Users()
                {
                    Email = users.Email,
                    Password = users.Password,
                };
                var authclaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };
                var JWTtoken = GenerateToken(authclaims);

                Profile profile = new Profile()
                {
                    
                    UId = user.Id,
                    Email = user.Email,
                    Name = user.UserName


                };
                
                TokenResponse response = new TokenResponse()
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(JWTtoken),
                    Profiles = profile
                };

                return Ok(response);
            }
            return Unauthorized();
        }


        private JwtSecurityToken GenerateToken(List<Claim> claimss)
        {
            var securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_Configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_Configuration["Jwt:Issuer"],
                _Configuration["Jwt:Audience"],
                claims:claimss,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);


            return token;
        }
        [HttpGet]
        [Route("users")]
        [AllowAnonymous]
        public async Task<IActionResult> GetUsers()
        {
            
            var users = await _userManager.Users.ToListAsync();
            if (users == null)
            {
                StatusCode(404, new { error = "Users not found" });
            }
            List<GetUsers> UserDetails = new List<GetUsers>();

            foreach (var user in users)
            {
                GetUsers v = new GetUsers()
                {
                    UserId = user.Id,
                    UserEmail = user.Email,
                    UserName = user.UserName
                };
                UserDetails.Add(v);
            }
            Userss userss = new Userss()
            {
                Userrs = UserDetails
            };
            return Ok(UserDetails);
        }
    }
}
