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
    public class BiddingController : Controller
    {
        //
        // GET: /Bidding/
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [Authorize]
        public ActionResult Index(ServiceRequest serviceRequest)
        {
            var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
            ServiceRequestService serviceRequestService = new ServiceRequestService();
            serviceRequest.Type = "Bidding";
            serviceRequest.UpdatedTime =DateTime.Now;
            serviceRequest.Status = "Due";
            serviceRequest.UpdatedBy =user.UserId;
            serviceRequest =serviceRequestService.SaveService(serviceRequest);
            return RedirectToAction("Index", "BiddingConformation", serviceRequest);
        }
	}
}