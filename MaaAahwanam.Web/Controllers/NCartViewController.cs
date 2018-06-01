﻿using MaaAahwanam.Models;
using MaaAahwanam.Repository;
using MaaAahwanam.Service;
using MaaAahwanam.Utility;
using MaaAahwanam.Web.Custom;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MaaAahwanam.Web.Controllers
{
    public class NCartViewController : Controller
    {

        CartService cartService = new CartService();
        UserLoginDetailsService userLoginDetailsService = new UserLoginDetailsService();
        WhishListService whishListService = new WhishListService();
        VendorProductsService vendorProductsService = new VendorProductsService();

        private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");

        // GET: NCartView
        public ActionResult Index()
        {
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
                if (user.UserType == "User")
                {
                    var userdata = userLoginDetailsService.GetUser((int)user.UserId);
                    if (user.UserType == "Admin")
                    {
                        ViewBag.cartCount = cartService.CartItemsCount(0);
                        return PartialView("ItemsCartdetails");
                    }
                    ViewBag.cartCount = cartService.CartItemsCount((int)user.UserId);

                    List<GetCartItems_Result> cartlist = cartService.CartItemsList(int.Parse(user.UserId.ToString()));
                    decimal total = cartlist.Sum(s => s.TotalPrice);
                    ViewBag.cartitems = cartlist.OrderByDescending(m => m.UpdatedDate).Where(m=>m.Status == "Active");
                    ViewBag.Total = total;
                }
            }
            else
            {
                ViewBag.cartCount = cartService.CartItemsCount(0);
            }
            return View();
        }

        public JsonResult DeletecartItem(long cartId)
        {
            var message = cartService.Deletecartitem(cartId);
            return Json(message);
        }

        public ActionResult DealsSection(string type, string L1)
        {
            int takecount = (L1 != null) ? int.Parse(L1) : 6;
            if (type == null)  type = "Venue";
            //ViewBag.records = vendorProductsService.Getvendorproducts_Result("Venue").Take(4);
            //var deals = vendorProductsService.getalldeal().OrderBy(m => m.DealID).Where(m => m.VendorType == type);
            if (type != null) if (type.Split(',').Count() > 1) type = "Venue";
            if (type == "Conventions" || type == "Resorts" || type == "Hotels" || type == "Venues" || type == "Banquet Hall" || type == "Function Hall" || type == "Banquet" || type == "Function")
                type = "Venue";
            if (type == "Mehendi" || type == "Pandit")
                type = "Other";
            ViewBag.type = type;
            var records = vendorProductsService.Getvendorproducts_Result(type);
            ViewBag.deal = records.Take(takecount).ToList();
            int count = records.Count();

            ViewBag.count = (count >= takecount) ? "1" : "0";
            return PartialView();
        }
        public JsonResult addwishlistItem(long cartId)
        {
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
                if (user.UserType == "User")
                {
                    var userdata = userLoginDetailsService.GetUser((int)user.UserId);
                    ViewBag.cartCount = cartService.CartItemsCount((int)user.UserId);
                    var cartlist = cartService.CartItemsList(int.Parse(user.UserId.ToString()));
                    var cartdetails = cartlist.Where(m => m.CartId == cartId).FirstOrDefault();
                    AvailableWhishLists availableWhishLists = new AvailableWhishLists();
                    availableWhishLists.VendorID = cartdetails.Id.ToString();
                    availableWhishLists.VendorSubID = (cartdetails.subid).ToString();
                    availableWhishLists.BusinessName = cartdetails.BusinessName;
                    availableWhishLists.ServiceType = cartdetails.ServiceType;
                    availableWhishLists.UserID = user.UserId.ToString();
                    availableWhishLists.IPAddress = HttpContext.Request.UserHostAddress;
                    var list = whishListService.GetWhishList(user.UserId.ToString()).Where(m => m.VendorID == (cartdetails.Id).ToString() && m.VendorSubID == (cartdetails.subid).ToString() && m.ServiceType == cartdetails.ServiceType).Count(); //Checking whishlist availablility
                    if (list == 0)
                        availableWhishLists = whishListService.AddWhishList(availableWhishLists);

                    var message = cartService.Deletecartitem(cartId);
                    return Json(message);
                }
            }
            return Json(JsonRequestBehavior.AllowGet);

        }


        public ActionResult booknow(string cartnos)
        {

            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
                if (user.UserType == "User")
                {
                    var userdata = userLoginDetailsService.GetUser((int)user.UserId);
                    ViewBag.cartCount = cartService.CartItemsCount((int)user.UserId);
                    var cartlist = cartService.CartItemsList(int.Parse(user.UserId.ToString()));
                    var cartnos1 = cartnos.Split(',');
                    for (int i = 0; i <= cartnos1.Count(); i++)
                    {
                        string cartno2 = cartnos1[i];

                        string totalprice = "";
                        var cartdetails = cartlist.Where(m => m.CartId == Convert.ToInt64(cartno2)).FirstOrDefault();
                        string type = cartdetails.ServicType;
                        string guest = Convert.ToString(cartdetails.Quantity);
                        string price = Convert.ToString(cartdetails.Perunitprice);
                        string id = Convert.ToString(cartdetails.Id);
                        string did = Convert.ToString(cartdetails.DealId);
                        string timeslot = cartdetails.attribute;
                        string etype1 = cartdetails.ServiceType;
                        string vid = Convert.ToString(cartdetails.subid);

                        if (type == "Photography" || type == "Decorator" || type == "Other")
                        {
                            totalprice = price;
                            guest = "0";
                        }

                        else
                        {
                            totalprice = Convert.ToString(cartdetails.TotalPrice);
                        }
                        DateTime updateddate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
                        int userid = Convert.ToInt32(user.UserId);
                        //Saving Record in order Table
                        OrderService orderService = new OrderService();
                        Order order = new Order();
                        order.TotalPrice = Convert.ToDecimal(totalprice);
                        order.OrderDate = Convert.ToDateTime(updateddate); //Convert.ToDateTime(bookeddate);
                        order.UpdatedBy = (Int64)user.UserId;
                        order.OrderedBy = (Int64)user.UserId;
                        order.UpdatedDate = Convert.ToDateTime(updateddate);
                        order.Status = "Pending";
                        order = orderService.SaveOrder(order);


                        //Payment Section
                        Payment_orderServices payment_orderServices = new Payment_orderServices();
                        Payment_Orders payment_Orders = new Payment_Orders();
                        payment_Orders.cardnumber = "4222222222222";
                        payment_Orders.CVV = "214";
                        payment_Orders.paidamount = decimal.Parse(totalprice);
                        //payment_Orders.PaymentID = orderRequest.PaymentId;
                        payment_Orders.Paiddate = Convert.ToDateTime(updateddate);
                        payment_Orders.OrderID = order.OrderId;
                        payment_Orders = payment_orderServices.SavePayment_Orders(payment_Orders);


                        //Saving Order Details
                        OrderdetailsServices orderdetailsServices = new OrderdetailsServices();
                        OrderDetail orderDetail = new OrderDetail();
                        orderDetail.OrderId = order.OrderId;
                        orderDetail.OrderBy = user.UserId;
                        orderDetail.PaymentId = payment_Orders.PaymentID;
                        orderDetail.ServiceType = type;
                        orderDetail.ServicePrice = decimal.Parse(price);
                        orderDetail.attribute = timeslot;
                        orderDetail.TotalPrice = decimal.Parse(totalprice);
                        orderDetail.PerunitPrice = decimal.Parse(price);
                        orderDetail.Quantity = int.Parse(guest);
                        orderDetail.OrderId = order.OrderId;
                        orderDetail.VendorId = long.Parse(id);
                        orderDetail.Status = "Pending";
                        orderDetail.UpdatedDate = Convert.ToDateTime(updateddate);
                        orderDetail.UpdatedBy = user.UserId;
                        orderDetail.subid = long.Parse(vid);
                        orderDetail.BookedDate = updateddate;
                        orderDetail.EventType = etype1;
                        orderDetail.DealId = long.Parse(did);
                        orderdetailsServices.SaveOrderDetail(orderDetail);

                        var userlogdetails = userLoginDetailsService.GetUserId(userid);

                        string txtto = userlogdetails.UserName;
                        var userdetails = userLoginDetailsService.GetUser(userid);
                        string name = userdetails.FirstName;
                        name = Capitalise(name);
                        string OrderId = Convert.ToString(order.OrderId);
                        string url = Request.Url.Scheme + "://" + Request.Url.Authority;
                        FileInfo File = new FileInfo(Server.MapPath("/mailtemplate/order.html"));
                        string readFile = File.OpenText().ReadToEnd();
                        readFile = readFile.Replace("[ActivationLink]", url);
                        readFile = readFile.Replace("[name]", name);
                        readFile = readFile.Replace("[orderid]", OrderId);
                        string txtmessage = readFile;//readFile + body;
                        string subj = "Thanks for your order";
                        EmailSendingUtility emailSendingUtility = new EmailSendingUtility();
                        emailSendingUtility.Email_maaaahwanam(txtto, txtmessage, subj);

                        var vendordetails = userLoginDetailsService.getvendor(Convert.ToInt32(id));

                        string txtto1 = vendordetails.EmailId;
                        string vname = vendordetails.BusinessName;
                        vname = Capitalise(vname);

                        string url1 = Request.Url.Scheme + "://" + Request.Url.Authority;
                        FileInfo file1 = new FileInfo(Server.MapPath("/mailtemplate/vorder.html"));
                        string readfile1 = file1.OpenText().ReadToEnd();
                        readfile1 = readfile1.Replace("[ActivationLink]", url1);
                        readfile1 = readfile1.Replace("[name]", name);
                        readfile1 = readfile1.Replace("[vname]", vname);
                        readfile1 = readfile1.Replace("[orderid]", OrderId);
                        string txtmessage1 = readfile1;
                        string subj1 = "order has been placed";
                        emailSendingUtility.Email_maaaahwanam(txtto1, txtmessage1, subj1);
                        var message = cartService.Deletecartitem(long.Parse(cartno2));

                    }

                }
                return Json("Success", JsonRequestBehavior.AllowGet);
            }
            return Json(JsonRequestBehavior.AllowGet);
        }

        public ActionResult Updatecartitem(long cartId)
        {

           
                var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
                if (user.UserType == "User")
                {
                    var userdata = userLoginDetailsService.GetUser((int)user.UserId);
                    ViewBag.cartCount = cartService.CartItemsCount((int)user.UserId);
                    var cartlist = cartService.CartItemsList(int.Parse(user.UserId.ToString()));
                    var cartdetails = cartlist.Where(m => m.CartId == cartId).FirstOrDefault();


                    string updateddate = DateTime.UtcNow.ToShortDateString();
                    CartItem cartItem = new CartItem();
                    cartItem.VendorId = cartdetails.Id;
                    cartItem.ServiceType = cartdetails.ServiceType;
                    cartItem.Perunitprice = cartdetails.Perunitprice;
                    cartItem.TotalPrice = cartdetails.TotalPrice;
                    cartItem.Orderedby = user.UserId;
                    cartItem.Quantity = cartdetails.Quantity;
                    cartItem.UpdatedDate = Convert.ToDateTime(updateddate);
                    cartItem.attribute = cartdetails.attribute;
                    cartItem.CartId = cartdetails.CartId;
                    cartItem.Status = "Saved";
                    string mesaage = cartService.Updatecartitem(cartItem);
                    return Json(mesaage);
                }
            return Json(JsonRequestBehavior.AllowGet);
        }
           
        

        public string Capitalise(string str)
        {
            if (String.IsNullOrEmpty(str))
            return String.Empty;
            return Char.ToUpper(str[0]) + str.Substring(1).ToLower();
        }
    }
}
