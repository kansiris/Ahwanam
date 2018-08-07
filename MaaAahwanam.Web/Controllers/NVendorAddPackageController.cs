using MaaAahwanam.Models;
using MaaAahwanam.Service;
using MaaAahwanam.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MaaAahwanam.Web.Controllers
{
    public class NVendorAddPackageController : Controller
    {
        VenorVenueSignUpService vendorVenueSignUpService = new VenorVenueSignUpService();
        VendorProductsService vendorProductsService = new VendorProductsService();
        private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");


        public ActionResult Index(string ks)
        {
            try {

                TempData.Clear();

            if (TempData["Active"] != "")
            {
                ViewBag.Active = TempData["Active"];
            }
                string strReq = "";
                encptdecpt encript = new encptdecpt();
                strReq = encript.Decrypt(ks);
                //Parse the value... this is done is very raw format.. you can add loops or so to get the values out of the query string...
                string[] arrMsgs = strReq.Split('&');


                string[] arrIndMsg;
                string id = "";
                arrIndMsg = arrMsgs[0].Split('='); //Get the id
                id = arrIndMsg[1].ToString().Trim();

                var deals = vendorProductsService.getvendorsubid(id);
            ViewBag.venuerecord = deals;
            ViewBag.vendormasterid = id;
            ViewBag.id = id;
            return View();
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Nhomepage");
            }
        }


        public ActionResult addpackage(string id, string type, string packagename, string packageprice, string Packagedec, string foodtype, string minGuests,string maxGuests,string MinPrice,string MaxPrice)
        {
            try { 
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                if (type == "Type")
                {
                  //  TempData["Active"] = "Select Type";
                  //  return RedirectToAction("Index", "NVendorAddPackage", new { id = id });
           return Content("<script language='javascript' type='text/javascript'>alert('Select Type');location.href='" + @Url.Action("Index", "NVendorPkgs", new { id = id }) + "'</script>");

                    }
                    DateTime updateddate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
                string[] words = type.Split(',');
                string subid = words[0];
                string type1 = words[1];
                string subtype = words[2];
                Package package = new Package();
                package.VendorId = Convert.ToInt64(id);
                package.VendorSubId = Convert.ToInt64(subid);
                package.VendorType = type1;
                package.VendorSubType = subtype;
                package.PackageName = packagename;
                package.PackagePrice = packageprice;
                package.PackageDescription = Packagedec;
                package.Status = "Active";
                package.UpdatedDate = updateddate;
                    package.MinPrice = (MinPrice);
                    package.MaxPrice = MaxPrice;
                    package.MinGuests = minGuests;
                    if (type1 == "Venue" || type1 == "Catering")
                    {
                        
                        package.Category = foodtype;

                    }

                    package.MaxGuests = (maxGuests);
                    package = vendorVenueSignUpService.addpack(package);
                ViewBag.vendormasterid = id;
                    //  TempData["Active"] = "Package added";
                    //  return RedirectToAction("Index", "NVendorPkgs", new { id = id });
                    string vssid = Convert.ToString(id);
                    encptdecpt encript = new encptdecpt();

                    string encripted = encript.Encrypt(string.Format("Name={0}", vssid));
                    return Content("<script language='javascript' type='text/javascript'>alert('Package added successfully');location.href='" + @Url.Action("Index", "NVendorPkgs", new { ks = encripted }) + "'</script>");
            }
            else
            {
                return RedirectToAction("Index", "Nhomepage");
            }
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Nhomepage");
            }
        }
    }
}