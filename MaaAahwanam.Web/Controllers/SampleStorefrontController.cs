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
            userLogin.UserType = "Vendor";
            if (command == "Login")
            {
                var userResponse = venorVenueSignUpService.GetUserLogin(userLogin);
                if (userResponse != null)
                {
                    vendorMaster = vendorMasterService.GetVendorByEmail(userLogin.UserName);
                    string userData = JsonConvert.SerializeObject(userResponse);
                    ValidUserUtility.SetAuthCookie(userData, userResponse.UserLoginId.ToString());
                    //ValidUserUtility.SetAuthCookie(userData, userLogin.UserLoginId.ToString());
                    return RedirectToAction("Index", "NewVendorDashboard", new { id = vendorMaster.Id });
                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Wrong Credentials,Check Username and password');location.href='" + @Url.Action("Index", "SampleStorefront") + "'</script>");
                }
            }
            if (command == "VendorReg")
            {
                vendorMaster = venorVenueSignUpService.AddvendorMaster(vendorMaster);
                userLogin.UserName = vendorMaster.EmailId;
                userLogin.Password = randomPassword.GenerateString();
                userLogin = venorVenueSignUpService.AddUserLogin(userLogin);
                UserDetail userDetail = new UserDetail();
                userDetail.UserLoginId = userLogin.UserLoginId;
                userDetail = venorVenueSignUpService.AddUserDetail(userDetail, vendorMaster);
                if (vendorMaster.Id != 0)
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Registered Successfully!!! Our back office executive will get back to you as soon as possible');location.href='" + @Url.Action("Index", "SampleStorefront") + "'</script>");
                }
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
    }
}