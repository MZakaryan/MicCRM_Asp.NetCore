using System.ComponentModel.DataAnnotations;

namespace MicCRM.Data.Entities
{
    public class Teacher : EntityBase
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        public string Email { get; set; }

        public override string ToString()
        {
            return $"{FirstName} {LastName}";
        }
    }
}