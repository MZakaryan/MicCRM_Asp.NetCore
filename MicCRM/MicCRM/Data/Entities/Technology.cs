using System.ComponentModel.DataAnnotations;

namespace MicCRM.Data.Entities
{
    public class Technology : EntityBase
    {
        [Required]
        public string Name { get; set; }

        public override string ToString()
        {
            return $"{Name}";
        }
    }
}