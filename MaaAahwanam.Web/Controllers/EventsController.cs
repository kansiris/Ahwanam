using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaaAahwanam.Utility;
using MaaAahwanam.Models;
using MaaAahwanam.Service;
using MaaAahwanam.Repository;

namespace MaaAahwanam.Web.Controllers
{
    public class EventsController : Controller
    {
        EventsandtipsService eventsandtipsService = new EventsandtipsService();
        public ActionResult Index()
        {
            var Events = eventsandtipsService.EventsandTipsListUser("Event", 0).OrderByDescending(m => m.EventId).Take(4);
            ViewBag.UpcomingEvents = Events;
            ViewBag.EventLastRecord = Events.Last().EventId;
            return View();
        }

        public JsonResult Loadmore(string lastrecord)
        {
            var Events = eventsandtipsService.EventsandTipsListUser("Event", int.Parse(lastrecord)).OrderByDescending(m => m.EventId).Take(4);
            return Json(Events);
        }

    }
}