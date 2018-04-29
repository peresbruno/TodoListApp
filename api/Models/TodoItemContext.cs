using Microsoft.EntityFrameworkCore;

namespace api.Models
{
    public class TodoItemContext : DbContext
    {        
        public DbSet<TodoItem> Todos { get; set; }
        
        public TodoItemContext(DbContextOptions<TodoItemContext> options) : base(options) {}
    }
}