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
            if (TempData.ContainsKey("BoolEdit"))
            {
                ViewBag.BoolEdit = 'Y';
                ViewBag.ListId = TempData["ListId"];
                ViewBag.ListName = TempData["ListName"];
                TempData.Remove("BoolEdit");
                TempData.Remove("ListId");
                TempData.Remove("ListName");
            }
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
            if (currUserToDoTask.Count() > 0)
            {
                foreach (ToDoTask todo in currUserToDoTask)
                {
                    if (todo.Done)
                    {
                        currUserCount++;
                    }
                }
                ViewBag.percentComplete = Math.Round(100f * ((float)currUserCount / (float)currUserToDoTask.Count()));
            }

            ViewBag.ListName = db.ToDoLists.First(x => x.Id == ListId).Desc;
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
        public ActionResult AjaxCreateTask([Bind(Include = "Id,Desc,DueDate")] ToDoTask toDoTask)
        {
            string ListID = TempData["ListId"].ToString();
            if (ModelState.IsValid)
            {
                string CurrUserID = User.Identity.GetUserId();
                ApplicationUser CurrUser = db.Users.FirstOrDefault
                    (x => x.Id == CurrUserID);
                toDoTask.User = CurrUser;
                toDoTask.Done = false;
                toDoTask.DueDate = DateTime.Today.Date;
                toDoTask.EmailNotification = false;
                if (TempData.ContainsKey("ListId"))
                {
                    toDoTask.ListId = Convert.ToInt32(TempData["ListId"]);
                }
                int currTaskPriority = 0;
                int rowcount = db.ToDoTasks.ToList().Where(x => x.User == CurrUser && x.ListId == Convert.ToInt32(ListID)).Count();

                if (rowcount > 0)
                    currTaskPriority = db.ToDoTasks.ToList().Where(x => x.User == CurrUser && x.ListId == Convert.ToInt32(ListID)).Select(x => x.TaskPriority).OrderByDescending(x => x).First();

                toDoTask.TaskPriority = currTaskPriority + 1;

                db.ToDoTasks.Add(toDoTask);
                db.SaveChanges();
                TempData["ListId"] = ListID;
            }

            return PartialView("_ToDoTable", MyToDoTask(toDoTask.ListId));

        }

        /// <summary>
        /// Update Task order on row drag and drop. This method fires for every row change during row drag and drop
        /// </summary>
        /// <param name="id"> row id </param>
        /// <param name="fromPosition">from position priority id</param>
        /// <param name="toPosition">to position priority id</param>
        public void UpdateOrder(int id, int fromPosition, int toPosition)
        {
            string ListID = TempData["ListId"].ToString();
            TempData["ListId"] = ListID;
            string CurrUserID = User.Identity.GetUserId();
            ApplicationUser CurrUser = db.Users.FirstOrDefault
                (x => x.Id == CurrUserID);
                // update task priority for all the rows whose priority is changed after drag and drop
                db.ToDoTasks.ToList().First(c => c.User == CurrUser && c.TaskPriority == id && c.ListId == Convert.ToInt32(ListID)).TaskPriority = toPosition;
                db.SaveChanges();
           
        }



        /// <summary>
        /// Called to load the partial view for List edit
        /// </summary>
        /// <param name="id">List Id</param>
        /// <returns>Partial view that enables user to edit list details</returns>
        public ActionResult EditList(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ToDoList toDoList = db.ToDoLists.Find(id);
            if (toDoList == null)
            {
                return HttpNotFound();
            }
            //validation for not allowing others users to edit lists
            string CurrUserID = User.Identity.GetUserId();
            ApplicationUser CurrUser = db.Users.FirstOrDefault
                (x => x.Id == CurrUserID);

            if (toDoList.User != CurrUser)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return PartialView("_EditList", toDoList);
        }

        /// <summary>
        /// Called on save of list edit popup
        /// </summary>
        /// <param name="toDoList">List id and new description</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditList([Bind(Include = "Id,Desc")] ToDoList toDoList)
        {
            if (ModelState.IsValid)
            {
                db.ToDoLists.First(m => m.Id == toDoList.Id).Desc = toDoList.Desc;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        // GET: ToDoes/Edit/5
        /// <summary>
        /// Called to load the partial view for Task Edit
        /// </summary>
        /// <param name="id">Task Id</param>
        /// <returns>Partial view to edit the task details</returns>
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
            return PartialView("_Edit", toDo);
        }

        /// <summary>
        /// Called when save is clicked in popup edit for task
        /// </summary>
        /// <param name="toDoTask">Modified task details</param>
        /// <returns>Index view with curr used lists</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Desc,DueDate,EmailNotification")] ToDoTask toDoTask)
        {
            if (ModelState.IsValid)
            {
                db.ToDoTasks.First(m => m.Id == toDoTask.Id).Desc = toDoTask.Desc;
                db.ToDoTasks.First(m => m.Id == toDoTask.Id).DueDate = toDoTask.DueDate;
                db.ToDoTasks.First(m => m.Id == toDoTask.Id).EmailNotification = toDoTask.EmailNotification;
                db.SaveChanges();
                int ListId = db.ToDoTasks.First(m => m.Id == toDoTask.Id).ListId;
                string ListName = db.ToDoLists.First(m => m.Id == ListId).Desc;
                TempData["BoolEdit"] = 'Y';
                TempData["ListId"] = ListId;
                TempData["ListName"] = ListName;
                return RedirectToAction ("Index");
            }
            return RedirectToAction ("Index");
        }

        //The edit action is used by Ajax call when the check box is modified only
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
                int listId = db.ToDoTasks.FirstOrDefault(x => x.Id == id).ListId;
                toDo.Done = value;
                db.Entry(toDo).State = EntityState.Modified;
                db.SaveChanges();
                return PartialView("_ToDoTable", MyToDoTask(listId));
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

        /// <summary>
        /// Called when user confirms deletion of a list in popup list delete window
        /// </summary>
        /// <param name="id">List id to be deleted</param>
        /// <returns>Index</returns>
        [HttpPost, ActionName("DeleteList")]
        public ActionResult DeleteList(int id)
        {
            //deleting all the corresponding tasks for the list first
            List<ToDoTask> toDoTasks = db.ToDoTasks.Where(m => m.ListId == id).ToList();
            db.ToDoTasks.RemoveRange(toDoTasks);

            //deleting the list
            ToDoList toDoList = db.ToDoLists.First(m => m.Id == id);
            db.ToDoLists.Remove(toDoList);

            db.SaveChanges();

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Called when user clicks 'Go ahead' delete confirmation in delete popup of task
        /// </summary>
        /// <param name="id">Task id to be deleted</param>
        /// <returns>Displays the refreshed task list of the particular list for the user</returns>
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            int ListId = db.ToDoTasks.First(m => m.Id == id).ListId;
            string ListName = db.ToDoLists.First(m => m.Id == ListId).Desc;

            ToDoTask toDo = db.ToDoTasks.Find(id);

            db.ToDoTasks.Remove(toDo);

            string CurrUserID = User.Identity.GetUserId();
            ApplicationUser CurrUser = db.Users.FirstOrDefault
                (x => x.Id == CurrUserID);

            var taskList = db.ToDoTasks.ToList().Where(x => x.TaskPriority > toDo.TaskPriority && x.User == CurrUser && x.ListId == ListId);

            foreach(var listitem in taskList)
            {
                db.ToDoTasks.ToList().First(c => c.Id==listitem.Id && c.User == CurrUser  && c.ListId == ListId).TaskPriority--;
            }
           
            db.SaveChanges();

            return RedirectToAction("ToDoTaskTable", new {ListId = ListId,ListName = ListName});
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Get user login information
        /// </summary>
        /// <param name="userId">user id</param>
        /// <returns>return user information</returns>
        public string[] GetUserDetails(string userId)
        {
            string[] userDetails = new string[2];
            ApplicationUser currUser = db.Users.FirstOrDefault(x => x.Id == userId);

            userDetails[0] = currUser.FirstName;
            userDetails[1] = currUser.LastName;
            return userDetails;
        }
    }
}
