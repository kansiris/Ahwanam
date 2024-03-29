﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaaAahwanam.Service;
using MaaAahwanam.Models;
using MaaAahwanam.Web.Custom;
using MaaAahwanam.Utility;
using System.IO;
using MaaAahwanam.Repository;

namespace MaaAahwanam.Web.Controllers
{
    public class VDashboardController : Controller
    {

        Vendormaster vendorMaster = new Vendormaster();
        private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        OrderService orderService = new OrderService();
        VendorMasterService vendorMasterService = new VendorMasterService();
        UserLoginDetailsService userLoginDetailsService = new UserLoginDetailsService();
        VenorVenueSignUpService vendorVenueSignUpService = new VenorVenueSignUpService();
        VendorImageService vendorImageService = new VendorImageService();
        VendorProductsService vendorProductsService = new VendorProductsService();
        viewservicesservice viewservicesss = new viewservicesservice();
        ProductInfoService productInfoService = new ProductInfoService();
        VendorDatesService vendorDatesService = new VendorDatesService();
        VendorDashBoardService vendorDashBoardService = new VendorDashBoardService();


        const string imagepath = @"/vendorimages/";

        // GET: VDashboard
        public ActionResult Index(string c, string vsid, string loc, string eventtype, string count, string date)
        {
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                List<VendorImage> allimages = new List<VendorImage>();
                DateTime todatedate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE).Date;//Convert.ToDateTime(DateTime.UtcNow.ToShortDateString());
                DateTime tommarowdate = todatedate.AddDays(1).Date;
                var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
                string id = user.UserId.ToString();
                string email = userLoginDetailsService.Getusername(long.Parse(id));
                vendorMaster = vendorMasterService.GetVendorByEmail(email);
                string vid = vendorMaster.Id.ToString();
                ViewBag.Vendor = vendorMasterService.GetVendor(Convert.ToInt64(vendorMaster.Id));
                var orders = orderService.userOrderList().Where(m => m.vid == Convert.ToInt64(vendorMaster.Id)).ToList();
                var orders1 = orderService.userOrderList1().Where(m => m.vid == Convert.ToInt64(vendorMaster.Id)).ToList();

                //Orders Section
                ViewBag.currentorders = orders.Where(p => p.orderstatus == "Pending").Count();
                ViewBag.ordershistory = orders.Where(m => m.orderstatus != "Removed").Count();
                ViewBag.order = orders.OrderByDescending(m => m.OrderId);
                ViewBag.order1 = orders1.OrderByDescending(m => m.OrderId);
                ViewBag.todaysorder = orders.Where(p => p.BookedDate == todatedate).ToList();
                ViewBag.todaysorder1 = orders1.Where(p => p.BookedDate == todatedate).ToList();
                ViewBag.tommaroworder1 = orders1.Where(p => p.BookedDate == tommarowdate).ToList();
                ViewBag.upcominforder1 = orders1.Where(p => p.BookedDate >= tommarowdate).ToList();
                ViewBag.tommaroworder = orders.Where(p => p.BookedDate == tommarowdate).ToList();
                ViewBag.upcominforder = orders.Where(p => p.BookedDate >= tommarowdate).ToList();
                var venues = vendorVenueSignUpService.GetVendorVenue(long.Parse(vid)).ToList();
                List<VendorVenue> vendor = venues;
                List<SPGETNpkg_Result> package = new List<SPGETNpkg_Result>();
                List<VendorImage> vimg = new List<VendorImage>();
                List<string> availability = new List<string>();
                ViewBag.policy = vendorMasterService.Getpolicy(vid, vsid);

                //packages section
                package = viewservicesss.getvendorpkgs(vid).ToList(); ViewBag.availablepackages = package;
                ViewBag.particularVenue = vendor;
                if (eventtype != null && count != null && date != null)
                    ViewBag.venues = venues.Where(m => m.Minimumseatingcapacity >= int.Parse(count)).ToList();
                else
                    ViewBag.venues = venues;
                Addservices(vsid);
                if (venues.Count > 0)
                    ViewBag.enable = "second";
                else
                    ViewBag.enable = "first";
                if (c != null) ViewBag.enable = c;
                ViewBag.vsid = vsid;
                ViewBag.vendorid = vid;
                if (vsid != null && vsid != "")
                {
                    //Loading all services
                    Amenities(venues.Where(m => m.Id == long.Parse(vsid)).ToList());
                    allimages = vendorImageService.GetImages(long.Parse(vid), long.Parse(vsid));
                    var pkgsks = vendorVenueSignUpService.Getpackages((long.Parse(vid)), long.Parse(vsid)).FirstOrDefault(); //Remove FirstOrDefault() after finalising packages design
                    if (pkgsks != null)
                    {
                        ViewBag.package = pkgsks;
                        if (pkgsks.menu != "" && pkgsks.menu != null)
                        {
                            var pkgitems = pkgsks.menu.Trim(',').Split(',');
                            var pkgmitems = pkgsks.menuitems.Trim(',');
                            List<string> selecteditems = new List<string>();
                            for (int i = 0; i < pkgitems.Count(); i++)
                            {
                                selecteditems.Add(pkgmitems.Split(',')[i].Split('(')[0].Replace('/','_').Trim());
                            }
                            ViewBag.selecteditems = string.Join("," ,selecteditems);
                            ViewBag.pkgitems = pkgitems;
                        }
                        //else if (pkgsks.menuitems != "" && pkgsks.menuitems != null) ViewBag.pkgitems = pkgsks.menuitems.Split(',');
                        //else ViewBag.pkgitems = "Welcome Drinks,Starters,Rice,Bread,Curries,Fry/Dry,Salads,Soups,Deserts,Beverages,Fruits";
                    }
                    else ViewBag.package = new Package();
                    var policy1 = vendorMasterService.Getpolicy(vid, vsid);
                    ViewBag.policy = policy1;
                    var pkgmenuitems = vendorDashBoardService.GetParticularMenu("Veg", vid, vsid).FirstOrDefault();
                    var extramenuitems = "";
                    if (pkgmenuitems != null)
                    {
                        if (pkgmenuitems.Extra_Menu_Items != "" && pkgmenuitems.Extra_Menu_Items != null)
                        {
                            for (int i = 0; i < pkgmenuitems.Extra_Menu_Items.Split(',').Length; i++)
                            {
                                extramenuitems = extramenuitems + "," + pkgmenuitems.Extra_Menu_Items.Split(',')[i].Split('(')[0];
                            }
                        }
                    }
                    else
                    { int status = AddMenuList(vsid, vid); } // Adding Menu Items
                    ViewBag.extramenuitems = extramenuitems.Trim(',');
                }
                else
                    ViewBag.package = new Package();
                ViewBag.ksimages = allimages;
                ViewBag.images = allimages.Take(4).ToList();
                ViewBag.imagescount = (allimages.Count < 4) ? 4 - allimages.Count : 0;
                ViewBag.sliderimages = allimages.Where(m => m.ImageType == "Slider").Take(4).ToList();
                ViewBag.slidercount = (ViewBag.sliderimages.Count < 4) ? 4 - ViewBag.sliderimages.Count : 0;
                ViewBag.subtype = venues.FirstOrDefault().VenueType;

                //Dates Availability Section
                foreach (var item1 in venues)
                {
                    var Gettotaldates = vendorDatesService.GetDates(item1.VendorMasterId, item1.Id);
                    string orderdates = productInfoService.disabledate(item1.VendorMasterId, item1.Id, "Venue").Replace('/', '-');
                    var betweendates = new List<string>();
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
                    var vendoravailabledates = String.Join(",", betweendates);
                    if (orderdates != "")
                        vendoravailabledates = vendoravailabledates + "," + String.Join(",", orderdates.TrimStart(','));
                    availability.Add(vendoravailabledates);
                }
                ViewBag.availability = availability.ToList();
            }
            else
            {
                return RedirectToAction("Index", "home");
            }
            return View();
        }

        public List<string[]> seperatedates(List<packagevendordates_Result> data, string date, string type)
        {
            List<string[]> betweendates = new List<string[]>();
            string dates = "";
            // var  dates1 = date.Split('-');

            //var Gettotaldates = vendorDatesService.GetDates(long.Parse(id), long.Parse(vid));
            int recordcount = data.Count();
            foreach (var item in data)
            {
                var startdate = Convert.ToDateTime(date);
                var enddate = Convert.ToDateTime(date);
                if (startdate != enddate)
                {
                    string orderdates = productInfoService.disabledate(item.masterid, item.subid, type);
                    for (var dt = startdate; dt <= enddate; dt = dt.AddDays(1))
                    {
                        dates = (dates != "") ? dates + "," + dt.ToString("dd-MM-yyyy") : dt.ToString("dd-MM-yyyy");
                    }
                    if (dates.Split(',').Contains(date))
                    {
                        if (orderdates != "")
                            dates = String.Join(",", dates.Split(',').Where(i => !orderdates.Split(',').Any(e => i.Contains(e))));
                        string[] da = { item.masterid.ToString(), item.subid.ToString(), dates, item.BusinessName, item.ServiceType, item.VenueType, item.Type };
                        betweendates.Add(da);
                    }
                    dates = "";
                }
                else
                {
                    if (dates.Contains(date))
                    {
                        string[] da = { item.masterid.ToString(), item.subid.ToString(), startdate.ToString("dd-MM-yyyy"), item.BusinessName, item.ServiceType, item.VenueType, item.Type };
                        betweendates.Add(da);
                    }
                    dates = "";
                }
            }
            return betweendates;
        }

        public JsonResult UploadProfilePic(HttpPostedFileBase helpSectionImages, string email)
        {
            string fileName = string.Empty;
            string filename = string.Empty;
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
                UserLoginDetailsService userLoginDetailsService = new UserLoginDetailsService();
                var userdet = userLoginDetailsService.GetUserId(Convert.ToInt32(user.UserId));
                email = userdet.UserName;

                string path = System.IO.Path.GetExtension(helpSectionImages.FileName);
                filename = email + path;
                fileName = System.IO.Path.Combine(System.Web.HttpContext.Current.Server.MapPath(@"/ProfilePictures/" + filename));
                if (System.IO.File.Exists(fileName) == true)
                    System.IO.File.Delete(fileName);

                helpSectionImages.SaveAs(fileName);
                userLoginDetailsService.ChangeDP(int.Parse(user.UserId.ToString()), filename);
            }
            return Json(filename, JsonRequestBehavior.AllowGet);
        }

        public ActionResult sidebar()
        {
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
                string uid = user.UserId.ToString();

                string email = userLoginDetailsService.Getusername(long.Parse(uid));
                vendorMaster = vendorMasterService.GetVendorByEmail(email);
                string vid = vendorMaster.Id.ToString();
                ViewBag.Vendor = vendorMasterService.GetVendor(Convert.ToInt64(vendorMaster.Id));
                //ViewBag.id = ks;
                var venues = vendorVenueSignUpService.GetVendorVenue(long.Parse(vid)).ToList();
                var catering = vendorVenueSignUpService.GetVendorCatering(long.Parse(vid)).ToList();
                var photography = vendorVenueSignUpService.GetVendorPhotography(long.Parse(vid));
                var decorators = vendorVenueSignUpService.GetVendorDecorator(long.Parse(vid));
                var others = vendorVenueSignUpService.GetVendorOther(long.Parse(vid));
                ViewBag.venues = venues;
                ViewBag.catering = catering;
                ViewBag.photography = photography;
                ViewBag.decorators = decorators;
                ViewBag.others = others;
            }
            else
            {
                //ViewBag.id = ks;
                ViewBag.Vendor = "";
                ViewBag.profilepic = "";
            }
            return PartialView("sidebar");
        }

        public ActionResult profilepic(string ks)
        {
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
                string id = user.UserId.ToString();
                string email = userLoginDetailsService.Getusername(long.Parse(id));
                vendorMaster = vendorMasterService.GetVendorByEmail(email);
                ViewBag.Vendor = vendorMasterService.GetVendor(Convert.ToInt64(vendorMaster.Id));
                ViewBag.profilepic = userLoginDetailsService.GetUser(int.Parse(user.UserId.ToString())).UserImgName;
            }
            else
            {
                ViewBag.id = ks;
                ViewBag.Vendor = "";

                ViewBag.profilepic = "";
            }
            return PartialView("profilepic");
        }

        public ActionResult Addservices(string vsid)
        {
            var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
            string uid = user.UserId.ToString();
            string email = userLoginDetailsService.Getusername(long.Parse(uid));
            vendorMaster = vendorMasterService.GetVendorByEmail(email);
            string vid = vendorMaster.Id.ToString();
            ViewBag.Vendor = vendorMasterService.GetVendor(Convert.ToInt64(vid));
            //var venues = (vendorVenueSignUpService.GetVendorVenue(Convert.ToInt64(vendorMaster.Id))).Where(p => p.Id == Convert.ToInt64(vsid)).ToList();
            //ViewBag.subvenues = venues;
            VendorVenueService vendorVenueService = new VendorVenueService();
            if (vsid == null || vsid == "" || vsid == "undefined")
            {
                ViewBag.ks = "ks"; ViewBag.service = new VendorVenue();
            }
            else
            {
                ViewBag.ks = "ksc";
                ViewBag.service = vendorVenueService.GetVendorVenue(long.Parse(vid), long.Parse(vsid));
                ViewBag.categorytype = ViewBag.service.VenueType;
                //ViewBag.images = vendorImageService.GetImages(long.Parse(vid), long.Parse(vsid));
                var pkgs = vendorProductsService.getvendorpkgs(vid);
                ViewBag.pacakagerecord = pkgs;
            }

            return PartialView("Addservices");
        }

        public JsonResult UpdateAmenities(string selectedamenities, string vsid, string command, string hdname, string Dimentions, string Minimumseatingcapacity, string Maximumcapacity, string Description, string Address, string Landmark, string City, string ZipCode)
        {
            var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
            string uid = user.UserId.ToString();
            string email = userLoginDetailsService.Getusername(long.Parse(uid));
            vendorMaster = vendorMasterService.GetVendorByEmail(email);
            string vid = vendorMaster.Id.ToString();

            Vendormaster vendormaster = new Vendormaster();
            VendorVenue vendorVenue = vendorVenueSignUpService.GetParticularVendorVenue(long.Parse(vid), long.Parse(vsid)); // Retrieving Particular Vendor Record

            if (command == "two")
            {
                //long count = 0;

                vendormaster.ServicType = vendorMaster.ServicType;
                vendormaster.ContactNumber = vendorMaster.ContactNumber;
                vendormaster.EmailId = vendorMaster.EmailId;
                vendormaster.LandlineNumber = vendorMaster.LandlineNumber;
                vendormaster.Description = vendorMaster.Description;
                vendormaster.Url = vendorMaster.Url;
                vendormaster.Address = vendorMaster.Address;
                vendormaster.Landmark = vendorMaster.Landmark;
                vendormaster.City = vendorMaster.City; vendormaster.State = vendorMaster.State; vendormaster.ZipCode = vendorMaster.ZipCode;
                string[] selectedamenitieslist = selectedamenities.Split(',');
                //if (vendormaster.ServicType == "Venue")
                //{
                if (selectedamenitieslist.Contains("CockTails")) vendorVenue.CockTails = "Yes"; else vendorVenue.CockTails = "No";
                if (selectedamenitieslist.Contains("Rooms")) vendorVenue.Rooms = "Yes"; else vendorVenue.Rooms = "No";
                if (selectedamenitieslist.Contains("Wifi")) vendorVenue.Wifi = "Yes"; else vendorVenue.Wifi = "No";
                if (selectedamenitieslist.Contains("Sufficient_Washroom")) vendorVenue.Sufficient_Washroom = "Yes"; else vendorVenue.Sufficient_Washroom = "No";
                if (selectedamenitieslist.Contains("Sufficient_Room_Size")) vendorVenue.Sufficient_Room_Size = "Yes"; else vendorVenue.Sufficient_Room_Size = "No";
                if (selectedamenitieslist.Contains("Intercom")) vendorVenue.Intercom = "Yes"; else vendorVenue.Intercom = "No";
                if (selectedamenitieslist.Contains("Single_Bed")) vendorVenue.Single_Bed = "Yes"; else vendorVenue.Single_Bed = "No";
                if (selectedamenitieslist.Contains("Queen_Bed")) vendorVenue.Queen_Bed = "Yes"; else vendorVenue.Queen_Bed = "No";
                if (selectedamenitieslist.Contains("King_Bed")) vendorVenue.King_Bed = "Yes"; else vendorVenue.King_Bed = "No";
                if (selectedamenitieslist.Contains("Balcony")) vendorVenue.Balcony = "Yes"; else vendorVenue.Balcony = "No";
                if (selectedamenitieslist.Contains("Full_Length_Mirrror")) vendorVenue.Full_Length_Mirrror = "Yes"; else vendorVenue.Full_Length_Mirrror = "No";
                if (selectedamenitieslist.Contains("Jacuzzi")) vendorVenue.Jacuzzi = "Yes"; else vendorVenue.Jacuzzi = "No";
                if (selectedamenitieslist.Contains("Sofa_Set")) vendorVenue.Sofa_Set = "Yes"; else vendorVenue.Sofa_Set = "No";
                if (selectedamenitieslist.Contains("Coffee_Tea_Maker")) vendorVenue.Coffee_Tea_Maker = "Yes"; else vendorVenue.Coffee_Tea_Maker = "No";
                if (selectedamenitieslist.Contains("Kindle")) vendorVenue.Kindle = "Yes"; else vendorVenue.Kindle = "No";
                if (selectedamenitieslist.Contains("Netflix")) vendorVenue.Netflix = "Yes"; else vendorVenue.Netflix = "No";
                if (selectedamenitieslist.Contains("Kitchen")) vendorVenue.Kitchen = "Yes"; else vendorVenue.Kitchen = "No";
                if (selectedamenitieslist.Contains("Bath_Tub")) vendorVenue.Bath_Tub = "Yes"; else vendorVenue.Bath_Tub = "No";
                if (selectedamenitieslist.Contains("Electricity")) vendorVenue.Electricity = "Yes"; else vendorVenue.AC = "No";
                if (selectedamenitieslist.Contains("Wellness_Center")) vendorVenue.Wellness_Center = "Yes"; else vendorVenue.Wellness_Center = "No";
                if (selectedamenitieslist.Contains("Spa")) vendorVenue.Spa = "Yes"; else vendorVenue.Spa = "No";
                if (selectedamenitieslist.Contains("HDTV")) vendorVenue.HDTV = "Yes"; else vendorVenue.HDTV = "No";
                if (selectedamenitieslist.Contains("Pet_Friendly")) vendorVenue.Pet_Friendly = "Yes"; else vendorVenue.Pet_Friendly = "No";
                if (selectedamenitieslist.Contains("Gym")) vendorVenue.Gym = "Yes"; else vendorVenue.Gym = "No";
                if (selectedamenitieslist.Contains("In_house_Restaurant")) vendorVenue.In_house_Restaurant = "Yes"; else vendorVenue.In_house_Restaurant = "No";
                if (selectedamenitieslist.Contains("Hair_Dryer")) vendorVenue.Hair_Dryer = "Yes"; else vendorVenue.Hair_Dryer = "No";
                if (selectedamenitieslist.Contains("Mini_Fridge")) vendorVenue.Mini_Fridge = "Yes"; else vendorVenue.Mini_Fridge = "No";
                if (selectedamenitieslist.Contains("In_Room_Safe")) vendorVenue.In_Room_Safe = "Yes"; else vendorVenue.In_Room_Safe = "No";
                if (selectedamenitieslist.Contains("Room_Heater")) vendorVenue.Room_Heater = "Yes"; else vendorVenue.Room_Heater = "No";
                if (selectedamenitieslist.Contains("Wheelchair_Accessible")) vendorVenue.Wheelchair_Accessible = "Yes"; else vendorVenue.Wheelchair_Accessible = "No";
                if (selectedamenitieslist.Contains("Power_Backup")) vendorVenue.Power_Backup = "Yes"; else vendorVenue.Power_Backup = "No";
                if (selectedamenitieslist.Contains("Dining_Area")) vendorVenue.Dining_Area = "Yes"; else vendorVenue.Dining_Area = "No";
                if (selectedamenitieslist.Contains("Bar")) vendorVenue.Bar = "Yes"; else vendorVenue.Bar = "No";
                if (selectedamenitieslist.Contains("Conference_Room")) vendorVenue.Conference_Room = "Yes"; else vendorVenue.Conference_Room = "No";
                if (selectedamenitieslist.Contains("Swimming_Pool")) vendorVenue.Swimming_Pool = "Yes"; else vendorVenue.AC = "No";
                if (selectedamenitieslist.Contains("CCTV_Cameras")) vendorVenue.CCTV_Cameras = "Yes"; else vendorVenue.CCTV_Cameras = "No";
                if (selectedamenitieslist.Contains("Laundry")) vendorVenue.Laundry = "Yes"; else vendorVenue.Laundry = "No";
                if (selectedamenitieslist.Contains("Banquet_Hall")) vendorVenue.Banquet_Hall = "Yes"; else vendorVenue.Banquet_Hall = "No";
                if (selectedamenitieslist.Contains("Lift_or_Elevator")) vendorVenue.Lift_or_Elevator = "Yes"; else vendorVenue.Lift_or_Elevator = "No";
                if (selectedamenitieslist.Contains("Card_Payment")) vendorVenue.Card_Payment = "Yes"; else vendorVenue.Card_Payment = "No";
                if (selectedamenitieslist.Contains("Parking_Facility")) vendorVenue.Parking_Facility = "Yes"; else vendorVenue.Parking_Facility = "No";
                if (selectedamenitieslist.Contains("Geyser")) vendorVenue.Geyser = "Yes"; else vendorVenue.AC = "No";
                if (selectedamenitieslist.Contains("Complimentary_Breakfast")) vendorVenue.Complimentary_Breakfast = "Yes"; else vendorVenue.Complimentary_Breakfast = "No";
                if (selectedamenitieslist.Contains("TV")) vendorVenue.TV = "Yes"; else vendorVenue.TV = "No";
                if (selectedamenitieslist.Contains("AC")) vendorVenue.AC = "Yes"; else vendorVenue.AC = "No";

                vendorVenue = vendorVenueSignUpService.UpdateVenue(vendorVenue, vendormaster, long.Parse(vid), long.Parse(vsid));
                //if (vendorVenue.Id != 0) count = vendorVenue.Id;
                //}
                return Json("Amenities Updated");
            }
            else if (command == "three")
            {
                vendorVenue.Dimentions = Dimentions;
                vendorVenue.Minimumseatingcapacity = Convert.ToInt16(Minimumseatingcapacity);
                vendorVenue.Maximumcapacity = Convert.ToInt16(Maximumcapacity);
                vendorVenue.name = hdname;
                vendorVenue = vendorVenueSignUpService.UpdateVenue(vendorVenue, vendormaster, long.Parse(vid), long.Parse(vsid));

                return Json("Hall details Updated");

            }
            else if (command == "four")
            {
                vendorVenue.Description = Description;

                vendorVenue.Address = Address;
                vendorVenue.City = City;
                //   vendorVenue.State = venuedata.State;
                vendorVenue.Landmark = Landmark;
                vendorVenue.ZipCode = ZipCode;
                // vendorVenue.GeoLocation = venuedata.GeoLocation;
                vendorVenue = vendorVenueSignUpService.UpdateVenue(vendorVenue, vendormaster, long.Parse(vid), long.Parse(vsid));
                return Json("Address Updated");
            }
            return Json(JsonRequestBehavior.AllowGet);
        }

        public void Amenities(List<VendorVenue> venues)
        {
            List<VendorVenue> amenities = venues;
            List<string> famenities = new List<string>();
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
            foreach (var item in allamenities)
            {
                string value1 = string.Join(",", item).Replace("{", "").Replace("}", "");
                var availableamenities1 = value1.Split(',');
                value1 = "";
                for (int i = 0; i < availableamenities1.Length; i++)
                {
                    if (availableamenities1[i].Split('=')[1].Trim() == "Yes")
                        value1 = value1 + "," + availableamenities1[i].Split('=')[0].Trim();
                }
                famenities.Add(value1.TrimStart(','));
            }
            string combindedString = string.Join(",", famenities.ToArray());
            ViewBag.amenities1 = combindedString;
            string value = string.Join(",", allamenities).Replace("{", "").Replace("}", "");
            var availableamenities = value.Split(',');
            ViewBag.amenities = availableamenities;
        }

        public JsonResult addnewservice(string serviceselection, string subcategory)
        {
            var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
            string uid = user.UserId.ToString();
            string email = userLoginDetailsService.Getusername(long.Parse(uid));
            vendorMaster = vendorMasterService.GetVendorByEmail(email);
            string vid = vendorMaster.Id.ToString();
            Vendormaster vendormaster = new Vendormaster();
            var msg = "";
            if (subcategory != "Select Sub-Category")
            {
                long count = 0;
                count = addnewservice1(serviceselection, subcategory, long.Parse(vid));
                int status = AddMenuList(count.ToString(),vid); // Adding Menu Items
                if (count > 0)
                    msg = "Service Added Successfully";
                else if (serviceselection == "Select Service Type")
                    msg = "Failed To Add Sevice";
                else
                    msg = "Failed To Add Sevice";
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddSubService(string type, string subcategory, string subid)
        {
            var msg = "";
            var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
            string uid = user.UserId.ToString();
            string email = userLoginDetailsService.Getusername(long.Parse(uid));
            vendorMaster = vendorMasterService.GetVendorByEmail(email);
            string vid = vendorMaster.Id.ToString();
            long count = updateservice(type, subcategory, long.Parse(vid), long.Parse(subid));//addnewservice1(type, subcategory, long.Parse(vid));
            if (count > 0)
                msg = "Service Updated Successfully";
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        public long addnewservice1(string category, string subcategory, long id)
        {
            long count = 0;
            if (category == "Venue")
            {
                VendorVenue vendorVenue = new VendorVenue();
                vendorVenue.VendorMasterId = id;
                vendorVenue.VenueType = subcategory;
                vendorVenue = vendorVenueSignUpService.AddVendorVenue(vendorVenue);
                if (vendorVenue.Id != 0) count = vendorVenue.Id;
            }
            else if (category == "Catering")
            {
                VendorsCatering vendorsCatering = new VendorsCatering();
                vendorsCatering.VendorMasterId = id;
                vendorsCatering.CuisineType = subcategory;
                vendorsCatering = vendorVenueSignUpService.AddVendorCatering(vendorsCatering);
                if (vendorsCatering.Id != 0) count = vendorsCatering.Id;
            }
            else if (category == "Photography")
            {
                VendorsPhotography vendorsPhotography = new VendorsPhotography();
                vendorsPhotography.VendorMasterId = id;
                vendorsPhotography.PhotographyType = subcategory;
                vendorsPhotography = vendorVenueSignUpService.AddVendorPhotography(vendorsPhotography);
                if (vendorsPhotography.Id != 0) count = vendorsPhotography.Id;
            }
            else if (category == "Decorator")
            {
                VendorsDecorator vendorsDecorator = new VendorsDecorator();
                vendorsDecorator.VendorMasterId = id;
                vendorsDecorator.DecorationType = subcategory;
                vendorsDecorator = vendorVenueSignUpService.AddVendorDecorator(vendorsDecorator);
                if (vendorsDecorator.Id != 0) count = vendorsDecorator.Id;
            }
            //if (category == "EventManagement")
            //{
            //    VendorsEventOrganiser vendorsEventOrganiser = new VendorsEventOrganiser();
            //    vendorsEventOrganiser.VendorMasterId = id;
            //    vendorsEventOrganiser = vendorVenueSignUpService.AddVendorEventOrganiser(vendorsEventOrganiser);
            //    if (vendorsEventOrganiser.Id != 0) count = vendorsEventOrganiser.Id;
            //}
            else if (category == "Other")
            {
                VendorsOther vendorsOther = new VendorsOther();
                vendorsOther.VendorMasterId = id;
                vendorsOther.MinOrder = "0";
                vendorsOther.MaxOrder = "0";
                vendorsOther.Status = "InActive";
                vendorsOther.UpdatedBy = 2;
                vendorsOther.UpdatedDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);//Convert.ToDateTime(DateTime.UtcNow.ToShortDateString());
                vendorsOther.type = subcategory;
                vendorsOther = vendorVenueSignUpService.AddVendorOther(vendorsOther);
                if (vendorsOther.Id != 0) count = vendorsOther.Id;
            }
            return count;
        }

        public long updateservice(string category, string subcategory, long id, long vid)
        {
            Vendormaster vendormaster = new Vendormaster();
            long count = 0;
            if (category == "Venue")
            {
                VendorVenue vendorVenue = vendorVenueSignUpService.GetParticularVendorVenue(id, vid);
                vendorVenue.VendorMasterId = id;
                vendorVenue.VenueType = subcategory;
                vendorVenue = vendorVenueSignUpService.UpdateVenue(vendorVenue, vendormaster, id, vid);
                if (vendorVenue.Id != 0) count = vendorVenue.Id;
            }
            else if (category == "Catering")
            {
                VendorsCatering vendorsCatering = vendorVenueSignUpService.GetParticularVendorCatering(id, vid);
                vendorsCatering.VendorMasterId = id;
                vendorsCatering.CuisineType = subcategory;
                vendorsCatering = vendorVenueSignUpService.UpdateCatering(vendorsCatering, vendormaster, id, vid);
                if (vendorsCatering.Id != 0) count = vendorsCatering.Id;
            }
            else if (category == "Photography")
            {
                VendorsPhotography vendorsPhotography = vendorVenueSignUpService.GetParticularVendorPhotography(id, vid);
                vendorsPhotography.VendorMasterId = id;
                vendorsPhotography.PhotographyType = subcategory;
                vendorsPhotography = vendorVenueSignUpService.UpdatePhotography(vendorsPhotography, vendormaster, id, vid);
                if (vendorsPhotography.Id != 0) count = vendorsPhotography.Id;
            }
            else if (category == "Decorator")
            {
                VendorsDecorator vendorsDecorator = vendorVenueSignUpService.GetParticularVendorDecorator(id, vid);
                vendorsDecorator.VendorMasterId = id;
                vendorsDecorator.DecorationType = subcategory;
                vendorsDecorator = vendorVenueSignUpService.UpdateDecorator(vendorsDecorator, vendormaster, id, vid);
                if (vendorsDecorator.Id != 0) count = vendorsDecorator.Id;
            }
            //if (category == "EventManagement")
            //{
            //    VendorsEventOrganiser vendorsEventOrganiser = new VendorsEventOrganiser();
            //    vendorsEventOrganiser.VendorMasterId = id;
            //    vendorsEventOrganiser = vendorVenueSignUpService.AddVendorEventOrganiser(vendorsEventOrganiser);
            //    if (vendorsEventOrganiser.Id != 0) count = vendorsEventOrganiser.Id;
            //}
            else if (category == "Other")
            {
                VendorsOther vendorsOther = vendorVenueSignUpService.GetParticularVendorOther(id, vid);
                vendorsOther.VendorMasterId = id;
                vendorsOther.MinOrder = "0";
                vendorsOther.MaxOrder = "0";
                vendorsOther.Status = "InActive";
                vendorsOther.UpdatedBy = 2;
                vendorsOther.UpdatedDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);//Convert.ToDateTime(DateTime.UtcNow.ToShortDateString());
                vendorsOther.type = subcategory;
                vendorsOther = vendorVenueSignUpService.UpdateOther(vendorsOther, vendormaster, id, vid);
                if (vendorsOther.Id != 0) count = vendorsOther.Id;
            }
            return count;
        }

        [HttpPost]
        public JsonResult UploadImages(HttpPostedFileBase helpSectionImages, string vsid)
        {
            string fileName = string.Empty;
            string filename = string.Empty;
            VendorImage vendorImage = new VendorImage();
            Vendormaster vendorMaster = new Vendormaster();
            var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
            string uid = user.UserId.ToString();
            string email = userLoginDetailsService.Getusername(long.Parse(uid));
            vendorMaster = vendorMasterService.GetVendorByEmail(email);
            string vid = vendorMaster.Id.ToString();
            if (helpSectionImages != null)
            {
                string path = System.IO.Path.GetExtension(helpSectionImages.FileName);
                int imageno = 0;
                int imagecount = 8;
                var list = vendorImageService.GetImages(long.Parse(vid), long.Parse(vsid));

                if (list.Count <= imagecount && Request.Files.Count <= imagecount - list.Count)
                {
                    //getting max imageno
                    if (list.Count != 0)
                    {
                        string lastimage = list.OrderByDescending(m => m.ImageId).FirstOrDefault().ImageName;
                        var splitimage = lastimage.Split('_', '.');
                        imageno = int.Parse(splitimage[3]);
                    }
                    //Uploading images in db & folder
                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        int j = imageno + i + 1;
                        var file1 = Request.Files[i];
                        if (file1 != null && file1.ContentLength > 0)
                        {
                            filename = vendorMaster.ServicType + "_" + vsid + "_" + vid + "_" + j + path;
                            fileName = System.IO.Path.Combine(System.Web.HttpContext.Current.Server.MapPath(imagepath + filename));
                            file1.SaveAs(fileName);
                            vendorImage.ImageName = filename;
                            vendorImage.ImageType = "Slider";
                            vendorImage.VendorId = long.Parse(vsid);
                            vendorImage = vendorImageService.AddVendorImage(vendorImage, vendorMaster);
                        }
                    }
                }
            }
            return Json(filename, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateImageInfo(string ks, string vid, string description)
        {
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                VendorImage vendorImage = new VendorImage();
                string fileName = string.Empty;
                string imgdesc = description;
                Vendormaster vendorMaster = new Vendormaster();
                string strReq = "";
                encptdecpt encript = new encptdecpt();
                strReq = encript.Decrypt(ks);
                //Parse the value... this is done is very raw format.. you can add loops or so to get the values out of the query string...
                string[] arrMsgs = strReq.Split('&');
                string[] arrIndMsg;
                string id = "";
                arrIndMsg = arrMsgs[0].Split('='); //Get the id
                id = arrIndMsg[1].ToString().Trim();
                ViewBag.id = ks;
                vendorMaster.Id = long.Parse(id);
                vendorImage.VendorId = long.Parse(vid);
                string status = "";
                var images = vendorImageService.GetImages(long.Parse(id), long.Parse(vid));
                if (images.Count != 0)
                {
                    //Updating images info
                    for (int i = 0; i < images.Count; i++)
                    {
                        vendorImage.ImageType = images[i].ImageType;
                        vendorImage.Imagedescription = imgdesc.Split(',')[i];
                        vendorImage.ImageName = images[i].ImageName;
                        vendorImage.ImageId = images[i].ImageId;
                        vendorImage.VendorId = images[i].VendorId;
                        vendorImage.VendorMasterId = images[i].VendorMasterId;
                        vendorImage.UpdatedBy = images[i].UpdatedBy;
                        vendorImage.UpdatedDate = images[i].UpdatedDate;
                        vendorImage.Status = images[i].Status;
                        vendorImage.ImageLimit = "6";
                        status = vendorImageService.UpdateVendorImage(vendorImage, long.Parse(id), long.Parse(vid));
                    }
                }
                return Json(JsonRequestBehavior.AllowGet);
            }
            return Json(JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddPackage(Package package)
        {
            DateTime updateddate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
            package.price1 = package.PackagePrice = package.normaldays;
            //Add Package Code
            package.Status = "Active";
            package.UpdatedDate = updateddate;
            package.timeslot = package.timeslot.Trim(',');
            package = vendorVenueSignUpService.addpack(package);
            string msg = string.Empty;
            if (package.PackageID > 0) msg = "Package Added SuccessFully!!!";
            else msg = "Failed TO Add Package";
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdatePackage(Package package)
        {
            DateTime updateddate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
            package.price1 = package.PackagePrice = package.normaldays;
            package.menuitems = package.menuitems.Trim(',');
            //Add Package Code
            package.Status = "Active";
            package.UpdatedDate = updateddate;
            package.timeslot = package.timeslot.Trim(',');
            package = vendorVenueSignUpService.updatepack(package.PackageID.ToString(), package);
            string msg = string.Empty;
            if (package.PackageID > 0) msg = "Package Updated SuccessFully!!!";
            else msg = "Failed TO Update Package";
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Sendmsg(string msg, string Email)
        {
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {


                var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
                string uid = user.UserId.ToString();
                string vemail = userLoginDetailsService.Getusername(long.Parse(uid));
                vendorMaster = vendorMasterService.GetVendorByEmail(vemail);
                string vid = vendorMaster.Id.ToString();
                string fromemail = vemail;
                FileInfo File = new FileInfo(Server.MapPath("/mailtemplate/login.html"));
                string readFile = File.OpenText().ReadToEnd();
                string txtto = Email;
                //string txtmessage = readFile;//readFile + body;
                string txtmessage = msg;
                string subj = "Get Quote From orders Page";
                EmailSendingUtility emailSendingUtility = new EmailSendingUtility();
                emailSendingUtility.Email_maaaahwanam1(txtto, txtmessage, subj, fromemail);
            }
            var message = "success";
            return Json(message);
        }

        public JsonResult updatepolicy(string policycheck, string vsid, string Tax, string Decoration_starting_costs, string Rooms_Count, string Advance_Amount, string Room_Average_Price)
        {

            var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
            string uid = user.UserId.ToString();
            string email = userLoginDetailsService.Getusername(long.Parse(uid));
            vendorMaster = vendorMasterService.GetVendorByEmail(email);
            string vid = vendorMaster.Id.ToString();

            Policy policy = new Policy();
            policy.ServiceType = vendorMaster.ServicType;
            policy.VendorId = vid;
            policy.VendorSubId = vsid;


            string[] selectedamenitieslist = policycheck.Split(',');
            //if (vendormaster.ServicType == "Venue")
            //{
            if (selectedamenitieslist.Contains("Outside_decorators_allowed")) policy.Outside_decorators_allowed = "Yes"; else policy.Outside_decorators_allowed = "No";
            if (selectedamenitieslist.Contains("Decor_provided")) policy.Decor_provided = "Yes"; else policy.Decor_provided = "No";
            if (selectedamenitieslist.Contains("Decorators_allowed_with_royalty")) policy.Decorators_allowed_with_royalty = "Yes"; else policy.Decorators_allowed_with_royalty = "No";
            if (selectedamenitieslist.Contains("Food_provided")) policy.Food_provided = "Yes"; else policy.Food_provided = "No";
            if (selectedamenitieslist.Contains("Outside_food_or_caterer_allowed")) policy.Outside_food_or_caterer_allowed = "Yes"; else policy.Outside_food_or_caterer_allowed = "No";
            if (selectedamenitieslist.Contains("NonVeg_allowed")) policy.NonVeg_allowed = "Yes"; else policy.NonVeg_allowed = "No";
            if (selectedamenitieslist.Contains("Alcohol_allowed")) policy.Alcohol_allowed = "Yes"; else policy.Alcohol_allowed = "No";
            if (selectedamenitieslist.Contains("Outside_Alcohol_allowed")) policy.Outside_Alcohol_allowed = "Yes"; else policy.Outside_Alcohol_allowed = "No";
            if (selectedamenitieslist.Contains("Valet_Parking")) policy.Valet_Parking = "Yes"; else policy.Valet_Parking = "No";
            if (selectedamenitieslist.Contains("Parking_Space")) policy.Parking_Space = "Yes"; else policy.Parking_Space = "No";
            if (selectedamenitieslist.Contains("Cancellation")) policy.Cancellation = "Yes"; else policy.Cancellation = "No";
            if (selectedamenitieslist.Contains("Available_Rooms")) policy.Available_Rooms = "Yes"; else policy.Available_Rooms = "No";
            if (selectedamenitieslist.Contains("Changing_Rooms_AC")) policy.Changing_Rooms_AC = "Yes"; else policy.Changing_Rooms_AC = "No";
            if (selectedamenitieslist.Contains("Complimentary_Changing_Room")) policy.Complimentary_Changing_Room = "Yes"; else policy.Complimentary_Changing_Room = "No";
            if (selectedamenitieslist.Contains("Music_Allowed_Late")) policy.Music_Allowed_Late = "Yes"; else policy.Music_Allowed_Late = "No";
            if (selectedamenitieslist.Contains("Halls_AC")) policy.Halls_AC = "Yes"; else policy.Halls_AC = "No";
            if (selectedamenitieslist.Contains("Ample_Parking")) policy.Ample_Parking = "Yes"; else policy.Ample_Parking = "No";
            if (selectedamenitieslist.Contains("Baarat_Allowed")) policy.Baarat_Allowed = "Yes"; else policy.Baarat_Allowed = "No";
            if (selectedamenitieslist.Contains("Fire_Crackers_Allowed")) policy.Fire_Crackers_Allowed = "Yes"; else policy.Fire_Crackers_Allowed = "No";
            if (selectedamenitieslist.Contains("Hawan_Allowed")) policy.Hawan_Allowed = "Yes"; else policy.Hawan_Allowed = "No";
            if (selectedamenitieslist.Contains("Overnight_wedding_Allowed")) policy.Overnight_wedding_Allowed = "Yes"; else policy.Overnight_wedding_Allowed = "No";

            string msg;

            policy.Decoration_starting_costs = Decoration_starting_costs;
            policy.Tax = Tax;
            policy.Advance_Amount = Advance_Amount;
            policy.Room_Average_Price = Room_Average_Price;
            policy.Rooms_Count = Rooms_Count;
            var policy1 = vendorMasterService.Getpolicy(vid, vsid);
            if (policy1 == null)
            {
                var data = vendorMasterService.insertpolicy(policy, vid, vsid);
                msg = "policies inserted";
            }
            else
            {
                var data = vendorMasterService.updatepolicy(policy, vid, vsid);
                msg = "policies Updated";
            }
            return Json(msg);

        }

        public JsonResult GetPackagemenuItem(string menuitem, string category, string vsid)
        {
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                List<VendorImage> allimages = new List<VendorImage>();
                var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
                string email = userLoginDetailsService.Getusername(long.Parse(user.UserId.ToString()));
                vendorMaster = vendorMasterService.GetVendorByEmail(email);
                var particularitem = "";
                var package = vendorDashBoardService.GetParticularMenu(category, vendorMaster.Id.ToString(), vsid).Select(m => new
                {
                    m.Welcome_Drinks,
                    m.Starters,
                    m.Rice,
                    m.Bread,
                    m.Curries,
                    m.Fry_Dry,
                    m.Salads,
                    m.Soups,
                    m.Deserts,
                    m.Beverages,
                    m.Fruits,m.Extra_Menu_Items
                }).ToList();
                var serialised = String.Join("#", package).Replace("{", "").Replace("}", "");
                var convertedlist = serialised.Split('#', ',');
                if (new[] { "Welcome Drinks", "Starters", "Rice", "Bread", "Curries", "Fry/Dry", "Salads", "Soups", "Deserts", "Beverages", "Fruits"}.Contains(menuitem))
                {
                    var mitem = menuitem.Replace(" ", "_").Replace("/", "_");
                    for (int i = 0; i < convertedlist.Count(); i++)
                    {
                        if (convertedlist[i].Split('=')[0].Trim() == mitem) particularitem = convertedlist[i].Split('=')[1];
                    }
                    return Json(particularitem, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    if (package.FirstOrDefault().Extra_Menu_Items != "" && package.FirstOrDefault().Extra_Menu_Items != null)
                    {
                        var extramenus = package.FirstOrDefault().Extra_Menu_Items.Trim(',').Split(',');
                        for (int i = 0; i < extramenus.Count(); i++)
                        {
                            if (extramenus[i].Split('(')[0].Trim() == menuitem.Trim()) particularitem = extramenus[i].Split('(')[1].Split(')')[0];
                        }
                    }
                    return Json(particularitem, JsonRequestBehavior.AllowGet);
                }
            }
            return Json("Failed");
        }

        public JsonResult GetMenuItem(string pkgid)
        {
            var pkg = vendorProductsService.getpartpkgs(pkgid);//.Select(m => m.menuitems).FirstOrDefault();
            var list = pkg.Select(m => m.menu).FirstOrDefault();
            if(list == "" || list == null) list = pkg.Select(m => m.menuitems).FirstOrDefault();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateMenuItems(PackageMenu PackageMenu, string type)
        {
            var getmenu = vendorDashBoardService.GetParticularMenu(PackageMenu.Category, PackageMenu.VendorMasterID, PackageMenu.VendorID).FirstOrDefault();
            PackageMenu.Extra_Menu_Items = getmenu.Extra_Menu_Items.Trim(',');
            PackageMenu.UpdatedDate = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
            string status = vendorDashBoardService.UpdateMenuItems(PackageMenu, type);
            if (status == "Updated") status = "" + type + " Items Updated";
            return Json(status, JsonRequestBehavior.AllowGet);
        }

        public JsonResult deleteservice(string ks, string vsid, string type)
        {
            try
            {
                var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
                string uid = user.UserId.ToString();
                string email = userLoginDetailsService.Getusername(long.Parse(uid));
                vendorMaster = vendorMasterService.GetVendorByEmail(email);
                string vid = vendorMaster.Id.ToString();
                string msg = vendorVenueSignUpService.RemoveVendorService(vsid, type);
                string message = vendorImageService.DeleteAllImages(long.Parse(vid), long.Parse(vsid));
                return Json(message, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {
                return Json("something went wrong", JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult NewCourse(PackageMenu packageMenu,string type)
        {
            var getmenu = vendorDashBoardService.GetParticularMenu(packageMenu.Category, packageMenu.VendorMasterID, packageMenu.VendorID).FirstOrDefault();
            if (getmenu.Extra_Menu_Items != null)
                packageMenu.Extra_Menu_Items = packageMenu.Extra_Menu_Items + "," + getmenu.Extra_Menu_Items;
            else
                packageMenu.Extra_Menu_Items = packageMenu.Extra_Menu_Items;
            packageMenu.UpdatedDate = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
            string status = vendorDashBoardService.UpdateMenuItems(packageMenu, type);
            if (status == "Updated") status = "New Course & Course Items Added";
            return Json(status ,JsonRequestBehavior.AllowGet);
        }

        public int AddMenuList(string VendorID,string VendorMasterID)
        {
            PackageMenu packageMenu = new PackageMenu();
            packageMenu.UpdatedDate = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
            //Adding Veg Menu
            packageMenu.VendorID = VendorID;
            packageMenu.VendorMasterID = VendorMasterID;
            packageMenu.Category = "Veg";
            packageMenu.Welcome_Drinks = "Hot Badam Milk!Cold Badam Milk!Mosambi Juice!Pineapple Juice!Water Melon!Black Grape!Orange Juice!Apple Juice!Fruit Juice!Lychee Punch!Mango Blossom!Orange Blossom!Sweet Lassi!Salted Lassi!Pink Lady!Strawberry Surprise!Kiwi Kiss!Passion Punch!Blue Passion!Misty Mint!Virgin Mojito!200 ML Water Bottle (Branded)!Tea or Coffee!Any Soft Drinks (Branded)";
            packageMenu.Starters = "Gold Coin!Alu 65!Gobi 65!Babycorn 65!Veg. Manchurian!Paneer Manchurian!Gobi Manchurian!Pepper Gobi!Corn Kernel!Boiled Corn!Babycorn Pakoda!Babycorn Manchurian!Capsicum Rings!Onion Rings!Cheese Ball!Boiled Pally!Masala Pally!Chana Roast!Mirchi Bajji!Cut Mirchi!Moongdal Pakoda!Alu Bonda!Minapa Garelu!Paneer Tikka!Paneer Stick!Veg. Stick!Harabara Tikka!Masala Tikka!Dragon Roll!Paneer Roll!Cheese Roll!Veg. Bullet!Alu Samosa!Irani Samosa!Veg. Spring Roll!Cocktail Samosa!Moongdal Pakoda!Corn Pakoda!Finger Paneer!Crispy Babycorn!Crispy Veg.!Smilies!French Fries!Babycorn Golden Fry!Veg. Seekh Kabab!Palak Makkai Kabab";
            packageMenu.Rice = "Veg Fried Rice!Mongolian Rice!Shezwan Rice!Singapore Noodles!Hot Garlic Sauce!American Chopsuey!Veg Dumpling!Veg Chow Chow!Veg Manchurian (Wt)!Thai Veg (Wet.)!Chilli Veg (Wet)!Chilli Paneer!Chilli Baby corn!Gobi Manchurian!Veg Biryani!Tomato Rice!emon Rice!White Rice!Bagara Rice!Curd Rice!Veg Biryani!Veg Pulav!Peas Pulav!Malai Koftha!Diwani Handi!Mushroom Kaju!unakkaya Kaju!Mirch Ka Saalan!Bagara Baingan!Capsicum Masala!Tomato Masala!Mirchi Tomato Masala!Dondakaya Masala!Sorakaya Masala!Beerakaya Masala!Mealmaker Masala!Rajma Masala!Chole Masala!hendi Masala!Palak Koftha Curry!kaddu Koftha Curry!Chama Gadda Pulusu!Gumadikaya Pulusu!Bendakaya Pulusu!Kakarkaya Pulusu!Beerakaya Alsandalu!eera Kaya Methi!beera kaya Boondi!Alu Palak!Capsicum Tomata Masala!na Palak!Chikkudukaya Alu!Vankaya Alu!Vankaya Alu Tomato!Dosakaya Tomato!lu Gobi Kurma!Alu Mutter!Mixed Veg Kurma!Gangavali Tomato";
            packageMenu.Bread = "Naan!Butter Naan!Garlic Naan!Tandoor Roti!Jawar Roti!Paratha!Sheermal!Pulkha!Aloo Paratha!Methi Paratha!Mooli Paratha!Makki ki roti!Kulcha";
            packageMenu.Curries = "Paneer Butter Masala!Paneer Tikka Masala!Paneer Babycorn Masala!Paneer Capsicum!Paneer Chatpata!Paneer Do Pyasa!Bhendi Do Pyasa!Hariyali Paneer!Khaju Paneer!Methi Chaman!Achari Veg.!Palak Paneer!Kadai Paneer!Navratan Kurma!Paneer Shahi Kurma!Veg Chatpata!Veg Kolhapur!Corn Palak!Chum Alu!Tawa Vegetable!Babycorn Do Pyaza!Koftha Palak!Veg. Jaipuri";
            packageMenu.Fry_Dry = "Alu Methi Fry!Alu Gobi Fry!Alu Capsicum Fry!Chikkudukaya Alu Fry!Jeera Aloo Fry!Bendi Pakoda Fry!Bendi Kaju Fry!Jaipur Bendi!Chat Pat bendi!Kanda kaju Fry!Donda kaju Fry!Aratikaya Fry!Chamagadda Fry!Beans Coconut Fry!Kakarakaya Fry!Chikkudukaya Fry!Cabbage Coconut Fry!Nutrilla Kaju Fry!Guthivankaya Fry!Mixed Vegetable Fry!Vankaya Pakoda!Dondakaya Pakoda!Veg. Dalcha!Munakkaya Charu!Tomato Charu!Ulawa Charu!Bendakaya Charu!omato Rasam!Miryala Rasam!Pachi Pulusu!Majjiga Pulusu!Kadi Pakoda!Dosa Avakaya!Gobi Avakaya!onda Avakaya!Mango Avakaya!Lime Pickle!Mixed Veg.Pickle!Gongoora Pickle!ongoora Chutney!Tomata Chutney!Cabbage Chutney!Carrot Chutney!Beerakaya Chutney!Kandi Podi!coconut Chutney!Nallakaram Podi!Putnaala Podi!Kariyepak Podi!Nuvvula Karram!Ellipaya Kaaram!Allam Chutney!Pudina Chutney!Chukka Kura Chutney!Kothimeera Chuyney!Kobbari Kaaram Podi!Pudina Coconut Chutney!Mango Coconut Chutney!Vankaya Dosakaya Chutney";
            packageMenu.Salads = "Carrot Salad!Green Salad!Ceaser Salad!Barley Salad!Sprouts Salad!Onion Salad!Green Bean Salad!Leafy Salad with nuts!Lintel SaladTomato Soup!Tomato Shorba!Veg. Corn Soup!Coriander Soup!Hot & Sour Soup!Sweetcorn Soup";
            packageMenu.Soups = "Tomato Soup!Tomato Shorba!Veg. Corn Soup!Coriander Soup!Hot & Sour Soup!Sweetcorn Soup";
            packageMenu.Deserts = "Vanilla!Strawberry!Chocolate!Mango!Butter Scotch!Seethapal!Choco chips!Pista!Cassata!Kulfi Sticks!Cake Ice Cream!Trifle Pudding!Asmar Cream!Zouceshani!cold Stone!chocobar Stick!King Cone Chocolate!King Cone Butter Scotch!Matka Kulfi!Roller Ice Cream!Jhangri!Khova Burf!Malai Roll!Green Guava!Sweet Tamarind!Black Grapes!Australian Grapes";
            packageMenu.Beverages = "Mini Orange!Rambutan!Dragon Fruit!Mango Steam!Cherry!Water bottle!Choclate coffee!Coffee!Tea!Soft drinks!Cuppuccino!Lata";
            packageMenu.Fruits = "Apple!Lichi!Dates!Pears!Sapota!Grapes!Peaches!Thailand!Orange!Anjeer!Guava!Plums!Kiwi!Pineapple!Papaya!Water Melon!Musk Melon!pomegranate!Mango!Fuji Apple!Strawberry!Red Guava";
            // Saving Veg Menu
            int count = vendorDashBoardService.AddVegMenu(packageMenu);
            // Non-Veg Menu
            packageMenu.Category = "NonVeg";
            packageMenu.Starters = "Chicken 65!Spicy Wings!Chicken Manchurian!Chicken Lollypop!Chicken Tikka!Chicken Satay!Murg Malai Kabab!Chicken Garlic Kabab!Chicken Pahadi Kabab!Chicken Reshmi Kabab!Chicken Hariyali Kabab!Chicken Majestic!Chicken Nuggets!Shezwan Chicken!Chicken Pakoda!Pepper Chicken!Popcorn Chicken!Chilli Chicken!Loose Chilli Prawns!Golden Fried Prawns!Pepper Prawns!Chilli Prawns!Prawn Pakoda!Royyala Vepudu!Garlic Prawns!Prawns Iguru!Finger Fish!Apollo Fish!Fried Fish!Chilly Fish!Fish Tikka!Fish Fry (Murrel With Bone)!Fish Amrithsari!Tawa Fish Bone (Murrel)!Tawa Fish Boneless (Murrel)!Crab Wray Bheemavaram!Crab lguru";
            packageMenu.Rice = "Hyderabad Mutton Biryani!Mutton Sofiyani Biryani!Hyderabad Chicken Biryani!Chicken Sofiyani Biryani!Chicken Pulav!Prawns Pulav!Egg Biryani!Mixed Fried Rice!Egg Fried Rice!Egg Fried Rice!Chicken Fried Rice!Mixed Fried Rice!American Chopsoy (Non-Veg)!Chilly Chicken Wet!Chicken Manchurian Wet!Shezwan Chicken Wet!Garlic Chicken (Dry & Wet)";
            packageMenu.Curries = "Dhumka Chicken!Methi Chicken!Chilli Chicken!Ginger Chicken!Gongoora Chicken!Moghalai Chicken!Hariyali Chicken!Ankapur Country Chicken!Butter Chicken!Chicken Masala!Chilly Chicken Wet(Chinese)!Chicken Manchurian Wet!Chicken Diwani Handi!Mutton Curry!Moghalai Mutton!utton Pasinda!Mutton Kali Mirchi!Mutton Roganjosh!Dhum-ka- Bakra!utton Raan!ongoora Maamsam!Chukkakura Maamsam!Palak Mutton!iver Fry!Kidney Fry!okkala Charu!Mutton Dalcha!Boti Dhalcha!Thalakaya Kura!Keema Methi!Keema Palak!Keema Batana!eema Koftha Curry!Paya!Haleem!Butter Chicken!Anda Bhurji!Sarson KA Saag!Makki Ki Roti!Amritsari Kulcha!Punjabi Chole!Dal Makhni!akoda Khadi!Dal Fry!Labadar Paneer!Dal Tadka";
            packageMenu.Fry_Dry = "Boti Fry!Kidney Fry!Keema Shikhampuri!Mutton Boti Kabab!Mutton Seekh Kabab!Mutton Shami Kabab!Mutton Chops!Kheema Lukmi!Keema Balls!Liver Fry!Pathar ka Ghosh!Liver Kidney Fry!Mutton Fry (Telangana Style)";
            packageMenu.Soups = "Chicken Hot & Sour Soup!Paya Shorba!Badami Murg Shorba!Morag Soup!Wanton soup!Chicken clear soup!Canton soup!Chicken noodle soup!Chicken cream soup!Egg drop soup";
            packageMenu.UpdatedDate = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
            //Saving Non-Veg Menu
            count = vendorDashBoardService.AddNonVegMenu(packageMenu);
            return count;
        }
    }
}

