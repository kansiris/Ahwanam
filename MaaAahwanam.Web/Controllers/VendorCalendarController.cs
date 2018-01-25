using System;
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
            return View();
        }

        public JsonResult GetDates(string id)
        {
            long vendorsubid = vendorVenueSignUpService.GetVendorVenue(long.Parse(id)).FirstOrDefault().Id;
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
        public JsonResult SaveEvent(VendorDates vendorDates)
        {
            TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            long vendorsubid = vendorVenueSignUpService.GetVendorVenue(long.Parse(vendorDates.VendorId.ToString())).FirstOrDefault().Id;
            vendorDates.StartDate = TimeZoneInfo.ConvertTimeFromUtc(vendorDates.StartDate.ToUniversalTime(), tzi); 
            vendorDates.EndDate = TimeZoneInfo.ConvertTimeFromUtc(vendorDates.EndDate.ToUniversalTime(), tzi);
            var status = false;
            vendorDates.Vendorsubid = vendorsubid;
            //using (MyDatabaseEntities dc = new MyDatabaseEntities())
            //{
                if (vendorDates.Id > 0)
                {
                //Update the event
                vendorDates = vendorDatesService.UpdatesVendorDates(vendorDates,vendorDates.Id);
            }
                else
                {
                vendorDates = vendorDatesService.SaveVendorDates(vendorDates);
                    //dc.Events.Add(e);
                }

                //dc.SaveChanges();
                status = true;

            //}
            return new JsonResult { Data = new { status = status } };
        }
    }
}