using MaaAahwanam.Models;
using MaaAahwanam.Repository;
using MaaAahwanam.Service;
using MaaAahwanam.Utility;
using MaaAahwanam.Web.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace MaaAahwanam.Web.Controllers
{
    public class cartController : Controller
    {
        cartservices cartserve = new cartservices();
        CartService cartService = new CartService();
        UserLoginDetailsService userLoginDetailsService = new UserLoginDetailsService();

        // GET: cart
        public ActionResult Index()
        {
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
                if (user.UserType == "User")
                {
                    var userdata = userLoginDetailsService.GetUser((int)user.UserId);
                    if (user.UserType == "Admin")
                    {
                        ViewBag.cartCount = cartService.CartItemsCount(0);
                        return PartialView("ItemsCartdetails");
                    }
                    ViewBag.cartCount = cartService.CartItemsCount((int)user.UserId);

                    List<GetCartItems_Result> cartlist = cartService.CartItemsList(int.Parse(user.UserId.ToString()));
                    decimal total = cartlist.Where(m => m.Status == "Active").Sum(s => s.TotalPrice);
                    ViewBag.cartitems = cartlist.OrderByDescending(m => m.UpdatedDate).Where(m => m.Status == "Active");
                    ViewBag.Total = total;

                }
            }
            else
            {
                ViewBag.cartCount = cartService.CartItemsCount(0);
            }


            return View();
        }

       

      
    }
}

