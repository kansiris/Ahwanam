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
    public class NVendorPkgsController : Controller
    {
        VendorProductsService vendorProductsService = new VendorProductsService();
        VenorVenueSignUpService vendorVenueSignUpService = new VenorVenueSignUpService();
        private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");

        // GET: NVendorPackages
        public ActionResult Index(string ks)
        {
            try { 
            if (TempData["Active"] != "")
            {
                ViewBag.msg = TempData["Active"];
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

                var pkgs = vendorProductsService.getvendorpkgs(id);
            ViewBag.pacakagerecord = pkgs;
            ViewBag.id = ks;
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
                    //TempData["Active"] = "Package Updated";
                    //return RedirectToAction("Index", "NVendorPkgs",new { id = vid });
                    string vssid = Convert.ToString(vid);
                    encptdecpt encript = new encptdecpt();

                    string encripted = encript.Encrypt(string.Format("Name={0}", vssid));
                    return Content("<script language='javascript' type='text/javascript'>alert('Package Updated sucessfully');location.href='" + @Url.Action("Index", "NVendorPkgs", new { ks = encripted }) + "'</script>");

                }
                //    TempData["Active"] = "package updated";
                //return RedirectToAction("Index", "Nhomepage", new { id = vid });
                return Content("<script> alert('Please Login');location.href='" + @Url.Action("Index", "Nhomepage", new { id = vid }) + "'</script>");

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
                        //TempData["Active"] = "Package Deleted";
                        //return RedirectToAction("Index", "NVendorPkgs", new { id = vid });
                        return Content("<script language='javascript' type='text/javascript'>alert('Package Deleted sucessfully');location.href='" + @Url.Action("Index", "NVendorPkgs", new { ks = vid }) + "'</script>");

                    }
                }
                //TempData["Active"] = "Please Login";
                //return RedirectToAction("Index", "Nhomepage", new { id = vid });
                return Content("<script language='javascript' type='text/javascript'>alert('Please Login');location.href='" + @Url.Action("Index", "Nhomepage") + "'</script>");

            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Nhomepage");
            }
        }
    } 
}

