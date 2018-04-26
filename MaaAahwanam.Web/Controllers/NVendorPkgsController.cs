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
        public ActionResult Index(string id )
        {
            var pkgs = vendorProductsService.getvendorpkgs(id);
            ViewBag.pacakagerecord = pkgs;
            ViewBag.id = id;
            return View();
        }

        public ActionResult editpkg(string pid)
        {

            var pkgs = vendorProductsService.getpartpkgs(pid);
          
            //var pkgs = vendorProductsService.getvendorpkgs(id);
           ViewBag.pacakagerecord = pkgs;
            //ViewBag.id = id;
            return View();
        }
        public ActionResult updatepkg(string id, string packagename, string packageprice, string Packagedec)
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
                package = vendorVenueSignUpService.updatepack(id,package);
                ViewBag.vendormasterid = id;
                //return Content("<script type='text/javscript'> alert('package added'); location.href='/NVendorAddPackage/Index?id="+ id+ "</script>");
                return Content("<script language='javascript' type='text/javascript'>alert('package added');location.href='" + @Url.Action("Index", "NVendorAddPackage", new { id = id }) + "'</script>");
            }
            return View();
        }
    }
}