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
        //static int count = 0;
        // GET: NParticularVendor
        public ActionResult Index(string type, string id, string vid, string m)
        {
            if (m != null)
                WhishList(type, id, vid, "add", ""); //count++;

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
            //var records = venorVenueSignUpService.GetVendorVenue(long.Parse(id));
            //ViewBag.Venue = records;
            //ViewBag.venuetypes = records.Select(v => v.VenueType).Distinct();
            ViewBag.Venue = venorVenueSignUpService.GetVendorVenue(long.Parse(id)); //, long.Parse(vid)
            ViewBag.Catering = venorVenueSignUpService.GetVendorCatering(long.Parse(id)); //, long.Parse(vid)
            ViewBag.Decorator = venorVenueSignUpService.GetVendorDecorator(long.Parse(id)); //, long.Parse(vid)
            ViewBag.Photography = venorVenueSignUpService.GetVendorPhotography(long.Parse(id)); //, long.Parse(vid)
            ViewBag.Other = venorVenueSignUpService.GetVendorOther(long.Parse(id)); //, long.Parse(vid)
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


    }
}