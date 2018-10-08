using System;
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
                DateTime todatedate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);//Convert.ToDateTime(DateTime.UtcNow.ToShortDateString());
                DateTime tommarowdate = todatedate.AddDays(1);
                var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
                string id = user.UserId.ToString();
                string email = userLoginDetailsService.Getusername(long.Parse(id));
                vendorMaster = vendorMasterService.GetVendorByEmail(email);
                string vid = vendorMaster.Id.ToString();
                ViewBag.Vendor = vendorMasterService.GetVendor(Convert.ToInt64(vendorMaster.Id));
                var orders = orderService.userOrderList().Where(m => m.Id == Convert.ToInt64(vendorMaster.Id)).ToList();
                var orders1 = orderService.userOrderList1().Where(m => m.vid == Convert.ToInt64(vendorMaster.Id)).ToList();

                //Orders Section
                ViewBag.currentorders = orders.Where(p => p.Status == "Pending").Count();
                ViewBag.ordershistory = orders.Where(m => m.Status != "Removed").Count();
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
                    ViewBag.venues = venues.Where(m=>m.Minimumseatingcapacity >= int.Parse(count)).ToList();
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
                    if (pkgsks != null) ViewBag.package = pkgsks;
                    else ViewBag.package = new Package();
                    var policy1 = vendorMasterService.Getpolicy(vid, vsid);
                    ViewBag.policy = policy1;
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

                if (count > 0)


                    msg = "Service Added Successfully";



                else if (serviceselection == "Select Service Type")
                {
                    msg = "Failed To Add Sevice";

                }


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

        public JsonResult updatepolicy( string policycheck,string vsid, string Tax,string Decoration_starting_costs,string Rooms_Count,string Advance_Amount,string Room_Average_Price)
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
           policy.Rooms_Count = Rooms_Count ;
            var policy1 = vendorMasterService.Getpolicy(vid, vsid);
            if (policy1 == null) {
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

        public JsonResult GetPackagemenuItem(string menuitem, string category,string vsid)
        {
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                List<VendorImage> allimages = new List<VendorImage>();
                var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
                string email = userLoginDetailsService.Getusername(long.Parse(user.UserId.ToString()));
                vendorMaster = vendorMasterService.GetVendorByEmail(email);
                var particularitem = "";
                var package = vendorDashBoardService.GetParticularMenu(category, vendorMaster.Id.ToString(), vsid).Select(m=>new
                {
                    m.Welcome_Drinks,m.Starters,m.Rice,m.Bread,m.Curries,m.Fry_Dry,m.Salads,m.Soups,m.Deserts,m.Beverages,m.Fruits
                }).ToList();
                var serialised = String.Join("#", package).Replace("{", "").Replace("}", "");
                var convertedlist = serialised.Split('#',',');
                var mitem = menuitem.Replace(" ", "_").Replace("/","_");
                for (int i = 0; i < convertedlist.Count(); i++)
                {
                    if (convertedlist[i].Split('=')[0].Trim() == mitem)  particularitem = convertedlist[i].Split('=')[1]; 
                }
                return Json(particularitem, JsonRequestBehavior.AllowGet);
            }
            return Json("Failed");
        }

        public JsonResult GetMenuItem(string pkgid)
        {
            var pkg = vendorProductsService.getpartpkgs(pkgid).Select(m=>m.menuitems).FirstOrDefault();
            return Json(pkg,JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateMenuItems(PackageMenu PackageMenu,string type)
        {
            PackageMenu.UpdatedDate = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
            string status = vendorDashBoardService.UpdateMenuItems(PackageMenu, type);
            if (status == "Updated") status = ""+type+" Items Updated!!!Reload To See Changes";
            return Json(status,JsonRequestBehavior.AllowGet);
        }

        public ActionResult deleteservice(string ks, string vid, string type)
        {
            try
            {
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
                int count = 0;
                if (type == "Venue")
                    count = vendorVenueSignUpService.GetVendorVenue(long.Parse(id)).ToList().Count;
                if (type == "Catering")
                    count = vendorVenueSignUpService.GetVendorCatering(long.Parse(id)).ToList().Count;
                if (type == "Photography")
                    count = vendorVenueSignUpService.GetVendorPhotography(long.Parse(id)).Count;
                if (type == "EventManagement")
                    count = vendorVenueSignUpService.GetVendorEventOrganiser(long.Parse(id)).Count;
                if (type == "Decorator")
                    count = vendorVenueSignUpService.GetVendorDecorator(long.Parse(id)).Count;
                if (type == "Other")
                    count = vendorVenueSignUpService.GetVendorOther(long.Parse(id)).Count;
                if (count > 1)
                {
                    string msg = vendorVenueSignUpService.RemoveVendorService(vid, type);
                    string message = vendorImageService.DeleteAllImages(long.Parse(id), long.Parse(vid));

                    //  TempData["Active"] = "Service " + msg + "";
                    // return RedirectToAction("Index", "NVendorStoreFront", new { id = id });
                    return Content("<script language='javascript' type='text/javascript'>alert('Service " + msg + "');location.href='/NVendorStoreFront/Index?ks=" + ks + "'</script>");
                }
                else
                {
                    long value = vendorVenueSignUpService.UpdateVendorService(id, vid, type);
                    string message = vendorImageService.DeleteAllImages(long.Parse(id), long.Parse(vid));
                    if (value > 0)
                    {
                        //TempData["Active"] = "Service Removed";
                        // return RedirectToAction("Index", "NVendorStoreFront", new { id = id });
                        return Content("<script language='javascript' type='text/javascript'>alert('Service Removed');location.href='/NVendorStoreFront/Index?ks=" + ks + "'</script>");

                    }
                    else

                        // TempData["Active"] = "Something Went Wrong!!! Try Again After Some Time";
                        //return RedirectToAction("Index", "NVendorStoreFront", new { id = id });
                        return Content("<script language='javascript' type='text/javascript'>alert('Something Went Wrong!!! Try Again After Some Time');location.href='/NVendorStoreFront/Index?ks=" + ks + "'</script>");
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Nhomepage");
            }
        }

    }
}

