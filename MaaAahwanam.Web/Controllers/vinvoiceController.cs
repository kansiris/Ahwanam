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
    public class vinvoiceController : Controller
    {
        VendorMasterService vendorMasterService = new VendorMasterService();
        UserLoginDetailsService userLoginDetailsService = new UserLoginDetailsService();
        // GET: vinvoice
        public ActionResult Index()
        {
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
                string id = user.UserId.ToString();
                string email = userLoginDetailsService.Getusername(long.Parse(id));
                Vendormaster Vendormaster = vendorMasterService.GetVendorByEmail(email);
                ViewBag.Vendor = vendorMasterService.GetVendor(Convert.ToInt64(Vendormaster.Id));
            }
            return View();
        }
    }
}