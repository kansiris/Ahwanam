using MaaAahwanam.Models;
using MaaAahwanam.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MaaAahwanam.Web.Controllers
{
    public class NVendorDealsController : Controller
    {
        VenorVenueSignUpService vendorVenueSignUpService = new VenorVenueSignUpService();

        VendorProductsService vendorProductsService = new VendorProductsService();

        // GET: NVendorDeals
        public ActionResult Index(string id)
        {
            var deals = vendorProductsService.getvendordeals(id);
            ViewBag.dealrecord = deals;
            ViewBag.id = id;
            return View();
        }
        public ActionResult edit(string pid, string vid)
        {
            var deals = vendorProductsService.getpartdeals(pid);
            ViewBag.dealrecord1 = deals;
            ViewBag.id = vid;
            return View();
        }

        public ActionResult updatedeal(string id, string vid, string DealName, string OriginalPrice, string DealPrice,string minGuests, string maxGuests, string StartDate, string EndDate, string ddesc)
        {
            
            if (DealName == null)
            {
                return Content("<script> alert('enter dealname');location.href='" + @Url.Action("edit", "NVendorDeals", new { id = id }) + "' </script>");
            }
            if (OriginalPrice == null)
            {
                return Content("<script> alert('enter OriginalPrice');location.href='" + @Url.Action("edit", "NVendorDeals", new { id = id }) + "' </script>");
            }
            if (DealPrice == null)
            {
                return Content("<script> alert('enter DealPrice');location.href='" + @Url.Action("edit", "NVendorDeals", new { id = id }) + "' </script>");
            }
            if (minGuests == null)
            {
                return Content("<script> alert('enter minGuests');location.href='" + @Url.Action("edit", "NVendorDeals", new { id = id }) + "' </script>");
            }
            if (maxGuests == null)
            {
                return Content("<script> alert('enter maxGuests');location.href='" + @Url.Action("edit", "NVendorDeals", new { id = id }) + "' </script>");
            }
            if (StartDate == null)
            {
                return Content("<script> alert('enter StartDate');location.href='" + @Url.Action("edit", "NVendorDeals", new { id = id }) + "' </script>");
            }
            if (EndDate == null)
            {
                return Content("<script> alert('enter EndDate');location.href='" + @Url.Action("edit", "NVendorDeals", new { id = id }) + "' </script>");
            }
            if (ddesc == null)
            {
                return Content("<script> alert('enter ddesc');location.href='" + @Url.Action("edit", "NVendorDeals", new { id = id }) + "' </script>");
            }

            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {

                DateTime updateddate = DateTime.Now;
                NDeals deals = new NDeals();
                deals.VendorId = Convert.ToInt64(vid);
               
                deals.UpdatedDate = updateddate;
                deals.OriginalPrice = Decimal.Parse(OriginalPrice);
                deals.MinMemberCount = minGuests;
                deals.MaxMemberCount = maxGuests;
                
                deals.DealPrice = decimal.Parse(DealPrice);
                deals.DealStartDate = Convert.ToDateTime(StartDate);
                deals.DealEndDate = Convert.ToDateTime(EndDate);
                deals.DealDescription = ddesc;
               
                deals.TermsConditions = "TAXES EXTRA @ 18% PER PERSON / PER ROOM";
                deals = vendorVenueSignUpService.updatedeal(long.Parse(id),deals);
                //return Content("<script type='text/javscript'> alert('package added'); location.href='/NVendorAddPackage/Index?id="+ id+ "</script>");
                return Content("<script language='javascript' type='text/javascript'>alert('deal updated');location.href='" + @Url.Action("Index", "NVendorDeals", new { id = vid }) + "'</script>");
            }
            return Content("<script language='javascript' type='text/javascript'>alert('Please login');location.href='" + @Url.Action("Index", "Nhomepage", new { id = vid }) + "'</script>");
        }

        public ActionResult deletedeal(string id, string vid)
        {

            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {

                string message = vendorVenueSignUpService.deletedeal(id);
                ViewBag.vendormasterid = id;
                if (message == "success")
                {
                    //return Content("<script type='text/javscript'> alert('package added'); location.href='/NVendorAddPackage/Index?id="+ id+ "</script>");
                    return Content("<script language='javascript' type='text/javascript'>alert('deal deleted');location.href='" + @Url.Action("Index", "NVendorDeals", new { id = vid }) + "'</script>");
                }
            }
            return Content("<script language='javascript' type='text/javascript'>alert('Please login');location.href='" + @Url.Action("Index", "Nhomepage", new { id = vid }) + "'</script>");
        }

    }
}