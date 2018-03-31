using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TickTaskDoe.Models
{
    public class NationalityGreeting
    {
        public int Id { get; set; }

        public string Nation { get; set; }

        public string Greeting { get; set; }

        public ICollection<ApplicationUser> Users { get; set; } 
    }
}