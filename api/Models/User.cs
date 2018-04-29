using System.ComponentModel.DataAnnotations;

namespace api.Models
{
    public class User
    {
        public long Id { get; set; }
        [Required]
        [MinLength(5)]
        public string FirstName { get; set; }
        [Required]
        [MinLength(5)]
        public string LastName { get; set; }
        public int Age { get; set; }
    }
}