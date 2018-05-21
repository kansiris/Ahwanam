﻿using MaaAahwanam.Models;
using MaaAahwanam.Service;
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


        public ActionResult Index(string id)
        {
            if (TempData["Active"] != "")
            {
                ViewBag.Active = TempData["Active"];
            }
            var deals = vendorProductsService.getvendorsubid(id);
            ViewBag.venuerecord = deals;
            ViewBag.vendormasterid = id;
            ViewBag.id = id;
            return View();
        }


        public ActionResult addpackage(string id, string type, string packagename, string packageprice, string Packagedec)
        {
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {

                if (type == "Type")
                {
                    TempData["Active"] = "Select Type";
                    return RedirectToAction("Index", "NVendorAddPackage", new { id = id });

                  //  return Content("<script> alert('select type');location.href='" + @Url.Action("Index", "NVendorAddPackage", new { id = id }) + "' </script>");
                }
                DateTime updateddate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);

               // DateTime updateddate = DateTime.Now;
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
                package = vendorVenueSignUpService.addpack(package);
                ViewBag.vendormasterid = id;
                //return Content("<script type='text/javscript'> alert('package added'); location.href='/NVendorAddPackage/Index?id="+ id+ "</script>");
                TempData["Active"] = "Package added";
                return RedirectToAction("Index", "NVendorPkgs", new { id = id });

              //  return Content("<script language='javascript' type='text/javascript'>alert('package added');location.href='" + @Url.Action("Index", "NVendorPkgs", new { id = id }) + "'</script>");
            }
            else
            {
                return RedirectToAction("Index", "Nhomepage");
            }

        }


    }
}