﻿using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace api.Models
{
    public class User
    {
        public long Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        
        [Required]
        public string LastName { get; set; }
        public int Age { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
        
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [JsonIgnore]
        public string Password { get; set; }
    }
}