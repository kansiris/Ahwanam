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
        public ActionResult Index(string id, string vid)
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string id, string vid,string discount)
        {
            vendorMaster = vendorMasterService.GetVendor(long.Parse(id));
            vendorMaster.discount = discount;
            vendorMaster = vendorMasterService.UpdateVendorMaster(vendorMaster, long.Parse(id));
            //string testurl = Request.Url.Scheme + "://" + Request.Url.Authority + "/VendorSignUp4/Index?id=" + user.UserId + "&Oid=" + oid;
            UserLoginDetailsService userLoginDetailsService = new UserLoginDetailsService();
            string Username = vendorMaster.EmailId;//userLoginDetailsService.Getusername(long.Parse(id));
            StreamReader reader = new StreamReader(Server.MapPath("~/newdesign/mailtemplates/thankyou.html"));
            string readFile = reader.ReadToEnd();
            string StrContent = readFile;
            //StrContent = readFile + "<h2>Feedback Form</h2>" + testurl;
            //StrContent = StrContent.Replace("@@MessageDiv@@", Detdiv);
            //string Mailmessage = "<Table>" + Detdiv + "</Table>";

            EmailSendingUtility emailSendingUtility = new EmailSendingUtility();
            emailSendingUtility.Email_maaaahwanam(Username, StrContent.ToString(), "Registration Confirmation");
            return RedirectToAction("Index", "VendorSeccessReg");
        }
    }
}