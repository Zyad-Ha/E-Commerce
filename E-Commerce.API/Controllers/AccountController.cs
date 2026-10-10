using E_Commerce.BL.DTOs.UserDTOs;
using E_Commerce.DAL.Data;
using E_Commerce.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationContext _context;
        public AccountController(UserManager<ApplicationUser> userManager, ApplicationContext applicationContext)
        {
            _userManager = userManager;
            _context = applicationContext;
        }

        [HttpPost("Register")]
        public async Task<ActionResult> Register(RegisterDTO registerDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var newUser = new ApplicationUser
            {
                UserName = registerDTO.UserName,
                Email = registerDTO.Email
            };

            var result = await _userManager.CreateAsync(newUser, registerDTO.Password);
            if (result.Succeeded)
            {
                Cart cart = new Cart
                {
                    ApplicationUser_Id = newUser.Id
                };
                _context.carts.Add(cart);
                await _context.SaveChangesAsync();

                return Ok($"User {newUser.UserName} registered successfully");
            }
            return Unauthorized(result.Errors.FirstOrDefault());
        }

        [HttpPost("LogIn")]
        public async Task<ActionResult> LogIn(LogInDTO logInDTO)
        {
            if (!ModelState.IsValid) return Unauthorized("Invalid Email or Password");

            var user = await _userManager.FindByEmailAsync(logInDTO.Email);
            if (user == null) return NotFound("Invalid Email or Password");

            var checkPassword = await _userManager.CheckPasswordAsync(user, logInDTO.Password);
            if (!checkPassword) return Unauthorized("Invalid Email or Password");

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email)

            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("poidqpdiquofhknccmccvagfajasagacsbam"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: "https://localhost:7115/",
                audience: "https://localhost:4200/",
                expires: DateTime.Now.AddSeconds(60),
                claims: claims,
                signingCredentials: creds
                );

            return Ok(new JwtSecurityTokenHandler().WriteToken(token));
        }

    }
}
