using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaaAahwanam.Service;
using MaaAahwanam.Web.Custom;
using MaaAahwanam.Repository;

namespace MaaAahwanam.Web.Controllers
{
    public class NLiveDealsController : Controller
    {
        VendorProductsService vendorProductsService = new VendorProductsService();
        private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");

        // GET: NLiveDeals
        public ActionResult Index(string id, string eve)
        {
            try { 
          var  Deal1 = vendorProductsService.Getvendorproducts_Result("Venue").Take(4);//.Where(m => m.subtype == "Hotel");
          ViewBag.records = Deal1;
          return View();
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Nhomepage");
            }
        }
        
        public ActionResult Loadmore(string lastrecord, string eve)
        {
            try { 
            DateTime date = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);

            if (eve == null) { eve = "All"; }
            //int id = (lastrecord == null) ? 6 : int.Parse(lastrecord) + 6;
            int id = (lastrecord == null) ? 6 : int.Parse(lastrecord) + 6;
            //ViewBag.deal = vendorProductsService.getalldeal().OrderBy(m => m.DealID).Take(id);
            //var deals = vendorProductsService.getalldeal().OrderBy(m => m.DealID).Take(id);
            var deals = vendorProductsService.getalleventdeal(eve,date).OrderBy(m => m.DealID).Take(id);
            ViewBag.deal = deals;
            ViewBag.dealcount = vendorProductsService.getalleventdeal(eve,date).Count();
            ViewBag.dealLastRecord = id;
            ViewBag.dealLastRecordeve = eve;
            return PartialView("Loadmore");
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Nhomepage");
            }
        }
    }
}