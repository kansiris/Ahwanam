using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaaAahwanam.Service;
using MaaAahwanam.Web.Custom;
using MaaAahwanam.Models;

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
            if (type == "Venues" || type == "Hotel" || type == "Resort" || type == "Convention Hall" || type == "Venue")
            {
                if (ViewBag.particularVenue != null) { price = ViewBag.particularVenue.ServiceCost.ToString(); };
            }
            else if (type == "Catering")
            {
                if (ViewBag.particularCatering != null) { price = ViewBag.particularCatering.Veg.ToString(); };
            }
            else if (type == "Photography")
            {
                if (ViewBag.particularPhotography != null) { price = ViewBag.particularPhotography.StartingPrice.ToString(); };
            }
            else if (type == "Decorator")
            {
                if (ViewBag.particularDecorator != null) { price = ViewBag.particularDecorator.StartingPrice.ToString(); };
            }
            else if (type == "Mehendi" || type == "Pandit")
            {
                if (ViewBag.particularOther != null) { price = ViewBag.particularOther.ItemCost.ToString(); };
            }

            ViewBag.Venue = Venuerecords;
            ViewBag.Catering = Cateringrecords;
            ViewBag.Decorator = Decoratorrecords;
            ViewBag.Photography = Photographyrecords;
            ViewBag.Other = Otherrecords;
            ViewBag.servicetypeprice = price;

            //Loading Vendor deals
            //if (type.Split(',').Count() > 1) type = type.Split(',')[0];
            if (type == "Venues") type = "Venue";
            
            ViewBag.availabledeals = vendorProductsService.getpartvendordeal(id, type);
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
            ViewBag.records = vendorProductsService.Getvendorproducts_Result("Venue").Take(4);
            //var deals = vendorProductsService.getalldeal().OrderBy(m => m.DealID).Where(m => m.VendorType == type);
            var records = vendorProductsService.Getvendorproducts_Result("Venue");
            ViewBag.deal = records.Take(takecount);
            int count = records.Count();
            if (type != null) if (type.Split(',').Count() > 1) type = "Venue";
            if (type == "Conventions" || type == "Resorts" || type == "Hotels")
                type = "Venue";
            if (type == "Mehendi" || type == "Pandit")
                type = "Other";
            ViewBag.type = type;
            ViewBag.count = (count >= takecount) ? "1" : "0";
            return PartialView();
        }

        public ActionResult BookNow(string type, string eventtype, string timeslot, string date, string id, string vid, string price, string guest)
        {
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
                string updateddate = DateTime.UtcNow.ToShortDateString();

                //Saving Record in order Table
                OrderService orderService = new OrderService();
                Order order = new Order();
                order.TotalPrice = Convert.ToDecimal(price);
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
                orderDetail.TotalPrice = decimal.Parse(price);
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
                return Json("Success", JsonRequestBehavior.AllowGet);
            }
            return Json(JsonRequestBehavior.AllowGet);
        }
    }
}