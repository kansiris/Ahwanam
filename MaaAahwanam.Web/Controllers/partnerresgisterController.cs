using MaaAahwanam.Models;
using MaaAahwanam.Service;
using MaaAahwanam.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MaaAahwanam.Web.Controllers
{
    public class partnerresgisterController : Controller
    {
        UserLoginDetailsService userLoginDetailsService = new UserLoginDetailsService();
        VendorVenueService vendorVenueService = new VendorVenueService();
        VenorVenueSignUpService venorVenueSignUpService = new VenorVenueSignUpService();
        VendorMasterService vendorMasterService = new VendorMasterService();

        // GET: partnerresgister
        public ActionResult Index()
        {
            return View();
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
               // vendorsOther.type = vendorMaster.;
                vendorsOther = venorVenueSignUpService.AddVendorOther(vendorsOther);
                if (vendorsOther.Id != 0) count++;
            }
            return count;
        }
        public JsonResult vendorreg(string vname, string businessname,string mobile,string email,string businesstype,string serviceselection, string Password)
        {
            UserLogin userLogin1 = new UserLogin();
            UserDetail userDetail = new UserDetail();
            Vendormaster vendorMaster = new Vendormaster();
            VendorVenue vendorVenue = new VendorVenue();
            VendorsPhotography vendorsPhotography = new VendorsPhotography();
            VendorsDecorator vendorsDecorator = new VendorsDecorator();
            VendorOthersService other = new VendorOthersService();
             userLogin1.IPAddress = HttpContext.Request.UserHostAddress;
            userLogin1.ActivationCode = Guid.NewGuid().ToString();
            userLogin1.Status = "InActive";
            userLogin1.UserType = "Vendor";
            userDetail.FirstName = vname;
            userDetail.UserPhone = mobile;
            userLogin1.Password = Password;
            userLogin1.UserName = email;
            vendorMaster.BusinessName = businessname;
            vendorMaster.ContactPerson = vname;
            vendorMaster.ContactNumber = mobile;
            vendorMaster.ServicType = serviceselection;
            vendorMaster.EmailId = email;
            long data = userLoginDetailsService.GetLoginDetailsByEmail(userLogin1.UserName);
            if (data == 0)
            {
                int query = vendorMasterService.checkemail(vendorMaster.EmailId);
                if (query == 0)
                {

                    // response = userLoginDetailsService.AddUserDetails(userLogin, userDetail);
                   
                    vendorMaster = venorVenueSignUpService.AddvendorMaster(vendorMaster);
                    vendorMaster.EmailId = email;
                     userLogin1 = venorVenueSignUpService.AddUserLogin(userLogin1);
                    //UserDetail userDetail = new UserDetail();
                    userDetail.UserLoginId = userLogin1.UserLoginId;
                    userDetail = venorVenueSignUpService.AddUserDetail(userDetail, vendorMaster);
                    addservice(vendorMaster);
                       string activationcode = userLogin1.ActivationCode;
                    string txtto = userLogin1.UserName;
                    string username = userDetail.FirstName;
                    HomeController home = new HomeController();
                    username = home.Capitalise(username);
                    string emailid = userLogin1.UserName;
                    string url = Request.Url.Scheme + "://" + Request.Url.Authority + "/Home/ActivateEmail1?ActivationCode=" + activationcode + "&&Email=" + emailid;
                    FileInfo File = new FileInfo(Server.MapPath("/mailtemplate/welcome.html"));
                    string readFile = File.OpenText().ReadToEnd();
                    readFile = readFile.Replace("[ActivationLink]", url);
                    readFile = readFile.Replace("[name]", username);
                    string txtmessage = readFile;//readFile + body;
                    string subj = "Account Activation";
                    EmailSendingUtility emailSendingUtility = new EmailSendingUtility();
                    emailSendingUtility.Email_maaaahwanam(txtto, txtmessage, subj);
                    //TempData["Active"] = "Check your email to active your account to login";
                    //return RedirectToAction("Index", "NUserRegistration");
                    return Json("success");
                }
                }
            else
            {
                return Json("ks");
                //return Content("<script language='javascript' type='text/javascript'>alert('E-Mail ID Already Registered!!! Try Logging with your Password');location.href='" + @Url.Action("Index", "Home") + "'</script>");

                //TempData["Active"] = "E-Mail ID Already Registered!!! Try Logging with your Password";
                //return RedirectToAction("Index", "Home");
            }
          
            
            return Json("success1");
        }
    }
}