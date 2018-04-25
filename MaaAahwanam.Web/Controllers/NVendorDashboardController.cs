using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MaaAahwanam.Web.Controllers
{
    public class NVendorDashboardController : Controller
    {
        // GET: NVendorDashboard
        public ActionResult Index(string id)
        {
            ViewBag.id = id;
            return View();
        }
    }
}