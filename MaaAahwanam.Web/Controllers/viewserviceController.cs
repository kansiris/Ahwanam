﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaaAahwanam.Service;
using System.Web.Routing;
using MaaAahwanam.Web.Custom;
using MaaAahwanam.Models;
using MaaAahwanam.Utility;
using System.Net;
using System.IO;
using MaaAahwanam.Repository;
using System.Text;

namespace MaaAahwanam.Web.Controllers
{
    public class viewserviceController : Controller
    {
        VendorProductsService vendorProductsService = new VendorProductsService();
        UserLoginDetailsService userLoginDetailsService = new UserLoginDetailsService();
        CartService cartService = new CartService();
        private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        viewservicesservice viewservicesss = new viewservicesservice();
        ResultsPageService resultsPageService = new ResultsPageService();
        VendorMasterService vendorMasterService = new VendorMasterService();
        VendorDatesService vendorDatesService = new VendorDatesService();
        ProductInfoService productInfoService = new ProductInfoService();



        // GET: viewservice
        public ActionResult Index(string name, string type, string id)
        {
            type = (type == null) ? "Venue" : type;
            type = (type == "Convention") ? "Convention Hall" : type;
            type = (type == "Banquet") ? "Banquet Hall" : type;
            type = (type == "Function") ? "Function Hall" : type;

            if (name != null)
            {
                var ks = resultsPageService.GetAllVendors(type).Where(m => m.BusinessName.ToLower().Contains(name.ToLower().TrimEnd())).FirstOrDefault();
                id = ks.Id.ToString();
            }
            var venues = viewservicesss.GetVendorVenue(long.Parse(id)).ToList();
            if (type == "Venue" || type == "Convention" || type == "Banquet" || type == "Function")
            {
                var data = viewservicesss.GetVendor(long.Parse(id));
                var allimages = viewservicesss.GetVendorAllImages(long.Parse(id)).ToList();
                ViewBag.image = (allimages.Count() != 0) ? allimages.FirstOrDefault().ImageName.Replace(" ", "") : null;
                //ViewBag.allimages = allimages;
                ViewBag.Productinfo = data;
                ViewBag.location = data.Address;
                ViewBag.City = data.City;
                ViewBag.State = data.State;
                //if (data.GeoLocation.Split(',').Count() > 1)
                //{
                //    ViewBag.latitude = (data.GeoLocation != null && data.GeoLocation != "") ? data.GeoLocation.Split(',')[0] : "17.385044";
                //    ViewBag.longitude = (data.GeoLocation != null && data.GeoLocation != "") ? data.GeoLocation.Split(',')[1] : "78.486671";
                //}
                //else
                //{
                    ViewBag.latitude = "17.385044";
                    ViewBag.longitude = "78.486671";
                //}
                

                //var data = productInfoService.getProductsInfo_Result(int.Parse(id), type, int.Parse(vid));
                ViewBag.data = data;
                List<VendorVenue> vendor = venues;
                List<SPGETNpkg_Result> package = new List<SPGETNpkg_Result>();
                List<VendorVenue> amenities = new List<VendorVenue>();
                List<string> famenities = new List<string>();
                List<VendorImage> vimg = new List<VendorImage>();
                List<Policy> policy = new List<Policy>();
                    foreach (var item in venues)
                {
                    package.AddRange(viewservicesss.getvendorpkgs(id).Where(p => p.VendorSubId == long.Parse(item.Id.ToString())).ToList());
                    amenities.Add(item);
                    vimg.AddRange(allimages.Where(m => m.VendorId == long.Parse(id)));
                  var p1=  vendorMasterService.Getpolicy(id, item.Id.ToString());
                    if(p1 != null)
                    policy.Add(p1);
                   
                }
                ViewBag.allimages = allimages;
                ViewBag.particularVenue = vendor;
                ViewBag.availablepackages = package;
                var allamenities = amenities.Select(m => new
                {
                    #region Venue amenities
                    m.AC,
                    m.TV,
                    m.Complimentary_Breakfast,
                    m.Geyser,
                    m.Parking_Facility,
                    m.Card_Payment,
                    m.Lift_or_Elevator,
                    m.Banquet_Hall,
                    m.Laundry,
                    m.CCTV_Cameras,
                    m.Swimming_Pool,
                    m.Conference_Room,
                    m.Bar,
                    m.Dining_Area,
                    m.Power_Backup,
                    m.Wheelchair_Accessible,
                    m.Room_Heater,
                    m.In_Room_Safe,
                    m.Mini_Fridge,
                    m.In_house_Restaurant,
                    m.Gym,
                    m.Hair_Dryer,
                    m.Pet_Friendly,
                    m.HDTV,
                    m.Spa,
                    m.Wellness_Center,
                    m.Electricity,
                    m.Bath_Tub,
                    m.Kitchen,
                    m.Netflix,
                    m.Kindle,
                    m.Coffee_Tea_Maker,
                    m.Sofa_Set,
                    m.Jacuzzi,
                    m.Full_Length_Mirrror,
                    m.Balcony,
                    m.King_Bed,
                    m.Queen_Bed,
                    m.Single_Bed,
                    m.Intercom,
                    m.Sufficient_Room_Size,
                    m.Sufficient_Washroom
                    #endregion
                }).ToList();
                ViewBag.policy = policy;
                foreach (var item in allamenities)
                {
                    string value = string.Join(",", item).Replace("{", "").Replace("}", "");
                    var availableamenities = value.Split(',');
                    value = "";
                    for (int i = 0; i < availableamenities.Length; i++)
                    {
                        if (availableamenities[i].Split('=')[1].Trim() == "Yes")
                            value = value + "," + availableamenities[i].Split('=')[0].Trim();
                    }
                    famenities.Add(value.TrimStart(','));
                }
                ViewBag.amenities = famenities;
            }
            return View();
        }


        public JsonResult Availabledates( string pid,string type)
        {
            var msg = "";
            if (pid != "undefined")
            {
                type = (type == null) ? "Venue" : type;
                type = (type == "Convention") ? "Convention Hall" : type;
                type = (type == "Banquet") ? "Banquet Hall" : type;
                type = (type == "Function") ? "Function Hall" : type;
                var pkgs = vendorProductsService.getpartpkgs(pid).FirstOrDefault(); ;
                Int64 vid, vsid;
                vid = pkgs.VendorId;
                vsid = pkgs.VendorSubId;

                string orderdates = productInfoService.disabledate(vid, vsid, type).Replace('/', '-');

                if (vid != 0)
                {
                    var betweendates = new List<string>();
                    var Gettotaldates = vendorDatesService.GetDates(vid, vsid);
                    int recordcount = Gettotaldates.Count();
                    foreach (var item1 in Gettotaldates)
                    {
                        var startdate = Convert.ToDateTime(item1.StartDate);
                        var enddate = Convert.ToDateTime(item1.EndDate);
                        if (startdate != enddate)
                        {
                            for (var dt = startdate; dt <= enddate; dt = dt.AddDays(1))
                            {
                                betweendates.Add(dt.ToString("yyyy-MM-dd"));
                            }
                        }
                        else
                        {
                            betweendates.Add(startdate.ToString("yyyy-MM-dd"));
                        }
                    }
                    //ViewBag.vendoravailabledates = String.Join(",", betweendates,orderdates);
                    var vendoravailabledates = String.Join(",", betweendates);
                    if (orderdates != "")
                        vendoravailabledates = vendoravailabledates + "," + String.Join(",", orderdates.TrimStart(','));
                    string availdate = vendoravailabledates;
                    msg = availdate;
                }

            }
            return Json(msg);
    }

    public ActionResult addcnow(string pid, string guest, string dcval, string total, string pprice,string eventtype ,string bookdate)//(string totalprice, string id, string price,  string timeslot, )
        {
            //try
            //{
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                //if (type == "Photography" || type == "Decorator" || type == "Other")
                //{
                //    totalprice = price;
                //    guest = "0";
                //}
                var pkgs = vendorProductsService.getpartpkgs(pid).FirstOrDefault();
                var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
                List<GetCartItems_Result> cartlist = cartService.CartItemsList(int.Parse(user.UserId.ToString()));

                var vendor = vendorProductsService.getparticulardeal(Int32.Parse(pid), pkgs.VendorType).FirstOrDefault();
                DateTime updateddate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
                CartItem cartItem = new CartItem();
                cartItem.VendorId = (pkgs.VendorId);
                cartItem.ServiceType = pkgs.VendorType;
                cartItem.TotalPrice = decimal.Parse(total);
                cartItem.Orderedby = user.UserId;
                cartItem.UpdatedDate = Convert.ToDateTime(updateddate);
                cartItem.Category = "package";
                cartItem.SelectedPriceType = pkgs.PackageName;

                cartItem.Perunitprice = decimal.Parse(pprice);
                cartItem.Quantity = Convert.ToInt16(guest);
                cartItem.subid = Convert.ToInt64(pkgs.VendorSubId);
                //cartItem.attribute = timeslot;
                cartItem.DealId = Convert.ToInt64(pid);
                cartItem.Isdeal = false;
                cartItem.EventType = eventtype;
                cartItem.EventDate = Convert.ToDateTime(bookdate);
                long vendorid = Convert.ToInt64(pkgs.VendorId);
                var cartcount = cartlist.Where(m => m.Id == long.Parse((pkgs.VendorId).ToString()) && m.subid == long.Parse((pkgs.VendorSubId).ToString())).Count();
                if (cartcount > 0)
                    return Json("Exists", JsonRequestBehavior.AllowGet);
                else
                {
                    cartItem = cartService.AddCartItem(cartItem);
                    if (cartItem.CartId != 0)
                        return Json("Success", JsonRequestBehavior.AllowGet);
                    else
                        return Json("failed", JsonRequestBehavior.AllowGet);
                }

                //return Json("Success", JsonRequestBehavior.AllowGet);
            }
            return Json(JsonRequestBehavior.AllowGet);
        }
        //catch (Exception)
        //{
        //    return RedirectToAction("Index", "Nhomepage");
        //}
        //}
        //public JsonResult calender(string calender)
        //{
        //    //DateTime cdate = Convert.ToDateTime(calender);
        //    //int pdays = 3; var previousdays = ""; var nextdays = ""; ;
        //    //for (int i = 1; i < pdays; i++)
        //    //{
        //    //    int previous = pdays - i;
        //    //    previousdays = previousdays + "," + cdate.AddDays(-previous).ToString("dd/MMM/yyyy");
        //    //    nextdays = nextdays + "," + cdate.AddDays(i).ToString("dd/MMM/yyyy");
        //    //}
        //    //var data = previousdays.Trim(',') + "," + cdate.ToString("dd/MMM/yyyy") + "," + nextdays.Trim(',');
        //    //StringBuilder divs = new StringBuilder();
        //    //for (int i = 0; i < data.Split(',').Length; i++)
        //    //{
        //    //    //divs.Append("<div class='internal'><div class='kscalender'><p style='padding-left: 17px;padding-right:10px;font-size: 17px;font-weight: 700;color: #06c147;'>" + data.Split(',')[i].Split('-')[0] + "</p><p style='padding-left:10px'>" + data.Split(',')[i].Split('-')[1]+ "," +data.Split(',')[i].Split('-')[2] + "</p><label style='padding-left:10px'>₹<b id='pprice'>00</b></label></div></div>");
        //    //    divs.Append("<p>gfffdsgf</p>");

        //    //}
        //    return Json(divs);
        //}

        public PartialViewResult calender(string calender,string packageid)
        {
            DateTime cdate; string packprice;
            if (calender == null)
            {
                cdate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
            }
            else {
                DateTime  ctestdate = Convert.ToDateTime(calender);
                string cd1 = ctestdate.ToString("dd-mm-yyyy");
                cdate = Convert.ToDateTime(ctestdate);            }
            if (packageid == null || packageid == "undefined")
            {
                 packprice = "N/A";
            }
            else
            {
                var pkgs = vendorProductsService.getpartpkgs(packageid).FirstOrDefault();
                if (pkgs.PackagePrice != null) //|| pkgs.PackagePrice != "")
                {
                    packprice = pkgs.PackagePrice;
                }
                else { packprice = pkgs.price1; }
            }
            ViewBag.ppakprice = packprice;
            DateTime cdate1 = cdate.AddDays(-1);
            DateTime cdate2 = cdate;
            DateTime cdate3 = cdate.AddDays(1);
            DateTime cdate4 = cdate.AddDays(2);
            DateTime cdate5 = cdate.AddDays(3);
            DateTime cdate6 = cdate.AddDays(4);
            DateTime cdate7 = cdate.AddDays(5);
            DateTime cdate8 = cdate.AddDays(6);
            ViewBag.cdate1 = cdate1.ToString("dd/MMM/yyyy").Replace('-', '/');
            ViewBag.cdate2 = cdate2.ToString("dd/MMM/yyyy").Replace('-', '/');
            ViewBag.cdate3 = cdate3.ToString("dd/MMM/yyyy").Replace('-', '/');
            ViewBag.cdate4 = cdate4.ToString("dd/MMM/yyyy").Replace('-', '/');
            ViewBag.cdate5 = cdate5.ToString("dd/MMM/yyyy").Replace('-', '/');
            ViewBag.cdate6 = cdate6.ToString("dd/MMM/yyyy").Replace('-', '/');
            ViewBag.cdate7 = cdate7.ToString("dd/MMM/yyyy").Replace('-', '/');
            ViewBag.cdate8 = cdate8.ToString("dd/MMM/yyyy").Replace('-', '/');
            return PartialView("calender");
        }
    }
}