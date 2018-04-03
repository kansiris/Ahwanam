using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaaAahwanam.Service;
using MaaAahwanam.Repository;
using MaaAahwanam.Web.Custom;

namespace MaaAahwanam.Web.Controllers
{
    public class NViewDealController : Controller
    {
        VendorProductsService vendorProductsService = new VendorProductsService();
       
        // GET: NViewDeal
        public ActionResult Index(string id, string type)
        {
            try { 
            ViewBag.singledeal = vendorProductsService.getparticulardeal(Int32.Parse(id),  type).FirstOrDefault();

            return View();
            }
            catch(Exception)
            { return RedirectToAction("Index", "Nhomepage"); }
        }

        public PartialViewResult Loadmoredeals(string lastrecord)
        {
            int id = (lastrecord == null) ? 2 : int.Parse(lastrecord) + 2;
            ViewBag.deal = vendorProductsService.getalldeal().OrderBy(m => m.DealID).Take(id);
            var deals = vendorProductsService.getalldeal().OrderBy(m => m.DealID).Take(id);
            ViewBag.dealLastRecord = id;
            ViewBag.dealcount = vendorProductsService.getalldeal().Count();
            return PartialView("Loadmoredeals");
        }

    }
}