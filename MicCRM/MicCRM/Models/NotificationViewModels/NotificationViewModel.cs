using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MicCRM.Models.NotificationViewModels
{
    public class NotificationViewModel
    {
        public IEnumerable<NotificationInfoViewModel> Notifications { get; set; }
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public string Technology { get; set; }
        public bool IsMuted { get; set; }

    }
}
