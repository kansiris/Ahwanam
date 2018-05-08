using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaaAahwanam.Models;
using MaaAahwanam.Service;

namespace MaaAahwanam.Web.Controllers
{
    public class OthersFAQController : Controller
    {
        Vendormaster vendorMaster = new Vendormaster();
        VenorVenueSignUpService venorVenueSignUpService = new VenorVenueSignUpService();
        VendorOthersService vendorOthersService = new VendorOthersService();

        // GET: OthersFAQ
        public ActionResult Index(string id, string vid)
        {
            ViewBag.id = id;
            ViewBag.vid = vid;
            ViewBag.other = vendorOthersService.GetVendorOther(long.Parse(id), long.Parse(vid));
            return View();
        }

        [HttpPost]
        public ActionResult Index(string id, string vid, VendorsOther vendorsOther)
        {
            var data = vendorOthersService.GetVendorOther(long.Parse(id), long.Parse(vid));
            vendorsOther.UpdatedBy = vendorMaster.UpdatedBy = 2;
            vendorsOther.type = data.type;
            vendorsOther.MinOrder = data.MinOrder;
            vendorsOther.MaxOrder = data.MaxOrder;
            vendorsOther.discount = data.discount;
            vendorsOther.Address = data.Address;
            vendorsOther.City = data.City;
            vendorsOther.State = data.State;
            vendorsOther.Landmark = data.Landmark;
            vendorsOther.ZipCode = data.ZipCode;
            vendorsOther.name = data.name;
            vendorsOther.Status = data.Status;
            long masterid = vendorsOther.VendorMasterId = vendorMaster.Id = long.Parse(id);
            vendorsOther = venorVenueSignUpService.UpdateOther(vendorsOther, vendorMaster, masterid, long.Parse(vid));
            return Content("<script language='javascript' type='text/javascript'>alert('Details Saved Successfully');location.href='AvailableServices/Index?id=" + id + "&&vid=" + vid + "'</script>");
        }
    }
}