using MaaAahwanam.Models;
using MaaAahwanam.Service;
using MaaAahwanam.Utility;
using MaaAahwanam.Web.Custom;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace MaaAahwanam.Web.Controllers
{
    public class vinvoiceController : Controller
    {
        VendorMasterService vendorMasterService = new VendorMasterService();
        UserLoginDetailsService userLoginDetailsService = new UserLoginDetailsService();
        OrderService orderService = new OrderService();
        OrderdetailsServices orderdetailService = new OrderdetailsServices();
        ReceivePaymentService rcvpaymentservice = new ReceivePaymentService();
        VendorDashBoardService mnguserservice = new VendorDashBoardService();
        private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        decimal amount;
        // GET: vinvoice
        public ActionResult Index(string oid)
        {
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
                string id = user.UserId.ToString();
                string email = userLoginDetailsService.Getusername(long.Parse(id));
                Vendormaster Vendormaster = vendorMasterService.GetVendorByEmail(email);
                ViewBag.Vendor = vendorMasterService.GetVendor(Convert.ToInt64(Vendormaster.Id));
                if (oid != null && oid != "")
                {
                    var orderdetails = orderService.userOrderList().Where(m=>m.OrderId == long.Parse(oid)).ToList();
                    if (orderdetails == null || orderdetails.Count == 0)
                    {
                      var  orderdetails1 = orderService.userOrderList1().Where(m => m.OrderId == long.Parse(oid)).ToList();
                        ViewBag.username = orderdetails1.FirstOrDefault().firstname + " " + orderdetails1.FirstOrDefault().lastname;
                        ViewBag.vendorname = orderdetails1.FirstOrDefault().BusinessName;
                        ViewBag.vendoraddress = orderdetails1.FirstOrDefault().Address + "," + orderdetails1.FirstOrDefault().Landmark + "," + orderdetails1.FirstOrDefault().City;
                        ViewBag.vendorcontact = orderdetails1.FirstOrDefault().ContactNumber;
                        ViewBag.bookeddate = Convert.ToDateTime(orderdetails1.FirstOrDefault().BookedDate).ToString("MMM d,yyyy");
                        ViewBag.orderdate = Convert.ToDateTime(orderdetails1.FirstOrDefault().OrderDate).ToString("MMM d,yyyy");
                        ViewBag.orderdetails = orderdetails1;
                        ViewBag.totalprice = orderdetails1.FirstOrDefault().TotalPrice;
                        var payments = rcvpaymentservice.getPayments(oid).ToList();
                        ViewBag.payment = payments;
                        foreach (var reports in payments)
                        {
                            string amount1 = reports.Received_Amount;

                            amount = Convert.ToInt64(amount) + Convert.ToInt64(amount1);

                        }
                        decimal paidamount;
                        if (amount == '0')
                        {
                            paidamount = orderdetails1.FirstOrDefault().TotalPrice;
                        }
                        else { paidamount = orderdetails1.FirstOrDefault().TotalPrice - amount; }
                        ViewBag.paidamount = paidamount;
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
                    //ViewBag.paymentlist = rcvpaymentservice.Getpmntdetails(oid);
                    ViewBag.orderid = oid;
                    
                }
            }
            return View();
        }
        [HttpPost]
        public ActionResult Index(Payment payments)
        {
            payments.User_Type = "VendorUser";
            payments.UpdatedDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
            payments.Payment_Date = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
            payments = rcvpaymentservice.SavePayments(payments);

            //return Json("Payment Successfull", JsonRequestBehavior.AllowGet);
            return Content("<script language='javascript' type='text/javascript'>alert('payment Successfull');location.href='/vinvoice'</script>");

        }

        public ActionResult Email(string oid)
        {
            HomeController home = new HomeController();
          
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
                string vid = user.UserId.ToString();
                ViewBag.userid = vid;
                string email = userLoginDetailsService.Getusername(long.Parse(vid));
                ViewBag.vemail = email;
                Vendormaster Vendormaster = vendorMasterService.GetVendorByEmail(email);
                List<Payment> payment = rcvpaymentservice.getPayments(oid);
                var userlogdetails = mnguserservice.getuserbyid(Convert.ToInt32(vid));
                string txtto = userlogdetails.email;

                string name = userlogdetails.firstname;
                name = home.Capitalise(name);
                //string OrderId = Convert.ToString(order.OrderId);
                StringBuilder cds = new StringBuilder();
                cds.Append("<table style='border:1px black solid;'><tbody>");
                cds.Append("<tr><td> Payment Id</td><td> Payment Type </td><td> Payment Date </td><td> Received Amount </td></tr>");
                foreach (var item in payment)
                {
                    cds.Append("<tr><td style = 'width: 75px;border: 1px black solid;'> " + item.Payment_Id + "</td><td style = 'width: 75px;border: 1px black solid;' > " + item.Payment_Type + " </td><td style = 'width: 75px;border: 1px black solid;'> " + item.Payment_Date + " </td><td style = 'width: 50px;border: 1px black solid;'> " + item.Received_Amount + " </td></tr>");  //<td style = 'width: 50px;border: 2px black solid;'> " + item.eventstartdate + " </td><td> date </td>
                }
                cds.Append("</tbody></table>");
                //string url = Request.Url.Scheme + "://" + Request.Url.Authority;
                string url = cds.ToString();
                FileInfo File = new FileInfo(Server.MapPath("/mailtemplate/order.html"));
                string readFile = File.OpenText().ReadToEnd();
                readFile = readFile.Replace("[ActivationLink]", url);
                readFile = readFile.Replace("[name]", name);
                //readFile = readFile.Replace("[orderid]", OrderId);
                string txtmessage = readFile;//readFile + body;
                string subj = "Thanks for your order";
                EmailSendingUtility emailSendingUtility = new EmailSendingUtility();
                emailSendingUtility.Email_maaaahwanam(txtto, txtmessage, subj);
                emailSendingUtility.Email_maaaahwanam("seema@xsilica.com ", txtmessage, subj);
                
            }

            return Json("success", JsonRequestBehavior.AllowGet);

        }
    }
}