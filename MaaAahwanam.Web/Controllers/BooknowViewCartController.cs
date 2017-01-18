using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaaAahwanam.Service;
using MaaAahwanam.Web.Custom;

namespace MaaAahwanam.Web.Controllers
{
    public class BooknowViewCartController : Controller
    {
        DashBoardService dashBoardService = new DashBoardService();
        Payment_orderServices payment_orderServices = new Payment_orderServices();
        public ActionResult Index(string id)
        {
            var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
            //ViewBag.Type = ValidUserUtility.UserType();
            ViewBag.Type = user.UserType;
            if (id != null)
            {
                var data  = dashBoardService.GetOrderDetailService(long.Parse(id));
                ViewBag.OrderDetail = data;
                ViewBag.date = data[0].UpdatedDate;
                decimal totalamount = 0;
                foreach (var item in data)
                {
                    if (item.Isdeal == true)
                    {
                        totalamount = item.TotalPrice;
                    }
                    else
                    {
                        totalamount = item.PerunitPrice;
                    }
                    
                }
                ViewBag.subtotal = totalamount;
                ViewBag.payment = payment_orderServices.GetPaymentOrderService(long.Parse(id));
            }
            return View();
        }
	}
}