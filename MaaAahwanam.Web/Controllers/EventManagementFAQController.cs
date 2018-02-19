using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaaAahwanam.Models;
using MaaAahwanam.Service;

namespace MaaAahwanam.Web.Controllers
{
    public class EventManagementFAQController : Controller
    {
        Vendormaster vendorMaster = new Vendormaster();
        VenorVenueSignUpService venorVenueSignUpService = new VenorVenueSignUpService();
        VendorEventOrganiserService vendorEventOrganiserService = new VendorEventOrganiserService();

        // GET: EventManagementFAQ
        public ActionResult Index(string id, string vid)
        {
            ViewBag.id = id;
            ViewBag.vid = vid;
            ViewBag.Event = vendorEventOrganiserService.GetVendorEventOrganiser(long.Parse(id), long.Parse(vid));
            return View();
        }

        [HttpPost]
        public ActionResult Index(string id, string vid, VendorsEventOrganiser vendorsEventOrganiser)
        {
            var data = vendorEventOrganiserService.GetVendorEventOrganiser(long.Parse(id), long.Parse(vid));
            vendorsEventOrganiser.UpdatedBy = vendorMaster.UpdatedBy = 2;
            vendorsEventOrganiser.discount = data.discount;
            vendorsEventOrganiser.Address = data.Address;
            vendorsEventOrganiser.City = data.City;
            vendorsEventOrganiser.State = data.State;
            vendorsEventOrganiser.Landmark = data.Landmark;
            vendorsEventOrganiser.ZipCode = data.ZipCode;
            vendorsEventOrganiser.name = data.name;
            vendorsEventOrganiser.Status = data.Status;
            long masterid = vendorsEventOrganiser.VendorMasterId = vendorMaster.Id = long.Parse(id);
            vendorsEventOrganiser = venorVenueSignUpService.UpdateEventOrganiser(vendorsEventOrganiser, vendorMaster, masterid, long.Parse(vid));
            return Content("<script language='javascript' type='text/javascript'>alert('Details Saved Successfully');location.href='AvailableServices/Index?id=" + id + "&&vid=" + vid + "'</script>");
        }
    }
}