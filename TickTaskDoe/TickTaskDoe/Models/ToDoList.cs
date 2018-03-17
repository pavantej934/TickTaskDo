using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TickTaskDoe.Models
{
    [Table("List")]
    public class UserList
    {
        [Key]
        public int ListID { get; set; }
       public string ListName { get; set; }
       public string UserID { get; set; }
    }

    public class Menu
    {
        public List<MenuItem> Items { get; set; }
    }

    public class MenuItem
    {
        public string LinkText { get; set; }
        public string ActionName { get; set; }
        public string ControllerName { get; set; }
        public string HTMLArguments { get; set; }
    }

    public class ListMenu
    {
        public ToDo todo { get; set; }
        public Menu menu { get; set; }

    }
}