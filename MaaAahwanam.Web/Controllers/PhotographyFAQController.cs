using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaaAahwanam.Models;
using MaaAahwanam.Service;

namespace MaaAahwanam.Web.Controllers
{
    public class PhotographyFAQController : Controller
    {
        Vendormaster vendorMaster = new Vendormaster();
        VenorVenueSignUpService venorVenueSignUpService = new VenorVenueSignUpService();
        VendorPhotographyService vendorPhotographyService = new VendorPhotographyService();
        // GET: PhotographyFAQ
        public ActionResult Index(string id, string vid)
        {
            ViewBag.id = id;
            ViewBag.vid = vid;
            ViewBag.photographer = vendorPhotographyService.GetVendorPhotography(long.Parse(id), long.Parse(vid));
            return View();
        }

        [HttpPost]
        public ActionResult Index(string id, string vid, VendorsPhotography vendorPhotography)
        {
            var data = vendorPhotographyService.GetVendorPhotography(long.Parse(id), long.Parse(vid));
            vendorPhotography.UpdatedBy = vendorMaster.UpdatedBy = 2;
            vendorPhotography.PhotographyType = data.PhotographyType;
            vendorPhotography.discount = data.discount;
            vendorPhotography.Address = data.Address;
            vendorPhotography.City = data.City;
            vendorPhotography.State = data.State;
            vendorPhotography.Landmark = data.Landmark;
            vendorPhotography.ZipCode = data.ZipCode;
            vendorPhotography.name = data.name;
            vendorPhotography.Status = data.Status;
            vendorPhotography.tier = data.tier;
            long masterid = vendorPhotography.VendorMasterId = vendorMaster.Id = long.Parse(id);
            vendorPhotography = venorVenueSignUpService.UpdatePhotography(vendorPhotography, vendorMaster, masterid, long.Parse(vid));
            return Content("<script language='javascript' type='text/javascript'>alert('FAQs Updated');location.href='AvailableServices/Index?id=" + id + "&&vid=" + vid + "'</script>");
        }
    }
}