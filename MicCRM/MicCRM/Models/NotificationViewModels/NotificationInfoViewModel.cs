using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MicCRM.Models.NotificationViewModels
{
    public class NotificationInfoViewModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string Email { get; set; }
        public DateTime Date { get; set; }
        public string Technology { get; set; }
        public bool IsMuted { get; set; }
    }
}
