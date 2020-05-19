using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Projet.API.Dto;
using Projet.API.Model;

namespace Projet.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController: ControllerBase
    {

        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        private readonly RoleManager<Role> _roleManager;
        private readonly SignInManager<User> _signInManager;

        private readonly IConfiguration _config;
        public AuthController(UserManager<User> userManager, 
        IMapper mapper, RoleManager<Role> roleManager, 
         SignInManager<User> signInManager,
        IConfiguration config
        )
        {
            _userManager = userManager;
            _mapper = mapper;
            _roleManager = roleManager;
           _signInManager = signInManager;
             _config = config;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto)
        {
            /// Data Transfer object
            /// AutoMapper
            var roles = new List<Role>{
                new Role{Name = "Prof"},
                new Role {Name = "Etudiant"}
            };

            foreach(var role in roles)
            {
                await _roleManager.CreateAsync(role);
            }

            var user = _mapper.Map<User>(userForRegisterDto);

           var result = await _userManager.CreateAsync(user, userForRegisterDto.Password);

           if(result.Succeeded) {
               await _userManager.AddToRoleAsync(user, roles[0].Name);
               return Ok("L'enregistrement réussi");
           }
           return BadRequest(result.Errors);
        }

        [HttpPost("login")]
         public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
        {
            var user = await _userManager.FindByNameAsync(userForLoginDto.UserName);

            var result = await _signInManager.CheckPasswordSignInAsync(user, userForLoginDto.Password, false);


            if(result.Succeeded) {
                var roles = await _userManager.GetRolesAsync(user);
                return Ok( 
                    new {
                        token = GenerateJwtToken(user, roles)
                    });
            }
            

            return Unauthorized("UserName ou le mot de passe sont erronés");
        } 


       
            /// Json Web Token
        private async Task<string> GenerateJwtToken(User user, IList<string> roles)
        {
                //// Spécifier Data
                var claims = new List<Claim>{
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.UserName)
                };

                foreach(var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

                /// Ecrire les informations de Token
                var tokenDescriptor = new SecurityTokenDescriptor{
                    Subject = new ClaimsIdentity(claims),
                    SigningCredentials = creds,
                    Expires = DateTime.Now.AddDays(1)
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescriptor);

                 return tokenHandler.WriteToken(token);

        } 

        [Authorize]
        [HttpGet]
        public IActionResult Hello(){
            return Ok("Valide");
        }
    }
}