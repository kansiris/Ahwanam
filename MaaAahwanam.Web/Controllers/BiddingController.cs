using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaaAahwanam.Models;
using MaaAahwanam.Service;

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
        public ActionResult Index(ServiceRequest serviceRequest)
        {
            ServiceRequestService serviceRequestService = new ServiceRequestService();
            serviceRequest.Type = "Bidding";
            serviceRequest.UpdatedTime =DateTime.Now;
            serviceRequest.Status = "Due";
            serviceRequest =serviceRequestService.SaveService(serviceRequest);
            return RedirectToAction("Index", "BiddingConformation", serviceRequest);
        }
	}
}