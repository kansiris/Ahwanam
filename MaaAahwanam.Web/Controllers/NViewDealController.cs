using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaaAahwanam.Service;
using MaaAahwanam.Repository;
using MaaAahwanam.Web.Custom;
using MaaAahwanam.Models;
using MaaAahwanam.Utility;

namespace MaaAahwanam.Web.Controllers
{
    public class NViewDealController : Controller
    {
        VendorProductsService vendorProductsService = new VendorProductsService();

        // GET: NViewDeal
        public ActionResult Index(string id, string type, string eve)
        {
            try
            {

                //                ViewBag.singledeal = vendorProductsService.getparticulardeal(Int32.Parse(id), type).FirstOrDefault();
                ViewBag.singledeal = vendorProductsService.getpartvendordeal(id, type).FirstOrDefault();
                if (eve != "")
                {
                    var data = vendorProductsService.getpartvendordeal(id, type).Where(m => m.Category == eve);
                    ViewBag.singledeal1 = data ;
                    ViewBag.events = data.Select(m => m.Category).Distinct();

                }
                else
                {
                    var data = vendorProductsService.getpartvendordeal(id, type);
                    ViewBag.singledeal1 = data;
                    ViewBag.events = data.Select(m => m.Category).Distinct();
                }

                return View();
            }
            catch (Exception ex)
            { return RedirectToAction("Index", "Nhomepage"); }
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

        public ActionResult booknow(string type, string etype1, string date, string totalprice, string id,string price, string guest)
     {

            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
                var vendor = vendorProductsService.getparticulardeal(Int32.Parse(id), type).FirstOrDefault();
                string updateddate = DateTime.UtcNow.ToShortDateString();
                CartItem cartItem = new CartItem();
                cartItem.VendorId = vendor.Id;
                cartItem.ServiceType = etype1;
                cartItem.TotalPrice = decimal.Parse(totalprice);
                cartItem.Orderedby = user.UserId;
                cartItem.UpdatedDate = Convert.ToDateTime(updateddate);
                cartItem.Perunitprice = decimal.Parse(price);
                cartItem.Quantity = Convert.ToInt16(guest);
                cartItem.subid = vendor.subid;
              //  cartItem.attribute = orderRequest.attribute;
                cartItem.DealId = Convert.ToInt64(id);
                CartService cartService = new CartService();
                cartItem = cartService.AddCartItem(cartItem);
                return Json("Success", JsonRequestBehavior.AllowGet);
            }
            return Json(JsonRequestBehavior.AllowGet);
        }
    }
}