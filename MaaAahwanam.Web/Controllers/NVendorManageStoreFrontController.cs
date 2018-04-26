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
        VendorMasterService vendorMasterService = new VendorMasterService();
        // GET: NVendorManageStoreFront
        public ActionResult Index(string id)
        {
            ViewBag.id = id;
            ViewBag.Vendor = vendorMasterService.GetVendor(long.Parse(id));
            return View();
        }

        public JsonResult filtercategories(string type)
        {
            string venueservices = "Select Sub-Category,Convention Hall,Function Hall,Banquet Hall,Meeting Room,Open Lawn,Roof Top,Hotel,Resort";
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

        public JsonResult UpdateStoreFront(string command, string category,string subcategory, Vendormaster vendormaster,VendorVenue vendorVenue)
        {
            if (command == "one")
            {
                //vendormaster = vendorMasterService.UpdateVendorMaster(vendormaster, long.Parse(id));
            }
            return Json(JsonRequestBehavior.AllowGet);
        }
    }
}