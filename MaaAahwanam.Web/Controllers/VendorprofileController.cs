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
using System.Globalization;



namespace MaaAahwanam.Web.Controllers
{


    public class VendorprofileController : Controller
    {
        VenorVenueSignUpService vendorVenueSignUpService = new VenorVenueSignUpService();
        VendorMasterService vendorMasterService = new VendorMasterService();
        const string imagepath = @"/vendorimages/";
        VendorImageService vendorImageService = new VendorImageService();
        Vendormaster vendorMaster = new Vendormaster();
        UserLoginDetailsService userLoginDetailsService = new UserLoginDetailsService();


        // signup1

        // GET: profile
        public ActionResult Index(string id)
        {
            //id = ViewBag.Vendormaster.Id;
            ViewBag.images = vendorImageService.GetVendorAllImages(long.Parse(id));
            var data = vendorMasterService.GetVendor(long.Parse(id));
            ViewBag.ServicType = data.ServicType.Split(',');
            ViewBag.data = data;
            return View();
        }

    }

}
