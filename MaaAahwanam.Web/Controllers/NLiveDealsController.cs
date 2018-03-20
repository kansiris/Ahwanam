using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaaAahwanam.Service;
using MaaAahwanam.Repository;

namespace MaaAahwanam.Web.Controllers
{
    public class NLiveDealsController : Controller
    {
        VendorProductsService vendorProductsService = new VendorProductsService();

        // GET: NLiveDeals
        public ActionResult Index(string id)
        {
            ViewBag.records = vendorProductsService.Getvendorproducts_Result("Venue").Take(4);//.Where(m => m.subtype == "Hotel");
            return View();
        }




        public PartialViewResult Loadmore(string lastrecord)
        {
            int id = (lastrecord == null) ? 6 : int.Parse(lastrecord) + 6;
            ViewBag.deal = vendorProductsService.getalldeal().OrderBy(m => m.DealID).Take(id);
            var deals = vendorProductsService.getalldeal().OrderBy(m => m.DealID).Take(id);
            ViewBag.dealLastRecord = id;
            ViewBag.dealcount = vendorProductsService.getalldeal().Count();
            return PartialView("Loadmore");
        }


    }
}