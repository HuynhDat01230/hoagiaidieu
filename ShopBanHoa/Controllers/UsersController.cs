using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Security.Cryptography;
using ShopBanHoa.Models;


namespace ShopBanHoa.Controllers
{
    public class UsersController : Controller
    {
        private QLBHEntities db = new QLBHEntities();

        // GET: Users
        public ActionResult Index(string optionU, string searchU)
        {
            var users = db.Users.Include(u => u.UserType);
            if (optionU == "Name")
            {
                users = users.Where(s => s.Name.StartsWith(searchU));
                //return View(db.Users.Where(x => x.Name.StartsWith(searchU)).ToList());
            }
            else if(optionU !=null)
            {
                int i = int.Parse(searchU);
                users = users.Where(s => s.UserTypeID == i);

                //return View(db.Users.Where(x => x.UserTypeID == i).ToList());
            }
            //var users = db.Users.Include(u => u.UserType);
            return View(users.ToList());
        }
        /*public ActionResult SearchFunctionUser(string optionU, string searchU)
        {
            if (optionU == "Name")
            {
                return View(db.Users.Where(x => x.Name.StartsWith(searchU)).ToList());
            }
            else
            {
                int i = int.Parse(searchU);
                return View(db.Users.Where(x => x.UserTypeID == i).ToList());
            }
        }*/

        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }
        // GET: Users/PurchaseHistory/5
        public ActionResult PurchaseHistory(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            ViewBag.UserTypeID = new SelectList(db.UserTypes, "UserTypeID", "Description");
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserID,Name,Email,Password,Phone,ConfirmPassword,Address,UserTypeID")] User user)
        {
            if (ModelState.IsValid)
            {

                var listCustomerID = db.Users;
                int tmp = 0;
                for (int i = 1; i <= listCustomerID.ToList().Count; i++)
                {
                    User user1 = db.Users.Find(i);
                    if (user1 == null) tmp = i;
                }
                user.UserID = tmp;
                Session["UserID"] = user.UserID.ToString();
                user.Password = GetMD5(user.Password);
                user.ConfirmPassword = GetMD5(user.ConfirmPassword);
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserTypeID = new SelectList(db.UserTypes, "UserTypeID", "Description", user.UserTypeID);
            return View(user);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserTypeID = new SelectList(db.UserTypes, "UserTypeID", "Description", user.UserTypeID);
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserID,Name,Email,Password,Phone,ConfirmPassword,Address,UserTypeID")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                user.Password = GetMD5(user.Password);
                user.ConfirmPassword = GetMD5(user.ConfirmPassword);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserTypeID = new SelectList(db.UserTypes, "UserTypeID", "Description", user.UserTypeID);
            return View(user);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
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
        
        
        public static string GetMD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.UTF8.GetBytes(str);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;

            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x2");

            }
            return byte2String;
        }
    }
}
