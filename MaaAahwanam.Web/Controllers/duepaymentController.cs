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
                    ViewBag.paymentbycustmname = paymentbycustomer.FirstOrDefault().PaymentBy.Replace("_", " ");
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
        public ActionResult Email(string oid)
        {
            HomeController home = new HomeController();

            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
                string vid = user.UserId.ToString();
                ViewBag.userid = vid;
                string email = newmanageuse.Getusername(long.Parse(vid));
                ViewBag.vemail = email;
                Vendormaster Vendormaster = vendorMasterService.GetVendorByEmail(email);
                List<Payment> payment = rcvpaymentservice.getPayments(oid);               
                var orderdetails1 = newmanageuse.allOrderList().Where(m => m.orderid == long.Parse(oid)).ToList();
                string txtto = orderdetails1.FirstOrDefault().username;
                string name = orderdetails1.FirstOrDefault().fname+" "+orderdetails1.FirstOrDefault().lname;
                StringBuilder cds = new StringBuilder();
                cds.Append("<table style='border:1px black solid;'><tbody>");
                cds.Append("<tr><td>Order Id</td><td>Order Date</td><td> Event Type </td><td>Guest Count</td><td>Perunit Price</td><td>Total Price</td></tr>");
                cds.Append("<tr><td style = 'width: 75px;border: 1px black solid;'> " + orderdetails1.FirstOrDefault().orderid + "</td><td style = 'width: 75px;border: 1px black solid;' > " + orderdetails1.FirstOrDefault().bookdate + " </td><td style = 'width: 75px;border: 1px black solid;'> " + orderdetails1.FirstOrDefault().eventtype + " </td><td style = 'width: 50px;border: 1px black solid;'> " + orderdetails1.FirstOrDefault().guestno + " </td> <td style = 'width: 50px;border: 1px black solid;'> " + orderdetails1.FirstOrDefault().perunitprice + " </td><td style = 'width: 50px;border: 1px black solid;'> " + orderdetails1.FirstOrDefault().totalprice + " </td></tr>");  //<td style = 'width: 50px;border: 2px black solid;'> " + item.eventstartdate + " </td><td> date </td>
                cds.Append("</tbody></table>");
                if (payment.Count != 0)
                {
                    cds.Append("<table style='border:1px black solid;'><tbody>");
                    cds.Append("<tr><td> Payment Id</td><td> Payment By</td><td> Payment Type </td><td> Payment Date </td><td> Received Amount </td></tr>");
                    foreach (var item in payment)
                    {
                        cds.Append("<tr><td style = 'width: 75px;border: 1px black solid;'> " + item.Payment_Id + "</td><td style = 'width: 75px;border: 1px black solid;'> " + item.PaymentBy + "</td><td style = 'width: 75px;border: 1px black solid;' > " + item.Payment_Type + " </td><td style = 'width: 75px;border: 1px black solid;'> " + item.Payment_Date + " </td><td style = 'width: 50px;border: 1px black solid;'> " + item.Received_Amount + " </td></tr>");  //<td style = 'width: 50px;border: 2px black solid;'> " + item.eventstartdate + " </td><td> date </td>
                    }
                    cds.Append("</tbody></table>");
                }
                string url = Request.Url.Scheme + "://" + Request.Url.Authority;
                //string url = cds.ToString();
                FileInfo File = new FileInfo(Server.MapPath("/mailtemplate/order.html"));
                string readFile = File.OpenText().ReadToEnd();
                readFile = readFile.Replace("[ActivationLink]", url);
                readFile = readFile.Replace("[name]", name);
                readFile = readFile.Replace("[table]", cds.ToString());
                readFile = readFile.Replace("[orderid]", oid);
                string txtmessage = readFile;//readFile + body;
                string subj = "Thanks for your order";
                EmailSendingUtility emailSendingUtility = new EmailSendingUtility();
                emailSendingUtility.Email_maaaahwanam(txtto, txtmessage, subj, null);
                string targetmails = "lakshmi.p@xsilica.com,seema.g@xsilica.com,rameshsai@xsilica.com";
                emailSendingUtility.Email_maaaahwanam(targetmails, txtmessage, subj, null);
            }

            return Json("success", JsonRequestBehavior.AllowGet);
        }
    }
}