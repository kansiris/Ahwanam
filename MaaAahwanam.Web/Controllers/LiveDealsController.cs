using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaaAahwanam.Service;
using MaaAahwanam.Repository;

namespace MaaAahwanam.Web.Controllers
{
    public class LiveDealsController : Controller
    {
        // GET: LiveDeals
        public ActionResult Index(string search, string type, string name, string sort, string minval, string maxval)

        {
            VendorProductsService vendorProductsService = new VendorProductsService();
            ViewBag.search = search;
            ViewBag.name = name;
            ViewBag.Venuemaxval = vendorProductsService.getdealsearch(search, "Venue").Max(m => m.cost);
            ViewBag.Hotelsmaxval = vendorProductsService.getdealsearch(search, "Hotel").Max(m => m.cost);
            ViewBag.Resortsmaxval = vendorProductsService.getdealsearch(search, "Resort").Max(m => m.cost);
            ViewBag.Conventionsmaxval = vendorProductsService.getdealsearch(search, "Convention Hall").Max(m => m.cost);
            ViewBag.Cateringmaxval = vendorProductsService.getdealsearch(search, "Catering").Max(m => m.cost);
            ViewBag.Photographymaxval = vendorProductsService.getdealsearch(search, "Photography").Max(m => m.cost);
            ViewBag.Decoratormaxval = vendorProductsService.getdealsearch(search, "Decorator").Max(m => m.cost);
            ViewBag.Mehendimaxval = vendorProductsService.getdealsearch(search, "Mehendi").Max(m => m.cost);


            if (ViewBag.Venuemaxval == null)
            {
                ViewBag.Venuemaxval = "0";
            }
            if (ViewBag.Hotelsmaxval == null)
            {
                ViewBag.Hotelsmaxval = "0";
            }
            if (ViewBag.Resortsmaxval == null)
            {
                ViewBag.Resortsmaxval = "0";
            }
            if (ViewBag.Conventionsmaxval == null)
            {
                ViewBag.Conventionsmaxval = "0";
            }
            if (ViewBag.Cateringmaxval == null)
            {
                ViewBag.Cateringmaxval = "0";
            }
            if (ViewBag.Photographymaxval == null)
            {
                ViewBag.Photographymaxval = "0";
            }
            if (ViewBag.Decoratormaxval == null)
            {
                ViewBag.Decoratormaxval = "0";
            }
            if (ViewBag.Mehendimaxval == null)
            {
                ViewBag.Mehendimaxval = "0";
            }


            if (name == null)
            {


                ViewBag.Venue = vendorProductsService.getdealsearch(search, "Venue").OrderByDescending(m => m.cost);
                ViewBag.Hotels = vendorProductsService.getdealsearch(search, "Hotel");
                ViewBag.Resorts = vendorProductsService.getdealsearch(search, "Resort");
                ViewBag.Conventions = vendorProductsService.getdealsearch(search, "Convention Hall");
                ViewBag.Catering = vendorProductsService.getdealsearch(search, "Catering");
                ViewBag.Photography = vendorProductsService.getdealsearch(search, "Photography");
                ViewBag.Decorator = vendorProductsService.getdealsearch(search, "Decorator");
                ViewBag.Mehendi = vendorProductsService.getdealsearch(search, "Mehendi");
            }

            else if (search == null)
            {

                ViewBag.search = search;
                ViewBag.Venue = vendorProductsService.getdealname(name, "Venue");
                ViewBag.Hotels = vendorProductsService.getdealname(name, "Hotel");
                ViewBag.Resorts = vendorProductsService.getdealname(name, "Resort");
                ViewBag.Conventions = vendorProductsService.getdealname(name, "Convention Hall");
                ViewBag.Catering = vendorProductsService.getdealname(name, "Catering");
                ViewBag.Photography = vendorProductsService.getdealname(name, "Photography");
                ViewBag.Decorator = vendorProductsService.getdealname(name, "Decorator");
                ViewBag.Mehendi = vendorProductsService.getdealname(name, "Mehendi");
            }
            else if (sort == "1" && search != null && name == "" && name == null)
            {
                ViewBag.search = search;
                ViewBag.Venue = vendorProductsService.getdealsearch(search, "Venue").OrderBy(m => m.cost);
                ViewBag.Hotels = vendorProductsService.getdealsearch(search, "Hotel").OrderBy(m => m.cost);
                ViewBag.Resorts = vendorProductsService.getdealsearch(search, "Resort").OrderBy(m => m.cost);
                ViewBag.Conventions = vendorProductsService.getdealsearch(search, "Convention Hall").OrderBy(m => m.cost);
                ViewBag.Catering = vendorProductsService.getdealsearch(search, "Catering").OrderBy(m => m.cost);
                ViewBag.Photography = vendorProductsService.getdealsearch(search, "Photography").OrderBy(m => m.cost);
                ViewBag.Decorator = vendorProductsService.getdealsearch(search, "Decorator").OrderBy(m => m.cost);
                ViewBag.Mehendi = vendorProductsService.getdealsearch(search, "Mehendi").OrderBy(m => m.cost);
            }

            else if (sort == "2" && search != null && name == "" && name == null)
            {
                ViewBag.search = search;
                ViewBag.Venue = vendorProductsService.getdealsearch(search, "Venue").OrderByDescending(m => m.cost);
                ViewBag.Hotels = vendorProductsService.getdealsearch(search, "Hotel").OrderByDescending(m => m.cost);
                ViewBag.Resorts = vendorProductsService.getdealsearch(search, "Resort").OrderByDescending(m => m.cost);
                ViewBag.Conventions = vendorProductsService.getdealsearch(search, "Convention Hall").OrderByDescending(m => m.cost);
                ViewBag.Catering = vendorProductsService.getdealsearch(search, "Catering").OrderByDescending(m => m.cost);
                ViewBag.Photography = vendorProductsService.getdealsearch(search, "Photography").OrderByDescending(m => m.cost);
                ViewBag.Decorator = vendorProductsService.getdealsearch(search, "Decorator").OrderByDescending(m => m.cost);
                ViewBag.Mehendi = vendorProductsService.getdealsearch(search, "Mehendi").OrderByDescending(m => m.cost);
            }

            else if (sort == "1" && search == null && search == "" && name != null || name != "")
            {
                ViewBag.search = search;
                ViewBag.Venue = vendorProductsService.getdealname(name, "Venue").OrderBy(m => m.cost);
                ViewBag.Hotels = vendorProductsService.getdealname(name, "Hotel").OrderBy(m => m.cost);
                ViewBag.Resorts = vendorProductsService.getdealname(name, "Resort").OrderBy(m => m.cost);
                ViewBag.Conventions = vendorProductsService.getdealname(name, "Convention Hall").OrderBy(m => m.cost);
                ViewBag.Catering = vendorProductsService.getdealname(name, "Catering").OrderBy(m => m.cost);
                ViewBag.Photography = vendorProductsService.getdealname(name, "Photography").OrderBy(m => m.cost);
                ViewBag.Decorator = vendorProductsService.getdealname(name, "Decorator").OrderBy(m => m.cost);
                ViewBag.Mehendi = vendorProductsService.getdealname(name, "Mehendi").OrderBy(m => m.cost);
            }

            else if (sort == "2" && search == null && search == "" && name != null || name != "")
            {
                ViewBag.search = search;
                ViewBag.Venue = vendorProductsService.getdealname(name, "Venue").OrderByDescending(m => m.cost);
                ViewBag.Hotels = vendorProductsService.getdealname(name, "Hotel").OrderByDescending(m => m.cost);
                ViewBag.Resorts = vendorProductsService.getdealname(name, "Resort").OrderByDescending(m => m.cost);
                ViewBag.Conventions = vendorProductsService.getdealname(name, "Convention Hall").OrderByDescending(m => m.cost);
                ViewBag.Catering = vendorProductsService.getdealname(name, "Catering").OrderByDescending(m => m.cost);
                ViewBag.Photography = vendorProductsService.getdealname(name, "Photography").OrderByDescending(m => m.cost);
                ViewBag.Decorator = vendorProductsService.getdealname(name, "Decorator").OrderByDescending(m => m.cost);
                ViewBag.Mehendi = vendorProductsService.getdealname(name, "Mehendi").OrderByDescending(m => m.cost);
            }
            else if (sort == "1")
            {
                ViewBag.search = search;
                ViewBag.Venue = vendorProductsService.getdealsearch(search, "Venue").OrderBy(m => m.cost);
                ViewBag.Hotels = vendorProductsService.getdealsearch(search, "Hotel").OrderBy(m => m.cost);
                ViewBag.Resorts = vendorProductsService.getdealsearch(search, "Resort").OrderBy(m => m.cost);
                ViewBag.Conventions = vendorProductsService.getdealsearch(search, "Convention Hall").OrderBy(m => m.cost);
                ViewBag.Catering = vendorProductsService.getdealsearch(search, "Catering").OrderBy(m => m.cost);
                ViewBag.Photography = vendorProductsService.getdealsearch(search, "Photography").OrderBy(m => m.cost);
                ViewBag.Decorator = vendorProductsService.getdealsearch(search, "Decorator").OrderBy(m => m.cost);
                ViewBag.Mehendi = vendorProductsService.getdealsearch(search, "Mehendi").OrderBy(m => m.cost);
            }


            else if (sort == "2")
            {
                ViewBag.search = search;
                ViewBag.Venue = vendorProductsService.getdealsearch(search, "Venue").OrderByDescending(m => m.cost);
                ViewBag.Hotels = vendorProductsService.getdealsearch(search, "Hotel").OrderByDescending(m => m.cost);
                ViewBag.Resorts = vendorProductsService.getdealsearch(search, "Resort").OrderByDescending(m => m.cost);
                ViewBag.Conventions = vendorProductsService.getdealsearch(search, "Convention Hall").OrderByDescending(m => m.cost);
                ViewBag.Catering = vendorProductsService.getdealsearch(search, "Catering").OrderByDescending(m => m.cost);
                ViewBag.Photography = vendorProductsService.getdealsearch(search, "Photography").OrderByDescending(m => m.cost);
                ViewBag.Decorator = vendorProductsService.getdealsearch(search, "Decorator").OrderByDescending(m => m.cost);
                ViewBag.Mehendi = vendorProductsService.getdealsearch(search, "Mehendi").OrderByDescending(m => m.cost);
            }

            else if (minval != null || maxval != null || search != null)
            {
                ViewBag.search = search;
                decimal minval1 = decimal.Parse(minval);
                decimal maxval1 = decimal.Parse(maxval);

                ViewBag.Venue = vendorProductsService.getdealsearch(search, "Venue").Where(m => m.cost <= minval1 && m.cost >= maxval1);
                ViewBag.Hotels = vendorProductsService.getdealsearch(search, "Hotel").Where(m => m.cost <= minval1 && m.cost >= maxval1);
                ViewBag.Resorts = vendorProductsService.getdealsearch(search, "Resort").Where(m => m.cost <= minval1 && m.cost >= maxval1);
                ViewBag.Conventions = vendorProductsService.getdealsearch(search, "Convention Hall").Where(m => m.cost <= minval1 && m.cost >= maxval1);
                ViewBag.Catering = vendorProductsService.getdealsearch(search, "Catering").Where(m => m.cost <= minval1 && m.cost >= maxval1);
                ViewBag.Photography = vendorProductsService.getdealsearch(search, "Photography").Where(m => m.cost <= minval1 && m.cost >= maxval1);
                ViewBag.Decorator = vendorProductsService.getdealsearch(search, "Decorator").Where(m => m.cost <= minval1 && m.cost >= maxval1);
                ViewBag.Mehendi = vendorProductsService.getdealsearch(search, "Mehendi").Where(m => m.cost <= minval1 && m.cost >= maxval1);
            }

            else if (minval != null || maxval != null || name != null)
            {
                ViewBag.search = search;
                decimal minval1 = decimal.Parse(minval);
                decimal maxval1 = decimal.Parse(maxval);

                ViewBag.Venue = vendorProductsService.getdealname(name, "Venue").Where(m => m.cost <= minval1 && m.cost >= maxval1);
                ViewBag.Hotels = vendorProductsService.getdealname(name, "Hotel").Where(m => m.cost <= minval1 && m.cost >= maxval1);
                ViewBag.Resorts = vendorProductsService.getdealname(name, "Resort").Where(m => m.cost <= minval1 && m.cost >= maxval1);
                ViewBag.Conventions = vendorProductsService.getdealname(name, "Convention Hall").Where(m => m.cost <= minval1 && m.cost >= maxval1);
                ViewBag.Catering = vendorProductsService.getdealname(name, "Catering").Where(m => m.cost <= minval1 && m.cost >= maxval1);
                ViewBag.Photography = vendorProductsService.getdealname(name, "Photography").Where(m => m.cost <= minval1 && m.cost >= maxval1);
                ViewBag.Decorator = vendorProductsService.getdealname(name, "Decorator").Where(m => m.cost <= minval1 && m.cost >= maxval1);
                ViewBag.Mehendi = vendorProductsService.getdealname(name, "Mehendi").Where(m => m.cost <= minval1 && m.cost >= maxval1);
            }



            else if (sort == "2" && search != null && name == "" && name == null)
            {
                ViewBag.search = search;
                ViewBag.Venue = vendorProductsService.getdealsearch(search, "Venue").OrderByDescending(m => m.cost);
                ViewBag.Hotels = vendorProductsService.getdealsearch(search, "Hotel").OrderByDescending(m => m.cost);
                ViewBag.Resorts = vendorProductsService.getdealsearch(search, "Resort").OrderByDescending(m => m.cost);
                ViewBag.Conventions = vendorProductsService.getdealsearch(search, "Convention Hall").OrderByDescending(m => m.cost);
                ViewBag.Catering = vendorProductsService.getdealsearch(search, "Catering").OrderByDescending(m => m.cost);
                ViewBag.Photography = vendorProductsService.getdealsearch(search, "Photography").OrderByDescending(m => m.cost);
                ViewBag.Decorator = vendorProductsService.getdealsearch(search, "Decorator").OrderByDescending(m => m.cost);
                ViewBag.Mehendi = vendorProductsService.getdealsearch(search, "Mehendi").OrderByDescending(m => m.cost);
            }


            return View();
        }
        public JsonResult AutoCompletelocation()
        {
            VendorMasterService allVendorsService = new VendorMasterService();
            var Listoflocations = String.Join(",", allVendorsService.GetVendorLocations().Distinct());
            //return Json(Listoflocations,JsonRequestBehavior.AllowGet);
            return new JsonResult { Data = Listoflocations, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult AutoCompletename()
        {
            VendorMasterService allVendorsService = new VendorMasterService();
            var Listoflocations = String.Join(",", allVendorsService.GetVendorname().Distinct());
            //return Json(Listoflocations,JsonRequestBehavior.AllowGet);
            return new JsonResult { Data = Listoflocations, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }
}