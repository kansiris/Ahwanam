using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaaAahwanam.Web.Custom;
using MaaAahwanam.Service;

namespace MaaAahwanam.Web.Controllers
{
    public class HomePageController : Controller
    {
        // GET: HomePage
        UserLoginDetailsService userLoginDetailsService = new UserLoginDetailsService();
        VendorProductsService vendorProductsService = new VendorProductsService();
        public ActionResult Index()
        {
            ViewBag.Venue = vendorProductsService.Getvendorproducts_Result("Venue").Take(6);
            ViewBag.Catering = vendorProductsService.Getvendorproducts_Result("Catering").Take(6);
            ViewBag.Photography = vendorProductsService.Getvendorproducts_Result("Photography").Take(6);
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
                var userdata = userLoginDetailsService.GetUser((int)user.UserId);
                ViewBag.username = ""+userdata.FirstName+" "+userdata.LastName+"";
            }
                return View();
        }
    }
}