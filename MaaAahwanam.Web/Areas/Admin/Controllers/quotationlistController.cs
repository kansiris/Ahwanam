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

namespace MaaAahwanam.Web.Areas.Admin.Controllers
{

    public class quotationlistController : Controller
    {
        OrderService orderService = new OrderService();
        VendorDashBoardService mnguserservice = new VendorDashBoardService();
        UserLoginDetailsService userLoginDetailsService = new UserLoginDetailsService();
        string name;
        private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        // GET: Admin/quotationlist
        public ActionResult Index()
        {
            var orderss = orderService.allorderslist().Where(m => m.ordertype == "Quote").ToList();
            ViewBag.order = orderss.OrderByDescending(m => m.OrderId);
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
        public ActionResult FilteredVendors1(string id, string type)
        {
            var orders = orderService.alluserOrderList(type).Where(m => m.ordertype == "Quote").ToList();
            var s12 = ViewBag.orderdetails = orders.Where(m => m.OrderId == long.Parse(id)).ToList();

            var s1 = ViewBag.orderdetails = orders.Where(m => m.OrderId == long.Parse(id)).FirstOrDefault();
            string txtto; 
            if (type == "Vendor")
            {
                var userlogdetails = mnguserservice.getuserbyid(Convert.ToInt32(s1.userid));
                string txtto1 = userlogdetails.email;
                string name1 = userlogdetails.firstname;
                txtto = txtto1;
                name = name1;
                name = Capitalise(name);

            }
            else if (type == "User")
            {
                var userdata = userLoginDetailsService.GetUserId((int)s1.userid);
                var userdata1 = userLoginDetailsService.GetUser((int)s1.userid);
                string txtto2 = userdata.UserName;
                string name2 = userdata1.FirstName;
                txtto = txtto2;
                name = name2;
                name = Capitalise(name);

            }
            string OrderId = Convert.ToString(s1.OrderId);
            
            StringBuilder cds = new StringBuilder();

           // cds.Append("<table style='width:70%;'><tbody>");
           //cds.Append("<tr><td style = 'width:20%;'></td><td><strong> Details</strong> </td><td> <strong>Event Type</strong> </td><td><strong>Total Amount</strong></td></tr>");
           // foreach (var item in s12)
            //{
            //    string image;
            //    if (item.logo != null)
            //    {
            //        image = "/vendorimages/" + item.logo.Trim(',') + "";
            //    }
            //    else { image = "/noimages.png"; }
            //    cds.Append("<tr><td style = 'width:20%;'>  <img src = " + image + " style='height: 182px;width: 132px;'/></td><td style = '' > <p>" + "<strong>Business Name: </strong>" + item.BusinessName + "</p><p>" + "<strong>Packgename: </strong>" + item.PackageName + "</p><p>" + "<strong>Price/person:</strong> " + '₹'  + item.PerunitPrice.ToString().Replace(".00", "") + "</p><p>" + "<strong>No. of Guests:</strong> " + item.Quantity + "</p> </td><td > "  + item.EventType + " </td><td style = ''><p>" +'₹'+ item.TotalPrice.ToString().Replace(".00", "") + "</p> </td> </tr>");
            //}
            //cds.Append("</tbody></table>");



            
            string url = Request.Url.Scheme + "://" + Request.Url.Authority;
            ViewBag.ser = s12;
            ViewBag.url = url;
            ViewBag.name = name;
            ViewBag.OrderId = OrderId;
            ViewBag.table = cds.ToString();
            return View();
        }

        public ActionResult email(string id,string type)
        {
           
            var orders = orderService.alluserOrderList(type).Where(m => m.ordertype == "Quote").ToList();
            var s12 = ViewBag.orderdetails = orders.Where(m => m.OrderId == long.Parse(id)).ToList();
            var s1 = ViewBag.orderdetails = orders.Where(m => m.OrderId == long.Parse(id)).FirstOrDefault();
            string txtto;
            if (type == "Vendor")
            {
                var userlogdetails = mnguserservice.getuserbyid(Convert.ToInt32(s1.userid));
                string txtt1 = userlogdetails.email;
                string name1 = userlogdetails.firstname;
                txtto = txtt1;
                name = name1;
                name = Capitalise(name);

            }
            else if (type == "User")
            {
                var userdata = userLoginDetailsService.GetUserId((int)s1.userid);
                var userdata1 = userLoginDetailsService.GetUser((int)s1.userid);
                string txtto2 = userdata.UserName;
                string name2 = userdata1.FirstName;
                txtto = txtto2;
                name = name2;
                name = Capitalise(name);

            }
           
            name = Capitalise(name);
            string OrderId = Convert.ToString(s1.OrderId);
            StringBuilder cds = new StringBuilder();
            cds.Append("<table style='width:70%;'><tbody>");
            cds.Append("<tr><td style = 'width:20%;'></td><td><strong> Details</strong> </td><td> <strong>Event Type</strong> </td><td><strong>Total Amount</strong></td></tr>");
            foreach (var item in s12)
            {
                string image;
                if (item.logo != null)
                {
                    image = "/vendorimages/" + item.logo.Trim(',') + "";
                }
                else { image = "/noimages.png"; }
                cds.Append("<tr><td style = 'width:20%;'>  <img src = " + image + " style='height: 182px;width: 132px;'/></td><td style = '' > <p>" + "<strong>Business Name: </strong>" + item.BusinessName + "</p><p>" + "<strong>Packgename: </strong>" + item.PackageName + "</p><p>" + "<strong>Price/person:</strong> " + '₹' + item.PerunitPrice.ToString().Replace(".00", "") + "</p><p>" + "<strong>No. of Guests:</strong> " + item.Quantity + "</p> </td><td > " + item.EventType + " </td><td style = ''><p>" + '₹' + item.TotalPrice.ToString().Replace(".00", "") + "</p> </td> </tr>");
            }
            cds.Append("</tbody></table>");
            string url = Request.Url.Scheme + "://" + Request.Url.Authority;
            //FileInfo File = new FileInfo(Server.MapPath("/mailtemplate/order.html"));
            FileInfo File = new FileInfo(Server.MapPath("/mailtemplate/kansiris.html"));
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
            var msg = "email sent";
            return Json( msg,JsonRequestBehavior.AllowGet);
        }
    }
}
