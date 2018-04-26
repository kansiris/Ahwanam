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
           
            if (packagename == null)
            {

                return Content("<script> alert('enter package name');location.href='" + @Url.Action("editpkg", "NVendorPkgs", new { id = id }) + "' </script>");
            }
            if (packageprice == null)
            {

                return Content("<script> alert('enter package price');location.href='" + @Url.Action("editpkg", "NVendorPkgs", new { id = id }) + "' </script>");
            }
            if (Packagedec == null)
            {

                return Content("<script> alert('enter desciption');location.href='" + @Url.Action("editpkg", "NVendorPkgs", new { id = id }) + "' </script>");
            }
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
                //return Content("<script type='text/javscript'> alert('package added'); location.href='/NVendorAddPackage/Index?id="+ id+ "</script>");
                return Content("<script language='javascript' type='text/javascript'>alert('package updated');location.href='" + @Url.Action("Index", "NVendorPkgs", new { id = vid }) + "'</script>");
            }
            return Content("<script language='javascript' type='text/javascript'>alert('Please login');location.href='" + @Url.Action("Index", "Nhomepage", new { id = vid }) + "'</script>");
        }
        public ActionResult deletepkg(string id, string vid)
        {

            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {

                string message = vendorVenueSignUpService.deletepack(id);
                ViewBag.vendormasterid = id;
                if (message == "success")
                { 
                    //return Content("<script type='text/javscript'> alert('package added'); location.href='/NVendorAddPackage/Index?id="+ id+ "</script>");
                    return Content("<script language='javascript' type='text/javascript'>alert('package deleted');location.href='" + @Url.Action("Index", "NVendorPkgs", new { id = vid }) + "'</script>");
            }
            }
            return Content("<script language='javascript' type='text/javascript'>alert('Please login');location.href='" + @Url.Action("Index", "Nhomepage", new { id = vid }) + "'</script>");
        }
    }

    
}

