using System;
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
        VendorProductsService vendorProductsService = new VendorProductsService();
        UserLoginDetailsService userLoginDetailsService = new UserLoginDetailsService();
        OrderService orderService = new OrderService();
        VendorDatesService vendorDatesService = new VendorDatesService();
        CartService cartService = new CartService();
        private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        ResultsPageService resultsPageService = new ResultsPageService();

         string vid;
        // GET: viewservice
        public ActionResult Index(string name, string type)

        {
            try
            {
                type = (type == null) ? "Venue" : type;
                type = (type == "Convention") ? "Convention Hall" : type;
                type = (type == "Banquet") ? "Banquet Hall" : type;
                type = (type == "Function") ? "Function Hall" : type;
               
             // var ks1 = resultsPageService.GetAllVendors(type).ToList().Where(x=>x.BusinessName == businessname).FirstOrDefault();

                var ks = resultsPageService.GetAllVendors(type).Where(m => m.BusinessName.ToLower().Contains(name.ToLower().TrimEnd())).FirstOrDefault();
                string id =  ks.Id.ToString();
                vid = ks.subid.ToString();
                var data = viewservicesss.GetVendor(long.Parse(id)); //
                var allimages = viewservicesss.GetVendorAllImages(long.Parse(id)).ToList();
                ViewBag.image = (allimages.Count() != 0) ? allimages.FirstOrDefault().ImageName.Replace(" ", "") : null;
                ViewBag.allimages = allimages;
                ViewBag.Productinfo = data;
                ViewBag.latitude = (data.GeoLocation != null && data.GeoLocation != "") ? data.GeoLocation.Split(',')[0] : "17.385044";
                ViewBag.longitude = (data.GeoLocation != null && data.GeoLocation != "") ? data.GeoLocation.Split(',')[1] : "78.486671";

                List<VendorVenue> loadrecords = new List<VendorVenue>();
                List<VendorVenue> final = new List<VendorVenue>();
                var availableamenities = loadrecords.Select(m => new { m.AC, m.TV,m.ComplimentaryBreakfast,m.Geyser,m.ParkingFacility,m.CardPayment,m.LiftorElevator, m.BanquetHall,
                    m.Laundry, m.CCTVCameras,m.SwimmingPool,m.ConferenceRoom,m.Bar, m.DiningArea, m.PowerBackup, m.WheelchairAccessible, m.RoomHeater, m.InRoomSafe,
                    m.MiniFridge, m.InhouseRestaurant, m.Gym, m.HairDryer, m.PetFriendly,m.HDTV,m.Spa, m.WellnessCenter,m.Electricity, m.BathTub, m.Kitchen, m.Netflix,
                    m.Kindle,m.CoffeeTeaMaker, m.SofaSet,m.Jacuzzi,m.FullLengthMirrror,m.Balcony, m.KingBed,m.QueenBed,m.SingleBed,m.Intercom, m.SufficientRoomSize,
                    m.SufficientWashroom
                });
                final.AddRange(loadrecords);


                //var data = productInfoService.getProductsInfo_Result(int.Parse(id), type, int.Parse(vid));


                if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
                 ////   var list = whishListService.GetWhishList(user.UserId.ToString()).Where(a => a.VendorID == id && a.VendorSubID == vid && a.ServiceType == type); //Checking whishlist availablility
                 //   if (list.Count() > 0)
                 //   {
                 //       ViewBag.whishlistmsg = 1; ViewBag.whishlistid = list.FirstOrDefault().WhishListID;
                 //   }
                 //   else
                 //   {
                 //       ViewBag.whishlistmsg = 0; ViewBag.whishlistid = 0;
                 //   }

                }


                var Venuerecords = viewservicesss.GetVendorVenue(long.Parse(id)); //, long.Parse(vid)
                var Cateringrecords = viewservicesss.GetVendorCatering(long.Parse(id)); //, long.Parse(vid)
                var Decoratorrecords = viewservicesss.GetVendorDecorator(long.Parse(id)); //, long.Parse(vid)
                var Photographyrecords = viewservicesss.GetVendorPhotography(long.Parse(id)); //, long.Parse(vid)
                var Otherrecords = viewservicesss.GetVendorOther(long.Parse(id)); //, long.Parse(vid)

                ViewBag.particularVenue = Venuerecords.Where(c => c.Id == long.Parse(vid)).FirstOrDefault();
                //ViewBag.particularVenue1 = Venuerecords.Where(c => c.Id == long.Parse(vid)).Select.FirstOrDefault();


                ViewBag.particularCatering = Cateringrecords.Where(c => c.Id == long.Parse(vid)).FirstOrDefault();
                ViewBag.particularDecorator = Decoratorrecords.Where(c => c.Id == long.Parse(vid)).FirstOrDefault();
                ViewBag.particularPhotography = Photographyrecords.Where(c => c.Id == long.Parse(vid)).FirstOrDefault();
                ViewBag.particularOther = Otherrecords.Where(c => c.Id == long.Parse(vid)).FirstOrDefault();

                string price = "";

                if (ViewBag.location == null)
                {
                    ViewBag.location = data;
                }

                if (price == "")
                    price = "0";
                ViewBag.Venue = Venuerecords;
                ViewBag.Catering = Cateringrecords;
                ViewBag.Decorator = Decoratorrecords;
                ViewBag.Photography = Photographyrecords;
                ViewBag.Other = Otherrecords;
                ViewBag.servicetypeprice = price;

                int count = Venuerecords.Where(k => k.Id != long.Parse(vid)).Count() + Cateringrecords.Where(k => k.Id != long.Parse(vid)).Count() + Decoratorrecords.Where(k => k.Id != long.Parse(vid)).Count() + Photographyrecords.Where(k => k.Id != long.Parse(vid)).Count() + Otherrecords.Where(k => k.Id != long.Parse(vid)).Count();
                if (count == 0)
                    ViewBag.msg = "No Extra Services Available";
                else
                    ViewBag.msg = "0";

                //Loading Vendor deals

                ViewBag.availablepackages = viewservicesss.getvendorpkgs(id).Where(p => p.VendorSubId == long.Parse(vid)).ToList();




                //if (type.Split(',').Count() > 1) type = type.Split(',')[0];
                //if (type == "Venues" || type == "Banquet Hall" || type == "Function Hall") type = "Venue";
                //DateTime date = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
                //ViewBag.availabledeals = vendorProductsService.getpartvendordeal(id, type, date);
                //var orderdates = orderService.userOrderList().Where(k => k.Id == long.Parse(id) && k.Status == "Active").Select(k => k.OrderDate.Value.ToString("dd-MM-yyyy")).ToList();

                string orderdates = viewservicesss.disabledate(long.Parse(id), long.Parse(vid), type).Replace('/', '-');

                //Blocking Dates
              //  var vendorid = userLoginDetailsService.GetLoginDetailsByEmail(Productinfo.EmailId);
                if (int.Parse(id) != 0)
                {
                    var betweendates = new List<string>();
                    var Gettotaldates = viewservicesss.GetDates(long.Parse(id), long.Parse(vid));
                    int recordcount = Gettotaldates.Count();
                    foreach (var item in Gettotaldates)
                    {
                        var startdate = Convert.ToDateTime(item.StartDate);
                        var enddate = Convert.ToDateTime(item.EndDate);
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
                    ViewBag.vendoravailabledates = String.Join(",", betweendates,orderdates);
                    var vendoravailabledates = String.Join(",", betweendates);
                    if (orderdates != "")
                        vendoravailabledates = vendoravailabledates + "," + String.Join(",", orderdates.TrimStart(','));
                    ViewBag.vendoravailabledates = vendoravailabledates;
                    var today = DateTime.UtcNow;
                    var first = new DateTime(today.Year, today.Month, 1);

                    var vendordates = viewservicesss.GetCurrentMonthDates(long.Parse(id)).Select(n => n.StartDate.ToShortDateString()).ToArray();
                    var bookeddates = viewservicesss.GetCount(long.Parse(id), long.Parse(vid), type).Where(k => k.BookedDate > first).Select(l => l.BookedDate.Value.ToShortDateString()).Distinct().ToArray();

                    //var bookeddates = productInfoService.disabledate(vid, Svid, type).Split(',');
                    //var finalbookeddates = bookeddates.Except(vendordates).ToList();
                    //var finalvendordates = vendordates.Except(bookeddates).ToList();
                    ////var finalbookeddates1 = bookeddates;
                    ////var finalvendordates1 = vendordates;
                    //if (finalbookeddates.Count() != 0)
                    //    ViewBag.vendoravailabledates = string.Join(",", finalvendordates) + string.Join(",", finalbookeddates);
                    //else
                    //    ViewBag.vendoravailabledates = string.Join(",", finalvendordates);
                }
                return View();

            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Nhomepage");
            }
        }




    } }