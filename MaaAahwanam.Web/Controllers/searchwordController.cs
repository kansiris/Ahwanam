using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaaAahwanam.Service;
using MaaAahwanam.Repository;

namespace MaaAahwanam.Web.Controllers
{
    public class searchwordController : Controller
    {
        // GET: searchword
        public ActionResult Index(string search, string type, string sort, string minval, string maxval)
        {
            VendorProductsService vendorProductsService = new VendorProductsService();
           
                ViewBag.search = search;


                ViewBag.Venue = vendorProductsService.getwordsearch(search, type = "Venue");
                ViewBag.Hotels = vendorProductsService.getwordsearch(search, type = "Hotel");
                ViewBag.Resorts = vendorProductsService.getwordsearch(search, type = "Resort");
                ViewBag.Conventions = vendorProductsService.getwordsearch(search, type = "Convention Hall");
                ViewBag.Catering = vendorProductsService.getwordsearch(search, type = "Catering");
                ViewBag.Photography = vendorProductsService.getwordsearch(search, type = "Photography");
                ViewBag.Decorator = vendorProductsService.getwordsearch(search, type = "Decorator");
                ViewBag.Mehendi = vendorProductsService.getwordsearch(search, type = "Mehendi");
           
           if (sort == "1" && search != null)
            {
                ViewBag.search = search;
                ViewBag.Venue = vendorProductsService.getwordsearch(search, "Venue").OrderBy(m => m.cost);
                ViewBag.Hotels = vendorProductsService.getwordsearch(search, "Hotel").OrderBy(m => m.cost);
                ViewBag.Resorts = vendorProductsService.getwordsearch(search, "Resort").OrderBy(m => m.cost);
                ViewBag.Conventions = vendorProductsService.getwordsearch(search, "Convention Hall").OrderBy(m => m.cost);
                ViewBag.Catering = vendorProductsService.getwordsearch(search, "Catering").OrderBy(m => m.cost);
                ViewBag.Photography = vendorProductsService.getwordsearch(search, "Photography").OrderBy(m => m.cost);
                ViewBag.Decorator = vendorProductsService.getwordsearch(search, "Decorator").OrderBy(m => m.cost);
                ViewBag.Mehendi = vendorProductsService.getwordsearch(search, "Mehendi").OrderBy(m => m.cost);
            }


            else if (sort == "2" && search != null)
            {
                ViewBag.search = search;
                ViewBag.Venue = vendorProductsService.getwordsearch(search, "Venue").OrderByDescending(m => m.cost);
                ViewBag.Hotels = vendorProductsService.getwordsearch(search, "Hotel").OrderByDescending(m => m.cost);
                ViewBag.Resorts = vendorProductsService.getwordsearch(search, "Resort").OrderByDescending(m => m.cost);
                ViewBag.Conventions = vendorProductsService.getwordsearch(search, "Convention Hall").OrderByDescending(m => m.cost);
                ViewBag.Catering = vendorProductsService.getwordsearch(search, "Catering").OrderByDescending(m => m.cost);
                ViewBag.Photography = vendorProductsService.getwordsearch(search, "Photography").OrderByDescending(m => m.cost);
                ViewBag.Decorator = vendorProductsService.getwordsearch(search, "Decorator").OrderByDescending(m => m.cost);
                ViewBag.Mehendi = vendorProductsService.getwordsearch(search, "Mehendi").OrderByDescending(m => m.cost);
            }
            else if (minval != null || maxval != null && search != null)
            {
                ViewBag.search = search;
                decimal minval1 = decimal.Parse(minval);
                decimal maxval1 = decimal.Parse(maxval);
         
                ViewBag.Venue = vendorProductsService.getwordsearch(search, "Venue").Where(m => decimal.Parse(m.cost) <= minval1 && decimal.Parse(m.cost) >= maxval1);
                ViewBag.Hotels = vendorProductsService.getwordsearch(search, "Hotel").Where(m => decimal.Parse(m.cost) <= minval1 && decimal.Parse(m.cost) >= maxval1);
                ViewBag.Resorts = vendorProductsService.getwordsearch(search, "Resort").Where(m => decimal.Parse(m.cost) <= minval1 && decimal.Parse(m.cost) >= maxval1);
                ViewBag.Conventions = vendorProductsService.getwordsearch(search, "Convention Hall").Where(m => decimal.Parse(m.cost) <= minval1 && decimal.Parse(m.cost) >= maxval1);
                ViewBag.Catering = vendorProductsService.getwordsearch(search, "Catering").Where(m => decimal.Parse(m.cost) <= minval1 && decimal.Parse(m.cost) >= maxval1);
                ViewBag.Photography = vendorProductsService.getwordsearch(search, "Photography").Where(m => decimal.Parse(m.cost) <= minval1 && decimal.Parse(m.cost) >= maxval1);
                ViewBag.Decorator = vendorProductsService.getwordsearch(search, "Decorator").Where(m => decimal.Parse(m.cost) <= minval1 && decimal.Parse(m.cost) >= maxval1);
                ViewBag.Mehendi = vendorProductsService.getwordsearch(search, "Mehendi").Where(m => decimal.Parse(m.cost) <= minval1 && decimal.Parse(m.cost) >= maxval1);
            }
            return View();
        }
        public JsonResult AutoCompleteCountry()
        {
            VendorMasterService allVendorsService = new VendorMasterService();
            var Listofwords = String.Join(",", allVendorsService.GetVendorword().Distinct());
            //return Json(Listoflocations,JsonRequestBehavior.AllowGet);
            return new JsonResult { Data = Listofwords, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

    }
}