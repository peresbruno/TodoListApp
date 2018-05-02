using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using api.DataTransferObjects;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace api.Controllers
{
    [Route("api/users")]
    [Authorize]
    public class UsersController : Controller
    {
        private readonly UserContext _userContext;
        
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly JWTSettings _options;

        public UsersController(UserContext userContext,
            UserManager<IdentityUser> userMananager,
            SignInManager<IdentityUser> signInManager,
            IOptions<JWTSettings> optionsAccessor)
        {
            _userContext = userContext;
            _userManager = userMananager;
            _signInManager = signInManager;
            _options = optionsAccessor.Value;

        }
        
        [HttpGet("{userId:long}", Name = "GetUser")]
        public async Task<IActionResult> Get([FromRoute] long userId)
        {
            var user = await _userContext.Users.FindAsync(userId);

            if (user == null) return NotFound();

            return Ok(user);
        }

        [HttpGet(Name = "GetAllUsers")]
        public IActionResult GetAll()
        {
            return Ok(_userContext.Users.ToList());
        }

        [HttpPost(Name = "PostUser")]
        [AllowAnonymous]
        public async Task<IActionResult> Post([FromBody] CreateUserDTO createUserDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = new User
            {
                FirstName = createUserDto.FirstName,
                LastName = createUserDto.LastName,
                Age = createUserDto.Age,
                Email = createUserDto.Email,
                Password = createUserDto.Password
            };

            _userContext.Users.Add(user);
            _userContext.SaveChanges();
            
            var identity = new IdentityUser {UserName = user.Email, Email = user.Email};

            var result = await _userManager.CreateAsync(identity, user.Password);

            if (!result.Succeeded)
            {
                return Errors(result);
            }

            return CreatedAtAction(nameof(Get), new {userId = createUserDto.Id}, user);
        }

        [HttpPut("{userId:long}", Name = "PutUser")]
        public async Task<IActionResult> Put([FromRoute] long userId, [FromBody] EditUserDTO editUserDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = await _userContext.Users.FindAsync(userId);

            if (user == null) return NotFound();

            user.FirstName = editUserDto.FirstName;
            user.LastName = editUserDto.LastName;
            user.Age = editUserDto.Age;

            _userContext.Users.Update(user);
            await _userContext.SaveChangesAsync();

            return Ok(user);
        }

        [HttpDelete("{userId:long}", Name = "DeleteUser")]
        public async Task<ActionResult> Delete([FromRoute] long userId)
        {
            var user = _userContext.Users.Find(userId);

            if (user == null) return NotFound();

            _userContext.Users.Remove(user);
            await _userContext.SaveChangesAsync();
            
            return NoContent();
        }

        [HttpPost("sign-in", Name = "SignInUser")]
        [AllowAnonymous]
        public async Task<ActionResult> SignIn([FromBody] CredentialDTO credential)
        {
            if (!ModelState.IsValid) return new JsonResult("Unexpected error") {StatusCode = 400};

            var user = await _userManager.FindByEmailAsync(credential.Email);

            if (user == null) return BadRequest("Email not found");
            
            var checkPwd = await _signInManager.CheckPasswordSignInAsync(user, credential.Password, false);
            
            if (!checkPwd.Succeeded) return BadRequest("Invalid credentials");
            
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, user.Id),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_options.Issuer,
                _options.Audience,
                claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });

        }
        
        private JsonResult Errors(IdentityResult result)
        {
            var items = result.Errors
                .Select(x => x.Description)
                .ToArray();
            return new JsonResult(items) {StatusCode = 400};
        }
        
    }
}