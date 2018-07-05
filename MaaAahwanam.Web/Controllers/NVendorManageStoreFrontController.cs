using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaaAahwanam.Web.Custom;
using MaaAahwanam.Service;
using MaaAahwanam.Models;

namespace MaaAahwanam.Web.Controllers
{
    public class NVendorManageStoreFrontController : Controller
    {
        private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        const string imagepath = @"/vendorimages/";
        VendorImageService vendorImageService = new VendorImageService();
        VenorVenueSignUpService vendorVenueSignUpService = new VenorVenueSignUpService();
        VendorMasterService vendorMasterService = new VendorMasterService();
        // GET: NVendorManageStoreFront
        public ActionResult Index(string id, string vid, string category, string subcategory)
        {
            try
            { 
            ViewBag.id = id;
            ViewBag.vid = vid;
            ViewBag.category = category;
            ViewBag.subcategory = subcategory;
            if (vid != null)
            ViewBag.images = vendorImageService.GetImages(long.Parse(id), long.Parse(vid));
            ViewBag.Vendor = vendorMasterService.GetVendor(long.Parse(id));
            ViewBag.display = "0";
            if (vid != "" && vid != null)
            {
                ViewBag.display = "1";
                if (category == "Venue")
                {
                    VendorVenueService vendorVenueService = new VendorVenueService();
                    ViewBag.service = vendorVenueService.GetVendorVenue(long.Parse(id), long.Parse(vid));
                    ViewBag.categorytype = ViewBag.service.VenueType;
                    return View();
                }
                if (category == "Catering")
                {
                    VendorCateringService vendorCateringService = new VendorCateringService();
                    ViewBag.service = vendorCateringService.GetVendorCatering(long.Parse(id), long.Parse(vid));
                    ViewBag.categorytype = ViewBag.service.CuisineType;
                    return View();
                }
                if (category == "Photography")
                {
                    VendorPhotographyService vendorPhotographyService = new VendorPhotographyService();
                    ViewBag.service = vendorPhotographyService.GetVendorPhotography(long.Parse(id), long.Parse(vid));
                    ViewBag.categorytype = ViewBag.service.PhotographyType;
                    return View();
                }
                if (category == "Decorator")
                {
                    VendorDecoratorService vendorDecoratorService = new VendorDecoratorService();
                    ViewBag.service = vendorDecoratorService.GetVendorDecorator(long.Parse(id), long.Parse(vid));
                    ViewBag.categorytype = ViewBag.service.DecorationType;
                    return View();
                }
                if (category == "Other")
                {
                    VendorOthersService vendorOthersService = new VendorOthersService();
                    ViewBag.service = vendorOthersService.GetVendorOther(long.Parse(id), long.Parse(vid));
                    ViewBag.categorytype = ViewBag.service.type;
                    return View();
                }
            }
            return View();
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Nhomepage");
            }
        }

        [HttpPost]
        public ActionResult Index(string id, string command, string serviceselection, string subcategory, string vid)
        {
            try
            {
                string msg = "";

                if (subcategory != "Select Sub-Category")
                {
                    long count = 0;
                    if (command == "add")
                        count = addservice(serviceselection, subcategory, long.Parse(id));
                    else if (command == "update")
                        count = updateservice(serviceselection, subcategory, long.Parse(id), long.Parse(vid));
                    if (count > 0) msg = "Service Added Successfully";
                    else if (serviceselection == "Select Service Type")
                    {
                        msg = "Failed To Add Sevice";

                        return Content("<script language='javascript' type='text/javascript'>alert('" + msg + "');location.href='/NVendorManageStoreFront/Index?id=" + id + "'</script>");
                    }


                    else
                        msg = "Failed To Add Sevice";

                    return Content("<script language='javascript' type='text/javascript'>alert('" + msg + "');location.href='/NVendorManageStoreFront/Index?id=" + id + "&&vid=" + count + "&&category=" + serviceselection + "&&subcategory=" + subcategory + "'</script>");

                }
            
                msg = "Failed To Add Sub Sevice";

                return Content("<script language='javascript' type='text/javascript'>alert('" + msg + "');location.href='/NVendorManageStoreFront/Index?id=" + id + "'</script>");
            
            }
                  

            catch (Exception)
            {
                return RedirectToAction("Index", "Nhomepage");
            }
        }

        public JsonResult filtercategories(string type)
        {
            string venueservices = "Select Sub-Category,Convention Hall,Function Hall,Banquet Hall,Meeting Room,Open Lawn,Hotel,Resort"; //Roof Top,
            string cateringservices = "Select Sub-Category,Indian,Chinese,Mexican,South Indian,Continental,Multi Cuisine,Chaat,Fast Food,Others";
            string photographyservices = "Select Sub-Category,Wedding,Candid,Portfolio,Fashion,Toddler,Videography,Conventional,Cinematography,Others";
            //string eventservices = "Select Sub-Category,Corporate Events,Brand Promotion,Fashion Shows,Exhibition,Conference & Seminar,Wedding Management,Birthday Planning & Celebrations,Live Concerts,Musical Nights,Celebrity Shows";
            string decoratorservices = "Select Sub-Category,Florists,TentHouse Decorators,Others";
            string otherservices = "Select Sub-Category,Mehendi,Pandit";
            if (type == "Venue") return Json(venueservices, JsonRequestBehavior.AllowGet);
            else if (type == "Catering") return Json(cateringservices, JsonRequestBehavior.AllowGet);
            else if (type == "Photography") return Json(photographyservices, JsonRequestBehavior.AllowGet);
            else if (type == "Decorator") return Json(decoratorservices, JsonRequestBehavior.AllowGet);
            else if (type == "Other") return Json(otherservices, JsonRequestBehavior.AllowGet);
            return Json("Fail", JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateStoreFront(string command, string category, string subcategory, string id, string vid, Vendormaster vendormaster, VendorVenue vendorVenue)
        {
            vendormaster.ServicType = category;
            if (command == "one")
            {
                vendormaster = vendorMasterService.UpdateVendorStorefront(vendormaster, long.Parse(id)); //updating Vendor Master
                return Json("Basic Details Updated");
            }
            else if (command == "two")
            {
                UpdateAmenities(category, subcategory, vendorVenue.Distancefrommainplaceslike, id, vid);
                return Json("Amenities Updated");
            }
            else if (command == "three")
            {
                if (category == "Venue")
                {
                    var venuedata = vendorVenue;
                    vendorVenue = vendorVenueSignUpService.GetParticularVendorVenue(long.Parse(id), long.Parse(vid)); // Retrieving Particular Vendor Record
                    vendorVenue.Description = venuedata.Description;
                    vendorVenue.Dimentions = venuedata.Dimentions;
                    vendorVenue.Minimumseatingcapacity = venuedata.Minimumseatingcapacity;
                    vendorVenue.Maximumcapacity = venuedata.Maximumcapacity;
                    vendorVenue.name = venuedata.name;
                    vendorVenue = vendorVenueSignUpService.UpdateVenue(vendorVenue, vendormaster, long.Parse(id), long.Parse(vid));
                    return Json("Hall Details Updated");
                }
            }
            else if (command == "four")
            {
                var venuedata = vendorVenue;
                if (category == "Venue")
                {
                    vendorVenue = vendorVenueSignUpService.GetParticularVendorVenue(long.Parse(id), long.Parse(vid)); // Retrieving Particular Vendor Record
                    vendorVenue.Address = venuedata.Address;
                    vendorVenue.City = venuedata.City;
                    vendorVenue.State = venuedata.State;
                    vendorVenue.Landmark = venuedata.Landmark;
                    vendorVenue.ZipCode = venuedata.ZipCode;
                    vendorVenue.GeoLocation = venuedata.GeoLocation;
                    vendorVenue = vendorVenueSignUpService.UpdateVenue(vendorVenue, vendormaster, long.Parse(id), long.Parse(vid));
                }
                else if (category == "Catering")
                {
                    VendorCateringService vendorCateringService = new VendorCateringService();
                    VendorsCatering vendorsCatering = vendorVenueSignUpService.GetParticularVendorCatering(long.Parse(id), long.Parse(vid));
                    vendorsCatering.Address = vendorVenue.Address;
                    vendorsCatering.City = vendorVenue.City;
                    vendorsCatering.State = vendorVenue.State;
                    vendorsCatering.Landmark = vendorVenue.Landmark;
                    vendorsCatering.ZipCode = vendorVenue.ZipCode;
                    vendorsCatering = vendorVenueSignUpService.UpdateCatering(vendorsCatering, vendormaster, long.Parse(id), long.Parse(vid));
                }
                else if (category == "Photography")
                {
                    VendorsPhotography vendorsPhotography = vendorVenueSignUpService.GetParticularVendorPhotography(long.Parse(id), long.Parse(vid));
                    vendorsPhotography.Address = vendorVenue.Address;
                    vendorsPhotography.City = vendorVenue.City;
                    vendorsPhotography.State = vendorVenue.State;
                    vendorsPhotography.Landmark = vendorVenue.Landmark;
                    vendorsPhotography.ZipCode = vendorVenue.ZipCode;
                    vendorsPhotography = vendorVenueSignUpService.UpdatePhotography(vendorsPhotography, vendormaster, long.Parse(id), long.Parse(vid));
                }
                else if (category == "Decorator")
                {
                    VendorsDecorator vendorsDecorator = vendorVenueSignUpService.GetParticularVendorDecorator(long.Parse(id), long.Parse(vid));
                    vendorsDecorator.Address = vendorVenue.Address;
                    vendorsDecorator.City = vendorVenue.City;
                    vendorsDecorator.State = vendorVenue.State;
                    vendorsDecorator.Landmark = vendorVenue.Landmark;
                    vendorsDecorator.ZipCode = vendorVenue.ZipCode;
                    vendorsDecorator = vendorVenueSignUpService.UpdateDecorator(vendorsDecorator, vendormaster, long.Parse(id), long.Parse(vid));
                }
                else if (category == "Other")
                {
                    VendorsOther vendorsOther = vendorVenueSignUpService.GetParticularVendorOther(long.Parse(id), long.Parse(vid));
                    vendorsOther.Address = vendorVenue.Address;
                    vendorsOther.City = vendorVenue.City;
                    vendorsOther.State = vendorVenue.State;
                    vendorsOther.Landmark = vendorVenue.Landmark;
                    vendorsOther.ZipCode = vendorVenue.ZipCode;
                    vendorsOther = vendorVenueSignUpService.UpdateOther(vendorsOther, vendormaster, long.Parse(id), long.Parse(vid));
                }
                return Json("Address Updated");
            }
            else if (command == "six")
            {
                vendormaster = vendorMasterService.UpdateVendorStorefront(vendormaster, long.Parse(id)); //updating Vendor Master
                return Json("Your Address Updated");
            }
            else if (command == "seven")
            {
                var venuedata = vendorVenue;
                if (category == "Venue")
                {
                    vendorVenue = vendorVenueSignUpService.GetParticularVendorVenue(long.Parse(id), long.Parse(vid)); // Retrieving Particular Vendor Record
                    vendorVenue.ServiceCost = venuedata.ServiceCost;
                    vendorVenue.VegLunchCost = venuedata.VegLunchCost;
                    vendorVenue.NonVegLunchCost = venuedata.NonVegLunchCost;
                    vendorVenue.VegDinnerCost = venuedata.VegDinnerCost;
                    vendorVenue.NonVegDinnerCost = venuedata.NonVegDinnerCost;
                    vendorVenue.MinOrder = venuedata.MinOrder;
                    vendorVenue.MaxOrder = venuedata.MaxOrder;
                    vendorVenue = vendorVenueSignUpService.UpdateVenue(vendorVenue, vendormaster, long.Parse(id), long.Parse(vid));
                }
                else if (category == "Catering")
                {
                    VendorCateringService vendorCateringService = new VendorCateringService();
                    VendorsCatering vendorsCatering = vendorVenueSignUpService.GetParticularVendorCatering(long.Parse(id), long.Parse(vid));
                    vendorsCatering.Veg = vendorVenue.VegLunchCost;
                    vendorsCatering.NonVeg = vendorVenue.NonVegLunchCost;
                    vendorsCatering.MinOrder = vendorVenue.MinOrder;
                    vendorsCatering.MaxOrder = vendorVenue.MaxOrder;
                    //vendorsCatering.ZipCode = vendorVenue.ZipCode;
                    vendorsCatering = vendorVenueSignUpService.UpdateCatering(vendorsCatering, vendormaster, long.Parse(id), long.Parse(vid));
                }
                else if (category == "Photography")
                {
                    VendorsPhotography vendorsPhotography = vendorVenueSignUpService.GetParticularVendorPhotography(long.Parse(id), long.Parse(vid));
                    vendorsPhotography.StartingPrice = vendorVenue.ServiceCost;
                    vendorsPhotography = vendorVenueSignUpService.UpdatePhotography(vendorsPhotography, vendormaster, long.Parse(id), long.Parse(vid));
                }
                else if (category == "Decorator")
                {
                    VendorsDecorator vendorsDecorator = vendorVenueSignUpService.GetParticularVendorDecorator(long.Parse(id), long.Parse(vid));
                    vendorsDecorator.StartingPrice = vendorVenue.ServiceCost;
                    vendorsDecorator = vendorVenueSignUpService.UpdateDecorator(vendorsDecorator, vendormaster, long.Parse(id), long.Parse(vid));
                }
                else if (category == "Other")
                {
                    VendorsOther vendorsOther = vendorVenueSignUpService.GetParticularVendorOther(long.Parse(id), long.Parse(vid));
                    if(vendorVenue.ServiceCost != 0)
                    vendorsOther.ItemCost = vendorVenue.ServiceCost;
                    vendorsOther.MinOrder = vendorVenue.MinOrder;
                    vendorsOther.MaxOrder = vendorVenue.MaxOrder;
                    vendorsOther = vendorVenueSignUpService.UpdateOther(vendorsOther, vendormaster, long.Parse(id), long.Parse(vid));
                }
                return Json("Price Updated");
            }
            return Json(JsonRequestBehavior.AllowGet);
        }

        public long addservice(string category, string subcategory, long id)
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
                VendorVenue vendorVenue = new VendorVenue();
                vendorVenue.VendorMasterId = id;
                vendorVenue.VenueType = subcategory;
                vendorVenue = vendorVenueSignUpService.UpdateVenue(vendorVenue, vendormaster, id, vid);
                if (vendorVenue.Id != 0) count = vendorVenue.Id;
            }
            else if (category == "Catering")
            {
                VendorsCatering vendorsCatering = new VendorsCatering();
                vendorsCatering.VendorMasterId = id;
                vendorsCatering.CuisineType = subcategory;
                vendorsCatering = vendorVenueSignUpService.UpdateCatering(vendorsCatering, vendormaster, id, vid);
                if (vendorsCatering.Id != 0) count = vendorsCatering.Id;
            }
            else if (category == "Photography")
            {
                VendorsPhotography vendorsPhotography = new VendorsPhotography();
                vendorsPhotography.VendorMasterId = id;
                vendorsPhotography.PhotographyType = subcategory;
                vendorsPhotography = vendorVenueSignUpService.UpdatePhotography(vendorsPhotography, vendormaster, id, vid);
                if (vendorsPhotography.Id != 0) count = vendorsPhotography.Id;
            }
            else if (category == "Decorator")
            {
                VendorsDecorator vendorsDecorator = new VendorsDecorator();
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
                VendorsOther vendorsOther = new VendorsOther();
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
        public JsonResult UploadImages(HttpPostedFileBase file, string id, string vid, string type)
        {
            VendorImage vendorImage = new VendorImage();
            Vendormaster vendorMaster = new Vendormaster();
            vendorMaster.Id = long.Parse(id);
            vendorImage.VendorId = long.Parse(vid);
            string fileName = string.Empty;
            if (file != null)
            {
                string path = System.IO.Path.GetExtension(file.FileName);
                //if (path.ToLower() != ".jpg" && path.ToLower() != ".jpeg" && path.ToLower() != ".png")
                //    return Json("File");
                int imageno = 0;
                int imagecount = 8;
                var list = vendorImageService.GetImages(long.Parse(id), long.Parse(vid));
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
                            var filename = type + "_" + id + "_" + vid + "_" + j + path;
                            fileName = System.IO.Path.Combine(System.Web.HttpContext.Current.Server.MapPath(imagepath + filename));
                            file1.SaveAs(fileName);
                            vendorImage.ImageName = filename;
                            vendorImage = vendorImageService.AddVendorImage(vendorImage, vendorMaster);
                        }
                    }
                }
            }
            return Json("success");
        }

        public ActionResult Removeimage(string src, string id, string vid, string type)
        {
            try { 
            string delete = "";
            var vendorImage = vendorImageService.GetImageId(src, long.Parse(vid));
            delete = vendorImageService.DeleteImage(vendorImage);
            if (delete == "success")
            {
                string fileName = System.IO.Path.Combine(System.Web.HttpContext.Current.Server.MapPath(imagepath + src));
                System.IO.File.Delete(fileName);
                return Json("success");
            }
            else
            {
                return Json("Failed");
            }
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Nhomepage");
            }
        }

        public JsonResult UpdateImageInfo(string id, string vid, string description)
        {
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                VendorImage vendorImage = new VendorImage();
                string fileName = string.Empty;
                string imgdesc = description;
                Vendormaster vendorMaster = new Vendormaster();
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

        public JsonResult UploadProfilePic(HttpPostedFileBase helpSectionImages, string email)
        {
            string fileName = string.Empty;
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
                UserLoginDetailsService userLoginDetailsService = new UserLoginDetailsService();
                string path = System.IO.Path.GetExtension(helpSectionImages.FileName);
                var filename = email + path;
                fileName = System.IO.Path.Combine(System.Web.HttpContext.Current.Server.MapPath(@"/ProfilePictures/" + filename));
                helpSectionImages.SaveAs(fileName);
                userLoginDetailsService.ChangeDP(int.Parse(user.UserId.ToString()), filename);
            }
            return Json(JsonRequestBehavior.AllowGet);
        }

        public void UpdateAmenities(string category, string subcategory, string selectedamenities,string id,string vid)
        {
            //long count = 0;
            Vendormaster vendormaster = new Vendormaster();
            vendormaster.ServicType = category;
            string[] selectedamenitieslist = selectedamenities.Split(',');
            if (category == "Venue")
            {
                VendorVenue vendorVenue = vendorVenueSignUpService.GetParticularVendorVenue(long.Parse(id), long.Parse(vid)); // Retrieving Particular Vendor Record
                if (selectedamenitieslist.Contains("CockTails")) vendorVenue.CockTails = "Yes";
                if (selectedamenitieslist.Contains("Rooms")) vendorVenue.Rooms = "Yes";
                if (selectedamenitieslist.Contains("Wifi")) vendorVenue.Wifi = "Yes";
                if (selectedamenitieslist.Contains("Live Cooking Station")) vendorVenue.LiveCookingStation = "Yes";
                if (selectedamenitieslist.Contains("Decoration Allowed")) vendorVenue.DecorationAllowed = "Yes";
                vendorVenue = vendorVenueSignUpService.UpdateVenue(vendorVenue, vendormaster, long.Parse(id), long.Parse(vid));
                //if (vendorVenue.Id != 0) count = vendorVenue.Id;
            }
            else if (category == "Catering")
            {
                VendorsCatering vendorsCatering = vendorVenueSignUpService.GetParticularVendorCatering(long.Parse(id), long.Parse(vid));
                if (selectedamenitieslist.Contains("Mineral Water Included")) vendorsCatering.MineralWaterIncluded = "Yes";
                if (selectedamenitieslist.Contains("Transport Included")) vendorsCatering.TransportIncluded = "Yes";
                if (selectedamenitieslist.Contains("Live Cooking Station")) vendorsCatering.LiveCookingStation = "Yes";
                vendorsCatering = vendorVenueSignUpService.UpdateCatering(vendorsCatering, vendormaster, long.Parse(id), long.Parse(vid));
                //if (vendorsCatering.Id != 0) count = vendorsCatering.Id;
            }
            else if (category == "Photography")
            {
                VendorsPhotography vendorsPhotography = vendorVenueSignUpService.GetParticularVendorPhotography(long.Parse(id), long.Parse(vid));
                if (selectedamenitieslist.Contains("Pre Wedding Shoot")) vendorsPhotography.PreWeddingShoot = "Yes";
                if (selectedamenitieslist.Contains("Destination Photography")) vendorsPhotography.DestinationPhotography = "Yes";
                vendorsPhotography = vendorVenueSignUpService.UpdatePhotography(vendorsPhotography, vendormaster, long.Parse(id), long.Parse(vid));
                //if (vendorsPhotography.Id != 0) count = vendorsPhotography.Id;
            }
            else if (category == "Decorator")
            {
                VendorsDecorator vendorsDecorator = vendorVenueSignUpService.GetParticularVendorDecorator(long.Parse(id), long.Parse(vid));
                if (selectedamenitieslist.Contains("Archway")) vendorsDecorator.Archway = "Yes";
                if (selectedamenitieslist.Contains("Altar arrangements")) vendorsDecorator.Altararrangements = "Yes";
                if (selectedamenitieslist.Contains("Pew bows")) vendorsDecorator.Pewbows = "Yes";
                if (selectedamenitieslist.Contains("Aisle runner")) vendorsDecorator.Aislerunner = "Yes";
                if (selectedamenitieslist.Contains("Head pieces")) vendorsDecorator.Headpieces = "Yes";
                if (selectedamenitieslist.Contains("Center pieces")) vendorsDecorator.Centerpieces = "Yes";
                if (selectedamenitieslist.Contains("Chair covers")) vendorsDecorator.Chaircovers = "Yes";
                if (selectedamenitieslist.Contains("Head table decor")) vendorsDecorator.Headtabledecor = "Yes";
                if (selectedamenitieslist.Contains("Backdrops")) vendorsDecorator.Backdrops = "Yes";
                if (selectedamenitieslist.Contains("Ceiling canopies")) vendorsDecorator.Ceilingcanopies = "Yes";
                if (selectedamenitieslist.Contains("Mandaps")) vendorsDecorator.Mandaps = "Yes";
                if (selectedamenitieslist.Contains("Mehendi")) vendorsDecorator.Mehendi = "Yes";
                if (selectedamenitieslist.Contains("Sangeet")) vendorsDecorator.Sangeet = "Yes";
                if (selectedamenitieslist.Contains("Chuppas")) vendorsDecorator.Chuppas = "Yes";
                if (selectedamenitieslist.Contains("Lighting")) vendorsDecorator.Lighting = "Yes";
                if (selectedamenitieslist.Contains("Gifts for guests")) vendorsDecorator.Giftsforguests = "Yes";
                if (selectedamenitieslist.Contains("Gift table")) vendorsDecorator.Gifttable = "Yes";
                if (selectedamenitieslist.Contains("Basket or Box for gifts")) vendorsDecorator.BasketorBoxforgifts = "Yes";
                if (selectedamenitieslist.Contains("Place or seating cards")) vendorsDecorator.Placeorseatingcards = "Yes";
                if (selectedamenitieslist.Contains("Car decoration")) vendorsDecorator.Cardecoration = "Yes";
                if (selectedamenitieslist.Contains("Brides bouquet")) vendorsDecorator.Bridesbouquet = "Yes";
                if (selectedamenitieslist.Contains("Bridesmaids bouquets")) vendorsDecorator.Bridesmaidsbouquets = "Yes";
                if (selectedamenitieslist.Contains("Maid of honor bouquet")) vendorsDecorator.Maidofhonorbouquet = "Yes";
                if (selectedamenitieslist.Contains("Throw away bouquet")) vendorsDecorator.Throwawaybouquet = "Yes";
                if (selectedamenitieslist.Contains("Corsages")) vendorsDecorator.Corsages = "Yes";
                if (selectedamenitieslist.Contains("Boutonnieres (for groom, fathers, grandfathers, best man, groom’s men)")) vendorsDecorator.Boutonnieres = "Yes";
                if (selectedamenitieslist.Contains("Decora")) vendorsDecorator.Decora = "Yes";
                if (selectedamenitieslist.Contains("Just married clings")) vendorsDecorator.Justmarriedclings = "Yes";
                vendorsDecorator = vendorVenueSignUpService.UpdateDecorator(vendorsDecorator, vendormaster, long.Parse(id), long.Parse(vid));
                //if (vendorsDecorator.Id != 0) count = vendorsDecorator.Id;
            }
            //return count;
        }
    }
}