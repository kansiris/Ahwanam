using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaaAahwanam.Models;
using MaaAahwanam.Service;

namespace MaaAahwanam.Web.Controllers
{
    public class SampleStorefrontController : Controller
    {
        // GET: SampleStorefront
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(UserLogin userLogin)
        {
            VenorVenueSignUpService venorVenueSignUpService = new VenorVenueSignUpService();
            userLogin.UserType = "Vendor";
            var userResponse = venorVenueSignUpService.GetUserLogin(userLogin);
            if (userResponse.UserLoginId != 0)
            {
                return RedirectToAction("Index", "NewVendorDashboard");
            }
            else
            {
                return Content("<script language='javascript' type='text/javascript'>alert('Wrong Credentials,Check Username and password');location.href='" + @Url.Action("Index", "SampleStorefront") + "'</script>");
            }
        }
    }
}