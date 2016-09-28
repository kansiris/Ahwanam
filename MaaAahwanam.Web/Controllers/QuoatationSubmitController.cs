using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaaAahwanam.Models;
using MaaAahwanam.Service;
using MaaAahwanam.Web.Custom;
using System.Globalization;

namespace MaaAahwanam.Web.Controllers
{
    public class QuoatationSubmitController : Controller
    {
        //
        // GET: /QuoatationSubmit/
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [Authorize]
        public ActionResult Index(ServiceRequest serviceRequest)
        {
            //DateTime d = DateTime.Parse(serviceRequest.EventStartDate.ToString());
            //string Test = d.ToShortDateString();
            //DateTime d2= DateTime.ParseExact(d.ToString("dd-MM-yyyy"), "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            //DateTime dt = DateTime.ParseExact(Test.ToString(), "MM/dd/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
            //string s = dt.ToString("dd/M/yyyy", CultureInfo.InvariantCulture);
            var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
            ServiceRequestService serviceRequestService = new ServiceRequestService();
            serviceRequest.Type = "Quotation";
            serviceRequest.UpdatedTime = DateTime.Now;
            serviceRequest.UpdatedBy = (int)user.UserId;
            //serviceRequest.EventStartDate.Value.ToString("MM/dd/yyyy HH:mm:ss.fff");
            //serviceRequest.EventEnddate.Value.ToString("MM/dd/yyyy HH:mm:ss.fff");
            serviceRequest.Status = "Due";
            serviceRequest.ServiceType.TrimStart(',');
            serviceRequest = serviceRequestService.SaveService(serviceRequest);
            return RedirectToAction("Index","QuoatationConfirm", serviceRequest);
        }
	}
}