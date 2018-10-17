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
    public class vdbController : Controller
    {
        //public CustomPrincipal user = null;
        Vendormaster vendorMaster = new Vendormaster();
        private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        UserLoginDetailsService userLoginDetailsService = new UserLoginDetailsService();
        VendorMasterService vendorMasterService = new VendorMasterService();
        VenorVenueSignUpService vendorVenueSignUpService = new VenorVenueSignUpService();
        VendorImageService vendorImageService = new VendorImageService();
        VendorDashBoardService vendorDashBoardService = new VendorDashBoardService();
        OrderService orderService = new OrderService();
        const string imagepath = @"/vendorimages/";

        // GET: vdb
        public ActionResult Index(string c, string vid)
        {
            long id = GetVendorID(); // Here Vendor Master ID will be Retrieved
            if (id != 0)
            {
                //Retrieving Available Services
                var venues = vendorVenueSignUpService.GetVendorVenue(id).ToList();
                if (c == null || c == "") c = "second";
                #region c == "first" 
                if (c == "first" && vid != null)
                {
                    // Retrieving Particular Vendor Details
                    var vendordata = vendorMasterService.GetVendor(Convert.ToInt64(id));
                    ViewBag.Vendor = vendordata;
                    VendorVenueService vendorVenueService = new VendorVenueService();

                    // Retrieving Particular Service Info
                    var servicedata = venues.Where(m => m.Id == long.Parse(vid)).ToList();
                    ViewBag.service = servicedata.FirstOrDefault();

                    // Retrieving Hall name or VenueType
                    if (servicedata.FirstOrDefault().name != null && servicedata.FirstOrDefault().name != "")
                        ViewBag.name = servicedata.FirstOrDefault().name;
                    else
                        ViewBag.name = servicedata.FirstOrDefault().VenueType;

                    // Retrieving All Images
                    var allimages = vendorImageService.GetImages(id, long.Parse(vid));
                    if (allimages.Count() > 0)
                    {
                        ViewBag.bannerimage = "/vendorimages/" + allimages.FirstOrDefault().ImageName;
                        ViewBag.allimages = allimages.ToList();
                        ViewBag.imagescount = (allimages.Count < 4) ? 4 - allimages.Count : 0;
                        ViewBag.sliderimages = allimages.Where(m => m.ImageType == "Slider").Take(4).ToList();
                        ViewBag.slidercount = (ViewBag.sliderimages.Count < 4) ? 4 - ViewBag.sliderimages.Count : 0;
                    }
                    else
                    {
                        ViewBag.bannerimage = "~/newdesignstyles/images/banner3.jpg"; //Default Banner Image
                        ViewBag.imagescount = ViewBag.sliderimages = 0;
                        ViewBag.slidercount = 4;
                    }

                    // Packages Section
                    var pkgsks = vendorVenueSignUpService.Getpackages(id, long.Parse(vid)).FirstOrDefault(); //Remove FirstOrDefault() after finalising packages design
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
                                selecteditems.Add(pkgmitems.Split(',')[i].Split('(')[0].Replace('/', '_').Trim());
                            }
                            ViewBag.selecteditems = string.Join(",", selecteditems);
                            ViewBag.pkgitems = pkgitems;
                        }
                    }
                    else ViewBag.package = new Package();

                    //Package Menu Section
                    var pkgmenuitems = vendorDashBoardService.GetParticularMenu("Veg", id.ToString(), vid).FirstOrDefault();
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
                    { int status = AddMenuList(id.ToString(), vid); } // Adding Menu Items
                    ViewBag.extramenuitems = extramenuitems.Trim(',');

                    //Policy Section
                    ViewBag.policy = vendorMasterService.Getpolicy(id.ToString(), vid);

                    //Amenities Section
                    Amenities(servicedata);
                }
                #endregion

                #region c=="second"
                if (c=="second")
                {
                    // Assigning Available Services to viewbag
                    ViewBag.venues = venues;

                    //Packages Section 
                    viewservicesservice viewservicesss = new viewservicesservice();
                    ViewBag.availablepackages = viewservicesss.getvendorpkgs(id.ToString()).ToList();

                    //Orders Section
                    DateTime todatedate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE).Date;
                    DateTime tommarowdate = todatedate.AddDays(1).Date;
                    var orders = orderService.userOrderList().Where(m => m.vid == Convert.ToInt64(vendorMaster.Id)).ToList();
                    var orders1 = orderService.userOrderList1().Where(m => m.vid == Convert.ToInt64(vendorMaster.Id)).ToList();
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
                }
                #endregion

                if (c == "orders")
                {
                    var orders = orderService.userOrderList().Where(m => m.vid == id).ToList();
                    var orders1 = orderService.userOrderList1().Where(m => m.vid == id).ToList();
                    ViewBag.order = orders.OrderByDescending(m => m.OrderId);
                    ViewBag.order1 = orders1.OrderByDescending(m => m.OrderId);
                }
                ViewBag.enable = c; // Type
                ViewBag.id = id;   // Assigning Vendor Master ID to viewbag
                ViewBag.vid = vid; // Assigning Vendor Service ID to viewbag
                return View();
            }
            return Content("<script>alert('Session Timeout!!! Please Login'); location.href='/home'</script>");
        }

        public ActionResult profilepic()
        {
            long id = GetVendorID();
            if (id != 0)
            {
                var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
                ViewBag.Vendor = vendorMasterService.GetVendor(Convert.ToInt64(id));
                ViewBag.profilepic = userLoginDetailsService.GetUser(int.Parse(user.UserId.ToString())).UserImgName;
                return PartialView("profilepic");
            }
            return Content("<script>alert('Session Timeout!!! Please Login'); location.href='/home'</script>");
        }

        public ActionResult sidebar()
        {
            long id = GetVendorID();
            if (id != 0)
            {
                ViewBag.venues = vendorVenueSignUpService.GetVendorVenue(id).ToList();
                ViewBag.catering = vendorVenueSignUpService.GetVendorCatering(id).ToList();
                ViewBag.photography = vendorVenueSignUpService.GetVendorPhotography(id);
                ViewBag.decorators = vendorVenueSignUpService.GetVendorDecorator(id);
                ViewBag.others = vendorVenueSignUpService.GetVendorOther(id);
                return PartialView("sidebar");
            }
            return Content("<script>alert('Session Timeout!!! Please Login'); location.href='/home'</script>");
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

        public int AddMenuList(string VendorID, string VendorMasterID)
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

        public long GetVendorID()
        {
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
                string id = user.UserId.ToString();
                string email = userLoginDetailsService.Getusername(long.Parse(id));
                vendorMaster = vendorMasterService.GetVendorByEmail(email);
                return vendorMaster.Id;
            }
            return 0;
        }
    }
}