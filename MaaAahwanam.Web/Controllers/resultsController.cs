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
            ViewBag.venues = resultsPageService.GetAllVendors(type).Take(6).ToList();
            ViewBag.count = 6;
            return View();
        }

        public PartialViewResult Loadmore(string count, string type)
        {
            type = (type == null) ? "Venue" : type;
            var selectedservices = type.Split(',');
            //List<GetVendors_Result> list = new List<GetVendors_Result>();
            //int recordcount = 0;
            //recordcount = 6/selectedservices.Count() ;
            //for (int i = 0; i < selectedservices.Count(); i++)
            //{
            //    selectedservices[i] = (selectedservices[i] == "Convention") ? "Convention Hall" : selectedservices[i];
            //    selectedservices[i] = (selectedservices[i] == "Banquet") ? "Banquet Hall" : selectedservices[i];
            //    selectedservices[i] = (selectedservices[i] == "Function") ? "Function Hall" : selectedservices[i];
            //    var getrecords = resultsPageService.GetAllVendors(selectedservices[i]).Take(recordcount).ToList();
            //    list.AddRange(getrecords);
            //}
            ViewBag.count = 6;
            ViewBag.venues = vendorlist(6, selectedservices, "first"); //list; //resultsPageService.GetAllVendors(type).Take(takecount).ToList();
            return PartialView("Loadmore");
        }

        public JsonResult LazyLoad(string count, string type)
        {
            type = (type == null) ? "Venue" : type;
            var selectedservices = type.Split(',');
            int takecount = (count == "" || count == null) ? 6 : int.Parse(count) * 6;
            ViewBag.count = takecount;
            List<GetVendors_Result> vendorslist;
            if (takecount == 6)
                vendorslist = vendorlist(12, selectedservices, "first");//resultsPageService.GetAllVendors(type).Take(12).ToList();
            else
                vendorslist = vendorlist(6, selectedservices, "next"); //resultsPageService.GetAllVendors(type).Skip(takecount).Take(6).ToList();


            return Json(vendorslist);
        }

        public List<GetVendors_Result> vendorlist(int count, string[] selectedservices, string command)
        {
            List<GetVendors_Result> list = new List<GetVendors_Result>();
            int recordcount = 0;
            recordcount = count / selectedservices.Count();
            for (int i = 0; i < selectedservices.Count(); i++)
            {
                selectedservices[i] = (selectedservices[i] == "Convention") ? "Convention Hall" : selectedservices[i];
                selectedservices[i] = (selectedservices[i] == "Banquet") ? "Banquet Hall" : selectedservices[i];
                selectedservices[i] = (selectedservices[i] == "Function") ? "Function Hall" : selectedservices[i];
                var getrecords = resultsPageService.GetAllVendors(selectedservices[i]);//.Take(recordcount).ToList();
                if (command == "next")
                    getrecords = getrecords.Skip(recordcount).Take(recordcount).ToList();
                else if (command == "first")
                    getrecords = getrecords.Take(recordcount).ToList();
                list.AddRange(getrecords);
            }
            return list;
        }
    }
}