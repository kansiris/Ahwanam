using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaaAahwanam.Web.Custom;
using MaaAahwanam.Service;
using MaaAahwanam.Models;
using MaaAahwanam.Utility;
using MaaAahwanam.Repository;

namespace MaaAahwanam.Web.Controllers
{
    public class NHomePageController : Controller
    {
        // GET: NHomePage
        VendorProductsService vendorProductsService = new VendorProductsService();
        UserLoginDetailsService userLoginDetailsService = new UserLoginDetailsService();
        CartService cartService = new CartService();

        public ActionResult Index()
        {
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
                var userdata = userLoginDetailsService.GetUser((int)user.UserId);
                if (userdata.FirstName != "" && userdata.FirstName != null)
                    ViewBag.username = userdata.FirstName;
                else if (userdata.FirstName != "" && userdata.FirstName != null && userdata.LastName != "" && userdata.LastName != null)
                    ViewBag.username = "" + userdata.FirstName + " " + userdata.LastName + "";
                else
                    ViewBag.username = userdata.AlternativeEmailID;

                if (user.UserType == "Admin")
                {
                    ViewBag.cartCount = cartService.CartItemsCount(0);
                    return PartialView("ItemsCartViewBindingLayout");
                }
                ViewBag.cartCount = cartService.CartItemsCount((int)user.UserId);

                List<GetCartItems_Result> cartlist = cartService.CartItemsList(int.Parse(user.UserId.ToString()));
                //List<cartcount_Result> cartlist = cartService.cartcountservice(user.UserId);
                decimal total = cartlist.Sum(s => s.TotalPrice);
                ViewBag.cartitems = cartlist;
                ViewBag.Total = total;
            }
            else
            {
                ViewBag.cartCount = cartService.CartItemsCount(0);
            }
            return View();
        }

        public JsonResult AutoCompleteCountry()
        {
            VendorMasterService allVendorsService = new VendorMasterService();
            var Listoflocations = String.Join(",", allVendorsService.GetVendorCities().Distinct());
            //return Json(Listoflocations,JsonRequestBehavior.AllowGet);
            return new JsonResult { Data = Listoflocations, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public PartialViewResult SortVendorsBasedOnLocation(string search, string type, string location)
        {
            if (new string[] { "Wedding", "Party", "Corporate", "BabyFunction", "Birthday", "Engagement" ,"Venues"}.Contains(type))
            { type = "Venue"; }
            var value = (type == null) ? "Venue" : type;
            ViewBag.records = (search == null) ? vendorProductsService.Getsearchvendorproducts_Result("V", value).Where(m => m.landmark == location).Take(6).ToList() : vendorProductsService.Getsearchvendorproducts_Result(search, value).Take(6).ToList(); //.Where(m => m.landmark == location)
            return PartialView();
        }



        public ActionResult ItemsCartViewBindingLayout()
        {
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
                var userdata = userLoginDetailsService.GetUser((int)user.UserId);
                if (userdata.FirstName != "" && userdata.FirstName != null)
                    ViewBag.username = userdata.FirstName;
                else if (userdata.FirstName != "" && userdata.FirstName != null && userdata.LastName != "" && userdata.LastName != null)
                    ViewBag.username = "" + userdata.FirstName + " " + userdata.LastName + "";
                else
                    ViewBag.username = userdata.AlternativeEmailID;
                if (user.UserType == "Admin")
                {
                    ViewBag.cartCount = cartService.CartItemsCount(0);
                    return PartialView("ItemsCartViewBindingLayout");
                }
                ViewBag.cartCount = cartService.CartItemsCount((int)user.UserId);

                List<GetCartItems_Result> cartlist = cartService.CartItemsList(int.Parse(user.UserId.ToString()));
                //List<cartcount_Result> cartlist = cartService.cartcountservice(user.UserId);
                decimal total = cartlist.Sum(s => s.TotalPrice);
                ViewBag.cartitems = cartlist;
                ViewBag.Total = total;
                //ViewBag.cartcounttotal = cartService.cartcountservice(user.UserId).Count();
                //ViewBag.cartitems = cartService.cartcountservice(user.UserId);
            }
            else
            {
                ViewBag.cartCount = cartService.CartItemsCount(0);
            }
            return PartialView("ItemsCartViewBindingLayout");
        }
    }
}