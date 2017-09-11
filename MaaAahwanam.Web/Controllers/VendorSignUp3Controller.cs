using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaaAahwanam.Models;
using MaaAahwanam.Service;

namespace MaaAahwanam.Web.Controllers
{
    public class VendorSignUp3Controller : Controller
    {
        Vendormaster vendorMaster = new Vendormaster();
        VenorVenueSignUpService venorVenueSignUpService = new VenorVenueSignUpService();
        VendorVenueService vendorVenueService = new VendorVenueService();
        // GET: VendorSignUp3
        public ActionResult Index(string id, string vid)
        {
            ViewBag.id = id;
            ViewBag.vid = vid;
            return View();
        }

        [HttpPost]
        public ActionResult Index(string id, string vid,VendorVenue vendorVenue)
        {
            var data = vendorVenueService.GetVendorVenue(long.Parse(id), long.Parse(vid));
            vendorVenue.UpdatedBy = 2;
            vendorMaster.UpdatedBy = 2;
            vendorVenue.VenueType = data.VenueType;
            long masterid = vendorVenue.VendorMasterId = vendorMaster.Id = long.Parse(id);
            vendorVenue = venorVenueSignUpService.UpdateVenue(vendorVenue, vendorMaster, masterid, long.Parse(vid));
            //return RedirectToAction("Index", "VendorSignUp4", new { id = id, vid = vid });
            //return Content("<script language='javascript' type='text/javascript'>alert('FAQ's Updated');location.href='" + @Url.Action("Index", "VendorSignUp4", new { id = id, vid = vid }) + "'</script>");
            return Content("<script language='javascript' type='text/javascript'>alert('FAQs Updated');location.href='" + @Url.Action("Index", "VendorSignUp4", new { id = id, vid = vid }) + "'</script>");
        }
    }
}