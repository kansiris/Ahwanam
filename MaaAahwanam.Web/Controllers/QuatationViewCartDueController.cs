using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaaAahwanam.Service;

namespace MaaAahwanam.Web.Controllers
{
    public class QuatationViewCartDueController : Controller
    {
        DashBoardService dashBoardService = new DashBoardService();
        PaymentRequestService paymentRequestService = new PaymentRequestService();
        public ActionResult Index(string id)
        {
            if (id!=null)
            {
                ViewBag.OrderDetail = dashBoardService.GetParticularService(int.Parse(id));
                var list = paymentRequestService.GetServiceResponse(long.Parse(id)).Select(m=>m.ResponseId);
                ViewBag.payment = paymentRequestService.GetPaymentRequest(long.Parse(id));
                ViewBag.date = dashBoardService.GetParticularDate(long.Parse(id));
                ViewBag.servicetype = dashBoardService.GetServiceType(long.Parse(id));
            }
            return View();
        }
	}
}