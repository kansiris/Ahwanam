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
            //string sdate = String.Format("{0:MM-dd-yyyy}", serviceRequest.EventEnddate);
            //serviceRequest.EventEnddate = DateTime.ParseExact(sdate, "MM-dd-yyyy", new CultureInfo("en-US"), DateTimeStyles.None);
            var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
            ServiceRequestService serviceRequestService = new ServiceRequestService();
            serviceRequest.Type = "Quotation";
            //serviceRequest.UpdatedTime = DateTime.Parse(String.Format("{0:MM-dd-yyyy}", DateTime.Now));
            serviceRequest.UpdatedTime = DateTime.Now;
            serviceRequest.UpdatedBy = (int)user.UserId;
            serviceRequest.Status = "Due";
            serviceRequest.ServiceType.TrimStart(',');
            serviceRequest = serviceRequestService.SaveService(serviceRequest);
            return RedirectToAction("Index", "QuoatationConfirm", serviceRequest);
        }
    }
}