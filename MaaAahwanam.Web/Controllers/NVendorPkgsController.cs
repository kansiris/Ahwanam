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

        // GET: NVendorPackages
        public ActionResult Index(string id)
        {
            if (TempData["Active"] != "")
            {
                ViewBag.msg = TempData["Active"];
            }
            var pkgs = vendorProductsService.getvendorpkgs(id);
            ViewBag.pacakagerecord = pkgs;
            ViewBag.id = id;
            return View();
        }

        public ActionResult editpkg(string pid, string vid)
        {

            var pkgs = vendorProductsService.getpartpkgs(pid);

            //var pkgs = vendorProductsService.getvendorpkgs(id);
            ViewBag.pacakagerecord = pkgs;
            ViewBag.id = vid;
            return View();
        }
        public ActionResult updatepkg(string id, string vid, string packagename, string packageprice, string Packagedec)
        {
           
            
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {

                DateTime updateddate = DateTime.Now;
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
                //return Content("<script type='text/javscript'> alert('package added'); location.href='/NVendorAddPackage/Index?id="+ id+ "</script>");
                //return Content("<script language='javascript' type='text/javascript'>alert('package updated');location.href='" + @Url.Action("Index", "NVendorPkgs", new { id = vid }) + "'</script>");
            }
            TempData["Active"] = "package updated";
            return RedirectToAction("Index", "Nhomepage", new { id = vid });
          //  return Content("<script language='javascript' type='text/javascript'>alert('Please login');location.href='" + @Url.Action("Index", "Nhomepage", new { id = vid }) + "'</script>");
        }
        public ActionResult deletepkg(string id, string vid)
        {

            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {

                string message = vendorVenueSignUpService.deletepack(id);
                ViewBag.vendormasterid = id;
                if (message == "success")
                {
                    //TempData["Active"] = "Package deleted";
                    //return RedirectToAction("Index", "NVendorPkgs", new { id = vid });

                    //return Content("<script type='text/javscript'> alert('package added'); location.href='/NVendorAddPackage/Index?id="+ id+ "</script>");
                    TempData["Active"] = "Package Deleted";
                    return RedirectToAction("Index", "NVendorPkgs", new { id = vid });
                  //  return Content("<script language='javascript' type='text/javascript'>alert('package deleted');location.href='" + @Url.Action("Index", "NVendorPkgs", new { id = vid }) + "'</script>");
                }
            }
            TempData["Active"] = "Please Login";
            return RedirectToAction("Index", "Nhomepage", new { id = vid });
          //  return Content("<script language='javascript' type='text/javascript'>alert('Please login');location.href='" + @Url.Action("Index", "Nhomepage", new { id = vid }) + "'</script>");
        }
    }

    
}

