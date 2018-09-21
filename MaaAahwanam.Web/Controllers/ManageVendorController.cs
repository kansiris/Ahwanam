using MaaAahwanam.Models;
using MaaAahwanam.Service;
using MaaAahwanam.Web.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MaaAahwanam.Web.Controllers
{
    public class ManageVendorController : Controller
    {
        Vendormaster vendorMaster = new Vendormaster();
        private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        VendorMasterService vendorMasterService = new VendorMasterService();
        UserLoginDetailsService userLoginDetailsService = new UserLoginDetailsService();
        VenorVenueSignUpService vendorVenueSignUpService = new VenorVenueSignUpService();
        VendorDashBoardService mngvendorservice = new VendorDashBoardService();
        // GET: ManageVendor
        [HttpGet]
        public ActionResult Index(string VendorId)
        {
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
                string uid = user.UserId.ToString();
                string vemail = userLoginDetailsService.Getusername(long.Parse(uid));
                vendorMaster = vendorMasterService.GetVendorByEmail(vemail);
                VendorId = vendorMaster.Id.ToString();
                ViewBag.vendorlist = mngvendorservice.getvendor(VendorId);

            }
            
            return View();
        }

        [HttpPost]
        public ActionResult AddVendorDetails(ManageVendor mngvendor)
        {
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
                string uid = user.UserId.ToString();
                string vemail = userLoginDetailsService.Getusername(long.Parse(uid));
                vendorMaster = vendorMasterService.GetVendorByEmail(vemail);
                mngvendor.vendorId = vendorMaster.Id.ToString();
                mngvendor.registereddate = DateTime.Now;
                mngvendor.updateddate = DateTime.Now;
                mngvendor = mngvendorservice.SaveVendor(mngvendor);

            }
            return RedirectToAction("Index", "ManageVendor");
        }
    }
}