using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaaAahwanam.Service;
using MaaAahwanam.Models;

namespace MaaAahwanam.Web.Areas.Admin.Controllers
{
    public class ndealsController : Controller
    {
        VendorSetupService vendorSetupService = new VendorSetupService();
        Ndealservice ndealservice = new Ndealservice();
        VendorProductsService vendorProductsService = new VendorProductsService();

        // GET: Admin/ndeals
        public ActionResult Index()
        {
            //ViewBag.deals = vendorProductsService.getalleventdeal("All", DateTime.Now).OrderByDescending(m => m.DealID).ToList();
            ViewBag.deals = ndealservice.GetAllDeals().OrderByDescending(m => m.DealID).ToList();
            return View();
        }

        public JsonResult FilterVendors(string category, string subcategory)
        {
            var VendorList = vendorSetupService.AllVendorList(category);
            if (subcategory != null && subcategory != "")
                VendorList = VendorList.Where(m => m.VenueType == subcategory).ToList();
            return Json(VendorList);
        }

        public JsonResult SaveRecord(string start, string end, string cat, string subcat, string selected, string dealname, string dealprice, string originalprice)
        {
            NDeals deal = new NDeals();
            deal.DealStartDate = Convert.ToDateTime(start);
            deal.DealEndDate = Convert.ToDateTime(end);
            deal.VendorType = cat;
            deal.VendorSubType = subcat;
            deal.DealName = deal.DealDescription = dealname;
            for (int i = 0; i < selected.Split(',').Count(); i++)
            {
                deal.VendorId = long.Parse(selected.Split(',')[i].Split('!')[0]);
                deal.VendorSubId = long.Parse(selected.Split(',')[i].Split('!')[1]);
                deal.OriginalPrice = Decimal.Parse(originalprice.Split(',')[i]);
                deal.DealPrice = Decimal.Parse(dealprice.Split(',')[i]);
                deal = ndealservice.AddDeal(deal);
            }
            string msg = "Deal Added Successfully!!!";
            if (deal.DealID == 0) msg = "Failed To Add Deal";
            return Json(msg);
        }
    }
}