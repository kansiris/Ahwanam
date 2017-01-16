using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaaAahwanam.Service;

namespace MaaAahwanam.Web.Controllers
{
    public class EventInfoController : Controller
    {
        EventsandtipsService eventsandtipsService = new EventsandtipsService();
        public ActionResult Index(long id)
        {

            if (id != 0)
            {
                var eventinfo = eventsandtipsService.GetEventandTip(id);
                var Imagelist = eventinfo.Image.Split(',');
                ViewBag.Imagelist = Imagelist;
                return View(eventinfo);
            }
            return View();
        }
    }
}