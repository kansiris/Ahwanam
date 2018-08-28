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

namespace MaaAahwanam.Web.Controllers
{
    public class viewserviceController : Controller
    {

        viewservicesservice viewservicesss = new viewservicesservice();


        ProductInfoService productInfoService = new ProductInfoService();

       
        WhishListService whishListService = new WhishListService();
       
        UserLoginDetailsService userLoginDetailsService = new UserLoginDetailsService();
        OrderService orderService = new OrderService();
      
        CartService cartService = new CartService();
        private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        ResultsPageService resultsPageService = new ResultsPageService();

         string vid;
        // GET: viewservice
        public ActionResult Index(string name, string type)

        {
            //try
            //{
            
               type = (type == null) ? "Venue" : type;
                type = (type == "Convention") ? "Convention Hall" : type;
                type = (type == "Banquet") ? "Banquet Hall" : type;
                type = (type == "Function") ? "Function Hall" : type;
               

                var ks = resultsPageService.GetAllVendors(type).Where(m => m.BusinessName.ToLower().Contains(name.ToLower().TrimEnd())).FirstOrDefault(); 
                                    string id = ks.Id.ToString();
            var venues = viewservicesss.GetVendorVenue(long.Parse(id)).ToList();
            if (type == "Venue" || type == "Convention" || type == "Banquet" || type == "Function")
            {
                var data = viewservicesss.GetVendor(long.Parse(id)); //
                var allimages = viewservicesss.GetVendorAllImages(long.Parse(id)).ToList();
                ViewBag.image = (allimages.Count() != 0) ? allimages.FirstOrDefault().ImageName.Replace(" ", "") : null;
                ViewBag.allimages = allimages;
                ViewBag.Productinfo = data;
                ViewBag.latitude = (data.GeoLocation != null && data.GeoLocation != "") ? data.GeoLocation.Split(',')[0] : "17.385044";
                ViewBag.longitude = (data.GeoLocation != null && data.GeoLocation != "") ? data.GeoLocation.Split(',')[1] : "78.486671";
                ViewBag.data = data;
                List<VendorVenue> vendor = new List<VendorVenue>();
                List<SPGETNpkg_Result> package = new List<SPGETNpkg_Result>();
                foreach (var item in venues)
                {
                    vid = item.Id.ToString();

                   


                    var Venuerecords = viewservicesss.GetVendorVenue(long.Parse(id)); //, long.Parse(vid)
                    vendor.AddRange(Venuerecords.Where(c => c.Id == long.Parse(vid)).ToList());
                    

                    string price = "";

                    if (ViewBag.location == null)
                    {
                        ViewBag.location = data;
                    }

                    if (price == "")
                        price = "0";
                   
                    ViewBag.servicetypeprice = price;

                    int count = Venuerecords.Where(k => k.Id != long.Parse(vid)).Count() ;
                    if (count == 0)
                        ViewBag.msg = "No Extra Services Available";
                    else
                        ViewBag.msg = "0";

                    //Loading Vendor deals

                  
                      //package = viewservicesss.getvendorpkgs(id).Where(p => p.VendorSubId == long.Parse(vid)).ToList();
                    package.AddRange(viewservicesss.getvendorpkgs(id).Where(p => p.VendorSubId == long.Parse(vid)).ToList());
                    
                }
                ViewBag.particularVenue = vendor;
                ViewBag.availablepackages = package;
                if (type.Split(',').Count() > 1) type = type.Split(',')[0];
                if (type == "Venues" || type == "Banquet Hall" || type == "Function Hall") type = "Venue";
                DateTime date = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
                var orderdates = orderService.userOrderList().Where(k => k.Id == long.Parse(id) && k.Status == "Active").Select(k => k.OrderDate.Value.ToString("dd-MM-yyyy")).ToList();


                //Blocking Dates
                if (int.Parse(id) != 0)
                {
                    var betweendates = new List<string>();
                    var Gettotaldates = viewservicesss.GetDates(long.Parse(id), long.Parse(vid));
                    int recordcount = Gettotaldates.Count();
                    foreach (var item1 in Gettotaldates)
                    {
                        var startdate = Convert.ToDateTime(item1.StartDate);
                        var enddate = Convert.ToDateTime(item1.EndDate);
                        if (startdate != enddate)
                        {
                            for (var dt = startdate; dt <= enddate; dt = dt.AddDays(1))
                            {
                                betweendates.Add(dt.ToString("dd-MM-yyyy"));
                            }
                        }
                        else
                        {
                            betweendates.Add(startdate.ToString("dd-MM-yyyy"));
                        }
                    }
                    ViewBag.vendoravailabledates = String.Join(",", betweendates, orderdates);
                    var vendoravailabledates = String.Join(",", betweendates);
                    
                    ViewBag.vendoravailabledates = vendoravailabledates;
                    var today = DateTime.UtcNow;
                    var first = new DateTime(today.Year, today.Month, 1);

                    var vendordates = viewservicesss.GetCurrentMonthDates(long.Parse(id)).Select(n => n.StartDate.ToShortDateString()).ToArray();
                    var bookeddates = viewservicesss.GetCount(long.Parse(id), long.Parse(vid), type).Where(k => k.BookedDate > first).Select(l => l.BookedDate.Value.ToShortDateString()).Distinct().ToArray();

                   // var bookeddates = productInfoService.disabledate(vid, Svid, type).Split(',');
                    var finalbookeddates = bookeddates.Except(vendordates).ToList();
                    var finalvendordates = vendordates.Except(bookeddates).ToList();
                    var finalbookeddates1 = bookeddates;
                    var finalvendordates1 = vendordates;
                    if (finalbookeddates.Count() != 0)
                        ViewBag.vendoravailabledates = string.Join(",", finalvendordates) + string.Join(",", finalbookeddates);
                    else
                        ViewBag.vendoravailabledates = string.Join(",", finalvendordates);
                }
            }
            else
            {
                var Cateringrecords = viewservicesss.GetVendorCatering(long.Parse(id)); //, long.Parse(vid)
                var Decoratorrecords = viewservicesss.GetVendorDecorator(long.Parse(id)); //, long.Parse(vid)
                var Photographyrecords = viewservicesss.GetVendorPhotography(long.Parse(id)); //, long.Parse(vid)
                var Otherrecords = viewservicesss.GetVendorOther(long.Parse(id)); //, long.Parse(vid)}
                ViewBag.particularCatering = Cateringrecords.Where(c => c.Id == long.Parse(vid)).FirstOrDefault();
                ViewBag.particularDecorator = Decoratorrecords.Where(c => c.Id == long.Parse(vid)).FirstOrDefault();
                ViewBag.particularPhotography = Photographyrecords.Where(c => c.Id == long.Parse(vid)).FirstOrDefault();
                ViewBag.particularOther = Otherrecords.Where(c => c.Id == long.Parse(vid)).FirstOrDefault();
                ViewBag.Catering = Cateringrecords;
                ViewBag.Decorator = Decoratorrecords;
                ViewBag.Photography = Photographyrecords;
                ViewBag.Other = Otherrecords;
                int count = Cateringrecords.Where(k => k.Id != long.Parse(vid)).Count() + Decoratorrecords.Where(k => k.Id != long.Parse(vid)).Count() + Photographyrecords.Where(k => k.Id != long.Parse(vid)).Count() + Otherrecords.Where(k => k.Id != long.Parse(vid)).Count();
                if (count == 0)
                    ViewBag.msg = "No Extra Services Available";
                else
                    ViewBag.msg = "0";

            }
            return View();

            }

            //catch (Exception)
            //{
            //    return RedirectToAction("Index", "Nhomepage");
            //}
       // }




    } }