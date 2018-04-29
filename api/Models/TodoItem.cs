using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace api.Models
{
    public class TodoItem
    {
        public long Id { get; set; }
        public DateTimeOffset Date { get; set; }
        [Required]
        public string Description { get; set; }
        public bool Done { get; set; } = false;
        [JsonIgnore]
        public long UserId { get; set; }
    }
}