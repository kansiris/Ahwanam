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
using IronPdf;
using System.Text.RegularExpressions;

namespace MaaAahwanam.Web.Controllers
{
    public class vinvoiceController : Controller
    {
        decimal ksra1;
        newmanageuser newmanageuse = new newmanageuser();

        VendorMasterService vendorMasterService = new VendorMasterService();
        OrderdetailsServices orderdetailservices = new OrderdetailsServices();


        ReceivePaymentService rcvpaymentservice = new ReceivePaymentService();
        VendorDashBoardService mnguserservice = new VendorDashBoardService();
        private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        decimal amount;
        decimal tsprice;
        decimal balndue;
        // GET: vinvoice
        public ActionResult Index(string oid)
        {
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
                string id = user.UserId.ToString();
                ViewBag.userid = id;
                string email = newmanageuse.Getusername(long.Parse(id));
                Vendormaster Vendormaster = vendorMasterService.GetVendorByEmail(email);
                ViewBag.Vendor = vendorMasterService.GetVendor(Convert.ToInt64(Vendormaster.Id));
                if (oid != null && oid != "")
                {
                    var orderdetails = newmanageuse.userOrderList().Where(m=>m.OrderId == long.Parse(oid)).ToList();
                    if (orderdetails == null || orderdetails.Count == 0)
                    {
                      var  orderdetails1 = newmanageuse.userOrderList1().Where(m => m.OrderId == long.Parse(oid)).ToList();
                        ViewBag.username = orderdetails1.FirstOrDefault().firstname + " " + orderdetails1.FirstOrDefault().lastname;
                        ViewBag.vendorname = orderdetails1.FirstOrDefault().BusinessName;
                        ViewBag.vendoraddress = orderdetails1.FirstOrDefault().Address + "," + orderdetails1.FirstOrDefault().Landmark + "," + orderdetails1.FirstOrDefault().City;
                        ViewBag.vendorcontact = orderdetails1.FirstOrDefault().ContactNumber;
                        ViewBag.bookeddate = Convert.ToDateTime(orderdetails1.FirstOrDefault().BookedDate).ToString("MMM d,yyyy");
                        ViewBag.orderdate = Convert.ToDateTime(orderdetails1.FirstOrDefault().OrderDate).ToString("MMM d,yyyy");
                        ViewBag.Servicetype = orderdetails1.FirstOrDefault().ServiceType;
                        ViewBag.serviceprice = orderdetails1.FirstOrDefault().PerunitPrice * orderdetails1.FirstOrDefault().Quantity;
                        ViewBag.orderdetailid = orderdetails1.FirstOrDefault().OrderDetailId;
                        ViewBag.orderdetails = orderdetails1;
                        string odid = string.Empty;
                        foreach (var item in orderdetails1)
                        {
                            odid = odid + item.OrderDetailId + ",";
                            var price = item.TotalPrice;
                            tsprice = Convert.ToInt64(tsprice) + Convert.ToInt64(price);
                            ViewBag.total = tsprice;
                            var bdue = item.Due;
                            balndue = Convert.ToInt64(balndue) + Convert.ToInt64(bdue);
                            ViewBag.balance = balndue;

                        }
                        ViewBag.totalprice = orderdetails1.FirstOrDefault().TotalPrice;
                        ViewBag.orderdetailid = orderdetails1.FirstOrDefault().OrderDetailId;
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
                            //paidamount = orderdetails1.FirstOrDefault().PerunitPrice * orderdetails1.FirstOrDefault().Quantity;

                        }
                        else
                        {
                            paidamount = orderdetails1.FirstOrDefault().TotalPrice - amount; //paidamount = (orderdetails1.FirstOrDefault().PerunitPrice * orderdetails1.FirstOrDefault().Quantity) - amount; 
                            ViewBag.paidamount = paidamount;
                        }
                    }
                    else
                    {
                        ViewBag.username = orderdetails.FirstOrDefault().FirstName + " " + orderdetails.FirstOrDefault().LastName;
                        ViewBag.vendorname = orderdetails.FirstOrDefault().BusinessName;
                        ViewBag.vendoraddress = orderdetails.FirstOrDefault().Address + "," + orderdetails.FirstOrDefault().Landmark + "," + orderdetails.FirstOrDefault().City;
                        ViewBag.vendorcontact = orderdetails.FirstOrDefault().ContactNumber;
                        ViewBag.bookeddate = Convert.ToDateTime(orderdetails.FirstOrDefault().BookedDate).ToString("MMM d,yyyy");
                        ViewBag.orderdate = Convert.ToDateTime(orderdetails.FirstOrDefault().OrderDate).ToString("MMM d,yyyy");
                        ViewBag.Servicetype = orderdetails.FirstOrDefault().ServiceType;
                        ViewBag.serviceprice = orderdetails.FirstOrDefault().PerunitPrice * orderdetails.FirstOrDefault().Quantity;
                        ViewBag.orderdetails = orderdetails;
                        ViewBag.totalprice = orderdetails.FirstOrDefault().TotalPrice;
                        ViewBag.orderdetailid = orderdetails.FirstOrDefault().OrderDetailId;
                        var payments = rcvpaymentservice.getPayments(oid).ToList();
                        foreach (var i in orderdetails)
                        {
                            var price = i.TotalPrice;
                            tsprice = Convert.ToInt64(tsprice) + Convert.ToInt64(price);
                            ViewBag.total = tsprice;
                            var bdue = i.Due;
                            balndue = Convert.ToInt64(balndue) + Convert.ToInt64(bdue);
                            ViewBag.balance = balndue;

                        }
                        ViewBag.payment = payments;
                        foreach (var reports in payments)
                        {
                            string amount1 = reports.Received_Amount;

                            amount = Convert.ToInt64(amount) + Convert.ToInt64(amount1);

                        }
                        decimal paidamount;
                        if (amount == '0')
                        {
                            paidamount = orderdetails.FirstOrDefault().TotalPrice;
                            //paidamount = orderdetails.FirstOrDefault().PerunitPrice * orderdetails.FirstOrDefault().Quantity;
                        }
                        else
                        {
                            paidamount = orderdetails.FirstOrDefault().TotalPrice - amount; paidamount = (orderdetails.FirstOrDefault().PerunitPrice * orderdetails.FirstOrDefault().Quantity) - amount; }
                        ViewBag.paidamount = paidamount;
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
            
            var orderdetails = newmanageuse.userOrderList().Where(m => m.OrderId == long.Parse(payments.OrderId)).ToList();
            if (orderdetails == null || orderdetails.Count == 0)
            {
                var orderdetails1 = newmanageuse.userOrderList1().Where(m => m.OrderId == long.Parse(payments.OrderId)).ToList();

                payments.User_Type = "VendorUser";
                payments.UpdatedDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
                payments.Payment_Date = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
                
                var odis = payments.OrderDetailId.Trim(',').Split(',');
                for (int i = 0; i < odis.Length; i++)
                {
                    decimal ksra = decimal.Parse(payments.Received_Amount);
                    //if (ksra >= 0)
                    //{
                        string str = odis[i] ;
                        str = Regex.Replace(str, @"\s", "");
                        var orderdetailid = str;
                        payments.OrderDetailId = orderdetailid;
                        //var datarecord = orderdetailservices.GetOrderDetailsByOrderdetailid(Convert.ToInt32(orderdetailid));
                        decimal dueamount;
                        var ksorder = orderdetails1.Where(m => m.OrderDetailId == long.Parse(orderdetailid)).FirstOrDefault();
                        if (ksorder.SUM_AP == null || ksorder.Due == null)
                        {
                        dueamount = ksorder.TotalPrice;
                        decimal amnt;
                        if (i == 0)
                        {  amnt = ksra;  }
                        else {  amnt = ksra1; }
                            if (dueamount > amnt)
                            {
                                ksra1 = dueamount - amnt;                               
                                payments.Opening_Balance = dueamount.ToString().Replace(".00", "");
                                 if(amnt == dueamount){ payments.Received_Amount = dueamount.ToString().Replace(".00", "");}
                                 else  { payments.Received_Amount = amnt.ToString().Replace(".00", "");}
                                payments.Current_Balance = ksra1.ToString().Replace(".00", "");
                            }
                            else
                            {
                            ksra1 = amnt - dueamount;
                                payments.Received_Amount = dueamount.ToString().Replace(".00", "");
                                payments.Opening_Balance = dueamount.ToString().Replace(".00", "");
                                if (ksra1 > 0)
                                {
                                    payments.Current_Balance = "0";
                                }

                            }
                        }
                       
                        else {
                            dueamount = (decimal)ksorder.Due;
                        }
                    if (payments.Current_Balance == "0")
                    {
                        Order orders = new Order();
                        OrderDetail orderdetils = new OrderDetail();
                        orders.Status = "Payment completed";
                        orderdetils.Status = "Payment completed";
                        var status = newmanageuse.updateOrderstatus(orders, orderdetils, Convert.ToInt64(payments.OrderId));
                        payments.Status = "Payment completed";
                    }
                    else
                    {
                        Order orders = new Order();
                        OrderDetail orderdetils = new OrderDetail();
                        orders.Status = "Payment pending";
                        orderdetils.Status = "Payment pending";
                        var status = newmanageuse.updateOrderstatus(orders, orderdetils, Convert.ToInt64(payments.OrderId));

                        payments.Status = "Payment pending";
                    }
                    payments = rcvpaymentservice.SavePayments(payments);
                }
            }
            return Json("Payment Successfull", JsonRequestBehavior.AllowGet);
            //return Content("<script language='javascript' type='text/javascript'>alert('payment Successfull');location.href='/vinvoice'</script>");

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
                string txtto = ""; string name = "";
                var orderdetails1 = newmanageuse.userOrderList1().Where(m => m.OrderId == long.Parse(oid)).ToList();
                if (orderdetails1.Count == 0)
                {
                    var orderdetails = newmanageuse.userOrderList().FirstOrDefault(m => m.OrderId == long.Parse(oid));
                    txtto = orderdetails.username;
                    name = home.Capitalise(orderdetails.FirstName + " " + orderdetails.LastName);
                }
                else
                {
                    txtto = orderdetails1.FirstOrDefault().username;
                    name = home.Capitalise(orderdetails1.FirstOrDefault().firstname + " " + orderdetails1.FirstOrDefault().lastname);
                }
                StringBuilder cds = new StringBuilder();
                cds.Append("<table style='border:1px black solid;'><tbody>");
                cds.Append("<tr><td>Order Id</td><td>Order Date</td><td> Event Type </td><td> Quantity</td><td>Perunit Price</td><td>Total Price</td></tr>");
                cds.Append("<tr><td style = 'width: 75px;border: 1px black solid;'> " + orderdetails1.FirstOrDefault().OrderId + "</td><td style = 'width: 75px;border: 1px black solid;' > " + orderdetails1.FirstOrDefault().BookedDate + " </td><td style = 'width: 75px;border: 1px black solid;'> " + orderdetails1.FirstOrDefault().EventType + " </td><td style = 'width: 50px;border: 1px black solid;'> " + orderdetails1.FirstOrDefault().Quantity + " </td> <td style = 'width: 50px;border: 1px black solid;'> " + orderdetails1.FirstOrDefault().PerunitPrice + " </td><td style = 'width: 50px;border: 1px black solid;'> " + orderdetails1.FirstOrDefault().TotalPrice + " </td></tr>");  //<td style = 'width: 50px;border: 2px black solid;'> " + item.eventstartdate + " </td><td> date </td>
                cds.Append("</tbody></table>");
                if (payment.Count != 0) { 
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
                readFile = readFile.Replace("[table]", cds.ToString());
                readFile = readFile.Replace("[orderid]", oid);
                string txtmessage = readFile;//readFile + body;
                string subj = "Thanks for your order";
                EmailSendingUtility emailSendingUtility = new EmailSendingUtility();
                emailSendingUtility.Email_maaaahwanam(txtto, txtmessage, subj);
                string targetmails = "lakshmi.p@xsilica.com,seema.g@xsilica.com,rameshsai@xsilica.com";
                emailSendingUtility.Email_maaaahwanam(targetmails, txtmessage, subj);
            }

            return Json("success", JsonRequestBehavior.AllowGet);

        }

        public ActionResult pdfdownload(string pdfhtml)
        {
            HtmlToPdf HtmlToPdf = new IronPdf.HtmlToPdf();
           var PDF = HtmlToPdf.RenderHtmlAsPdf(pdfhtml);
            var OutputPath = "HtmlToPDF.pdf";
            PDF.SaveAs(OutputPath);
            // This neat trick opens our PDF file so we can see the result in our default PDF viewer
            System.Diagnostics.Process.Start(OutputPath);
            return Json("success", JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetoderdetailsbyOrderdetailId(string odid)
        {
            var data = orderdetailservices.GetOrderDetailsByOrderdetailid(Convert.ToInt32(odid));

            return Json(data);
        }
    }
}