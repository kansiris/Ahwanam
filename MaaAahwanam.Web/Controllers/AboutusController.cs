using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaaAahwanam.Models;
using MaaAahwanam.Service;
using MaaAahwanam.Web.Custom;

namespace MaaAahwanam.Web.Controllers
{
    public class AboutusController : Controller
    {
        //
        // GET: /Aboutus/
        public ActionResult Index()
        {
            EventsService eventsService = new EventsService();
            ViewBag.EventsCount = eventsService.EventInformationCount();
            ticketsService ticketsService = new ticketsService();
            ViewBag.Ticketscount = ticketsService.TicketsCount();
            return View();
        }
    }
}