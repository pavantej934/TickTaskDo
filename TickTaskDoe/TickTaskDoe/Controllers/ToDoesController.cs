using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TickTaskDoe.Models;

namespace TickTaskDoe.Controllers
{
    public class ToDoesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ToDoes
        public ActionResult Index()
        {          
            return View();
        }

        /// <summary>
        ///Builds a ToDo list for the current user
        /// </summary>
        /// <returns>List of ToDoList's for the current user</returns>
        private IEnumerable<ToDoList> MyToDoList()
        {
            string CurrUserId = User.Identity.GetUserId();
            ApplicationUser CurrUser = db.Users.FirstOrDefault
                (x => x.Id == CurrUserId);

            IEnumerable<ToDoList> currUserToDoList = db.ToDoLists.ToList().Where(x => x.User == CurrUser);

            return currUserToDoList;
        }

        /// <summary>
        ///Builds a ToDo task list for the current user.
        /// </summary>
        /// <returns>List of ToDoTask's for the current user for the given ListId</returns>
        private IEnumerable<ToDoTask> MyToDoTask(int ListId)
        {
            string CurrUserId = User.Identity.GetUserId();
            ApplicationUser CurrUser = db.Users.FirstOrDefault
                (x => x.Id == CurrUserId);

            IEnumerable<ToDoTask> currUserToDoTask = db.ToDoTasks.ToList().Where(x => x.User == CurrUser && x.ListId == ListId);

            //Deriving the % of activities completed in the below function
            int currUserCount = 0;
            foreach (ToDoTask todo in currUserToDoTask)
            {
                if (todo.Done)
                {
                    currUserCount++;
                }
            }
            ViewBag.percentComplete = Math.Round(100f * ((float)currUserCount / (float)currUserToDoTask.Count()));

            return currUserToDoTask;
        }
         /// <summary>
         /// Updates the current ToDo list of current user
         /// </summary>
         /// <returns>Partial view that updates the ToDo list of current user</returns>
        public ActionResult ToDoListTable()
        {
            return PartialView("_ToDoListTable", MyToDoList());
        }

        /// <summary>
        /// Returns a partial view that updates the current task list for the given list id
        /// </summary>
        /// <param name="ListName">List name</param>
        /// <param name="ListID">List ID</param>
        /// <returns>Partial view for the given list id with list of tasks</returns>
        public ActionResult ToDoTaskTable(int ListId,string ListName)
        {
            ViewBag.ListName = ListName;
            TempData["ListId"] = ListId;
            return PartialView("_ToDoTable",MyToDoTask(ListId));
        }

        // GET: ToDoes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ToDoTask toDo = db.ToDoTasks.Find(id);
            if (toDo == null)
            {
                return HttpNotFound();
            }
            return View(toDo);
        }

        // GET: ToDoes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ToDoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Desc,Done")] ToDoTask toDo)
        {
            if (ModelState.IsValid)
            {
                string CurrUserID = User.Identity.GetUserId();
                ApplicationUser CurrUser = db.Users.FirstOrDefault
                    (x => x.Id == CurrUserID);
                toDo.User = CurrUser;
                db.ToDoTasks.Add(toDo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(toDo);
        }

        /// <summary>
        /// This action is used by Ajax box to add new lists
        /// </summary>
        /// <param name="toDo">toDoList model</param>
        /// <returns>partial view that updates List with added list</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AjaxCreateList([Bind(Include = "Id,Desc")] ToDoList toDoList)
        {
            if (ModelState.IsValid)
            {
                string CurrUserID = User.Identity.GetUserId();
                ApplicationUser CurrUser = db.Users.FirstOrDefault
                    (x => x.Id == CurrUserID);
                toDoList.User = CurrUser;
                db.ToDoLists.Add(toDoList);
                db.SaveChanges();
            }

            return PartialView("_ToDoListTable", MyToDoList());
        }

        /// <summary>
        /// This action is used by Ajax box to add new tasks
        /// </summary>
        /// <param name="toDo">toDoTask model</param>
        /// <returns>partial view that updates task list with added task</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AjaxCreateTask([Bind(Include = "Id,Desc")] ToDoTask toDoTask)
        {
            if (ModelState.IsValid)
            {
                string CurrUserID = User.Identity.GetUserId();
                ApplicationUser CurrUser = db.Users.FirstOrDefault
                    (x => x.Id == CurrUserID);
                toDoTask.User = CurrUser;
                toDoTask.Done = false;
                if (TempData.ContainsKey("ListId"))
                {
                    toDoTask.ListId = Convert.ToInt32(TempData["ListId"]);
                }
                db.ToDoTasks.Add(toDoTask);
                db.SaveChanges();
            }

            return PartialView("_ToDoTable", MyToDoTask(toDoTask.ListId));
        }

        // GET: ToDoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ToDoTask toDo = db.ToDoTasks.Find(id);
            if (toDo == null)
            {
                return HttpNotFound();
            }
            //validation for not allowing others users to edit tasks
            string CurrUserID = User.Identity.GetUserId();
            ApplicationUser CurrUser = db.Users.FirstOrDefault
                (x => x.Id == CurrUserID);

            if (toDo.User != CurrUser)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View(toDo);
        }

        // POST: ToDoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Desc,Done")] ToDoTask toDo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(toDo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(toDo);
        }

        //The edit action is used by Ajax call
        [HttpPost]
        public ActionResult AjaxEdit(int? id,bool value)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ToDoTask toDo = db.ToDoTasks.Find(id);
            if (toDo == null)
            {
                return HttpNotFound();
            }
            else
            {
                toDo.Done = value;
                db.Entry(toDo).State = EntityState.Modified;
                db.SaveChanges();
                return PartialView("_ToDoTable", MyToDoTask(1));
            }
        }

        // GET: ToDoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ToDoTask toDo = db.ToDoTasks.Find(id);
            if (toDo == null)
            {
                return HttpNotFound();
            }
            return View(toDo);
        }

        // POST: ToDoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ToDoTask toDo = db.ToDoTasks.Find(id);
            db.ToDoTasks.Remove(toDo);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
