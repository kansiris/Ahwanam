using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaaAahwanam.Service;
using MaaAahwanam.Web.Custom;
using MaaAahwanam.Repository;

namespace MaaAahwanam.Web.Controllers
{
    public class NLiveDealsController : Controller
    {
        VendorProductsService vendorProductsService = new VendorProductsService();
        UserLoginDetailsService userLoginDetailsService = new UserLoginDetailsService();
        // GET: NLiveDeals
        public ActionResult Index(string id)
        {
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
                var userdata = userLoginDetailsService.GetUser((int)user.UserId);
                if (userdata.FirstName != "" && userdata.FirstName != null)
                    ViewBag.username = userdata.FirstName;
                else if (userdata.FirstName != "" && userdata.FirstName != null && userdata.LastName != "" && userdata.LastName != null)
                    ViewBag.username = "" + userdata.FirstName + " " + userdata.LastName + "";
                else
                    ViewBag.username = userdata.AlternativeEmailID;
            }
            ViewBag.records = vendorProductsService.Getvendorproducts_Result("Venue").Take(4);//.Where(m => m.subtype == "Hotel");
            return View();
        }




        public PartialViewResult Loadmore(string lastrecord)
        {
            int id = (lastrecord == null) ? 6 : int.Parse(lastrecord) + 6;
            ViewBag.deal = vendorProductsService.getalldeal().OrderBy(m => m.DealID).Take(id);
            var deals = vendorProductsService.getalldeal().OrderBy(m => m.DealID).Take(id);
            ViewBag.dealLastRecord = id;
            ViewBag.dealcount = vendorProductsService.getalldeal().Count();
            return PartialView("Loadmore");
        }


    }
}