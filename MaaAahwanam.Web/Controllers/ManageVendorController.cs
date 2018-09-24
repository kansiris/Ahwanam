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
            var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
            string uid = user.UserId.ToString();
            string vemail = userLoginDetailsService.Getusername(long.Parse(uid));
            vendorMaster = vendorMasterService.GetVendorByEmail(vemail);
            VendorId = vendorMaster.Id.ToString();
            ViewBag.masterid = VendorId;
            ViewBag.vendorlist = mngvendorservice.getvendor(VendorId);
            return View();
        }

        [HttpPost]
        public ActionResult Index(ManageVendor mngvendor)
        {
            mngvendor.registereddate = DateTime.Now;
            mngvendor.updateddate = DateTime.Now;
            mngvendor = mngvendorservice.SaveVendor(mngvendor);
            return Content("<script language='javascript' type='text/javascript'>alert('Added New Vendor');location.href='/ManageVendor'</script>");
        }
        [HttpPost]
        public JsonResult GetVendorDetails(string id)
        {
            var data = mngvendorservice.getvendorbyid(int.Parse(id));
            return Json(data);
        }
        [HttpPost]
        public JsonResult UpdateVendorDetails(ManageVendor mngvendor, string id)
        {
            mngvendor.updateddate = DateTime.Now;
            mngvendor = mngvendorservice.UpdateVendor(mngvendor, int.Parse(id));
            return Json("Sucess", JsonRequestBehavior.AllowGet);
        }
        public JsonResult checkemail(string email, string id)
        {
            int query = mngvendorservice.checkvendoremail(email, id);
            if (query == 0)
                return Json("valid email");
            else
                return Json("already email is added");
        }
    }
}