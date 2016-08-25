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
    public class QuoatationSubmitController : Controller
    {
        //
        // GET: /QuoatationSubmit/
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(ServiceRequest serviceRequest)
        {
            var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
            ServiceRequestService serviceRequestService = new ServiceRequestService();
            serviceRequest.Type = "Quotation";
            serviceRequest.UpdatedTime = DateTime.Now;
            serviceRequest.UpdatedBy = (int)user.UserId;
            serviceRequest.Status = "Due";
            serviceRequest =serviceRequestService.SaveService(serviceRequest);
            return RedirectToAction("Index","QuoatationConfirm", serviceRequest);
        }
	}
}