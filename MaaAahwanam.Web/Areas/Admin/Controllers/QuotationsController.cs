using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaaAahwanam.Service;

namespace MaaAahwanam.Web.Areas.Admin.Controllers
{
    public class QuotationsController : Controller
    {
        QuotationListsService quotationListsService = new QuotationListsService();
        VendorDatesService vendorDatesService = new VendorDatesService();
        // GET: Admin/Quotations
        public ActionResult Index()
        {
            ViewBag.quotations = quotationListsService.GetAllQuotations().Where(m=>m.Status == "Active").ToList();
            return View();
        }

        public ActionResult QuoteReply(string id)
        {
            ViewBag.quotations = quotationListsService.GetAllQuotations().Where(m => m.Id == long.Parse(id)).FirstOrDefault();
            return View();
        }

        [HttpGet]
        public ActionResult FilteredVendors(string type,string date)
        {
            DateTime convertdate = Convert.ToDateTime(date);
            //var selecteddate = new DateTime(convertdate.Year, convertdate.Month, convertdate.Day);
            ViewBag.vendors = vendorDatesService.GetVendorsByService().Where(m => m.ServiceType == type && m.StartDate >= convertdate || m.EndDate <= convertdate).ToList();
            if (ViewBag.vendors.Count == 0)
            {
                ViewBag.novendor = "No Vendors Available On This Date";
            }
            return PartialView("FilteredVendors", "Quotations");
        }
    }
}