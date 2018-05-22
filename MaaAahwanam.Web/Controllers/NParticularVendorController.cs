using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaaAahwanam.Service;
using MaaAahwanam.Web.Custom;
using MaaAahwanam.Models;
using MaaAahwanam.Utility;
using System.Net;
using System.IO;

namespace MaaAahwanam.Web.Controllers
{
    public class NParticularVendorController : Controller
    {
        ProductInfoService productInfoService = new ProductInfoService();
        VendorMasterService vendorMasterService = new VendorMasterService();
        VendorImageService vendorImageService = new VendorImageService();
        VenorVenueSignUpService venorVenueSignUpService = new VenorVenueSignUpService();
        WhishListService whishListService = new WhishListService();
        VendorProductsService vendorProductsService = new VendorProductsService();
        UserLoginDetailsService userLoginDetailsService = new UserLoginDetailsService();
        OrderService orderService = new OrderService();
        private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");


        //static int count = 0;
        // GET: NParticularVendor
        public ActionResult Index(string type, string id, string vid, string m)
        {
            ViewBag.count = 2;
            if (m == "AWC")
                WhishList(type, id, vid, "add", ""); //count++;
            if (type != null) if (type.Split(',').Count() > 1) type = "Venue";
            if (type == "Conventions" || type == "Resorts" || type == "Hotels")
                type = "Venue";
            if (type == "Mehendi" || type == "Pandit")
                type = "Other";
            //var data = productInfoService.getProductsInfo_Result(int.Parse(id), type, int.Parse(vid));
            var data = vendorMasterService.GetVendor(long.Parse(id)); //
            var allimages = vendorImageService.GetVendorAllImages(long.Parse(id)).Where(a => a.VendorId == long.Parse(vid)).ToList();
            ViewBag.image = (allimages.Count() != 0) ? allimages.FirstOrDefault().ImageName.Replace(" ", "") : null;
            ViewBag.allimages = allimages;
            ViewBag.Productinfo = data;
            ViewBag.type = type;
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
                var list = whishListService.GetWhishList(user.UserId.ToString()).Where(a => a.VendorID == id && a.VendorSubID == vid && a.ServiceType == type); //Checking whishlist availablility
                if (list.Count() > 0)
                { ViewBag.whishlistmsg = 1; ViewBag.whishlistid = list.FirstOrDefault().WhishListID; }
                else
                { ViewBag.whishlistmsg = 0; ViewBag.whishlistid = 0; }

            }
            var Venuerecords = venorVenueSignUpService.GetVendorVenue(long.Parse(id)); //, long.Parse(vid)
            var Cateringrecords = venorVenueSignUpService.GetVendorCatering(long.Parse(id)); //, long.Parse(vid)
            var Decoratorrecords = venorVenueSignUpService.GetVendorDecorator(long.Parse(id)); //, long.Parse(vid)
            var Photographyrecords = venorVenueSignUpService.GetVendorPhotography(long.Parse(id)); //, long.Parse(vid)
            var Otherrecords = venorVenueSignUpService.GetVendorOther(long.Parse(id)); //, long.Parse(vid)

            ViewBag.particularVenue = Venuerecords.Where(c => c.Id == long.Parse(vid)).FirstOrDefault();
            ViewBag.particularCatering = Cateringrecords.Where(c => c.Id == long.Parse(vid)).FirstOrDefault();
            ViewBag.particularDecorator = Decoratorrecords.Where(c => c.Id == long.Parse(vid)).FirstOrDefault();
            ViewBag.particularPhotography = Photographyrecords.Where(c => c.Id == long.Parse(vid)).FirstOrDefault();
            ViewBag.particularOther = Otherrecords.Where(c => c.Id == long.Parse(vid)).FirstOrDefault();

            string price = "";
            if (type == "Venues" || type == "Hotel" || type == "Resort" || type == "Convention Hall" || type == "Venue" || type == "Banquet Hall" || type == "Function Hall")
            {
                if (ViewBag.particularVenue != null) { price = ViewBag.particularVenue.ServiceCost.ToString(); };
                ViewBag.location = ViewBag.particularVenue;
            }
            else if (type == "Catering")
            {
                if (ViewBag.particularCatering != null) { price = ViewBag.particularCatering.Veg.ToString(); };
                ViewBag.location = ViewBag.particularCatering;
            }
            else if (type == "Photography")
            {
                if (ViewBag.particularPhotography != null) { price = ViewBag.particularPhotography.StartingPrice.ToString(); };
                ViewBag.location = ViewBag.particularPhotography;
            }
            else if (type == "Decorator")
            {
                if (ViewBag.particularDecorator != null) { price = ViewBag.particularDecorator.StartingPrice.ToString(); };
                ViewBag.location = ViewBag.particularDecorator;
            }
            else if (type == "Mehendi" || type == "Pandit" || type == "Other")
            {
                if (ViewBag.particularOther != null) { price = ViewBag.particularOther.ItemCost.ToString(); };
                ViewBag.location = ViewBag.particularOther;
            }

            if (price == "")
                price = "0";
            ViewBag.Venue = Venuerecords;
            ViewBag.Catering = Cateringrecords;
            ViewBag.Decorator = Decoratorrecords;
            ViewBag.Photography = Photographyrecords;
            ViewBag.Other = Otherrecords;
            ViewBag.servicetypeprice = price;

            //Loading Vendor deals
            //if (type.Split(',').Count() > 1) type = type.Split(',')[0];
            if (type == "Venues" || type == "Banquet Hall" || type == "Function Hall") type = "Venue";
            DateTime date = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
            ViewBag.availabledeals = vendorProductsService.getpartvendordeal(id, type,date);
            ViewBag.availablepackages = vendorProductsService.getvendorpkgs(id).Where(p => p.VendorSubId == long.Parse(vid)).ToList();
            return View();
        }

        public ActionResult WhishList(string type, string id, string vid, string command, string wid)
        {
            //if (count <= 1) // if count > 1 new whishlist will not be created
            //{
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                if (command == "remove")
                {
                    string msg = whishListService.RemoveWhishList(int.Parse(wid));
                    return Json("removed", JsonRequestBehavior.AllowGet);
                    //return Content("<script language='javascript' type='text/javascript'>alert('Removed From Whishlist');location.href='/NParticularVendor/Index?id=" + id + "&&vid=" + vid + "&&type=" + type + "'</script>");
                }
                var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
                AvailableWhishLists availableWhishLists = new AvailableWhishLists();
                availableWhishLists.VendorID = id;
                availableWhishLists.VendorSubID = vid;
                availableWhishLists.BusinessName = vendorMasterService.GetVendor(long.Parse(id)).BusinessName;
                availableWhishLists.ServiceType = type;
                availableWhishLists.UserID = user.UserId.ToString();
                availableWhishLists.IPAddress = HttpContext.Request.UserHostAddress;
                var list = whishListService.GetWhishList(user.UserId.ToString()).Where(m => m.VendorID == id && m.VendorSubID == vid && m.ServiceType == type).Count(); //Checking whishlist availablility
                if (list == 0)
                    availableWhishLists = whishListService.AddWhishList(availableWhishLists);
                return Json("Success", JsonRequestBehavior.AllowGet);
            }
            //}
            return Json(JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult DealsSection(string type, string L1)
        {
            int takecount = (L1 != null) ? int.Parse(L1) : 2;
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
        public string Capitalise(string str)
        {
            if (String.IsNullOrEmpty(str))
                return String.Empty;
            return Char.ToUpper(str[0]) + str.Substring(1).ToLower();
        }
        public ActionResult BookNow(string type, string eventtype, string timeslot, string date, string id, string vid, string price, string guest)
        {
            
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
                var orders = orderService.userOrderList().Where(m => m.UserLoginId == (int)user.UserId);
                var orderscount = orders.Where(m => m.Id == long.Parse(id)).Count();
                var serviceselected = orders.Where(m => m.ServicType.Split(',').Contains(type)).Count();
                if (orderscount > 0 && serviceselected > 0)
                {
                    return Json("Exists", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    string updateddate = DateTime.UtcNow.ToShortDateString();
                    int userid = Convert.ToInt32(user.UserId);
                    decimal totalprice = 0;
                    if (type == "Catering")
                        totalprice = int.Parse(guest) * decimal.Parse(price);
                    else
                        totalprice = Convert.ToDecimal(price);
                    //Saving Record in order Table
                    //OrderService orderService = new OrderService();
                    Order order = new Order();
                    order.TotalPrice = totalprice;//Convert.ToDecimal(price);
                    order.OrderDate = Convert.ToDateTime(updateddate); //Convert.ToDateTime(bookeddate);
                    order.UpdatedBy = (Int64)user.UserId;
                    order.OrderedBy = (Int64)user.UserId;
                    order.UpdatedDate = Convert.ToDateTime(updateddate);
                    order.Status = "Pending";
                    order = orderService.SaveOrder(order);

                    //Saving Order Details
                    OrderdetailsServices orderdetailsServices = new OrderdetailsServices();
                    OrderDetail orderDetail = new OrderDetail();
                    orderDetail.OrderId = order.OrderId;
                    orderDetail.OrderBy = user.UserId;
                    orderDetail.PaymentId = 0;
                    orderDetail.ServiceType = type;
                    orderDetail.ServicePrice = decimal.Parse(price);
                    orderDetail.attribute = timeslot;
                    orderDetail.TotalPrice = totalprice;
                    orderDetail.PerunitPrice = decimal.Parse(price);
                    orderDetail.Quantity = int.Parse(guest);
                    orderDetail.OrderId = order.OrderId;
                    orderDetail.VendorId = long.Parse(id);
                    orderDetail.Status = "Pending";
                    orderDetail.UpdatedDate = Convert.ToDateTime(updateddate);
                    orderDetail.UpdatedBy = user.UserId;
                    orderDetail.subid = long.Parse(vid);
                    orderDetail.BookedDate = Convert.ToDateTime(date);
                    orderDetail.EventType = eventtype;
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
                    var vendordetails = userLoginDetailsService.getvendor(Convert.ToInt16(id));


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


                    return Json("Success", JsonRequestBehavior.AllowGet);
                }
                
            }
            return Json(JsonRequestBehavior.AllowGet);
        }
    }
}