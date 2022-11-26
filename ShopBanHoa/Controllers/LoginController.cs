using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopBanHoa.Models;
using System.Security.Cryptography;


namespace ShopBanHoa.Controllers
{
    public class LoginController : Controller
    {
        private QLBHEntities db = new QLBHEntities();
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            if (ModelState.IsValid)
            {
                var f_password = GetMD5(password);
                using (QLBHEntities db = new QLBHEntities())
                {
                    var obj = db.Users.Where(a => a.Name.Equals(username) && (a.Password.Equals(f_password) || a.Password.Equals(password))).FirstOrDefault();
                    if (obj != null)
                    {
                        Session["username"] = obj.Name.ToString();
                        Session["userID"] = obj.UserID.ToString();
                        Session["userType"] = obj.UserTypeID.ToString();
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ViewBag.error = "Invalid Account";
                        return View("Index");
                    }
                }
            }

            if (username.Equals("acc1") && password.Equals("123"))
            {
                Session["username"] = "username";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.error = "Invalid Account";
                return View("Index");
            }

        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(User user)
        {
            if (ModelState.IsValid)
            {
                var checkName = db.Users.FirstOrDefault(s => s.Name == user.Name);
                var checkEmail = db.Users.FirstOrDefault(e => e.Email == user.Email);
                var checkPhoneNumber = db.Users.FirstOrDefault(n => n.Phone == user.Phone);
                if (checkName == null && checkEmail == null && checkPhoneNumber == null)
                {
                    var listCustomerID = db.Users;
                    int tmp = 0;
                    for (int i = 1; i <= listCustomerID.ToList().Count; i++)
                    {
                        User user1 = db.Users.Find(i);
                        if (user1 == null) tmp = i;
                    }
                    user.UserID = tmp;
                    user.Password = GetMD5(user.Password);
                    user.ConfirmPassword = GetMD5(user.ConfirmPassword);
                    user.UserTypeID = 3;
                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.Users.Add(user);
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.error = "Username,Email or Phone number may already exists";

                    return View();
                }
            }
            return View();
        }


        [HttpGet]
        public ActionResult Logout()
        {
            Session.Remove("username");
            Session.Remove("userID");
            return RedirectToAction("Index", "Home");
        }

        //create a string MD5

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