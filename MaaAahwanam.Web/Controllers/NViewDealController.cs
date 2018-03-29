using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaaAahwanam.Service;
using MaaAahwanam.Repository;
using MaaAahwanam.Web.Custom;

namespace MaaAahwanam.Web.Controllers
{
    public class NViewDealController : Controller
    {
        VendorProductsService vendorProductsService = new VendorProductsService();
        UserLoginDetailsService userLoginDetailsService = new UserLoginDetailsService();
        // GET: NViewDeal
        public ActionResult Index(string id, string vid, string type)
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
            ViewBag.singledeal = vendorProductsService.getparticulardeal(Int32.Parse(id), Int32.Parse(vid), type).FirstOrDefault();

            return View();
        }

        public PartialViewResult Loadmoredeals(string lastrecord)
        {
            int id = (lastrecord == null) ? 2 : int.Parse(lastrecord) + 2;
            ViewBag.deal = vendorProductsService.getalldeal().OrderBy(m => m.DealID).Take(id);
            var deals = vendorProductsService.getalldeal().OrderBy(m => m.DealID).Take(id);
            ViewBag.dealLastRecord = id;
            ViewBag.dealcount = vendorProductsService.getalldeal().Count();
            return PartialView("Loadmoredeals");
        }

    }
}