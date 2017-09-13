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
        public ActionResult Index(string id)
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string id,string discount)
        {
            vendorMaster = vendorMasterService.GetVendor(long.Parse(id));
            vendorMaster.discount = discount;
            vendorMaster = vendorMasterService.UpdateVendorMaster(vendorMaster, long.Parse(id));
            string Username = vendorMaster.EmailId;
            StreamReader reader = new StreamReader(Server.MapPath("~/newdesign/mailtemplates/thankyou.html"));
            string StrContent = reader.ReadToEnd();
            EmailSendingUtility emailSendingUtility = new EmailSendingUtility();
            //emailSendingUtility.Email_maaaahwanam(Username, StrContent.ToString(), "Registration Confirmation");
            emailSendingUtility.Email_maaaahwanam("amit.saxena@ahwanam.com", StrContent.ToString(), "Test Mail");
            emailSendingUtility.Email_maaaahwanam("srinivas.b@ahwanam.com", StrContent.ToString(), "Test Mail");
            //return RedirectToAction("Index", "VendorSeccessReg");
            return Content("<script language='javascript' type='text/javascript'>alert('Registration successful. Please click on Activation link which has been sent to your Email to enable your Login Access.');location.href='" + @Url.Action("Index", "VendorSeccessReg") + "'</script>");
        }
    }
}