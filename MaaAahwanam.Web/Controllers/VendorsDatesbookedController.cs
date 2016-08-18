using MaaAahwanam.Repository;
using MaaAahwanam.Service;
using MaaAahwanam.Utility;
using MaaAahwanam.Web.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MaaAahwanam.Web.Controllers
{
    public class VendorsDatesbookedController : Controller
    {

        ServiceResponseService serviceResponseService = new ServiceResponseService();
        SP_vendordatesbooked_Result SP_vendordatesbooked_Result = new SP_vendordatesbooked_Result();
        // GET: VendorsDatesbooked
        public ActionResult Index()
        {
            // GET: VendorsDatesbooked
            var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
            if (user.UserId != 0 && (user.UserType == "Vendor"))
            {
                int a = ValidUserUtility.ValidUser();
                ViewBag.Vdatesbooked = serviceResponseService.GetVendordatesbooked(a);
            }
            return View();
        }
    }
}