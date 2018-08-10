using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaaAahwanam.Service;

namespace MaaAahwanam.Web.Controllers
{
    public class resultsController : Controller
    {
        ResultsPageService resultsPageService = new ResultsPageService();
        // GET: results
        public ActionResult Index(string type)
        {
            type = (type == null) ? "Venue" : type;
            ViewBag.venues = resultsPageService.GetAllVendors(type);//.Take(6).ToList();
            return View();
        }

        //public PartialViewResult Loadmore(string count,string type)
        //{
        //    type = (type == null) ? "Venue" : type;
        //    int takecount = (count == "" || count == null) ? 6 : int.Parse(count) +6;
        //    ViewBag.count = takecount;
        //    ViewBag.venues = resultsPageService.GetAllVendors(type).Take(takecount).ToList();
        //    return PartialView("Loadmore");
        //}
    }
}