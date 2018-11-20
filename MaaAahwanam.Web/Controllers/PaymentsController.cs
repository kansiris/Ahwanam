using MaaAahwanam.Service;
using MaaAahwanam.Web.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaaAahwanam.Models;
using MaaAahwanam.Utility;
using System.IO;
using System.Text;

namespace MaaAahwanam.Web.Controllers
{
    public class PaymentsController : Controller
    {
        VendorMasterService vendorMasterService = new VendorMasterService();
        UserLoginDetailsService userLoginDetailsService = new UserLoginDetailsService();
        newmanageuser newmanageuse = new newmanageuser();
        OrderService orderService = new OrderService();
        OrderdetailsServices orderdetailService = new OrderdetailsServices();
        VendorDashBoardService mnguserservice = new VendorDashBoardService();
        ReceivePaymentService rcvpmntservice = new ReceivePaymentService();
        private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        decimal amount;
        // GET: Payments
        public ActionResult Index(string Oid)
        {
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                var user = (CustomPrincipal)System.Web.HttpContext.Current.User;

                string id = user.UserId.ToString();
                ViewBag.userid = id;
                string email = userLoginDetailsService.Getusername(long.Parse(id));
                ViewBag.vemail = email;
                Vendormaster Vendormaster = vendorMasterService.GetVendorByEmail(email);
                ViewBag.Vendor = vendorMasterService.GetVendor(Convert.ToInt64(Vendormaster.Id));
                if (Oid != null && Oid != "")
                {
                    var OrderDetailsbyOid = orderdetailService.GetOrderDetails(Oid).ToList();
                    ViewBag.OrderDetailsbyOid = OrderDetailsbyOid;
                    //var orderdetails = orderService.userOrderList().Where(m => m.OrderId == long.Parse(Oid)).ToList();
                    //if (orderdetails == null || orderdetails.Count == 0)
                    //{
                    //    var orderdetails1 = orderService.userOrderList1().Where(m => m.OrderId == long.Parse(Oid)).ToList();
                    var orderdetails1 = newmanageuse.allOrderList().Where(m => m.orderid == long.Parse(Oid)).ToList();
                    ViewBag.UserID = orderdetails1.FirstOrDefault().cutomerid;
                        ViewBag.username = orderdetails1.FirstOrDefault().fname + " " + orderdetails1.FirstOrDefault().lname;
                        ViewBag.vendorname = orderdetails1.FirstOrDefault().BusinessName;
                        ViewBag.vendoraddress = orderdetails1.FirstOrDefault().Address + "," + orderdetails1.FirstOrDefault().Landmark + "," + orderdetails1.FirstOrDefault().City;
                        ViewBag.vendorcontact = orderdetails1.FirstOrDefault().ContactNumber;
                        ViewBag.bookeddate = Convert.ToDateTime(orderdetails1.FirstOrDefault().bookdate).ToString("MMM d,yyyy");
                        ViewBag.orderdate = Convert.ToDateTime(orderdetails1.FirstOrDefault().orderdate).ToString("MMM d,yyyy");
                        ViewBag.orderdetails = orderdetails1;
                        ViewBag.receivedTrnsDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE).ToString("dd - MMM - yyyy");
                        ViewBag.totalprice = orderdetails1.FirstOrDefault().totalprice;
                        ViewBag.orderdetailid = orderdetails1.FirstOrDefault().orderdetailedid;
                        var payments = rcvpmntservice.getPayments(Oid).ToList();
                        ViewBag.payment = payments;
                        foreach (var reports in payments)
                        {
                           string  amount1 = reports.Received_Amount;

                            amount = Convert.ToInt64(amount) + Convert.ToInt64(amount1);

                        }
                        decimal paidamount;
                        if (amount == '0')
                        {
                             paidamount = orderdetails1.FirstOrDefault().totalprice;
                        }
                        else {  paidamount = orderdetails1.FirstOrDefault().totalprice - amount; }
                        ViewBag.paidamount = paidamount;
                    }
                //    else
                //    {
                //        ViewBag.username = orderdetails.FirstOrDefault().FirstName + " " + orderdetails.FirstOrDefault().LastName;
                //        ViewBag.vendorname = orderdetails.FirstOrDefault().BusinessName;
                //        ViewBag.vendoraddress = orderdetails.FirstOrDefault().Address + "," + orderdetails.FirstOrDefault().Landmark + "," + orderdetails.FirstOrDefault().City;
                //        ViewBag.vendorcontact = orderdetails.FirstOrDefault().ContactNumber;
                //        ViewBag.bookeddate = Convert.ToDateTime(orderdetails.FirstOrDefault().BookedDate).ToString("MMM d,yyyy");
                //        ViewBag.orderdate = Convert.ToDateTime(orderdetails.FirstOrDefault().OrderDate).ToString("MMM d,yyyy");
                //        ViewBag.orderdetails = orderdetails;
                //        ViewBag.totalprice = orderdetails.FirstOrDefault().TotalPrice;
                //        ViewBag.orderdetailid = orderdetails.FirstOrDefault().OrderDetailId;
                //        var payments = rcvpmntservice.getPayments(Oid).ToList();
                //        ViewBag.payment = payments;
                //        foreach (var reports in payments)
                //        {
                            
                //            string amount1 = reports.Received_Amount;

                //            amount = Convert.ToInt64(amount) + Convert.ToInt64(amount1);

                //        }
                //        decimal paidamount;
                //        if (amount == '0')
                //        {
                //            paidamount = orderdetails.FirstOrDefault().TotalPrice;
                //        }
                //        else { paidamount = orderdetails.FirstOrDefault().TotalPrice - amount; }
                //        ViewBag.paidamount = paidamount;

                //    }
                //    ViewBag.orderid = Oid;
                //}
            }
            return View();
        }
        [HttpPost]
        public ActionResult Index(Payment payments)
        {
            payments.User_Type = "VendorUser";
            payments.UpdatedDate =TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
            payments.Payment_Date =TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
            if(payments.Current_Balance =="0")
            {
                Order orders = new Order();
                OrderDetail orderdetils = new OrderDetail();
                orders.Status = "Payment completed";
                orderdetils.Status = "Payment completed";
                var status = orderService.updateOrderstatus(orders, orderdetils, Convert.ToInt64(payments.OrderId));
                payments.Status = "Payment completed";
            }
            else
            {
                Order orders = new Order();
                OrderDetail orderdetils = new OrderDetail();
                orders.Status = "Payment pending";
                orderdetils.Status = "Payment pending";
                var status = orderService.updateOrderstatus(orders, orderdetils, Convert.ToInt64(payments.OrderId));
               
                payments.Status = "Payment pending";
            }
            payments = rcvpmntservice.SavePayments(payments);
             //string msg = "Payment saved";
            return Json("Payment Successfull", JsonRequestBehavior.AllowGet);
            //return Content("<script language='javascript' type='text/javascript'>alert('" + msg + "');location.href='/ManageVendor'</script>"); 
        }

        public JsonResult Email(string Oid)
        {
            HomeController home = new HomeController();
           
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
                string id = user.UserId.ToString();
                ViewBag.userid = id;
                string email = userLoginDetailsService.Getusername(long.Parse(id));
                //ViewBag.vemail = email;
                Vendormaster Vendormaster = vendorMasterService.GetVendorByEmail(email);
                List<Payment> payment = rcvpmntservice.getPayments(Oid);
                //long userid = 0;
                string txtto = "";string name = "";
                //var orderdetails1 = orderService.userOrderList1().Where(m => m.OrderId == long.Parse(Oid)).ToList();
                //if (orderdetails1.Count == 0)
                //{
                //    var orderdetails = orderService.userOrderList().FirstOrDefault(m => m.OrderId == long.Parse(Oid));
                //    txtto = orderdetails.username;
                //    name = home.Capitalise(orderdetails.FirstName + " " + orderdetails.LastName);
                //}
                //else
                //{
                //    txtto = orderdetails1.FirstOrDefault().username;
                //    name = home.Capitalise(orderdetails1.FirstOrDefault().firstname + " " + orderdetails1.FirstOrDefault().lastname);
                //}
                var orderdetails1 = newmanageuse.allOrderList().Where(m => m.orderid == long.Parse(Oid)).ToList();
                StringBuilder cds = new StringBuilder();
                cds.Append("<table style='border:1px black solid;'><tbody>");
                cds.Append("<tr><td>Order Id</td><td>Order Date</td><td> Event Type </td><td>Guest Count</td><td>Perunit Price</td><td>Total Price</td></tr>");
                cds.Append("<tr><td style = 'width: 75px;border: 1px black solid;'> " + orderdetails1.FirstOrDefault().orderid + "</td><td style = 'width: 75px;border: 1px black solid;' > " + orderdetails1.FirstOrDefault().bookdate + " </td><td style = 'width: 75px;border: 1px black solid;'> " + orderdetails1.FirstOrDefault().eventtype + " </td><td style = 'width: 50px;border: 1px black solid;'> " + orderdetails1.FirstOrDefault().guestno + " </td> <td style = 'width: 50px;border: 1px black solid;'> " + orderdetails1.FirstOrDefault().perunitprice + " </td><td style = 'width: 50px;border: 1px black solid;'> " + orderdetails1.FirstOrDefault().totalprice + " </td></tr>");  //<td style = 'width: 50px;border: 2px black solid;'> " + item.eventstartdate + " </td><td> date </td>
                cds.Append("</tbody></table>");
                if (payment.Count != 0)
                {
                    cds.Append("<table style='border:1px black solid;'><tbody>");
                    cds.Append("<tr><td> Payment Id</td><td> Payment Type </td><td> Payment Date </td><td> Received Amount </td></tr>");
                    foreach (var item in payment)
                    {
                        cds.Append("<tr><td style = 'width: 75px;border: 1px black solid;'> " + item.Payment_Id + "</td><td style = 'width: 75px;border: 1px black solid;' > " + item.Payment_Type + " </td><td style = 'width: 75px;border: 1px black solid;'> " + item.Payment_Date + " </td><td style = 'width: 50px;border: 1px black solid;'> " + item.Received_Amount + " </td></tr>");  //<td style = 'width: 50px;border: 2px black solid;'> " + item.eventstartdate + " </td><td> date </td>
                    }
                    cds.Append("</tbody></table>");
                }
                string url = Request.Url.Scheme + "://" + Request.Url.Authority;
                //string url = cds.ToString();
                FileInfo File = new FileInfo(Server.MapPath("/mailtemplate/order.html"));
                string readFile = File.OpenText().ReadToEnd();
                readFile = readFile.Replace("[ActivationLink]", url);
                readFile = readFile.Replace("[name]", name);
                readFile = readFile.Replace("[orderid]", Oid);
                readFile = readFile.Replace("[table]", cds.ToString());
                string txtmessage = readFile;//readFile + body;
                string subj = "Thanks for your order";
                EmailSendingUtility emailSendingUtility = new EmailSendingUtility();
                emailSendingUtility.Email_maaaahwanam(txtto, txtmessage, subj);
                string targetmails = "lakshmi.p@xsilica.com,seema.g@xsilica.com,rameshsai@xsilica.com";
                emailSendingUtility.Email_maaaahwanam(targetmails, txtmessage, subj);
            }

              return Json("Email sent Successfully", JsonRequestBehavior.AllowGet);

            }
        }
}