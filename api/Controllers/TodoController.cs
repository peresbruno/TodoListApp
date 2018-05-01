using System.Linq;
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
        public ActionResult GetAll([FromRoute] long userId)
        {
            var user = _userContext.Users.Find(userId);

            if (user == null) return NotFound("User not found");

            var todos = from t in _todoItemContext.Todos where t.UserId == user.Id select t;

            return Ok(todos);
        }

        [HttpGet("{todoId:long}", Name = "GetTodoForUser")]
        public ActionResult Get([FromRoute] long userId, [FromRoute] long todoId)
        {
            var user = _userContext.Users.Find(userId);

            if (user == null) return NotFound("User not found");

            var todo = _todoItemContext.Todos.Find(todoId);

            if (todo.UserId != userId) return NotFound("Todo item not found");

            return Ok(todo);
        }

        [HttpPost(Name = "PostTodoForUser")]
        public ActionResult Post([FromRoute] long userId, [FromBody] TodoItem todoItem)
        {
            var user = _userContext.Users.Find(userId);

            if (user == null) return NotFound("User not found");

            todoItem.UserId = userId;

            _todoItemContext.Todos.Add(todoItem);
            _todoItemContext.SaveChanges();
            
            return CreatedAtAction(nameof(Get), new {userId = user.Id, todoId = todoItem.Id}, todoItem);

        }

        [HttpPut("{todoId:long}", Name = "PutTodoForUser")]
        public ActionResult Put([FromRoute] long userId, [FromRoute] long todoId, [FromBody] TodoItem todoItem)
        {
            var user = _userContext.Users.Find(userId);

            if (user == null) return NotFound("User not found");
            
            var todo = _todoItemContext.Todos.Find(todoId);

            if (todo.UserId != userId) return NotFound("Todo item not found");

            todo.Done = todoItem.Done;
            todo.Description = todoItem.Description;
            todo.Date = todoItem.Date;

            _todoItemContext.Todos.Update(todo);
            _todoItemContext.SaveChanges();

            return Ok(todo);
        }

        [HttpDelete("{todoId:long}", Name = "DeleteTodoForUser")]
        public ActionResult Delete([FromRoute] long userId, [FromRoute] long todoId)
        {
            var user = _userContext.Users.Find(userId);

            if (user == null) return NotFound("User not found");
            
            var todo = _todoItemContext.Todos.Find(todoId);

            if (todo.UserId != userId) return NotFound("Todo item not found");

            _todoItemContext.Todos.Remove(todo);
            _todoItemContext.SaveChanges();
            
            return NoContent();
        }
        
    }
}