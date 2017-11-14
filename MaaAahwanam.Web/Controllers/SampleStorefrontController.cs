using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaaAahwanam.Models;
using MaaAahwanam.Service;
using Newtonsoft.Json;
using MaaAahwanam.Utility;
using MaaAahwanam.Web.Custom;
using System.Web.Security;

namespace MaaAahwanam.Web.Controllers
{
    public class SampleStorefrontController : Controller
    {
        VenorVenueSignUpService venorVenueSignUpService = new VenorVenueSignUpService();
        Vendormaster vendorMaster = new Vendormaster();
        VendorMasterService vendorMasterService = new VendorMasterService();
        UserLoginDetailsService userLoginDetailsService = new UserLoginDetailsService();
        RandomPassword randomPassword = new RandomPassword();

        // GET: SampleStorefront
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index([Bind(Prefix = "Item1")]UserLogin userLogin, [Bind(Prefix = "Item2")] Vendormaster vendorMaster, string command)
        {
            if (command == "Login")
            {
                //userLogin.UserType = "Vendor";
                var userResponse = venorVenueSignUpService.GetUserLogin(userLogin);
                if (userResponse != null)
                {
                    vendorMaster = vendorMasterService.GetVendorByEmail(userLogin.UserName);
                    string userData = JsonConvert.SerializeObject(userResponse);
                    ValidUserUtility.SetAuthCookie(userData, userResponse.UserLoginId.ToString());
                    //ValidUserUtility.SetAuthCookie(userData, userLogin.UserLoginId.ToString());
                    if (userResponse.UserType == "Vendor")
                        return RedirectToAction("Index", "NewVendorDashboard", new { id = vendorMaster.Id });
                    else
                        return RedirectToAction("Index", "HomePage");
                }
                else
                {
                    int query = vendorMasterService.checkemail(userLogin.UserName);
                    if (query == 0)
                        return Content("<script language='javascript' type='text/javascript'>alert('User Record Not Available');location.href='" + @Url.Action("Index", "SampleStorefront") + "'</script>");
                    else
                        return Content("<script language='javascript' type='text/javascript'>alert('Wrong Credentials,Check Username and password');location.href='" + @Url.Action("Index", "SampleStorefront") + "'</script>");
                }
            }
            if (command == "VendorReg")
            {
                int query = vendorMasterService.checkemail(vendorMaster.EmailId);
                if (query == 0)
                {
                    if (vendorMaster.ServicType == "Mehendi")
                        vendorMaster.ServicType = "Other";
                    UserLogin userLogin1 = new UserLogin();
                    userLogin1.UserType = "Vendor";
                    vendorMaster = venorVenueSignUpService.AddvendorMaster(vendorMaster);
                    userLogin1.UserName = vendorMaster.EmailId;
                    userLogin1.Password = "Temp1234";//randomPassword.GenerateString();
                    userLogin1 = venorVenueSignUpService.AddUserLogin(userLogin1);
                    UserDetail userDetail = new UserDetail();
                    userDetail.UserLoginId = userLogin1.UserLoginId;
                    userDetail = venorVenueSignUpService.AddUserDetail(userDetail, vendorMaster);
                    addservice(vendorMaster);
                    if (vendorMaster.Id != 0)
                    {
                        return Content("<script language='javascript' type='text/javascript'>alert('Registered Successfully!!! Our back office executive will get back to you as soon as possible');location.href='" + @Url.Action("Index", "SampleStorefront") + "'</script>");
                    }
                }
                else
                    return Content("<script language='javascript' type='text/javascript'>alert('E-Mail ID Already Taken!!! Try Another');location.href='" + @Url.Action("Index", "SampleStorefront") + "'</script>");
            }
            return View();
        }

        [HttpGet]
        public PartialViewResult ProfileProgressPartial(string id)
        {
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
                string email = userLoginDetailsService.Getusername(user.UserId);
                vendorMaster = vendorMasterService.GetVendorByEmail(email);
                return PartialView("ProfileProgressPartial", vendorMaster);
            }
            return PartialView("ProfileProgressPartial", null);
        }

        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "HomePage");
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
            if (vendorMaster.ServicType == "Other")
            {
                VendorsOther vendorsOther = new VendorsOther();
                vendorsOther.VendorMasterId = vendorMaster.Id;
                vendorsOther.MinOrder = "0";
                vendorsOther.MaxOrder = "0";
                vendorsOther.Status = "InActive";
                vendorsOther.UpdatedBy = 2;
                vendorsOther.UpdatedDate = Convert.ToDateTime(DateTime.UtcNow.ToShortDateString());
                vendorsOther = venorVenueSignUpService.AddVendorOther(vendorsOther);
                if (vendorsOther.Id != 0) count++;
            }
            return count;
        }
    }
}