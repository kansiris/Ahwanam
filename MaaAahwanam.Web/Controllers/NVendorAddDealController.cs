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

        public ActionResult adddeal(string id, string DealName, string OriginalPrice, string type, string foodtype, string DealPrice, string catogary,string minGuests,string maxGuests, string StartDate, string EndDate, string ddesc, string timeslot,string timeslot1)
        {
            if (type == "Select Type")
            {     return Content("<script> alert('select type');location.href='" + @Url.Action("Index", "NVendorAddDeal", new { id = id }) + "' </script>");       }

            if (DealName == null || DealName == "")
            {
                return Content("<script> alert('enter dealname');location.href='" + @Url.Action("edit", "NVendorDeals", new { id = id }) + "' </script>");
            }
            if (OriginalPrice == null || OriginalPrice == "")
            {
                return Content("<script> alert('enter OriginalPrice');location.href='" + @Url.Action("edit", "NVendorDeals", new { id = id }) + "' </script>");
            }
            if (DealPrice == null || DealPrice == "")
            {
                return Content("<script> alert('enter DealPrice');location.href='" + @Url.Action("edit", "NVendorDeals", new { id = id }) + "' </script>");
            }
            if (minGuests == null || minGuests == "")
            {
                return Content("<script> alert('enter minGuests');location.href='" + @Url.Action("edit", "NVendorDeals", new { id = id }) + "' </script>");
            }
            if (maxGuests == null || maxGuests == "")
            {
                return Content("<script> alert('enter maxGuests');location.href='" + @Url.Action("edit", "NVendorDeals", new { id = id }) + "' </script>");
            }
            if (StartDate == null || StartDate == "")
            {
                return Content("<script> alert('enter StartDate');location.href='" + @Url.Action("edit", "NVendorDeals", new { id = id }) + "' </script>");
            }
            if (EndDate == null || EndDate == "")
            {
                return Content("<script> alert('enter EndDate');location.href='" + @Url.Action("edit", "NVendorDeals", new { id = id }) + "' </script>");
            }
            if (ddesc == null || ddesc == "")
            {
                return Content("<script> alert('enter ddesc');location.href='" + @Url.Action("edit", "NVendorDeals", new { id = id }) + "' </script>");
            }
            if (timeslot == null && timeslot1 == null || timeslot1 == "" || timeslot == "")
            { return Content("<script> alert('select timeslot');location.href='" + @Url.Action("Index", "NVendorAddDeal", new { id = id }) + "' </script>"); }

            string time = null;
            if (timeslot == null || timeslot == "")
            { time = timeslot1; }

            if (timeslot1 == null || timeslot1 == "")
            { time = timeslot; }

            if (timeslot1 != null && timeslot != null)
            { time = timeslot + ',' + timeslot1; }
            

            DateTime date = DateTime.Now;
            string[] word = type.Split(',');
            string subid = word[0];
            string type1 = word[1];
            string subtype = word[2];

            NDeals deals = new NDeals();
            deals.DealName = DealName;
            deals.TimeSlot = time;
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