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
    public class ReSellerManagementController : Controller
    {
        VendorVenueService vendorVenueService = new VendorVenueService();
        viewservicesservice viewservicesss = new viewservicesservice();
        newmanageuser newmanageuse = new newmanageuser();
        Vendormaster vendorMaster = new Vendormaster();
        private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        PartnerService partnerservice = new PartnerService();
        // GET: ReSellerManagement
        public ActionResult Index(string partid)
        {
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
                string uid = user.UserId.ToString();
                string vemail = newmanageuse.Getusername(long.Parse(uid));
                vendorMaster = newmanageuse.GetVendorByEmail(vemail);
                string VendorId = vendorMaster.Id.ToString();
                ViewBag.masterid = VendorId;
                var resellers = partnerservice.GetPartners(VendorId);
                if(partid != "" && partid != null)
                {
                    ViewBag.resellers = resellers.Where(m => m.PartnerID == long.Parse(partid)).FirstOrDefault();
                }
                ViewBag.package = viewservicesss.getvendorpkgs(VendorId).ToList();
                return View();
            }
            else {
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpPost]
        public JsonResult Index(Partner partner,string command,string partid)
        {
            partner.RegisteredDate = DateTime.Now;
            partner.UpdatedDate = DateTime.Now;
            if (command == "save") { partner = partnerservice.AddPartner(partner); }
            else if(command == "Update") { partner = partnerservice.UpdatePartner(partner,partid); }

            var emailid = partner.emailid;
            var getpartner = partnerservice.getPartner(emailid);
            return Json(getpartner, JsonRequestBehavior.AllowGet);
        }
       
    }
}