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
        public ActionResult Index()
        {
            //ViewBag.venues = resultsPageService.GetAllVendors("Venue");.Take(6).ToList();
            return View();
        }

        public PartialViewResult Loadmore(string count)
        {
            int takecount = (count == null) ? 6 : int.Parse(count) +6;
            ViewBag.count = takecount;
            ViewBag.venues = resultsPageService.GetAllVendors("Venue").Take(takecount).ToList();
            return PartialView("Loadmore");
        }
    }
}