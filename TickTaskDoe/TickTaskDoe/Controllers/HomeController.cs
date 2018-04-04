using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using TickTaskDoe.Models;

namespace TickTaskDoe.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PostLoginIndex()
        {
            return View();
        }


        public ActionResult About()
        {
            ViewBag.Message = "TickTaskDo application description page.";

            return View();
        }

        public ActionResult Help()
        {
            ViewBag.Message = "TickTaskDo help page.";

            return View();
        }


        /// <summary>  
        /// GET: /Home/GetPostLoginSummary  
        /// </summary>  
        /// <returns>Return data</returns>  
        public ActionResult GetPostLoginStatus()
        {
            // Initialization.  
            JsonResult result = new JsonResult();

                string CurrUserId = User.Identity.GetUserId();
                ApplicationUser CurrUser = db.Users.FirstOrDefault(x => x.Id == CurrUserId);
            var DoneCount = db.ToDoTasks.ToList().Where(x => x.User == CurrUser).GroupBy(x => new { x.Done }).Select(
                y => new
            {
                label = y.Key.Done.Equals(false) ? "In Progress" : "Completed",
                count = y.Count()
                });

                result = this.Json(DoneCount, JsonRequestBehavior.AllowGet);
         
            return result;
        }


        /// <summary>  
        /// GET: /Home/GetPostLoginSummary  
        /// </summary>  
        /// <returns>Return data</returns>  
        public ActionResult GetPostLoginTopCategory()
        {
            // Initialization.  
            JsonResult result = new JsonResult();

            string CurrUserId = User.Identity.GetUserId();
            ApplicationUser CurrUser = db.Users.FirstOrDefault(x => x.Id == CurrUserId);

            var Top10Categories = db.ToDoLists  
                                            .Join(db.ToDoTasks, 
                                            x => x.Id,       
                                            y => y.ListId,  
                                            (x, y) => new { Category = x.Desc, UserName = x.User, List = y.ListId }).AsEnumerable() 
                                            .Where(x => x.UserName == CurrUser).GroupBy(x => new { x.Category }).Select(y => new
                                            {
                                                label = y.Key.Category,
                                                count = y.Count()
                                            }).OrderBy(n=>n.count).Take(10);
            result = this.Json(Top10Categories, JsonRequestBehavior.AllowGet);

            return result;
        }

    }
}