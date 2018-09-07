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
        //ProductInfoService productInfoService = new ProductInfoService();
        //WhishListService whishListService = new WhishListService();
        //UserLoginDetailsService userLoginDetailsService = new UserLoginDetailsService();
        //OrderService orderService = new OrderService();
        //private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        ResultsPageService resultsPageService = new ResultsPageService();


        // GET: viewservice
        public ActionResult Index(string name, string type, string id)
        {
            DateTime cdate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
            DateTime cdate1 = cdate.AddDays(1);
            DateTime cdate2 = cdate.AddDays(-1);

            ViewBag.cdate = cdate.ToString("dd/MMM/yyyy").Replace('-', '/');
            ViewBag.cdate1 = cdate1.ToString("dd/MMM/yyyy").Replace('-', '/');
            ViewBag.cdate2 = cdate2.ToString("dd/MMM/yyyy").Replace('-', '/');

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
                ViewBag.allimages = allimages;
                ViewBag.Productinfo = data;
                ViewBag.location = data.Address;
                ViewBag.City = data.City;
                ViewBag.State = data.State;
                ViewBag.latitude = (data.GeoLocation != null && data.GeoLocation != "") ? data.GeoLocation.Split(',')[0] : "17.385044";
                ViewBag.longitude = (data.GeoLocation != null && data.GeoLocation != "") ? data.GeoLocation.Split(',')[1] : "78.486671";

                //var data = productInfoService.getProductsInfo_Result(int.Parse(id), type, int.Parse(vid));
                ViewBag.data = data;
                List<VendorVenue> vendor = venues;
                List<SPGETNpkg_Result> package = new List<SPGETNpkg_Result>();
                List<VendorVenue> amenities = new List<VendorVenue>();
                List<string> famenities = new List<string>();
                foreach (var item in venues)
                {
                    package.AddRange(viewservicesss.getvendorpkgs(id).Where(p => p.VendorSubId == long.Parse(item.Id.ToString())).ToList());
                    amenities.Add(item);
                }
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
                
                foreach (var item in allamenities)
                {
                    string value = string.Join(",",item).Replace("{","").Replace("}","");
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


        public ActionResult addcnow(string pid,string guest,string dcval,string total,string pprice)//(string type, string etype1, string date, string totalprice, string id, string price, string guest, string timeslot, string vid, string did, string etype2)
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
                    //cartItem.EventType = etype1;
                    //cartItem.EventDate = Convert.ToDateTime(date);
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

    }
}