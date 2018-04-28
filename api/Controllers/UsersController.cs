using System.Linq;
using api.Models;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    
    public class UsersController : TodoAppController
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
            _userContext.SaveChanges();

            return CreatedAtAction(nameof(Get), new {id = user.Id}, user);
        }

        [HttpPut("{id}", Name = "PutUser")]
        public ActionResult Put(long id, [FromBody] User user)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var oldUser = _userContext.Users.Find(id);

            if (oldUser == null) return NotFound();

            oldUser.FirstName = user.FirstName;
            oldUser.LastName = user.LastName;
            oldUser.Age = user.Age;

            _userContext.Users.Update(oldUser);
            _userContext.SaveChanges();

            return Ok(oldUser);
        }
    }
}