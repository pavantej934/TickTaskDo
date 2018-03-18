using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TickTaskDoe.Models
{
    public class ToDoList
    {
        public int Id { get; set; }
        
        public string Desc { get; set; }

        public virtual ApplicationUser User { get; set; }

        public ICollection<ToDoTask> ToDoTasks { get; set; }
    }
}