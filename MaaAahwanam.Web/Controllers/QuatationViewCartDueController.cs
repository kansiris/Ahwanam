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
                ViewBag.OrderDetail = dashBoardService.GetParticularService(long.Parse(id));
                ViewBag.payment = paymentRequestService.GetPaymentRequest(long.Parse(id));
            }
            return View();
        }
	}
}