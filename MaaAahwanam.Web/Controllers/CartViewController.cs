using MaaAahwanam.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaaAahwanam.Service;
using MaaAahwanam.Utility;
using MaaAahwanam.Web.Custom;

namespace MaaAahwanam.Web.Controllers
{
    public class CartViewController : Controller
    {
        CartService cartService = new CartService();
        //
        // GET: /CartView/
        [Authorize]
        public ActionResult Index()
        {
            //if (ValidUserUtility.ValidUser() != 0 && (ValidUserUtility.UserType() == "User" || ValidUserUtility.UserType() == "Vendor"))
            //{
            //int vid = ValidUserUtility.ValidUser();
            var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
            List<GetCartItems_Result> cartlist = cartService.CartItemsList(int.Parse(user.UserId.ToString()));
            decimal total = cartlist.Sum(s => s.TotalPrice);
            ViewBag.Cartlist = cartlist;
            ViewBag.Total = total;
            return View();
            //}
            //return RedirectToAction("index", "Signin");
        }
    }
}