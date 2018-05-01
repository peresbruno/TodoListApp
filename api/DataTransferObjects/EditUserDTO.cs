using System.ComponentModel.DataAnnotations;

namespace api.DataTransferObjects
{
    public class EditUserDTO
    {
        public long Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        
        [Required]
        public string LastName { get; set; }
        public int Age { get; set; }

    }
}