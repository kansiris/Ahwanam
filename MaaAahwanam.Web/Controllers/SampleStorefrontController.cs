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
        // GET: SampleStorefront
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(UserLogin userLogin)
        {
            userLogin.UserType = "Vendor";
            var userResponse = venorVenueSignUpService.GetUserLogin(userLogin);
            if (userResponse.UserLoginId != 0)
            {
                vendorMaster = vendorMasterService.GetVendorByEmail(userLogin.UserName);
                string userData = JsonConvert.SerializeObject(userLogin);
                ValidUserUtility.SetAuthCookie(userData, userLogin.UserLoginId.ToString());
                return RedirectToAction("Index", "NewVendorDashboard", new { id = vendorMaster.Id });
            }
            else
            {
                return Content("<script language='javascript' type='text/javascript'>alert('Wrong Credentials,Check Username and password');location.href='" + @Url.Action("Index", "SampleStorefront") + "'</script>");
            }
        }

        [HttpGet]
        public PartialViewResult ProfileProgressPartial(string id)
        {
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                //var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
                vendorMaster = vendorMasterService.GetVendor(long.Parse(id));
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