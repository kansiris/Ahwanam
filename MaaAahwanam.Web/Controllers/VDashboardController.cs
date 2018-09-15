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
        const string imagepath = @"/vendorimages/";

        // GET: VDashboard
        public ActionResult Index(string c, string vsid)
        {
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
                string id = user.UserId.ToString();
                string email = userLoginDetailsService.Getusername(long.Parse(id));
                vendorMaster = vendorMasterService.GetVendorByEmail(email);
                string vid = vendorMaster.Id.ToString();
                ViewBag.Vendor = vendorMasterService.GetVendor(Convert.ToInt64(vendorMaster.Id));
                var orders = orderService.userOrderList().Where(m => m.Id == Convert.ToInt64(vendorMaster.Id));
                ViewBag.currentorders = orders.Where(p => p.Status == "Pending").Count();
                ViewBag.ordershistory = orders.Where(m => m.Status != "Removed").Count();
                ViewBag.order = orders;
                var venues = vendorVenueSignUpService.GetVendorVenue(long.Parse(vid)).ToList();
                ViewBag.venues = venues;
                Addservices(vsid);
                if (venues.Count > 0)
                    ViewBag.enable = "second";
                else
                    ViewBag.enable = "first";
                if (c != null) ViewBag.enable = c;
                ViewBag.vsid = vsid;
                ViewBag.vendorid = vid;
                if (vsid != null) Amenities(venues.Where(m => m.Id == long.Parse(vsid)).ToList());
                var allimages = vendorImageService.GetVendorAllImages(long.Parse(id));
                //List<VendorImage> image = new List<VendorImage>();
                //foreach (var item in allimages)
                //{
                //    image.AddRange(allimages.Where(m=>m.VendorId == ));
                //}
                ViewBag.ksimages = allimages;
            }
            else
            {
                return RedirectToAction("Index", "NUserRegistration");
            }
            return View();
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
            if (vsid == null || vsid == "undefined")
            {
                ViewBag.ks = "ks"; ViewBag.service = new List<VendorVenue>(); ViewBag.images = new List<VendorImage>();
            }
            else
            {
                ViewBag.ks = "ksc";
                ViewBag.service = vendorVenueService.GetVendorVenue(long.Parse(vid), long.Parse(vsid));
                ViewBag.categorytype = ViewBag.service.VenueType;
                ViewBag.images = vendorImageService.GetImages(long.Parse(vid), long.Parse(vsid));
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
                if (vendormaster.ServicType == "Venue")
                {
                    if (selectedamenitieslist.Contains("CockTails")) vendorVenue.CockTails = "Yes"; else vendorVenue.CockTails = "No";
                    if (selectedamenitieslist.Contains("Rooms")) vendorVenue.Rooms = "Yes"; else vendorVenue.Rooms = "No";
                    if (selectedamenitieslist.Contains("Wifi")) vendorVenue.Wifi = "Yes"; else vendorVenue.Wifi = "No";
                    if (selectedamenitieslist.Contains("Live Cooking Station")) vendorVenue.LiveCookingStation = "Yes"; else vendorVenue.LiveCookingStation = "No";
                    if (selectedamenitieslist.Contains("Decoration Allowed")) vendorVenue.DecorationAllowed = "Yes"; else vendorVenue.DecorationAllowed = "No";
                    if (selectedamenitieslist.Contains("Sufficient Washroom")) vendorVenue.Sufficient_Washroom = "Yes"; else vendorVenue.Sufficient_Washroom = "No";
                    if (selectedamenitieslist.Contains("Sufficient Room Size")) vendorVenue.Sufficient_Room_Size = "Yes"; else vendorVenue.Sufficient_Room_Size = "No";
                    if (selectedamenitieslist.Contains("Intercom")) vendorVenue.Intercom = "Yes"; else vendorVenue.Intercom = "No";
                    if (selectedamenitieslist.Contains("Single Bed")) vendorVenue.Single_Bed = "Yes"; else vendorVenue.Single_Bed = "No";
                    if (selectedamenitieslist.Contains("Queen Bed")) vendorVenue.Queen_Bed = "Yes"; else vendorVenue.Queen_Bed = "No";
                    if (selectedamenitieslist.Contains("King Bed")) vendorVenue.King_Bed = "Yes"; else vendorVenue.King_Bed = "No";
                    if (selectedamenitieslist.Contains("Balcony")) vendorVenue.Balcony = "Yes"; else vendorVenue.Balcony = "No";
                    if (selectedamenitieslist.Contains("Full Length Mirrror")) vendorVenue.Full_Length_Mirrror = "Yes"; else vendorVenue.Full_Length_Mirrror = "No";
                    if (selectedamenitieslist.Contains("Jacuzzi")) vendorVenue.Jacuzzi = "Yes"; else vendorVenue.Jacuzzi = "No";
                    if (selectedamenitieslist.Contains("Sofa Set")) vendorVenue.Sofa_Set = "Yes"; else vendorVenue.Sofa_Set = "No";
                    if (selectedamenitieslist.Contains("Coffee Tea Maker")) vendorVenue.Coffee_Tea_Maker = "Yes"; else vendorVenue.Coffee_Tea_Maker = "No";
                    if (selectedamenitieslist.Contains("Kindle")) vendorVenue.Kindle = "Yes"; else vendorVenue.Kindle = "No";
                    if (selectedamenitieslist.Contains("Netflix")) vendorVenue.Netflix = "Yes"; else vendorVenue.Netflix = "No";
                    if (selectedamenitieslist.Contains("Kitchen")) vendorVenue.Kitchen = "Yes"; else vendorVenue.Kitchen = "No";
                    if (selectedamenitieslist.Contains("Bath Tub")) vendorVenue.Bath_Tub = "Yes"; else vendorVenue.Bath_Tub = "No";
                    if (selectedamenitieslist.Contains("Electricity")) vendorVenue.Electricity = "Yes"; else vendorVenue.AC = "No";
                    if (selectedamenitieslist.Contains("Wellness Center")) vendorVenue.Wellness_Center = "Yes"; else vendorVenue.Wellness_Center = "No";
                    if (selectedamenitieslist.Contains("Spa")) vendorVenue.Spa = "Yes"; else vendorVenue.Spa = "No";
                    if (selectedamenitieslist.Contains("HDTV")) vendorVenue.HDTV = "Yes"; else vendorVenue.HDTV = "No";
                    if (selectedamenitieslist.Contains("Pet Friendly")) vendorVenue.Pet_Friendly = "Yes"; else vendorVenue.Pet_Friendly = "No";
                    if (selectedamenitieslist.Contains("Gym")) vendorVenue.Gym = "Yes"; else vendorVenue.Gym = "No";
                    if (selectedamenitieslist.Contains("In-house Restaurant")) vendorVenue.In_house_Restaurant = "Yes"; else vendorVenue.In_house_Restaurant = "No";
                    if (selectedamenitieslist.Contains("Hair Dryer")) vendorVenue.Hair_Dryer = "Yes"; else vendorVenue.Hair_Dryer = "No";
                    if (selectedamenitieslist.Contains("Mini Fridge")) vendorVenue.Mini_Fridge = "Yes"; else vendorVenue.Mini_Fridge = "No";
                    if (selectedamenitieslist.Contains("In-Room Safe")) vendorVenue.In_Room_Safe = "Yes"; else vendorVenue.In_Room_Safe = "No";
                    if (selectedamenitieslist.Contains("Room Heater")) vendorVenue.Room_Heater = "Yes"; else vendorVenue.Room_Heater = "No";
                    if (selectedamenitieslist.Contains("Wheelchair Accessible")) vendorVenue.Wheelchair_Accessible = "Yes"; else vendorVenue.Wheelchair_Accessible = "No";
                    if (selectedamenitieslist.Contains("Power Backup")) vendorVenue.Power_Backup = "Yes"; else vendorVenue.Power_Backup = "No";
                    if (selectedamenitieslist.Contains("Dining Area")) vendorVenue.Dining_Area = "Yes"; else vendorVenue.Dining_Area = "No";
                    if (selectedamenitieslist.Contains("Bar")) vendorVenue.Bar = "Yes"; else vendorVenue.Bar = "No";
                    if (selectedamenitieslist.Contains("Conference Room")) vendorVenue.Conference_Room = "Yes"; else vendorVenue.Conference_Room = "No";
                    if (selectedamenitieslist.Contains("Swimming Pool")) vendorVenue.Swimming_Pool = "Yes"; else vendorVenue.AC = "No";
                    if (selectedamenitieslist.Contains("CCTV Cameras")) vendorVenue.CCTV_Cameras = "Yes"; else vendorVenue.CCTV_Cameras = "No";
                    if (selectedamenitieslist.Contains("Laundry")) vendorVenue.Laundry = "Yes"; else vendorVenue.Laundry = "No";
                    if (selectedamenitieslist.Contains("Banquet Hall")) vendorVenue.Banquet_Hall = "Yes"; else vendorVenue.Banquet_Hall = "No";
                    if (selectedamenitieslist.Contains("Lift/Elevator")) vendorVenue.Lift_or_Elevator = "Yes"; else vendorVenue.Lift_or_Elevator = "No";
                    if (selectedamenitieslist.Contains("Card Payment")) vendorVenue.Card_Payment = "Yes"; else vendorVenue.Card_Payment = "No";
                    if (selectedamenitieslist.Contains("Parking Facility")) vendorVenue.Parking_Facility = "Yes"; else vendorVenue.Parking_Facility = "No";
                    if (selectedamenitieslist.Contains("Geyser")) vendorVenue.Geyser = "Yes"; else vendorVenue.AC = "No";
                    if (selectedamenitieslist.Contains("Complimentary Breakfast")) vendorVenue.Complimentary_Breakfast = "Yes"; else vendorVenue.Complimentary_Breakfast = "No";
                    if (selectedamenitieslist.Contains("TV")) vendorVenue.TV = "Yes"; else vendorVenue.TV = "No";
                    if (selectedamenitieslist.Contains("AC")) vendorVenue.AC = "Yes"; else vendorVenue.AC = "No";

                    vendorVenue = vendorVenueSignUpService.UpdateVenue(vendorVenue, vendormaster, long.Parse(vid), long.Parse(vsid));
                    //if (vendorVenue.Id != 0) count = vendorVenue.Id;
                }
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
            vendorMaster.Id = long.Parse(vid);
            vendorImage.VendorId = long.Parse(vid);
            VendorVenue vendorVenue = vendorVenueSignUpService.GetParticularVendorVenue(long.Parse(vid), long.Parse(vsid)); // Retrieving Particular Vendor Record
            var type = vendorVenue.VenueType;

            //string path = System.IO.Path.GetExtension(helpSectionImages.FileName);
            // filename = email + path;
            //fileName = System.IO.Path.Combine(System.Web.HttpContext.Current.Server.MapPath(@"/ProfilePictures/" + filename));
            if (System.IO.File.Exists(fileName) == true)
                System.IO.File.Delete(fileName);

            helpSectionImages.SaveAs(fileName);
            //userLoginDetailsService.ChangeDP(int.Parse(user.UserId.ToString()), filename);
            if (helpSectionImages != null)
            {
                string path = System.IO.Path.GetExtension(helpSectionImages.FileName);
                //if (path.ToLower() != ".jpg" && path.ToLower() != ".jpeg" && path.ToLower() != ".png")
                //    return Json("File");
                int imageno = 0;
                int imagecount = 8;
                var list = vendorImageService.GetImages(long.Parse(vsid), long.Parse(vid));
                if (list.Count <= imagecount && Request.Files.Count <= imagecount - list.Count)
                {
                    //getting max imageno
                    for (int i = 0; i < list.Count; i++)
                    {
                        string x = list[i].ImageName.ToString();
                        string[] y = x.Split('_', '.');
                        if (y[3] == "jpg")
                        {
                            imageno = int.Parse(y[2]);
                        }
                        else
                        {
                            imageno = int.Parse(y[3]);
                        }
                    }

                    //Uploading images in db & folder
                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        int j = imageno + i + 1;
                        var file1 = Request.Files[i];
                        if (file1 != null && file1.ContentLength > 0)
                        {
                            filename = type + "_" + vsid + "_" + vid + "_" + j + path;
                            fileName = System.IO.Path.Combine(System.Web.HttpContext.Current.Server.MapPath(imagepath + filename));
                            file1.SaveAs(fileName);
                            vendorImage.ImageName = filename;
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
            package.price1 = package.PackagePrice = package.normaldays;
            //Add Package Code
            return Json(JsonRequestBehavior.AllowGet);
        }

        //  }
        //public ActionResult Sendmsg(string msg)
        //  {
        //      if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
        //      {

        //          if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
        //          {
        //              var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
        //              string uid = user.UserId.ToString();
        //              string email = userLoginDetailsService.Getusername(long.Parse(uid));
        //              vendorMaster = vendorMasterService.GetVendorByEmail(email);
        //              string vid = vendorMaster.Id.ToString();

        //              FileInfo File = new FileInfo(Server.MapPath("/mailtemplate/login.html"));
        //              string readFile = File.OpenText().ReadToEnd();

        //              string txtmessage = readFile;//readFile + body;
        //              string subj = "Get Quote From Cart Page";
        //              EmailSendingUtility emailSendingUtility = new EmailSendingUtility();
        //              //emailSendingUtility.Email_maaaahwanam(txtto, txtmessage, subj);
        //          }
        //          return View();
        //      }

        // }

    }
}
