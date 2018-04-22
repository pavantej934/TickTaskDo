using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TickTaskDoe.Models
{
    public class ToDoTask
    {
        public int Id { get; set; }

        public string Desc { get; set; }

        public bool Done { get; set; }

        public bool EmailNotification { get; set; }

        public DateTime? DueDate { get; set; }
        public int TaskPriority { get; set; }

        public virtual ApplicationUser User { get; set; }

        [ForeignKey("ToDoList")]
        public int ListId { get; set; }
        public virtual ToDoList ToDoList { get; set; }

    }
}