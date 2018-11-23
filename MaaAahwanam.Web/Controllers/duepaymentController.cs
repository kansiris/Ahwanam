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
    public class duepaymentController : Controller
    {
        VendorVenueService vendorVenueService = new VendorVenueService();
        newmanageuser newmanageuse = new newmanageuser();
        Vendormaster vendorMaster = new Vendormaster();
        VendorMasterService vendorMasterService = new VendorMasterService();
        PartnerService partnerservice = new PartnerService();
        private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        // GET: duepayment
        public ActionResult Index(string oid)
        {
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
                string uid = user.UserId.ToString();
                string vemail = newmanageuse.Getusername(long.Parse(uid));
                vendorMaster = newmanageuse.GetVendorByEmail(vemail);
                string VendorId = vendorMaster.Id.ToString();
                ViewBag.masterid = VendorId;
                ViewBag.Vendor = vendorMasterService.GetVendor(Convert.ToInt64(VendorId));
                if (oid != null && oid != "")
                {

                    var orderdetails1 = newmanageuse.allOrderList().Where(m => m.orderid == long.Parse(oid)).ToList();
                    ViewBag.orderid = orderdetails1.FirstOrDefault().orderid;
                }
            }
                return View();
        }
    }
}