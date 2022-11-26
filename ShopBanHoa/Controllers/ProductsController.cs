using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.IO;
using ShopBanHoa.Models;

namespace ShopBanHoa.Controllers
{
    public class ProductsController : Controller
    {
        private QLBHEntities db = new QLBHEntities();
        
        // GET: Products
        public ActionResult Index(string option, string search, int CategoriesID = 0)
        {
            List<Category> cate = db.Categories.ToList();
            var product = db.Products.Include(p => p.Category);
            SelectList cateList = new SelectList(cate, "CategoryId", "CategoryName");
            ViewBag.CategoriesID = cateList;
            if(CategoriesID != 0)
            {
                product = product.Where(s => s.CategoryId == CategoriesID);
                
            }
            if (option == "PName")
            {
                product = product.Where(s => s.PName.StartsWith(search));
                //return View(db.Users.Where(x => x.Name.StartsWith(searchU)).ToList());
            }
            else if (option != null)
            {
                int i = int.Parse(search);
                product = product.Where(s => s.CategoryId == i);

                //return View(db.Users.Where(x => x.UserTypeID == i).ToList());
            }
            return View(product.ToList());
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductID,PName,PType,PPrice,PDescription,CategoryId,Img")] Product product, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                product.Img = "/Content/img/" + file.FileName;
                var listProductID = db.Products;
                int tmp = 0;
                for (int i = 1; i <= listProductID.ToList().Count; i++)
                {
                    Product product1 = db.Products.Find(i);
                    if (product1 == null) tmp = i;
                }
                product.ProductID = tmp;
                db.Products.Add(product);
                db.SaveChanges();
                Response.Write("<script language=javascript>alert('Message here.')</script>");
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName", product.CategoryId);
            return View(product);    
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName", product.CategoryId);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductID,PName,PType,PPrice,PDescription,CategoryId,Img")] Product product, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                product.Img = "/Content/img/" + file.FileName;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName", product.CategoryId);
            return View(product);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
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

        /*public ActionResult SearchFunction(string option, string search)
        {
            var product = db.Products.Include(p => p.Category);
            if (option == "PName")
            {
                product = product.Where(s => s.PName.StartsWith(search));
                //return View(db.Users.Where(x => x.Name.StartsWith(searchU)).ToList());
            }
            else if (option != null)
            {
                int i = int.Parse(search);
                product = product.Where(s => s.CategoryId == i);

                //return View(db.Users.Where(x => x.UserTypeID == i).ToList());
            }
            return View(product.ToList());
        }*/

        [HttpPost]
        public ActionResult imgupload(FormCollection fc, HttpPostedFileBase file)
        {
            Product tbl = new Product();
            var allowedExtensions = new[] { ".Jpg", ".png", ".jpg", "jpeg" };
            tbl.ProductID = Int32.Parse(fc["ProductID"].ToString());
            tbl.Img = file.ToString(); //getting complete url  
            tbl.PName = fc["PName"].ToString();
            var fileName = Path.GetFileName(file.FileName); //getting only file name(ex-ganesh.jpg)  
            var ext = Path.GetExtension(file.FileName); //getting the extension(ex-.jpg)  
            if (allowedExtensions.Contains(ext)) //check what type of extension  
            {
                tbl.Img = "/Content/img/" + file.FileName;
                db.Products.Add(tbl);
                db.SaveChanges();
            }
            else
            {
                ViewBag.message = "Please choose only Image file";
            }
            return View();
        }
    }
}
