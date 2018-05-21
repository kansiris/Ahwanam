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

namespace MaaAahwanam.Web.Controllers
{
    public class NUserProfileController : Controller
    {

        UserLoginDetailsService userLoginDetailsService = new UserLoginDetailsService();
        OrderService orderService = new OrderService();
        // GET: NUserProfile
        public ActionResult Index()
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
                    ViewBag.order = orders.OrderByDescending(m=>m.OrderId).Where(m => m.Status == "Pending" ).ToList();
                    ViewBag.orderhistory = orders.OrderByDescending(m => m.OrderId).Where(m=>m.Status == "InActive" || m.Status == "Cancelled").ToList();
                    WhishListService whishListService = new WhishListService();
                    ViewBag.whishlists = whishListService.GetWhishList(user.UserId.ToString());
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
        public ActionResult orderdelete(string orderid)
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
                    var orders = orderService.userOrderList().Where(m => m.OrderId  == Convert.ToInt64(orderid));
                    Order order = new Order();
                    OrderDetail orderdetail = new OrderDetail();
                    order.Status = "Removed";
                    orderdetail.Status = "Removed";
                    order = orderService.updateOrderstatus(order , orderdetail, Convert.ToInt64(orderid));
                    TempData["Active"] = "Order Deleted";
                    return RedirectToAction("Index", "NUserProfile");
                }
                TempData["Active"] = "Please Login";
                return RedirectToAction("Index", "NUserRegistration");
            }
            TempData["Active"] = "Please Login";
            return RedirectToAction("Index", "NUserRegistration");
        }

        public ActionResult ordercancel(string orderid)
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
                    return RedirectToAction("Index", "NUserProfile");
                }
                TempData["Active"] = "Please Login";
                return RedirectToAction("Index", "NUserRegistration");
            }
            TempData["Active"] = "Please Login";
            return RedirectToAction("Index", "NUserRegistration");
        }


        public ActionResult changepassword(UserLogin userLogin)
        {
            var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
            var userdata12 = userLoginDetailsService.GetUserId((int)user.UserId);
            userLoginDetailsService.changepassword(userLogin, (int)user.UserId);
            return Json("success");
        }

        public ActionResult updatedetails(UserDetail userdetail)
        {
            var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
            userLoginDetailsService.UpdateUserdetailsnew(userdetail, (int)user.UserId);
            return Json("success");
        }
    }
}