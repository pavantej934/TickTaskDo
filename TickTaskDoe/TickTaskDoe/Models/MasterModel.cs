using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TickTaskDoe.Models
{
    public class MasterModel
    {
        public ToDoList toDoList { get; set; }

        public ToDoTask toDoTask { get; set; }
    }
}