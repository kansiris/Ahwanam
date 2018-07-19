using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaaAahwanam.Service;
using MaaAahwanam.Repository;

namespace MaaAahwanam.Web.Areas.Admin.Controllers
{
    public class QuotationsController : Controller
    {
        ProductInfoService productInfoService = new ProductInfoService();
        QuotationListsService quotationListsService = new QuotationListsService();
        VendorDatesService vendorDatesService = new VendorDatesService();
        // GET: Admin/Quotations
        public ActionResult Index()
        {
            ViewBag.quotations = quotationListsService.GetAllQuotations().Where(m => m.Status == "Active").ToList();
            return View();
        }

        public ActionResult QuoteReply(string id)
        {
            ViewBag.quotations = quotationListsService.GetAllQuotations().Where(m => m.Id == long.Parse(id)).FirstOrDefault();
            return View();
        }

        [HttpGet]
        public ActionResult FilteredVendors(string type, string date)
        {
            if (type != null && date != null)
            {
                var data = vendorDatesService.GetVendorsByService().Where(m => m.ServiceType == type).ToList();
                ViewBag.display = "1";
                ViewBag.records = seperatedates(data, date, type);
            }
            else if (type == null && date == null)
            {
                ViewBag.novendor = "Select Service Type & Date To View Vendors";//"No Vendors Available On This Date";
            }
            return PartialView("FilteredVendors", "Quotations");
        }

        public List<string[]> seperatedates(List<filtervendordates_Result> data, string date, string type)
        {
            List<string[]> betweendates = new List<string[]>();
            string dates = "";
            //var Gettotaldates = vendorDatesService.GetDates(long.Parse(id), long.Parse(vid));
            int recordcount = data.Count();
            foreach (var item in data)
            {
                var startdate = Convert.ToDateTime(item.StartDate);
                var enddate = Convert.ToDateTime(item.EndDate);
                if (startdate != enddate)
                {
                    string orderdates = productInfoService.disabledate(item.masterid, item.subid, type);
                    for (var dt = startdate; dt <= enddate; dt = dt.AddDays(1))
                    {
                        dates = (dates != "") ? dates + "," + dt.ToString("dd-MM-yyyy") : dt.ToString("dd-MM-yyyy");
                    }
                    if (dates.Split(',').Contains(date))
                    {
                        if (orderdates != "")
                            dates = String.Join(",", dates.Split(',').Where(i => !orderdates.Split(',').Any(e => i.Contains(e))));
                        string[] da = { item.masterid.ToString(), item.subid.ToString(), dates, item.BusinessName, item.ServiceType, item.VenueType, item.Type };
                        betweendates.Add(da);
                    }
                    dates = "";
                }
                else
                {
                    if (dates.Contains(date))
                    {
                        string[] da = { item.masterid.ToString(), item.subid.ToString(), startdate.ToString("dd-MM-yyyy"), item.BusinessName, item.ServiceType, item.VenueType, item.Type };
                        betweendates.Add(da);
                    }
                    dates = "";
                }
            }
            return betweendates;
        }

    }
}