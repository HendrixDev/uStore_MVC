using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using uStore.DATA.EF;

namespace uStoreMVC.Controllers
{
    public class CartController : Controller
    {

        private uStoreEntities db = new uStoreEntities();

        // GET: Cart
        public ActionResult Index()
        {
            var userID = User.Identity.GetUserId();
            var cart = db.CartItems.Where(x => x.UserID == userID).ToList();

            return View(cart);
        }

        public ActionResult Add(int ID)
        {
            CartItem item = new CartItem();
            item.ProductID = ID;
            item.UserID = User.Identity.GetUserId();
            item.DateAdded = DateTime.Now;

            db.CartItems.Add(item);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult Delete(int ID)
        {
            var product = db.CartItems.Find(ID);
            db.CartItems.Remove(product);

            db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}