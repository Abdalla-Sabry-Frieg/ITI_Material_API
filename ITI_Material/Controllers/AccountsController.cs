using ITI_Material.DTOs;
using ITI_Material.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ITI_Material.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   // [Authorize]
    public class AccountsController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public AccountsController(UserManager<ApplicationUser> userManager , IConfiguration configuration)
        {
            _userManager = userManager;
         _configuration = configuration;
        }
        //Create Account new User "Registration" "Post"

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterUserDto userDto)
        {
            if(ModelState.IsValid) 
            {
                //Save
                var user = new ApplicationUser(); 
                user.UserName=userDto.userName;
                user.Email=userDto.Email;

                IdentityResult result =  await _userManager.CreateAsync(user,userDto.Password); // IdentityResult == var

                if (result.Succeeded) 
                {
                    return Ok("Account Added Success");
                }
                else
                {
                    return BadRequest("Account can't Added");
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login (LoginUserDTO loginUserDto) 
        {
            if(ModelState.IsValid == true)
            {
                // check usename && Pass
                var user = await _userManager.FindByNameAsync(loginUserDto.UserName);
                if (user != null) 
                {
                    bool found= await _userManager.CheckPasswordAsync(user, loginUserDto.Password);
                    if (found) 
                    {
                        //Create token
                        var clamis = new List<Claim>();
                        clamis.Add(new Claim(ClaimTypes.Name ,loginUserDto.UserName));  
                        clamis.Add(new Claim(ClaimTypes.NameIdentifier ,user.Id));
                        clamis.Add(new Claim(JwtRegisteredClaimNames.Jti , Guid.NewGuid().ToString()));

                        // Get roles
                        var roles = await _userManager.GetRolesAsync(user);
                        foreach (var role in roles)
                        {
                            clamis.Add(new Claim(ClaimTypes.Role , role));
                        }

                        SecurityKey key =
                            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]));

                        SigningCredentials signing =
                            new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                        JwtSecurityToken token = new JwtSecurityToken(
                            issuer: _configuration["JWT:Issure"],
                            audience: _configuration["JWT:Auduance"],
                            claims: clamis,
                            expires: DateTime.Now.AddHours(3),
                            signingCredentials: signing  // Segnrture

                            );
                        return Ok(new
                        {
                            tokenCreated = new JwtSecurityTokenHandler().WriteToken(token),
                            expiration = token.ValidTo

                        });
                       
                    }
                    else
                    {
                        return NotFound("User no't found");
                    }

                }
                else
                {
                    return Unauthorized();
                }

            }
            else
            {
                return Unauthorized();
            }
        }
    }
}
