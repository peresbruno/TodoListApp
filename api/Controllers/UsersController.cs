using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DataTransferObjects;
using api.Models;
using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace api.Controllers
{
    [Route("api/users")]
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
        public ActionResult Get([FromRoute] long userId)
        {
            var user = _userContext.Users.Find(userId);

            if (user == null) return NotFound();

            return Ok(user);
        }

        [HttpGet(Name = "GetAllUsers")]
        [Authorize]
        public ActionResult GetAll()
        {
            return Ok(_userContext.Users.ToList());
        }

        [HttpPost(Name = "PostUser")]
        public async Task<ActionResult> Post([FromBody] CreateUserDTO createUserDto)
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
        public ActionResult Put([FromRoute] long userId, [FromBody] EditUserDTO editUserDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = _userContext.Users.Find(userId);

            if (user == null) return NotFound();

            user.FirstName = editUserDto.FirstName;
            user.LastName = editUserDto.LastName;
            user.Age = editUserDto.Age;

            _userContext.Users.Update(user);
            _userContext.SaveChanges();

            return Ok(user);
        }

        [HttpDelete("{userId:long}", Name = "DeleteUser")]
        public ActionResult Delete([FromRoute] long userId)
        {
            var user = _userContext.Users.Find(userId);

            if (user == null) return NotFound();

            _userContext.Users.Remove(user);
            _userContext.SaveChanges();
            
            return NoContent();
        }

        [HttpPost("sign-in", Name = "SignInUser")]
        public async Task<ActionResult> SignIn([FromBody] CredentialDTO credential)
        {
            if (!ModelState.IsValid) return new JsonResult("Unexpected error") {StatusCode = 400};

            var result = _signInManager.PasswordSignInAsync(credential.Email, credential.Password, false, false);

            if (!result.IsCompletedSuccessfully) return new JsonResult("Unable to sign in") {StatusCode = 401};
            
            var user = await _userManager.FindByEmailAsync(credential.Email);
            return new JsonResult(  new Dictionary<string, object>
            {
                { "access_token", GetAccessToken(credential.Email) },
                { "id_token", GetIdToken(user) }
            });

            return new OkResult();
        }
        
        private JsonResult Errors(IdentityResult result)
        {
            var items = result.Errors
                .Select(x => x.Description)
                .ToArray();
            return new JsonResult(items) {StatusCode = 400};
        }
        
        private string GetAccessToken(string Email) {
            var payload = new Dictionary<string, object>
            {
                { "sub", Email },
                { "email", Email }
            };
            return GetToken(payload);
        }

        private string GetToken(IDictionary<string, object> payload) {
            var secret = _options.SecretKey;

            payload.Add("iss", _options.Issuer);
            payload.Add("aud", _options.Audience);
            payload.Add("nbf", ConvertToUnixTimestamp(DateTime.Now));
            payload.Add("iat", ConvertToUnixTimestamp(DateTime.Now));
            payload.Add("exp", ConvertToUnixTimestamp(DateTime.Now.AddDays(7)));
            IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
            IJsonSerializer serializer = new JsonNetSerializer();
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);

            return encoder.Encode(payload, secret);
        }
        
        private static double ConvertToUnixTimestamp(DateTime date)
        {
            var origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            var diff = date.ToUniversalTime() - origin;
            return Math.Floor(diff.TotalSeconds);
        }
        
        private string GetIdToken(IdentityUser user) {
            var payload = new Dictionary<string, object>
            {
                { "id", user.Id },
                { "sub", user.Email },
                { "email", user.Email },
                { "emailConfirmed", user.EmailConfirmed },
            };
            return GetToken(payload);
        }

    }
}