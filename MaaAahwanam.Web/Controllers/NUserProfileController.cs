using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaaAahwanam.Web.Custom;
using MaaAahwanam.Service;
using System.Net;
using MaaAahwanam.Models;
using MaaAahwanam.Utility;
using System.Text.RegularExpressions;
using MaaAahwanam.Repository;
using System.Globalization;
using System.IO;

namespace MaaAahwanam.Web.Controllers
{
    public class NUserProfileController : Controller
    {
        QuotationListsService quotationListsService = new QuotationListsService();
        UserLoginDetailsService userLoginDetailsService = new UserLoginDetailsService();
        OrderService orderService = new OrderService();
        // GET: NUserProfile
        public ActionResult Index()
        {
            try
            {
                if (TempData["Active"] != "")
                {
                    ViewBag.Active = TempData["Active"];
                }

                if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
                    if (user.UserType == "Vendor")
                    {
                        Response.Redirect("/AvailableServices/changeid?id=" + user.UserId + "");
                    }
                    if (user.UserType == "User")
                    {
                        var userdata = userLoginDetailsService.GetUser((int)user.UserId);
                        if (userdata.FirstName != "" && userdata.FirstName != null)
                            ViewBag.username = userdata.FirstName;
                        else if (userdata.FirstName != "" && userdata.FirstName != null && userdata.LastName != "" && userdata.LastName != null)
                            ViewBag.username = "" + userdata.FirstName + " " + userdata.LastName + "";
                        else
                            ViewBag.username = userdata.AlternativeEmailID;
                        ViewBag.phoneno = userdata.UserPhone;
                        var userdata1 = userLoginDetailsService.GetUserId((int)user.UserId);
                        ViewBag.emailid = userdata1.UserName;
                        var orders = orderService.userOrderList().Where(m => m.UserLoginId == (int)user.UserId);
                        ViewBag.order = orders.OrderByDescending(m => m.OrderId).Where(m => m.orderstatus == "Pending" || m.orderstatus == "Vendor Declined" || m.orderstatus == "Active").ToList();
                        ViewBag.orderhistory = orders.OrderByDescending(m => m.OrderId).Where(m => m.orderstatus == "InActive" || m.orderstatus == "Cancelled").ToList();
                        WhishListService whishListService = new WhishListService();
                        ViewBag.whishlists = whishListService.GetWhishList(user.UserId.ToString());
                        ViewBag.quotations = quotationListsService.GetAllQuotations().Where(m => m.EmailId == userdata1.UserName).ToList();
                        // OrderByDescending(m => m.OrderId).Take(10);
                        //   List<GetCartItemsnew_Result> cartlist = cartService.CartItemsListnew(int.Parse(user.UserId.ToString()));
                        //decimal total = cartlist.Sum(s => s.TotalPrice);
                        //ViewBag.Cartlist = cartlist;
                        // ViewBag.Total = total;
                        return View();
                    }
                    TempData["Active"] = "Please Login";
                    return RedirectToAction("Index", "NUserRegistration");
                }
                TempData["Active"] = "Please Login";
                return RedirectToAction("Index", "NUserRegistration");
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Nhomepage");
            }
        }
        public ActionResult orderdelete(string orderid)
        {
            try
            {
                if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
                    if (user.UserType == "Vendor")
                    {
                        Response.Redirect("/AvailableServices/changeid?id=" + user.UserId + "");
                    }
                    if (user.UserType == "User")
                    {
                        var userdata = userLoginDetailsService.GetUser((int)user.UserId);
                        var orders = orderService.userOrderList().Where(m => m.OrderId == Convert.ToInt64(orderid));
                        Order order = new Order();
                        OrderDetail orderdetail = new OrderDetail();
                        order.Status = "Removed";
                        orderdetail.Status = "Removed";
                        order = orderService.updateOrderstatus(order, orderdetail, Convert.ToInt64(orderid));
                        TempData["Active"] = "Order Deleted";
                        

                        return RedirectToAction("Index", "NUserProfile");
                    }
                    TempData["Active"] = "Please Login";
                    return RedirectToAction("Index", "NUserRegistration");
                }
                TempData["Active"] = "Please Login";
                return RedirectToAction("Index", "NUserRegistration");
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Nhomepage");
            }
        }

        public ActionResult ordercancel(string orderid)
        {
            try
            {
                if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
                    if (user.UserType == "Vendor")
                    {
                        Response.Redirect("/AvailableServices/changeid?id=" + user.UserId + "");
                    }
                    if (user.UserType == "User")
                    {
                        var userdata = userLoginDetailsService.GetUser((int)user.UserId);
                        var orders = orderService.userOrderList().Where(m => m.OrderId == Convert.ToInt64(orderid));
                        Order order = new Order();
                        OrderDetail orderdetail = new OrderDetail();
                        order.Status = "Cancelled";
                        orderdetail.Status = "Cancelled";
                        order = orderService.updateOrderstatus(order, orderdetail, Convert.ToInt64(orderid));
                        TempData["Active"] = "Order Cancelled";
                        SendEmail((int)user.UserId,orderid,orders.FirstOrDefault().vid.ToString(), "Order Cancelled");
                        return RedirectToAction("Index", "NUserProfile");
                    }
                    TempData["Active"] = "Please Login";
                    return RedirectToAction("Index", "NUserRegistration");
                }
                TempData["Active"] = "Please Login";
                return RedirectToAction("Index", "NUserRegistration");
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Nhomepage");
            }
        }


        public ActionResult changepassword(UserLogin userLogin)
        {
            try
            {
                var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
                var userdata12 = userLoginDetailsService.GetUserId((int)user.UserId);
                userLoginDetailsService.changepassword(userLogin, (int)user.UserId);
                return Json("success");
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Nhomepage");
            }
        }

        public ActionResult updatedetails(UserDetail userdetail)
        {
            try
            {
                var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
                userLoginDetailsService.UpdateUserdetailsnew(userdetail, (int)user.UserId);
                return Json("success");
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Nhomepage");
            }
        }

        public PartialViewResult VendorsLists(string qid)
        {
            if (qid != "" && qid != null)
            {
                var quotedata = quotationListsService.GetAllQuotations().FirstOrDefault(m => m.Id == long.Parse(qid));
                var quoteresponses = quotationListsService.GetAllQuoteResponses().Where(m => m.QuoteID == long.Parse(qid)).ToList();
                List<QuoteResponse> quotes = new List<QuoteResponse>();
                for (int i = 0; i < quoteresponses.Count; i++)
                {
                    var firstinstdate = Convert.ToInt32(quoteresponses[i].FirstInstallment.Split(',')[1]);
                    var secondinstdate = Convert.ToInt32(quoteresponses[i].SecondInstallment.Split(',')[1]);
                    var thirdinstdate = Convert.ToInt32(quoteresponses[i].ThirdInstallment.Split(',')[1]);
                    var fourthinstdate = Convert.ToInt32(quoteresponses[i].FourthInstallment.Split(',')[1]);
                    DateTime occasiondate = Convert.ToDateTime(quotedata.EventStartDate);
                    var first = occasiondate.AddDays(-firstinstdate);
                    var second = occasiondate.AddDays(-secondinstdate);
                    var third = occasiondate.AddDays(-thirdinstdate);
                    var fourth = occasiondate.AddDays(-fourthinstdate);
                    QuoteResponse q = new QuoteResponse();
                    q.FirstInstallment = first.ToString("dd-MMM-yyyy");
                    q.SecondInstallment = second.ToString("dd-MMM-yyyy");
                    q.ThirdInstallment = third.ToString("dd-MMM-yyyy");
                    q.FourthInstallment = fourth.ToString("dd-MMM-yyyy");
                    q.BusinessName = quoteresponses[i].BusinessName;
                    q.ServiceType = quoteresponses[i].ServiceType;
                    q.SubServiceType = quoteresponses[i].SubServiceType;
                    q.Installments = quoteresponses[i].Installments;
                    q.TokenAmount = quoteresponses[i].TokenAmount;
                    quotes.Add(q);
                    //var quotedisplay = new QuoteResponse {  first.ToString("dd-MMM-yyyy"), second.ToString("dd-MMM-yyyy"), third.ToString("dd-MMM-yyyy"), fourth.ToString("dd-MMM-yyyy") };
                }
                ViewBag.quotes = quotes;
            }
            return PartialView("VendorsLists");
        }

        public void SendEmail(int userid,string orderid,string vendorid,string msg)
        {
            var userlogdetails = userLoginDetailsService.GetUserId(userid);
            string txtto = userlogdetails.UserName;
            var userdetails = userLoginDetailsService.GetUser(userid);
            string name = userdetails.FirstName;
            name = Capitalise(name);
            string OrderId = orderid;
            string url = Request.Url.Scheme + "://" + Request.Url.Authority;
            FileInfo File = new FileInfo(Server.MapPath("/mailtemplate/cancelordeleteorder.html"));
            string readFile = File.OpenText().ReadToEnd();
            readFile = readFile.Replace("[ActivationLink]", url);
            readFile = readFile.Replace("[name]", name);
            readFile = readFile.Replace("[msg]", "Your Order has been Cancelled");
            readFile = readFile.Replace("[orderid]", OrderId);
            string txtmessage = readFile;//readFile + body;
            string subj = "Order#"+orderid+" Cancelled";
            EmailSendingUtility emailSendingUtility = new EmailSendingUtility();
            //emailSendingUtility.Email_maaaahwanam(txtto, txtmessage, subj);
            emailSendingUtility.Email_maaaahwanam(txtto, txtmessage, subj, null);

            var vendordetails = userLoginDetailsService.getvendor(Convert.ToInt32(vendorid));

            string txtto1 = vendordetails.EmailId;
            string vname = vendordetails.BusinessName;
            vname = Capitalise(vname);

            string url1 = Request.Url.Scheme + "://" + Request.Url.Authority;
            FileInfo file1 = new FileInfo(Server.MapPath("/mailtemplate/vcancelordeleteorder.html"));
            string readfile1 = file1.OpenText().ReadToEnd();
            readfile1 = readfile1.Replace("[ActivationLink]", url1);
            readfile1 = readfile1.Replace("[name]", name);
            readfile1 = readfile1.Replace("[vname]", vname);
            readfile1 = readfile1.Replace("[orderid]", OrderId);
            //readFile = readFile.Replace("[msg]", "Order has been Cancelled by "+name+"");
            string txtmessage1 = readfile1;
            string subj1 = "Order#" + orderid + " Cancelled";
            emailSendingUtility.Email_maaaahwanam("rameshsai@xsilica.com", txtmessage1, subj1, null); //Replace this email with txtto1
        }

        public string Capitalise(string str)
        {
            if (String.IsNullOrEmpty(str))
                return String.Empty;
            return Char.ToUpper(str[0]) + str.Substring(1).ToLower();
        }
    }
}