using MaaAahwanam.Utility;
using MaaAahwanam.Web.Custom;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaaAahwanam.Models;
using MaaAahwanam.Repository;
using MaaAahwanam.Service;

namespace MaaAahwanam.Web.Controllers
{
    public class NCartViewController : Controller
    {
        CartService cartService = new CartService();
        UserLoginDetailsService userLoginDetailsService = new UserLoginDetailsService();
        WhishListService whishListService = new WhishListService();
        VendorProductsService vendorProductsService = new VendorProductsService();
        Ndealservice NdealService = new Ndealservice();

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
                    decimal total = cartlist.Where(m => m.Status == "Active").Sum(s => s.TotalPrice);
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


        public ActionResult savedlater()
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
                  var data1  = cartlist.OrderByDescending(m => m.UpdatedDate).Where(m => m.Status == "Saved");

                    ViewBag.cartitems = data1;
                    ViewBag.savecount = data1.Count() ;

                    ViewBag.Total = total;
                }
            }
            else
            {
                ViewBag.cartCount = cartService.CartItemsCount(0);
            }
            return PartialView();
        }
        public ActionResult DealsSection(string type, string L1)
        {
            int takecount = (L1 != null) ? int.Parse(L1) : 4;
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

        public JsonResult getprice(string cartid)
        {
            var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
            var cartlist = cartService.CartItemsList(int.Parse(user.UserId.ToString()));
            var cartdetails = cartlist.Where(m => m.CartId == Convert.ToInt64(cartid)).FirstOrDefault();
            var nofguests = cartdetails.Quantity;
            var pricenew = "";
            var ISdeal = cartdetails.Isdeal;
            var sertype = cartdetails.ServiceType;
            var timeslot = cartdetails.attribute;

            var timeslot1 = timeslot.Split(',');
            var times = timeslot1.Length;

            if (ISdeal == false)
            {
                var dealid1 = cartdetails.DealId;
                if (dealid1 == 0)
                {
                    // var pricenew = cartdetails.TotalPrice;
                    var servicetype = cartdetails.ServiceType;
                    var vendorid = cartdetails.Id;
                    var vendorsubid = cartdetails.subid;
                    VendorProductsService vendorProductsService = new VendorProductsService();
                    var vendorcost = vendorProductsService.Getvendorproducts_Result(servicetype).Where(m => m.id == vendorid && m.subid == vendorsubid.ToString()).FirstOrDefault().cost;
                    //   int query = vendorMasterService.checkemail(emailid);
                    if (sertype == "Venues" || sertype == "Hotel" || sertype == "Resort" || sertype == "Convention Hall" || sertype == "Venue" || sertype == "Banquet Hall" || sertype == "Function Hall" || sertype == "Venue" || sertype == "Catering")
                    { pricenew = Convert.ToString( Convert.ToDecimal(vendorcost) * nofguests);  }
                    else
                    {
                        pricenew = vendorcost;
                    }

                }
            else
            {
                    var deal1 = cartdetails.DealId;

                    var type = cartdetails.ServiceType;
                var vendorid = cartdetails.Id;
                var vendorsubid = cartdetails.subid;
                var packPrice = vendorProductsService.getvendorpkgs(Convert.ToString(vendorid)).Where(p => p.VendorSubId == (vendorsubid) && p.PackageID == deal1).FirstOrDefault().PackagePrice;
                    if (sertype == "Venues" || sertype == "Hotel" || sertype == "Resort" || sertype == "Convention Hall" || sertype == "Venue" || sertype == "Banquet Hall" || sertype == "Function Hall" || sertype == "Venue" || sertype == "Catering")
                    { pricenew = Convert.ToString(Convert.ToDecimal(packPrice) * nofguests); }
                    else
                    {
                        pricenew = packPrice;
                    }
                }
        }
            else if (ISdeal == true)
            {
                var dealid = cartdetails.DealId;
               
                    DateTime ddate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
                    var data = NdealService.GetdealDetails((dealid));
                    var pric1 = (data.DealPrice);
                    if (data.VendorType == "Venue" || data.VendorType == "Catering")
                    {
                        pricenew = Convert.ToString(pric1 * nofguests);
                    }
                    else
                    {
                        pricenew = Convert.ToString(pric1);
                    }
               
               
            }
                if (pricenew != "0")
            {
                return Json(pricenew, JsonRequestBehavior.AllowGet);
            }
            return Json("exists", JsonRequestBehavior.AllowGet);
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
                    for (int i = 0; i < cartnos1.Count(); i++)
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
                        string etype1 = cartdetails.EventType;
                        string vid = Convert.ToString(cartdetails.subid);
                        DateTime date = Convert.ToDateTime(cartdetails.eventstartdate);
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
                        orderDetail.BookedDate = date;
                        orderDetail.EventType = etype1;
                        orderDetail.DealId = long.Parse(did);
                        orderDetail.ExtraDate1 = cartdetails.c1date;
                        orderDetail.ExtraDate2 = cartdetails.c2date;
                        orderDetail.ExtraDate3 = cartdetails.c3date;
                  
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



        public ActionResult movetocart(long cartId)
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
                cartItem.Status = "Active";
                string mesaage = cartService.Updatecartitem(cartItem);
             return   RedirectToAction("Index", "Ncartview");
            }
            return RedirectToAction("Index", "Ncartview");
        }


        public ActionResult multipledateinsert( string cartId,string price, string date, string total)
        {
            if (date == "")
            { return Json("datenotsaved"); }
            else
            {
                var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
                if (user.UserType == "User")
                {
                    var userdata = userLoginDetailsService.GetUser((int)user.UserId);
                    ViewBag.cartCount = cartService.CartItemsCount((int)user.UserId);
                    var cartlist = cartService.CartItemsList(int.Parse(user.UserId.ToString()));
                    var cartdetails = cartlist.Where(m => m.CartId == Convert.ToInt64(cartId)).FirstOrDefault();
                    var totalprice = cartdetails.TotalPrice;
                    var totalprice1 = totalprice + decimal.Parse(price);
                    string updateddate = DateTime.UtcNow.ToShortDateString();
                    CartItem cartItem = new CartItem();
                    var price2 = date + ',' + price;
                    if (cartdetails.c1date == null)
                    {
                        cartItem.ExtraDate1 = price2;
                        cartItem.ExtraDate2 = cartdetails.c2date;
                        cartItem.ExtraDate3 = cartdetails.c3date;
                    }
                    else if (cartdetails.c2date == null)
                    {
                        cartItem.ExtraDate1 = cartdetails.c1date;
                        cartItem.ExtraDate3 = cartdetails.c3date;

                        cartItem.ExtraDate2 = price2;
                    }
                    else if (cartdetails.c3date == null)
                    {
                        cartItem.ExtraDate1 = cartdetails.c1date;
                        cartItem.ExtraDate2 = cartdetails.c2date;
                        cartItem.ExtraDate3 = price2;
                    }
                    else if (cartdetails.c3date != null)
                    {
                        return Json("failed");
                    }
                    cartItem.TotalPrice = Convert.ToDecimal(totalprice1);
                    cartItem.UpdatedDate = Convert.ToDateTime(updateddate);
                    cartItem.CartId = cartdetails.CartId;
                    string mesaage = cartService.adddatecartitem(cartItem);
                    return Json(mesaage);
                }
            }
                return View();
            
        }

        public ActionResult multipledatedelete(string cartId, string price, string date, string total)
        {
            price = price.Replace(" ", "");
            date = date.Replace(" ", "");
            var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
            if (user.UserType == "User")
            {
                var userdata = userLoginDetailsService.GetUser((int)user.UserId);
                ViewBag.cartCount = cartService.CartItemsCount((int)user.UserId);
                var cartlist = cartService.CartItemsList(int.Parse(user.UserId.ToString()));
                var cartdetails = cartlist.Where(m => m.CartId == Convert.ToInt64(cartId)).FirstOrDefault();
                var totalprice = cartdetails.TotalPrice;
                var totalprice1 = totalprice - decimal.Parse(price);
                string updateddate = DateTime.UtcNow.ToShortDateString();
                CartItem cartItem = new CartItem();
                var price2 = date+','+price;
                if (cartdetails.c1date == price2)
                {
                    cartItem.ExtraDate1 = null;
                    cartItem.ExtraDate3 = cartdetails.c3date;
                    cartItem.ExtraDate2 = cartdetails.c2date;
                }
                else if (cartdetails.c2date == price2)
                {
                    cartItem.ExtraDate1 = cartdetails.c1date;
                    cartItem.ExtraDate3 = cartdetails.c3date;
                    cartItem.ExtraDate2 = null;
                }
                else if (cartdetails.c3date == price2)
                {
                    cartItem.ExtraDate1 = cartdetails.c1date;
                    cartItem.ExtraDate2 = cartdetails.c2date;
                    cartItem.ExtraDate3 = null;
                }
                else if (cartdetails.c3date != null)
                {
                    return Json("failed");
                }
                cartItem.TotalPrice = Convert.ToDecimal(totalprice1);
                cartItem.UpdatedDate = Convert.ToDateTime(updateddate);
                cartItem.CartId = cartdetails.CartId;
                string mesaage = cartService.adddatecartitem(cartItem);
                return Json(mesaage);
            }
            return View();
        }

        public string Capitalise(string str)
        {
            if (String.IsNullOrEmpty(str))
            return String.Empty;
            return Char.ToUpper(str[0]) + str.Substring(1).ToLower();
        }
    }
}