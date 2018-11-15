using MaaAahwanam.Models;
using MaaAahwanam.Repository;
using MaaAahwanam.Service;
using MaaAahwanam.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using MaaAahwanam.Utility;


namespace MaaAahwanam.Web.Areas.Admin.Controllers
{

    public class quotationlistController : Controller
    {
        OrderService orderService = new OrderService();
        VendorDashBoardService mnguserservice = new VendorDashBoardService();

        private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        // GET: Admin/quotationlist
        public ActionResult Index()
        {
            var orderss = orderService.allorderslist().Where(m => m.ordertype == "Quote").ToList();
           // var orders = orderService.userOrderList().Where(m => m.ordertype == "Quote").ToList();
            //orders.RemoveRange(orders.Where(c => c.OrderId == OrderId));
           // var orders1 = orderService.userOrderList1().Where(m => m.ordertype == "Quote").ToList();
            ViewBag.order = orderss.OrderByDescending(m => m.OrderId);
           // ViewBag.order1 = orders1.OrderByDescending(m => m.OrderId);


            return View();
        }
        public ActionResult QuoteReply(string id)
        {
            var orders = orderService.userOrderList().Where(m => m.ordertype == "Quote").ToList();
            var orders1 = orderService.userOrderList1().Where(m => m.ordertype == "Quote").ToList();
            ViewBag.orderdetails = orders.Where(m => m.OrderId == long.Parse(id));
            ViewBag.orderdetails1 = orders1.Where(m => m.OrderId == long.Parse(id));
            return View();
        }

        public ActionResult FilteredVendors(string type, string date, string id)
        {
            
            return PartialView("FilteredVendors", "Quotations");
        }

        public string Capitalise(string str)
        {
            if (String.IsNullOrEmpty(str))
                return String.Empty;
            return Char.ToUpper(str[0]) + str.Substring(1).ToLower();
        }
        public ActionResult FilteredVendors1(string id)
        {
            var orders = orderService.userOrderList().Where(m => m.ordertype == "Quote").ToList();
            var orders1 = orderService.userOrderList1().Where(m => m.ordertype == "Quote").ToList();
           var s1= ViewBag.orderdetails = orders.Where(m => m.OrderId == long.Parse(id)).FirstOrDefault();
            var s2 = ViewBag.orderdetails1 = orders1.Where(m => m.OrderId == long.Parse(id)).FirstOrDefault();
            var userlogdetails = mnguserservice.getuserbyid(Convert.ToInt32(s1.userid));
            string txtto = userlogdetails.email;
            string name = userlogdetails.firstname;
            name = Capitalise(name);
            string OrderId = Convert.ToString(s1.OrderId);
            StringBuilder cds = new StringBuilder();
            cds.Append("<table style='border:1px black solid;'><tbody>");
            cds.Append("<tr><td>Order Id</td><td>Order Date</td><td> Event Type </td><td> Quantity</td><td>Perunit Price</td><td>Total Price</td></tr>");
            cds.Append("<tr><td style = 'width: 75px;border: 1px black solid;'> " + s1.OrderId + "</td><td style = 'width: 75px;border: 1px black solid;' > " + s1.BookedDate + " </td><td style = 'width: 75px;border: 1px black solid;'> " + s1.EventType + " </td><td style = 'width: 50px;border: 1px black solid;'> " + s1.Quantity + " </td> <td style = 'width: 50px;border: 1px black solid;'> " + s1.PerunitPrice + " </td><td style = 'width: 50px;border: 1px black solid;'> " + s1.TotalPrice + " </td></tr>");  //<td style = 'width: 50px;border: 2px black solid;'> " + item.eventstartdate + " </td><td> date </td>
            cds.Append("</tbody></table>");
            string url = Request.Url.Scheme + "://" + Request.Url.Authority;
            //FileInfo File = new FileInfo(Server.MapPath("/mailtemplate/order.html"));
            //string readFile = File.OpenText().ReadToEnd();
            //readFile = readFile.Replace("[ActivationLink]", url);
            //readFile = readFile.Replace("[name]", name);
            //readFile = readFile.Replace("[orderid]", OrderId);
            //readFile = readFile.Replace("[table]", cds.ToString());
            ViewBag.url = url;
            ViewBag.name = name;
            ViewBag.OrderId = OrderId;
            ViewBag.table = cds.ToString();
            return View();
        }

        public ActionResult email(string id)
        {
            var orders = orderService.userOrderList().Where(m => m.ordertype == "Quote").ToList();
            var orders1 = orderService.userOrderList1().Where(m => m.ordertype == "Quote").ToList();
            var s3 = new List<sp_vendoruserorddisplay_Result>();
            var s12 = ViewBag.orderdetails = orders.Where(m => m.OrderId == long.Parse(id));
            var s1 = ViewBag.orderdetails = orders.Where(m => m.OrderId == long.Parse(id)).FirstOrDefault();
            var s22 = ViewBag.orderdetails1 = orders1.Where(m => m.OrderId == long.Parse(id));
            var s2 = ViewBag.orderdetails1 = orders1.Where(m => m.OrderId == long.Parse(id)).FirstOrDefault();

            if(s1 == null) { s3 = s22; } else { s3 = s12; }
            var userlogdetails = mnguserservice.getuserbyid(Convert.ToInt32(s1.userid));
            string txtto = userlogdetails.email;
            string name = userlogdetails.firstname;
            name = Capitalise(name);
            string OrderId = Convert.ToString(s1.OrderId);
            StringBuilder cds = new StringBuilder();
            cds.Append("<table style='border:1px black solid;'><tbody>");
            cds.Append("<tr><td>Order Id</td><td>Order Date</td><td> Event Type </td><td> Quantity</td><td>Perunit Price</td><td>Total Price</td></tr>");
            foreach (var item in s3)
            {
                cds.Append("<tr><td style = 'width: 75px;border: 1px black solid;'> " + item.OrderId + "</td><td style = 'width: 75px;border: 1px black solid;' > " + item.BookedDate + " </td><td style = 'width: 75px;border: 1px black solid;'> " + item.EventType + " </td><td style = 'width: 50px;border: 1px black solid;'> " + item.Quantity + " </td> <td style = 'width: 50px;border: 1px black solid;'> " + item.PerunitPrice + " </td><td style = 'width: 50px;border: 1px black solid;'> " + item.TotalPrice + " </td></tr>");
            }//<td style = 'width: 50px;border: 2px black solid;'> " + item.eventstartdate + " </td><td> date </td>
            cds.Append("</tbody></table>");
            string url = Request.Url.Scheme + "://" + Request.Url.Authority;
            FileInfo File = new FileInfo(Server.MapPath("/mailtemplate/order.html"));
            string readFile = File.OpenText().ReadToEnd();
            readFile = readFile.Replace("[ActivationLink]", url);
            readFile = readFile.Replace("[name]", name);
            readFile = readFile.Replace("[orderid]", OrderId);
            readFile = readFile.Replace("[table]", cds.ToString());
            string txtmessage1 = readFile;
            string subj1 = "order has been placed";
                        string txtto1 = s1.username;
            EmailSendingUtility emailSendingUtility = new EmailSendingUtility();
            emailSendingUtility.Email_maaaahwanam(txtto1, txtmessage1, subj1);
            return View();
        }
    }
}
