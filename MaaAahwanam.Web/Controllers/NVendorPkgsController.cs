using MaaAahwanam.Models;
using MaaAahwanam.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MaaAahwanam.Web.Controllers
{
    public class NVendorPkgsController : Controller
    {
        VendorProductsService vendorProductsService = new VendorProductsService();
        VenorVenueSignUpService vendorVenueSignUpService = new VenorVenueSignUpService();
        private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");

        // GET: NVendorPackages
        public ActionResult Index(string id)
        {
            try { 
            if (TempData["Active"] != "")
            {
                ViewBag.msg = TempData["Active"];
            }
            var pkgs = vendorProductsService.getvendorpkgs(id);
            ViewBag.pacakagerecord = pkgs;
            ViewBag.id = id;
            return View();
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Nhomepage");
            }
        }

        public ActionResult editpkg(string pid, string vid)
        {
            try { 
            var pkgs = vendorProductsService.getpartpkgs(pid);
            ViewBag.pacakagerecord = pkgs;
            ViewBag.id = vid;
            return View();
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Nhomepage");
            }
        }
        public ActionResult updatepkg(string id, string vid, string packagename, string packageprice, string Packagedec)
        {
            try { 
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                DateTime updateddate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
                Package package = new Package();
                package.PackageName = packagename;
                package.PackagePrice = packageprice;
                package.PackageDescription = Packagedec;
                package.Status = "Active";
                package.UpdatedDate = updateddate;
                package = vendorVenueSignUpService.updatepack(id, package);
                ViewBag.vendormasterid = id;
                TempData["Active"] = "Package Updated";
                return RedirectToAction("Index", "NVendorPkgs",new { id = vid });
            }
            TempData["Active"] = "package updated";
            return RedirectToAction("Index", "Nhomepage", new { id = vid });
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Nhomepage");
            }
        }

        public ActionResult deletepkg(string id, string vid)
        {
            try { 
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                string message = vendorVenueSignUpService.deletepack(id);
                ViewBag.vendormasterid = id;
                if (message == "success")
                {
                    TempData["Active"] = "Package Deleted";
                    return RedirectToAction("Index", "NVendorPkgs", new { id = vid });
                }
            }
            TempData["Active"] = "Please Login";
            return RedirectToAction("Index", "Nhomepage", new { id = vid });
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Nhomepage");
            }
        }
    } 
}

