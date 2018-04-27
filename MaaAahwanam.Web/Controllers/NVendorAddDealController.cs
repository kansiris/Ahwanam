using MaaAahwanam.Models;
using MaaAahwanam.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MaaAahwanam.Web.Controllers
{
    public class NVendorAddDealController : Controller
    {
        // GET: NVendorAddDeal
        VenorVenueSignUpService vendorVenueSignUpService = new VenorVenueSignUpService();
        VendorProductsService vendorProductsService = new VendorProductsService();
        public ActionResult Index(string id)
        {
            var deals = vendorProductsService.getvendorsubid(id);
            ViewBag.venuerecord = deals;
            ViewBag.vendormasterid = id;
            ViewBag.id = id;
            return View();
        }

        public ActionResult adddeal(string id, string DealName, string OriginalPrice, string type, string foodtype, string DealPrice, string catogary,string minGuests,string maxGuests, string StartDate, string EndDate, string ddesc)
        {
            if (type == null)
            {

                return Content("<script> alert('select type');location.href='" + @Url.Action("Index", "NVendorAddPackage", new { id = id }) + "' </script>");
            }
            if (DealName == null)
            {

                return Content("<script> alert('enter dealname');location.href='" + @Url.Action("Index", "NVendorAddPackage", new { id = id }) + "' </script>");
            }
            if (OriginalPrice == null)
            {

                return Content("<script> alert('enter OriginalPrice');location.href='" + @Url.Action("Index", "NVendorAddPackage", new { id = id }) + "' </script>");
            }
            if (DealPrice == null)
            {

                return Content("<script> alert('enter DealPrice');location.href='" + @Url.Action("Index", "NVendorAddPackage", new { id = id }) + "' </script>");
            }

            if (foodtype == null)
            {

                return Content("<script> alert('select foodtype');location.href='" + @Url.Action("Index", "NVendorAddPackage", new { id = id }) + "' </script>");
            }
            if (catogary == null)
            {

                return Content("<script> alert('select catogary');location.href='" + @Url.Action("Index", "NVendorAddPackage", new { id = id }) + "' </script>");
            }
            if (minGuests == null)
            {

                return Content("<script> alert('enter minimum guest');location.href='" + @Url.Action("Index", "NVendorAddPackage", new { id = id }) + "' </script>");
            }
            if (maxGuests == null)
            {

                return Content("<script> alert('enter maximum Guests');location.href='" + @Url.Action("Index", "NVendorAddPackage", new { id = id }) + "' </script>");
            }
            if (StartDate == null)
            {

                return Content("<script> alert('enter startdate');location.href='" + @Url.Action("Index", "NVendorAddPackage", new { id = id }) + "' </script>");
            }
            if (EndDate == null)
            {

                return Content("<script> alert('enter enddate');location.href='" + @Url.Action("Index", "NVendorAddPackage", new { id = id }) + "' </script>");
            }
            if (ddesc == null)
            {

                return Content("<script> alert('enter description');location.href='" + @Url.Action("Index", "NVendorAddPackage", new { id = id }) + "' </script>");
            }

            DateTime date = DateTime.Now;
            string[] word = type.Split(',');
            string subid = word[0];
            string type1 = word[1];
            string subtype = word[2];

            NDeals deals = new NDeals();
            deals.VendorId = Convert.ToInt64(id);
            deals.VendorSubId = Convert.ToInt64(subid);
            deals.VendorSubType = subtype;
            deals.VendorType = type1;
            deals.UpdatedDate = date;
            deals.OriginalPrice = Decimal.Parse(OriginalPrice);
            deals.MinMemberCount = minGuests;
            deals.MaxMemberCount = maxGuests;
            deals.FoodType = foodtype;
            deals.DealPrice =  decimal.Parse(DealPrice);
            deals.DealStartDate = Convert.ToDateTime(StartDate);
            deals.DealEndDate = Convert.ToDateTime(EndDate);
            deals.DealDescription = ddesc;
            deals.Category = catogary;
            deals.TermsConditions = "TAXES EXTRA @ 18% PER PERSON / PER ROOM";
            deals = vendorVenueSignUpService.adddeal(deals);


            return Content("<script> alert('deal is saved');location.href='"+Url.Action("Index", "NVendorDeals", new {id = id }) + "'</script>");
        }

        }
}