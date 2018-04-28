using System;

namespace api.Models
{
    public class TodoItem
    {
        public string Id { get; set; }
        public DateTimeOffset Date { get; set; }
        public string Description { get; set; }
        public bool Done { get; set; } = false;
    }
}