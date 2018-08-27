using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaaAahwanam.Service;
using MaaAahwanam.Repository;

namespace MaaAahwanam.Web.Controllers
{
    public class resultsController : Controller
    {
        ResultsPageService resultsPageService = new ResultsPageService();
        // GET: results
        public ActionResult Index(string type, string loc, string eventtype, string count, string date)
        {
            type = (type == null) ? "Venue" : type;
            var data = resultsPageService.GetAllVendors(type);
            ViewBag.venues = data.Take(6).ToList();
            ViewBag.minprice = data.Select(m => m.cost1).Min();
            ViewBag.maxprice = data.Select(m => m.cost1).Max();
            ViewBag.count = 6;
            return View();
        }

        public PartialViewResult Loadmore(string count, string type)
        {
            type = (type == null || type == "") ? "Venue" : type;
            var selectedservices = type.Split(',');
            ViewBag.count = 6;
            ViewBag.venues = vendorlist(6, selectedservices, "first", 6); //list; //resultsPageService.GetAllVendors(type).Take(takecount).ToList();
            return PartialView("Loadmore");
        }

        public JsonResult LazyLoad(string count, string type)
        {
            type = (type == null || type == "") ? "Venue" : type;
            var selectedservices = type.Split(',');
            int takecount = (count == "" || count == null) ? 6 : int.Parse(count) * 6;
            ViewBag.count = takecount;
            List<GetVendors_Result> vendorslist;
            if (takecount == 6)
                vendorslist = vendorlist(12, selectedservices, "first", takecount);//resultsPageService.GetAllVendors(type).Take(12).ToList(); 
            else
                vendorslist = vendorlist(6, selectedservices, "next", takecount);//resultsPageService.GetAllVendors(type).Skip(takecount).Take(6).ToList(); 
            return Json(vendorslist);
        }

        public List<GetVendors_Result> vendorlist(int count, string[] selectedservices, string command,int takecount)
        {
            List<GetVendors_Result> list = new List<GetVendors_Result>();
            int recordcount = 0;
            recordcount = count / selectedservices.Count();
            takecount = takecount / selectedservices.Count();
            for (int i = 0; i < selectedservices.Count(); i++)
            {
                selectedservices[i] = (selectedservices[i] == "Convention") ? "Convention Hall" : selectedservices[i];
                selectedservices[i] = (selectedservices[i] == "Banquet") ? "Banquet Hall" : selectedservices[i];
                selectedservices[i] = (selectedservices[i] == "Function") ? "Function Hall" : selectedservices[i];
                var getrecords = resultsPageService.GetAllVendors(selectedservices[i]);//.Take(recordcount).ToList();
                if (command == "next")
                    getrecords = getrecords.Skip(takecount).Take(recordcount).ToList();
                else if (command == "first")
                    getrecords = getrecords.Take(recordcount).ToList();
                list.AddRange(getrecords);
            }
            return list;
        }
    }
}