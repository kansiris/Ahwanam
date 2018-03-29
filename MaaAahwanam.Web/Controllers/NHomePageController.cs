using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaaAahwanam.Web.Custom;
using MaaAahwanam.Service;
using MaaAahwanam.Models;
using MaaAahwanam.Utility;

namespace MaaAahwanam.Web.Controllers
{
    public class NHomePageController : Controller
    {
        // GET: NHomePage
        VendorProductsService vendorProductsService = new VendorProductsService();
        UserLoginDetailsService userLoginDetailsService = new UserLoginDetailsService();
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

        public PartialViewResult SortVendorsBasedOnLocation(string search,string type, string location)
        {
            var value = (type == null) ? "Venue" : type;
            ViewBag.records = (search == null) ? vendorProductsService.Getsearchvendorproducts_Result("V",value).Where(m => m.landmark == location).Take(6).ToList() : vendorProductsService.Getsearchvendorproducts_Result(search,value).Take(6).ToList(); //.Where(m => m.landmark == location)
            return PartialView();
        }

    }
}