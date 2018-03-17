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

        public ActionResult showList()
        {
            return PartialView("_ListMenu", GetMenu());
        }

        // Ajax is used to add new Lists
        [HttpPost]
        public ActionResult AjaxListUpdate(string ListName)
        {
            if (ModelState.IsValid)
            {
                string CurrUserID = User.Identity.GetUserId();
                ApplicationUser CurrUser = db.Users.FirstOrDefault
                    (x => x.Id == CurrUserID);
                UserList lst = new UserList();
                lst.ListName = ListName;
                lst.UserID = CurrUserID;              
                db.List.Add(lst);
                db.SaveChanges();
            }

            return PartialView("_ListMenu", GetMenu());
        }

        public Menu GetMenu()
        {
            Menu menu = new Menu();
            menu.Items = new List<MenuItem>();
            string CurrUserId = User.Identity.GetUserId();

            //list name, user ID

            IEnumerable<UserList> UserLists = db.List.ToList().Where(x => x.UserID == CurrUserId).Select(t => new UserList
            {
                ListID = t.ListID,
                ListName = t.ListName,
                UserID = t.UserID
            }) ;

            foreach(UserList l in UserLists)
            {
                menu.Items.Add(new MenuItem() { LinkText = l.ListName, ActionName = "ToDoTable", ControllerName="ToDoes", HTMLArguments= l.ListID.ToString()});
            }

            return menu;
        }

        /// <summary>
        ///Builds a ToDo list for the current user.
        /// </summary>
        /// <returns></returns>
        private IEnumerable<ToDo> MyToDoList(int ListID)
        {
            string CurrUserId = User.Identity.GetUserId();
            ApplicationUser CurrUser = db.Users.FirstOrDefault
                (x => x.Id == CurrUserId);

            IEnumerable<ToDo> currUserToDo = db.ToDos.ToList().Where(x => x.User == CurrUser && x.ListID== ListID);

            //Deriving the % of activities completed in the below function
            int currUserCount = 0;
            foreach (ToDo todo in currUserToDo)
            {
                if (todo.Done)
                {
                    currUserCount++;
                }
            }
            ViewBag.percentComplete = Math.Round(100f * ((float)currUserCount / (float)currUserToDo.Count()));

            return currUserToDo;
        }

        [HttpPost]
        public ActionResult ToDoTable(string ListName, string ListID)
        {
            ViewBag.ListName = ListName;
            return PartialView("_ToDoTable",MyToDoList(Convert.ToInt32(ListID)));
        }

       

        // GET: ToDoes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ToDo toDo = db.ToDos.Find(id);
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
        public ActionResult Create([Bind(Include = "Id,Desc,Done")] ToDo toDo)
        {
            if (ModelState.IsValid)
            {
                string CurrUserID = User.Identity.GetUserId();
                ApplicationUser CurrUser = db.Users.FirstOrDefault
                    (x => x.Id == CurrUserID);
                toDo.User = CurrUser;
                db.ToDos.Add(toDo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(toDo);
        }

        // This action is used by Ajax box to add new items
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AjaxCreate([Bind(Include = "Id,Desc")] ToDo toDo)
        {
            if (ModelState.IsValid)
            {
                string CurrUserID = User.Identity.GetUserId();
                ApplicationUser CurrUser = db.Users.FirstOrDefault
                    (x => x.Id == CurrUserID);
                toDo.User = CurrUser;
                toDo.Done = false;
                db.ToDos.Add(toDo);
                db.SaveChanges();
            }

            return PartialView("_ToDoTable", MyToDoList(1));
        }

        // GET: ToDoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ToDo toDo = db.ToDos.Find(id);
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
        public ActionResult Edit([Bind(Include = "Id,Desc,Done")] ToDo toDo)
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
            ToDo toDo = db.ToDos.Find(id);
            if (toDo == null)
            {
                return HttpNotFound();
            }
            else
            {
                toDo.Done = value;
                db.Entry(toDo).State = EntityState.Modified;
                db.SaveChanges();
                return PartialView("_ToDoTable", MyToDoList(1));
            }
        }

        // GET: ToDoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ToDo toDo = db.ToDos.Find(id);
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
            ToDo toDo = db.ToDos.Find(id);
            db.ToDos.Remove(toDo);
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
