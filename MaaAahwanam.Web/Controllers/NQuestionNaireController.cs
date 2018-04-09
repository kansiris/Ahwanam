using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MaaAahwanam.Web.Controllers
{
    public class NQuestionNaireController : Controller
    {
        // GET: NQuestionNaire
        public ActionResult Index(string location,string servicetype,string occasiondate)
        {
            ViewBag.selectedlocation = location;
            ViewBag.selectedoccasiondate = occasiondate;
            return View();
        }
    }
}