using System.Linq;
using System.Threading.Tasks;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/users/{userId:long}/todos")]
    [Authorize]
    public class TodoController : Controller
    {
        private readonly UserContext _userContext;
        private readonly TodoItemContext _todoItemContext;
        
        public TodoController(UserContext userContext, TodoItemContext todoItemContext)
        {
            _userContext = userContext;
            _todoItemContext = todoItemContext;
        }

        [HttpGet(Name = "GetTodosForUser")]
        public async Task<ActionResult> GetAll([FromRoute] long userId)
        {
            var user = await _userContext.Users.FindAsync(userId);

            if (user == null) return NotFound("User not found");

            var todos = from t in _todoItemContext.Todos where t.UserId == user.Id select t;

            return Ok(todos);
        }

        [HttpGet("{todoId:long}", Name = "GetTodoForUser")]
        public async Task<IActionResult> Get([FromRoute] long userId, [FromRoute] long todoId)
        {
            var user = await _userContext.Users.FindAsync(userId);

            if (user == null) return NotFound("User not found");

            var todo = await _todoItemContext.Todos.FindAsync(todoId);

            if (todo?.UserId != userId) return NotFound("Todo item not found");

            return Ok(todo);
        }

        [HttpPost(Name = "PostTodoForUser")]
        public async Task<IActionResult> Post([FromRoute] long userId, [FromBody] TodoItem todoItem)
        {
            var user = await _userContext.Users.FindAsync(userId);

            if (user == null) return NotFound("User not found");

            todoItem.UserId = userId;

            await _todoItemContext.Todos.AddAsync(todoItem);
            await _todoItemContext.SaveChangesAsync();
            
            return CreatedAtAction(nameof(Get), new {userId = user.Id, todoId = todoItem.Id}, todoItem);
        }

        [HttpPut("{todoId:long}", Name = "PutTodoForUser")]
        public async Task<IActionResult> Put([FromRoute] long userId, [FromRoute] long todoId, [FromBody] TodoItem todoItem)
        {
            var user = await _userContext.Users.FindAsync(userId);

            if (user == null) return NotFound("User not found");
            
            var todo = await _todoItemContext.Todos.FindAsync(todoId);

            if (todo?.UserId != userId) return NotFound("Todo item not found");

            todo.Done = todoItem.Done;
            todo.Description = todoItem.Description;
            todo.Date = todoItem.Date;

            _todoItemContext.Todos.Update(todo);
            await _todoItemContext.SaveChangesAsync();

            return Ok(todo);
        }

        [HttpDelete("{todoId:long}", Name = "DeleteTodoForUser")]
        public async Task<IActionResult> Delete([FromRoute] long userId, [FromRoute] long todoId)
        {
            var user = await _userContext.Users.FindAsync(userId);

            if (user == null) return NotFound("User not found");
            
            var todo = await _todoItemContext.Todos.FindAsync(todoId);

            if (todo?.UserId != userId) return NotFound("Todo item not found");

            _todoItemContext.Todos.Remove(todo);
            await _todoItemContext.SaveChangesAsync();
            
            return NoContent();
        }
    }
}