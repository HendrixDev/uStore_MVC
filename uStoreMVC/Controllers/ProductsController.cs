using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using uStore.DATA.EF;
using uStore.DATA.EF.Services;

namespace uStoreMVC.Controllers
{
    public class ProductsController : Controller
    {
        private uStoreEntities db = new uStoreEntities();

        LinqDAL linq = new LinqDAL();

        // GET: Products
        public ActionResult Index()
        {
            var products = db.Products.Include(p => p.ProductStatus);
            return View(products.ToList());
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
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            ViewBag.ProductStatusID = new SelectList(db.ProductStatuses, "ProductStatusID", "StatusName");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "ProductID,ProductName,ProductDescription,Price,UnitsInStock,ProductImage,ProductStatusID")] Product product, HttpPostedFileBase imageUpload)
        {
            if (ModelState.IsValid)
            {
                string fileName = "NoPicAvailable.png";

                if (imageUpload != null)
                {
                    fileName = imageUpload.FileName;

                    string ext = fileName.Substring(fileName.LastIndexOf("."));

                    string[] goodExts = { ".png", ".gif", ".jpg", ".jpeg" };

                    if (goodExts.Contains(ext.ToLower()))
                    {
                        fileName = Guid.NewGuid() + ext;

                        string savePath = Server.MapPath("~/Content/Images/");

                        Image convertedImage = Image.FromStream(imageUpload.InputStream);

                        int maxSize = 500;

                        int maxThumb = 500;

                        ImageServices.ResizeImage(savePath, fileName, convertedImage, maxSize, maxThumb);
                    }
                }

                product.ProductImage = fileName;

                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProductStatusID = new SelectList(db.ProductStatuses, "ProductStatusID", "StatusName", product.ProductStatusID);
            return View(product);
        }

        // GET: Products/Edit/5
        [Authorize(Roles = "Admin")]
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
            ViewBag.ProductStatusID = new SelectList(db.ProductStatuses, "ProductStatusID", "StatusName", product.ProductStatusID);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "ProductID,ProductName,ProductDescription,Price,UnitsInStock,ProductImage,ProductStatusID")] Product product, HttpPostedFileBase newImage)
        {
            if (ModelState.IsValid)
            {
                //string fileName = "NoPicAvailable.png";

                if (newImage != null)
                {
                    string fileName = newImage.FileName;

                    string ext = fileName.Substring(fileName.LastIndexOf("."));

                    string[] goodExts = { ".png", ".gif", ".jpg", ".jpeg" };

                    if (goodExts.Contains(ext.ToLower()))
                    {
                        fileName = Guid.NewGuid() + ext;

                        string savePath = Server.MapPath("~/Content/Images/");

                        Image convertedImage = Image.FromStream(newImage.InputStream);

                        int maxSize = 500;

                        int maxThumb = 500;

                        ImageServices.ResizeImage(savePath, fileName, convertedImage, maxSize, maxThumb);

                        product.ProductImage = fileName;

                    }
                    else
                    {
                        ViewBag.ErrorMessage = "File exts : .jpg, .jpeg, .gif, .png only";

                        return View(product);
                    }
                }


                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProductStatusID = new SelectList(db.ProductStatuses, "ProductStatusID", "StatusName", product.ProductStatusID);
            return View(product);
        }

        // GET: Products/Delete/5
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult OrderByLowestPrice()
        {
            return View(linq.OrderByLowestPrice());
        }

        public ActionResult OrderByHighestPrice()
        {
            return View(linq.OrderByHighestPrice());
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
