using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MaaAahwanam.Web.Controllers
{
    public class vdbController : Controller
    {
        // GET: vdb
        public ActionResult Index(string c)
        {
            ViewBag.enable = c;
            return View();
        }
    }
}