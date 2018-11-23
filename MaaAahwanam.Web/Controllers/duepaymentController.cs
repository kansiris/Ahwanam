using MaaAahwanam.Models;
using MaaAahwanam.Service;
using MaaAahwanam.Web.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MaaAahwanam.Web.Controllers
{
    public class duepaymentController : Controller
    {
        VendorVenueService vendorVenueService = new VendorVenueService();
        newmanageuser newmanageuse = new newmanageuser();
        ReceivePaymentService rcvpaymentservice = new ReceivePaymentService();
        Vendormaster vendorMaster = new Vendormaster();
        VendorMasterService vendorMasterService = new VendorMasterService();
        PartnerService partnerservice = new PartnerService();
        private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        // GET: duepayment
        public ActionResult Index(string oid,string paymentby)
        {
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
                string uid = user.UserId.ToString();
                string vemail = newmanageuse.Getusername(long.Parse(uid));
                vendorMaster = newmanageuse.GetVendorByEmail(vemail);
                string VendorId = vendorMaster.Id.ToString();
                ViewBag.masterid = VendorId;
                ViewBag.Vendor = vendorMasterService.GetVendor(Convert.ToInt64(VendorId));
                if (oid != null && oid != "")
                {

                    var orderdetails1 = newmanageuse.allOrderList().Where(m => m.orderid == long.Parse(oid)).ToList();
                    ViewBag.orderid = orderdetails1.FirstOrDefault().orderid;
                    ViewBag.username = orderdetails1.FirstOrDefault().fname + " " + orderdetails1.FirstOrDefault().lname;
                    ViewBag.vendorname = orderdetails1.FirstOrDefault().BusinessName;
                    ViewBag.vendoraddress = orderdetails1.FirstOrDefault().Address + "," + orderdetails1.FirstOrDefault().Landmark + "," + orderdetails1.FirstOrDefault().City;
                    ViewBag.vendorcontact = orderdetails1.FirstOrDefault().ContactNumber;
                    ViewBag.bookeddate = Convert.ToDateTime(orderdetails1.FirstOrDefault().bookdate).ToString("MMM d,yyyy");
                    ViewBag.orderdate = Convert.ToDateTime(orderdetails1.FirstOrDefault().orderdate).ToString("MMM d,yyyy");
                    ViewBag.orderdetailslst = orderdetails1;
                    var payments = rcvpaymentservice.getPayments(oid).ToList();
                    ViewBag.paymentslst = payments;

                    var paymentbycustomer = rcvpaymentservice.Getpaymentby(oid, paymentby).ToList();
                    ViewBag.paymentbycustmlst = paymentbycustomer;
                    ViewBag.paymentbycustmname = paymentbycustomer.FirstOrDefault().PaymentBy;
                    string odid = string.Empty;
                    foreach (var item in orderdetails1)
                    {
                        var orderdetailid = item.orderdetailedid.ToString();
                        odid = odid + item.orderdetailedid + ",";
                        ViewBag.orderdetailid5 = odid;
                        var paymentsbyodid = rcvpaymentservice.getPaymentsbyodid(orderdetailid).ToList();
                       
                    }
                }
            }
                return View();
        }
    }
}