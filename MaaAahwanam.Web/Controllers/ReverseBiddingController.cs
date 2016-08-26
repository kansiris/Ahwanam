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
    public class ReverseBiddingController : Controller
    {
        //
        // GET: /ReverseBidding/
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(ServiceRequest serviceRequest)
        {
            var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
            ServiceRequestService serviceRequestService = new ServiceRequestService();
            serviceRequest.Type = "ReverseBidding";
            serviceRequest.UpdatedTime = DateTime.Now;
            serviceRequest.Status = "Due";
            serviceRequest.UpdatedBy = user.UserId;
            serviceRequest =serviceRequestService.SaveService(serviceRequest);
            return RedirectToAction("Index", "BiddingConformation",serviceRequest);
        }

        public JsonResult Vendorlist(string selectedservice)
        {
            ServiceRequestService serviceRequestService = new ServiceRequestService();
            List<Vendormaster> vlist = serviceRequestService.getvendorslistRB(selectedservice);
            return Json(vlist);
        }
	}
}