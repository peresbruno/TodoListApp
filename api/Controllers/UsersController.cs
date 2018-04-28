using System.Collections.Generic;
using System.Linq;
using api.Models;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly UserContext _userContext;

        public UsersController(UserContext userContext)
        {
            _userContext = userContext;
        }

        [HttpGet("{id}", Name = "GetUser")]
        public ActionResult Get(long id)
        {
            var user = _userContext.Users.Find(id);

            if (user == null) return NotFound();

            return Ok(user);
        }

        [HttpGet(Name = "GetAllUsers")]
        public ActionResult GetAll()
        {
            return Ok(_userContext.Users.ToList());
        }

        [HttpPost(Name = "PostUser")]
        public ActionResult Post([FromBody] User user)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            _userContext.Users.Add(user);

            return CreatedAtAction(nameof(Get), new {id = user.Id}, user);
        }
    }
}