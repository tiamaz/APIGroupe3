using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
        public AuthController(UserManager<User> userManager, IMapper mapper, RoleManager<Role> roleManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _mapper = mapper;
            _roleManager = roleManager;
            _signInManager = signInManager;
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
               return Ok("L'enregistrement a été bien fait");
           }
           return BadRequest(result.Errors);
        }


        public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
        {
            var user = await _userManager.FindByNameAsync(userForLoginDto.UserName);

            var result = await _signInManager.CheckPasswordSignInAsync(user, userForLoginDto.Password, false);

            if(result.Succeeded) return Ok("OK");
            
            return Unauthorized("UserName ou le mot de passe sont erronés");
        }

            /// Json Web Token
       /*  public async Task<string> GenerateJwtToken(User user, IList<string> roles)
        {



        } */
    }
}