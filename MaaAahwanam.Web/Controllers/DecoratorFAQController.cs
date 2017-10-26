using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaaAahwanam.Models;
using MaaAahwanam.Service;

namespace MaaAahwanam.Web.Controllers
{
    public class DecoratorFAQController : Controller
    {
        Vendormaster vendorMaster = new Vendormaster();
        VenorVenueSignUpService venorVenueSignUpService = new VenorVenueSignUpService();
        VendorDecoratorService vendorDecoratorService = new VendorDecoratorService();
        // GET: DecoratorFAQ
        public ActionResult Index(string id, string vid)
        {
            ViewBag.id = id;
            ViewBag.vid = vid;
            ViewBag.decorator = vendorDecoratorService.GetVendorDecorator(long.Parse(id), long.Parse(vid));
            return View();
        }

        [HttpPost]
        public ActionResult Index(string id, string vid, VendorsDecorator vendorsDecorator)
        {
            var data = vendorDecoratorService.GetVendorDecorator(long.Parse(id), long.Parse(vid));
            vendorsDecorator.UpdatedBy = vendorMaster.UpdatedBy = 2;
            vendorsDecorator.DecorationType = data.DecorationType;
            long masterid = vendorsDecorator.VendorMasterId = vendorMaster.Id = long.Parse(id);
            vendorsDecorator = venorVenueSignUpService.UpdateDecorator(vendorsDecorator, vendorMaster, masterid, long.Parse(vid));
            return Content("<script language='javascript' type='text/javascript'>alert('Details Updated');location.href='AvailableServices/Index?id=" + id + "&&vid=" + vid + "'</script>");
        }
    }
}