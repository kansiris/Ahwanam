using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaaAahwanam.Models;
using MaaAahwanam.Service;
using MaaAahwanam.Utility;
using System.IO;

namespace MaaAahwanam.Web.Controllers
{
    public class NVendorSignUpController : Controller
    {
        VenorVenueSignUpService venorVenueSignUpService = new VenorVenueSignUpService();
        VendorMasterService vendorMasterService = new VendorMasterService();
        // GET: NVendorSignUp
        string type = "";
        public ActionResult Index()
        {
            if (TempData["Active"] != "")
            {
                ViewBag.Active = TempData["Active"];
            }
            return View();
        }

        [HttpPost]
        public ActionResult Index([Bind(Prefix = "Item1")]UserLogin userLogin, [Bind(Prefix = "Item2")] Vendormaster vendorMaster, string command)
        {
            //if (command == "VendorReg")
            //{
                int query = vendorMasterService.checkemail(vendorMaster.EmailId);

                if (query == 0)
                {
                    if (vendorMaster.ServicType == "Mehendi" || vendorMaster.ServicType == "Pandit")
                    {
                        type = vendorMaster.ServicType;
                        vendorMaster.ServicType = "Other";
                    }
                    UserLogin userLogin1 = new UserLogin();
                    userLogin1.ActivationCode = Guid.NewGuid().ToString();
                    userLogin1.UserType = "Vendor";
                    vendorMaster = venorVenueSignUpService.AddvendorMaster(vendorMaster);
                    userLogin1.UserName = vendorMaster.EmailId;
                    userLogin1.Password = userLogin.Password;
                    userLogin1 = venorVenueSignUpService.AddUserLogin(userLogin1);
                    UserDetail userDetail = new UserDetail();
                    userDetail.UserLoginId = userLogin1.UserLoginId;
                    userDetail = venorVenueSignUpService.AddUserDetail(userDetail, vendorMaster);
                    addservice(vendorMaster);

                 string activationcode = userLogin1.ActivationCode;
                    string txtto = userLogin1.UserName;
                    string username = userDetail.FirstName;
                    string emailid = userLogin1.UserName;
                    string url = Request.Url.Scheme + "://" + Request.Url.Authority + "/NUserRegistration/ActivateEmail1?ActivationCode=" + activationcode + "&&Email=" + emailid;
                    FileInfo File = new FileInfo(Server.MapPath("/mailtemplate/welcome.html"));
                    string readFile = File.OpenText().ReadToEnd();
                    readFile = readFile.Replace("[ActivationLink]", url);
                    readFile = readFile.Replace("[name]", username);
                    string txtmessage = readFile;//readFile + body;
                    string subj = "Account Activation";
                    EmailSendingUtility emailSendingUtility = new EmailSendingUtility();
                    emailSendingUtility.Email_maaaahwanam(txtto, txtmessage, subj);

                    TempData["Active"] = "Check your email to active your account to login";
                    return RedirectToAction("Index", "NUserRegistration");

                   // return Content("<script language='javascript' type='text/javascript'>alert('Check your email to active your account to login');location.href='" + @Url.Action("Index", "NUserRegistration") + "'</script>");
                    //if (vendorMaster.Id != 0)
                    //{
                    //    return Content("<script language='javascript' type='text/javascript'>alert('Registered Successfully!!! Our back office executive will get back to you as soon as possible');location.href='" + @Url.Action("Index", "NVendorSignUp") + "'</script>");
                    //}
                }
                else
                    TempData["Active"] = "E-Mail ID Already Taken!!! Try Another";
                return RedirectToAction("Index", "NUserRegistration");
               // return Content("<script language='javascript' type='text/javascript'>alert('E-Mail ID Already Taken!!! Try Another');location.href='" + @Url.Action("Index", "NVendorSignUp") + "'</script>");
            //}
           // return View();
        }

        public JsonResult checkemail(string emailid)
        {
            int query = vendorMasterService.checkemail(emailid);
            if (query != 0)
            {
                return Json("exists", JsonRequestBehavior.AllowGet);
            }
            return Json("valid", JsonRequestBehavior.AllowGet);
        }

        public int addservice(Vendormaster vendorMaster)
        {
            int count = 0;
            if (vendorMaster.ServicType == "Venue")
            {
                VendorVenue vendorVenue = new VendorVenue();
                vendorVenue.VendorMasterId = vendorMaster.Id;
                vendorVenue = venorVenueSignUpService.AddVendorVenue(vendorVenue);
                if (vendorVenue.Id != 0) count++;
            }
            if (vendorMaster.ServicType == "Catering")
            {
                VendorsCatering vendorsCatering = new VendorsCatering();
                vendorsCatering.VendorMasterId = vendorMaster.Id;
                vendorsCatering = venorVenueSignUpService.AddVendorCatering(vendorsCatering);
                if (vendorsCatering.Id != 0) count++;
            }
            if (vendorMaster.ServicType == "Photography")
            {
                VendorsPhotography vendorsPhotography = new VendorsPhotography();
                vendorsPhotography.VendorMasterId = vendorMaster.Id;
                vendorsPhotography = venorVenueSignUpService.AddVendorPhotography(vendorsPhotography);
                if (vendorsPhotography.Id != 0) count++;
            }
            if (vendorMaster.ServicType == "Decorator")
            {
                VendorsDecorator vendorsDecorator = new VendorsDecorator();
                vendorsDecorator.VendorMasterId = vendorMaster.Id;
                vendorsDecorator = venorVenueSignUpService.AddVendorDecorator(vendorsDecorator);
                if (vendorsDecorator.Id != 0) count++;
            }
            //if (vendorMaster.ServicType == "EventManagement")
            //{
            //    VendorsEventOrganiser vendorsEventOrganiser = new VendorsEventOrganiser();
            //    vendorsEventOrganiser.VendorMasterId = vendorMaster.Id;
            //    vendorsEventOrganiser = venorVenueSignUpService.AddVendorEventOrganiser(vendorsEventOrganiser);
            //    if (vendorsEventOrganiser.Id != 0) count++;
            //}
            if (vendorMaster.ServicType == "Other")
            {
                VendorsOther vendorsOther = new VendorsOther();
                vendorsOther.VendorMasterId = vendorMaster.Id;
                vendorsOther.MinOrder = "0";
                vendorsOther.MaxOrder = "0";
                vendorsOther.Status = "InActive";
                vendorsOther.UpdatedBy = 2;
                vendorsOther.UpdatedDate = Convert.ToDateTime(DateTime.UtcNow.ToShortDateString());
                vendorsOther.type = type;
                vendorsOther = venorVenueSignUpService.AddVendorOther(vendorsOther);
                if (vendorsOther.Id != 0) count++;
            }
            return count;
        }
    }
}