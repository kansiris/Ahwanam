using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaaAahwanam.Service;

namespace MaaAahwanam.Web.Controllers
{
    public class ContestsController : Controller
    {
        ContestsService contestsService = new ContestsService();
        // GET: Contests
        public ActionResult Index()
        {
            var contests = contestsService.GetAllContests().Where(m => m.Status == "Active");
            ViewBag.contests = contests;
            return View();
        }
    }
}