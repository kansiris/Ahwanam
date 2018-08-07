using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaaAahwanam.Service;

namespace MaaAahwanam.Web.Controllers
{
    public class HomeController : Controller
    {
        ResultsPageService resultsPageService = new ResultsPageService();
        // GET: Home
        public ActionResult Index()
        {
            ViewBag.venues = resultsPageService.GetAllVendors("Venue").Take(3);
            return View();
        }
    }
}