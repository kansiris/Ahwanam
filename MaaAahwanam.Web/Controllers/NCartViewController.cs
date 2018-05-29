using MaaAahwanam.Models;
using MaaAahwanam.Repository;
using MaaAahwanam.Service;
using MaaAahwanam.Web.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MaaAahwanam.Web.Controllers
{
    public class NCartViewController : Controller
    {

        CartService cartService = new CartService();
        UserLoginDetailsService userLoginDetailsService = new UserLoginDetailsService();
        WhishListService whishListService = new WhishListService();

        // GET: NCartView
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
                    decimal total = cartlist.Sum(s => s.TotalPrice);
                    ViewBag.cartitems = cartlist;
                    ViewBag.Total = total;
                }
            }
            else
            {
                ViewBag.cartCount = cartService.CartItemsCount(0);
            }
            return View();
        }

        public JsonResult DeletecartItem(long cartId)
        {
            var message = cartService.Deletecartitem(cartId);
            return Json(message);
        }

        public JsonResult addwishlistItem(long cartId)
        {
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
                if (user.UserType == "User")
                {
                    var userdata = userLoginDetailsService.GetUser((int)user.UserId);
                    ViewBag.cartCount = cartService.CartItemsCount((int)user.UserId);
                    var cartlist = cartService.CartItemsList(int.Parse(user.UserId.ToString()));
                    var cartdetails = cartlist.Where(m => m.Id == cartId).FirstOrDefault();
                    AvailableWhishLists availableWhishLists = new AvailableWhishLists();
                    availableWhishLists.VendorID = cartdetails.Id.ToString();
                    availableWhishLists.VendorSubID = (cartdetails.subid).ToString();
                    availableWhishLists.BusinessName = cartdetails.BusinessName;
                    availableWhishLists.ServiceType = cartdetails.ServiceType;
                    availableWhishLists.UserID = user.UserId.ToString();
                    availableWhishLists.IPAddress = HttpContext.Request.UserHostAddress;
                    var list = whishListService.GetWhishList(user.UserId.ToString()).Where(m => m.VendorID == (cartdetails.Id).ToString() && m.VendorSubID == (cartdetails.subid).ToString() && m.ServiceType == cartdetails.ServiceType).Count(); //Checking whishlist availablility
                    if (list == 0)
                        availableWhishLists = whishListService.AddWhishList(availableWhishLists);

                    var message = cartService.Deletecartitem(cartId);
                    return Json(message);
                }
            }
            return Json(JsonRequestBehavior.AllowGet);

        }
    }
}