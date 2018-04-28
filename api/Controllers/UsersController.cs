using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        // GET
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] {"User1", "User2"};
        }
    }
}