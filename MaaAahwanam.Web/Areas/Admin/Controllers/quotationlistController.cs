using MaaAahwanam.Models;
using MaaAahwanam.Repository;
using MaaAahwanam.Service;
using MaaAahwanam.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MaaAahwanam.Web.Areas.Admin.Controllers
{

    public class quotationlistController : Controller
    {
        ProductInfoService productInfoService = new ProductInfoService();
        QuotationListsService quotationListsService = new QuotationListsService();
        VendorDatesService vendorDatesService = new VendorDatesService();
        private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        // GET: Admin/quotationlist
        public ActionResult Index()
        {
            ViewBag.quotations = quotationListsService.GetAllQuotations().ToList(); //.Where(m => m.Status == "Active")
            return View();
        }
        public ActionResult QuoteReply(string id)
        {
            ViewBag.quotations = quotationListsService.GetAllQuotations().Where(m => m.Id == long.Parse(id)).FirstOrDefault();
            return View();
        }
        [HttpGet]
        public ActionResult FilteredVendors(string type, string date, string id)
        {
            if (type != null && date != null)
            {
                ViewBag.id = id;
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


        public JsonResult ParticularQuoteReply(string QuoteID, string id)
        {
            DateTime indianTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
            var particularquote = quotationListsService.GetAllQuotations().FirstOrDefault(m => m.Id == long.Parse(QuoteID));
            if (particularquote.FirstTime == null && particularquote.FirstTimeQuoteDate == null)
            {
                particularquote.FirstTime = id;
                particularquote.FirstTimeQuoteDate = indianTime.ToString("dd-MM-yyyy hh:mm:ss");
            }
            //else if (particularquote.FirstTime != null && particularquote.FirstTimeQuoteDate != null && particularquote.SecondTime == null && particularquote.SecondTimeQuoteDate == null)
            //{
            //    particularquote.SecondTime = id;
            //    particularquote.SecondTimeQuoteDate = indianTime.ToString("dd-MM-yyyy");
            //}
            //else if (particularquote.FirstTime != null && particularquote.FirstTimeQuoteDate != null && particularquote.SecondTime != null && particularquote.SecondTimeQuoteDate != null && particularquote.ThirdTime == null && particularquote.ThirdTimeQuoteDate == null)
            //{
            //    particularquote.ThirdTime = id;
            //    particularquote.ThirdTimeQuoteDate = indianTime.ToString("dd-MM-yyyy");
            //}

            FileInfo File = new FileInfo(Server.MapPath("/mailtemplate/QuoteReply.html"));
            string readFile = File.OpenText().ReadToEnd();
            readFile = readFile.Replace("[name]", particularquote.Name);
            readFile = readFile.Replace("[Email]", particularquote.EmailId);
            string txtto = particularquote.EmailId;
            string txtmessage = readFile;
            string subj = "Response to your Quote #" + particularquote.Id + "";
            int count = quotationListsService.UpdateQuote(particularquote);
            EmailSendingUtility emailSendingUtility = new EmailSendingUtility();
            emailSendingUtility.Email_maaaahwanam(txtto, txtmessage, subj);
            return Json("sucess");
        }

        public JsonResult SaveInstallments(QuoteResponse quoteResponse)
        {
            DateTime indianTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
            quoteResponse.UpdatedDate = indianTime;
            quoteResponse.Status = "Active";
            if (quoteResponse.SecondInstallment == ",")
            {
                quoteResponse.SecondInstallment = "0,0";
            }
            if (quoteResponse.ThirdInstallment == ",")
            {
                quoteResponse.ThirdInstallment = "0,0";
            }
            if (quoteResponse.FourthInstallment == ",")
            {
                quoteResponse.FourthInstallment = "0,0";
            }
            if (quoteResponse.FifthInstallment == ",")
            {
                quoteResponse.FifthInstallment = "0,0";
            }
            int count = quotationListsService.AddInstallments(quoteResponse);
            if (count > 0)
                return Json("Success");
            else
                return Json("Failed");
        }
    }
}
