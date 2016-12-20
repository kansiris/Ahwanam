using MaaAahwanam.Models;
using MaaAahwanam.Service;
using MaaAahwanam.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaaAahwanam.Web.Custom;

namespace MaaAahwanam.Web.Controllers
{
    public class AvailableDatesController : Controller
    {
        AvailabledatesService availabledatesService = new AvailabledatesService();
        VendorMasterService vendorMasterService = new VendorMasterService();
        // GET: AvailableDates
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(Availabledates availabledates,string availabledate)
        {
            var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
            string[] dates =  availabledate.Split(',');
                       
            string a = "";
            for (int i = 0; i < dates.Length - 1; i++)
            {
                availabledates.vendorId = (int)user.UserId;
                availabledates.servicetype = vendorMasterService.GetVendorServiceType(user.UserId).ServicType;
                availabledates.availabledate = Convert.ToDateTime(dates[i].Remove(dates[i].Length - 4));
                a = availabledatesService.saveavailabledates(availabledates);
            }
            if (a == "Success")
                return Content("<script language='javascript' type='text/javascript'>alert('Dates Submitted Successfully');location.href='" + @Url.Action("Index", "AvailableDates") + "'</script>");
            else
                return Content("<script language='javascript' type='text/javascript'>alert('Failed to Submitted dates');location.href='" + @Url.Action("Index", "AvailableDates") + "'</script>");
        }

        public JsonResult GetDates()
        {
            var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
            var availabledates = availabledatesService.GetDates(user.UserId).Select(m=>m.availabledate.ToShortDateString());
            return Json(availabledates,JsonRequestBehavior.AllowGet);
        }
    }
}