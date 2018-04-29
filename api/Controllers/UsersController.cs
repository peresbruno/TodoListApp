using System.Linq;
using api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace api.Controllers
{
    [Route("api/users")]
    public class UsersController : Controller
    {
        private readonly UserContext _userContext;

        public UsersController(UserContext userContext)
        {
            _userContext = userContext;
        }

        [HttpGet("{userId:long}", Name = "GetUser")]
        public ActionResult Get([FromRoute] long userId)
        {
            var user = _userContext.Users.Find(userId);

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
            _userContext.SaveChanges();

            return CreatedAtAction(nameof(Get), new {userId = user.Id}, user);
        }

        [HttpPut("{userId:long}", Name = "PutUser")]
        public ActionResult Put([FromRoute] long userId, [FromBody] User user)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var oldUser = _userContext.Users.Find(userId);

            if (oldUser == null) return NotFound();

            oldUser.FirstName = user.FirstName;
            oldUser.LastName = user.LastName;
            oldUser.Age = user.Age;

            _userContext.Users.Update(oldUser);
            _userContext.SaveChanges();

            return Ok(oldUser);
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
    }
}