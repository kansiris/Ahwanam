using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaaAahwanam.Models;
using MaaAahwanam.Service;
using System.IO;
using MaaAahwanam.Utility;

namespace MaaAahwanam.Web.Controllers
{
    public class VendorSignUp4Controller : Controller
    {
        Vendormaster vendorMaster = new Vendormaster();
        VenorVenueSignUpService venorVenueSignUpService = new VenorVenueSignUpService();
        VendorMasterService vendorMasterService = new VendorMasterService();
        // GET: VendorSignUp4
        public ActionResult Index(string id, string vid, string type)
        {
            if (type == "Venue")
            {
                VendorVenueService vendorVenueService = new VendorVenueService();
                ViewBag.data = vendorVenueService.GetVendorVenue(long.Parse(id), long.Parse(vid)).discount;
            }
            if (type == "Catering")
            {
                VendorCateringService vendorCateringService = new VendorCateringService();
                ViewBag.data = vendorCateringService.GetVendorCatering(long.Parse(id), long.Parse(vid)).discount;
            }
            if (type == "Photography")
            {
                VendorPhotographyService vendorPhotographyService = new VendorPhotographyService();
                ViewBag.data = vendorPhotographyService.GetVendorPhotography(long.Parse(id), long.Parse(vid)).discount;
            }
            if (type == "Decorator")
            {
                VendorDecoratorService vendorDecoratorService = new VendorDecoratorService();
                ViewBag.data = vendorDecoratorService.GetVendorDecorator(long.Parse(id), long.Parse(vid)).discount;
            }
            if (type == "EventManagement")
            {
                VendorEventOrganiserService vendorEventOrganiserService = new VendorEventOrganiserService();
                ViewBag.data = vendorEventOrganiserService.GetVendorEventOrganiser(long.Parse(id), long.Parse(vid)).discount;
            }
            if (type == "Other")
            {
                VendorOthersService vendorOthersService = new VendorOthersService();
                ViewBag.data = vendorOthersService.GetVendorOther(long.Parse(id), long.Parse(vid)).discount;
            }
            return View();
        }

        [HttpPost]
        public ActionResult Index(string id, string vid, string type, string discount,string txtdiscount)
        {
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                if (txtdiscount != "" && txtdiscount != null)
                    discount = txtdiscount+"%";
                vendorMaster = vendorMasterService.GetVendor(long.Parse(id));
                //vendorMaster.discount = discount;
                //vendorMaster = vendorMasterService.UpdateVendorMaster(vendorMaster, long.Parse(id));
                long count = venorVenueSignUpService.DiscountUpdate(type, id, vid, discount);
                string Username = vendorMaster.EmailId;
                StreamReader reader = new StreamReader(Server.MapPath("~/newdesign/mailtemplates/thankyou.html"));
                string StrContent = reader.ReadToEnd();
                EmailSendingUtility emailSendingUtility = new EmailSendingUtility();
                //emailSendingUtility.Email_maaaahwanam(Username, StrContent.ToString(), "Registration Confirmation");
                //emailSendingUtility.Email_maaaahwanam("amit.saxena@ahwanam.com", StrContent.ToString(), "Test Mail");
                //emailSendingUtility.Email_maaaahwanam("srinivas.b@ahwanam.com", StrContent.ToString(), "Test Mail");
                //return RedirectToAction("Index", "VendorSeccessReg");
                return Content("<script language='javascript' type='text/javascript'>alert('Discount Saved Successfully');location.href='" + @Url.Action("Index", "AvailableServices", new { id = vendorMaster.Id }) + "'</script>");
            }
            else
            {
                return RedirectToAction("Index", "HomePage");
            }
        }
    }
}