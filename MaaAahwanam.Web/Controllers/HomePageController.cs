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
            //ViewBag.Venue = vendorProductsService.Getvendorproducts_Result("Venue").Take(6);
            var venuerecords = vendorProductsService.Getvendorproducts_Result("Venue");
            ViewBag.Hotels = venuerecords.Where(m => m.subtype == "Hotel").Take(6); // Hotel records
            ViewBag.Resorts = venuerecords.Where(m => m.subtype == "Resort").Take(6); // Resort records
            ViewBag.Conventions = venuerecords.Where(m => m.subtype == "Convention Hall").Take(6); // Convention records
            ViewBag.Catering = vendorProductsService.Getvendorproducts_Result("Catering").Take(6);
            ViewBag.Photography = vendorProductsService.Getvendorproducts_Result("Photography").Take(6);
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                try
                {
                    var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
                    var userdata = userLoginDetailsService.GetUser((int)user.UserId);
                    ViewBag.username = "" + userdata.FirstName + " " + userdata.LastName + "";
                }
                catch (Exception)
                {
                    return RedirectToAction("SignOut", "SampleStorefront");
                }
                
            }
                return View();
        }
    }
}