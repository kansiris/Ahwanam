﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaaAahwanam.Web.Custom;
using MaaAahwanam.Service;
using MaaAahwanam.Models;

namespace MaaAahwanam.Web.Controllers
{
    public class VendorCalendarController : Controller
    {
        //AvailabledatesService availabledatesService = new AvailabledatesService();
        VendorDatesService vendorDatesService = new VendorDatesService();
        VenorVenueSignUpService vendorVenueSignUpService = new VenorVenueSignUpService();
        // GET: VendorCalendar
        public ActionResult Index(string id)
        {
            //ViewBag.vendorsubcatids = vendorVenueSignUpService.GetVendorVenue(long.Parse(id)).Select(m => m.Id);
            ViewBag.vendorsubcatids = vendorVenueSignUpService.GetVendorVenue(long.Parse(id)).Select(m => new SelectListItem { Text= m.VenueType ,Value= m.Id.ToString()}).ToList();
            return View();
        }

        public JsonResult GetDates(string id, string vid)
        {
            var vendorsubcatids = vendorVenueSignUpService.GetVendorVenue(long.Parse(id));
            long vendorsubid = (vid == null || vid == "undefined" || vid == "") ? vendorsubcatids.FirstOrDefault().Id : long.Parse(vid); // if vid is null then automatically first Subcategory ID will be considered
            var data = vendorDatesService.GetDates(long.Parse(id), vendorsubid);
            return new JsonResult { Data = data, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpPost]
        public JsonResult DeleteEvent(string id)
        {
            var status = false;
            string msg = vendorDatesService.removedates(long.Parse(id));
            if (msg == "Removed") status = true;
            return new JsonResult { Data = new { status = status } };
        }

        [HttpPost]
        public JsonResult SaveEvent(VendorDates vendorDates,string vid)
        {
            long vendorsubid = (vid == null || vid=="undefined"||vid=="")? vendorVenueSignUpService.GetVendorVenue(long.Parse(vendorDates.VendorId.ToString())).FirstOrDefault().Id : long.Parse(vid);
            var status = false;
            vendorDates.Vendorsubid = vendorsubid;
            if (vendorDates.Id > 0) //Update the event
                vendorDates = vendorDatesService.UpdatesVendorDates(vendorDates, vendorDates.Id);
            else //Save event
                vendorDates = vendorDatesService.SaveVendorDates(vendorDates);
            status = true;
            return new JsonResult { Data = new { status = status } };
        }

        [HttpGet]
        public JsonResult GetParticularDate(long id)
        {
            VendorDates dates = vendorDatesService.GetParticularDate(id);
            return new JsonResult { Data = dates, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }
}