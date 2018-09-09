using HomeShoppe.Common;
using HomeShoppe.Models;
using Model.DAO;
using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HomeShoppe.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            ViewBag.Slides= new SlideDAO().ListAll();
            var productDAO = new ProductDAO();
            ViewBag.NewProducts = productDAO.ListNewProduct(4);
            ViewBag.ListFeatureProducts = productDAO.ListFeatureProduct(4);
            return View();
        }

        [ChildActionOnly]
        // Use Cache Server with Duration 
        // Use can use Duration to Transection 
        [OutputCache(Duration =3600*24)]
        public ActionResult MainMenu()
        {
            var model = new MenuDAO().ListByGroupID(1);
            return PartialView(model);
        }

        [ChildActionOnly]
        [OutputCache(Duration = 3600 * 24)]
        public ActionResult TopMenu()
        {
            var model = new MenuDAO().ListByGroupID(2);
            return PartialView(model);
        }

        [ChildActionOnly]
        //[OutputCache(Duration = 3600 * 24)]
        public PartialViewResult HeaderCart()
        {
            var cart = Session[CommonConstants.CART_SESSION];
            var list = new List<CartItem>();
            if (cart != null)
            {
                list = (List<CartItem>)cart;
            }
            return PartialView(list);
        }

        [ChildActionOnly]
        [OutputCache(Duration =3600*24)]
        public ActionResult Footer()
        {
            var model = new FooterDAO().GetFooter();
            return PartialView(model);
        }
    }
}