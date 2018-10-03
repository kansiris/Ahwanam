using MaaAahwanam.Service;
using MaaAahwanam.Web.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaaAahwanam.Models;

namespace MaaAahwanam.Web.Controllers
{
    public class PaymentsController : Controller
    {
        VendorMasterService vendorMasterService = new VendorMasterService();
        UserLoginDetailsService userLoginDetailsService = new UserLoginDetailsService();
        OrderService orderService = new OrderService();
        OrderdetailsServices orderdetailService = new OrderdetailsServices();
        ReceivePaymentService rcvpmntservice = new ReceivePaymentService();
        // GET: Payments
        public ActionResult Index(string Oid)
        {
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
                string id = user.UserId.ToString();
                string email = userLoginDetailsService.Getusername(long.Parse(id));
                Vendormaster Vendormaster = vendorMasterService.GetVendorByEmail(email);
                ViewBag.Vendor = vendorMasterService.GetVendor(Convert.ToInt64(Vendormaster.Id));
                if (Oid != null && Oid != "")
                {
                    var orderdetails = orderService.userOrderList().Where(m => m.OrderId == long.Parse(Oid)).ToList();
                    if (orderdetails == null || orderdetails.Count == 0)
                    {
                        var orderdetails1 = orderService.userOrderList1().Where(m => m.OrderId == long.Parse(Oid)).ToList();
                        ViewBag.username = orderdetails1.FirstOrDefault().firstname + " " + orderdetails1.FirstOrDefault().lastname;
                        ViewBag.vendorname = orderdetails1.FirstOrDefault().BusinessName;
                        ViewBag.vendoraddress = orderdetails1.FirstOrDefault().Address + "," + orderdetails1.FirstOrDefault().Landmark + "," + orderdetails1.FirstOrDefault().City;
                        ViewBag.vendorcontact = orderdetails1.FirstOrDefault().ContactNumber;
                        ViewBag.bookeddate = Convert.ToDateTime(orderdetails1.FirstOrDefault().BookedDate).ToString("MMM d,yyyy");
                        ViewBag.orderdate = Convert.ToDateTime(orderdetails1.FirstOrDefault().OrderDate).ToString("MMM d,yyyy");
                        ViewBag.orderdetails = orderdetails1;
                        ViewBag.receivedTrnsDate = DateTime.Now;
                        ViewBag.totalprice = orderdetails1.FirstOrDefault().TotalPrice;
                    }
                    else
                    {
                        ViewBag.username = orderdetails.FirstOrDefault().FirstName + " " + orderdetails.FirstOrDefault().LastName;
                        ViewBag.vendorname = orderdetails.FirstOrDefault().BusinessName;
                        ViewBag.vendoraddress = orderdetails.FirstOrDefault().Address + "," + orderdetails.FirstOrDefault().Landmark + "," + orderdetails.FirstOrDefault().City;
                        ViewBag.vendorcontact = orderdetails.FirstOrDefault().ContactNumber;
                        ViewBag.bookeddate = Convert.ToDateTime(orderdetails.FirstOrDefault().BookedDate).ToString("MMM d,yyyy");
                        ViewBag.orderdate = Convert.ToDateTime(orderdetails.FirstOrDefault().OrderDate).ToString("MMM d,yyyy");
                        ViewBag.orderdetails = orderdetails;
                        ViewBag.totalprice = orderdetails.FirstOrDefault().TotalPrice;
                        

                    }
                    ViewBag.orderid = Oid;
                }
            }
            return View();
        }
        [HttpPost]
        public ActionResult Index(Payment payments)
        {
                payments.Payment_Date = DateTime.Now;
                payments = rcvpmntservice.SavePayments(payments);
                return View();
        }
    }
}