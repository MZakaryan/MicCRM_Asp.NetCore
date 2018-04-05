using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicCRM.Data.Entities
{
    public class Notification : EntityBase
    {
        public string FirstName { get; set; }
        public string  LastName { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string Email { get; set; }
        public DateTime Date { get; set; }
        public string Technology { get; set; }

    }
}
