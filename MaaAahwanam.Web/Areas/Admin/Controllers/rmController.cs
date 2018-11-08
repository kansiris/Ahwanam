using DotNetOpenAuth.Messaging;
using MaaAahwanam.Models;
using MaaAahwanam.Repository;
using MaaAahwanam.Service;
using MaaAahwanam.Web.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MaaAahwanam.Web.Areas.Admin.Controllers
{
    public class rmController : Controller
    {
        VenorVenueSignUpService vendorVenueSignUpService = new VenorVenueSignUpService();
        VendorVenueService vendorVenueService = new VendorVenueService();
        viewservicesservice viewservicesss = new viewservicesservice();
        newmanageuser newmanageuse = new newmanageuser();
        Vendormaster vendorMaster = new Vendormaster();
        private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        PartnerService partnerservice = new PartnerService();
        const string imagepath = @"/partnerdocs/";

        // GET: Admin/rm
        public ActionResult Index(string partid,string vid)

        {

            var allresellers = partnerservice.GetallPartners();
            var allresellerspack = partnerservice.getallPartnerPackage();
            ViewBag.allresellers = allresellers;

            if (partid != null)
            {
                var part1 = allresellers.Where(m => m.PartnerID == long.Parse(partid)).FirstOrDefault();
                ViewBag.masterid = vid = part1.VendorId.ToString();
                var resellers = partnerservice.GetPartners(vid);
                var resellerspack = partnerservice.getPartnerPackage(vid);
                var pkgs = viewservicesss.getvendorpkgs(vid).ToList();
                List<string> pppl = new List<string>();
                List<PartnerPackage> p = new List<PartnerPackage>();
                List<SPGETNpkg_Result> p1 = new List<SPGETNpkg_Result>();
                if (partid != "" && partid != null)
                {
                    var resellerspacklist = resellerspack.Where(m => m.PartnerID == long.Parse(partid)).ToList();
                    var pkglist = resellerspacklist.Select(m => m.packageid).ToList();
                    foreach (var item in pkgs)
                    {
                        if (pkglist.Contains(item.PackageID.ToString()))
                            p.AddRange(resellerspack.Where(m => m.packageid == item.PackageID.ToString()).ToList());
                        else
                            p1.AddRange(pkgs.Where(m => m.PackageID == item.PackageID));
                    }
                    ViewBag.resellerpkg = p;
                    ViewBag.pkg = p1;
                    ViewBag.resellers = resellers.Where(m => m.PartnerID == long.Parse(partid)).FirstOrDefault();
                    ViewBag.resellersfiles = partnerservice.GetFiles(vid, partid);
                }
                ViewBag.package = pkgs;
                var venues = vendorVenueSignUpService.GetVendorVenue(long.Parse(vid)).ToList();
                var catering = vendorVenueSignUpService.GetVendorCatering(long.Parse(vid)).ToList();
                var photography = vendorVenueSignUpService.GetVendorPhotography(long.Parse(vid));
                var decorators = vendorVenueSignUpService.GetVendorDecorator(long.Parse(vid));
                var others = vendorVenueSignUpService.GetVendorOther(long.Parse(vid));
                ViewBag.venues = venues;
                ViewBag.catering = catering;
                ViewBag.photography = photography;
                ViewBag.decorators = decorators;
                ViewBag.others = others;
                ViewBag.partid = partid;
            }
            else
            {
                ViewBag.partid = "ks";
            }
            return View();
            }

        public ActionResult revenumodelvendors()
        {
           
            return View();
        }
    }
}