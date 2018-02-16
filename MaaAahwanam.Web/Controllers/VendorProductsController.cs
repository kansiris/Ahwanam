using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaaAahwanam.Service;
using MaaAahwanam.Repository;

namespace MaaAahwanam.Web.Controllers
{
    public class VendorProductsController : Controller
    {
        // GET: VendorProducts
        VendorProductsService vendorProductsService = new VendorProductsService();
        public ActionResult Index(string service)
        {
            ViewBag.service = service;
            if (service == "Hotels")
                ViewBag.records = vendorProductsService.Getvendorproducts_Result("Hotel");//.Where(m => m.subtype == "Hotel");
            else if (service == "Resorts")
                ViewBag.records = vendorProductsService.Getvendorproducts_Result("Resort");//.Where(m => m.subtype == "Resort");
            else if (service == "Conventions")
                ViewBag.records = vendorProductsService.Getvendorproducts_Result("Convention Hall");//.Where(m => m.subtype == "Convention Hall");
            else
                ViewBag.records = vendorProductsService.Getvendorproducts_Result(service);
            return View();
        }

        public ActionResult SearchResult(string location, string category, string subcategory, string startdate, string enddate, string minguest, string maxguest, string minbudget, string maxbudget)
        {
            string[] splittedcategories = category.Split(',');
            string[] splittedsubcategories = subcategory.Split(',');
            var selectedvenuescategories= ""; var selectedvenues = "";
            selectedvenuescategories = String.Join(",", Enumerable.Range(0, splittedcategories.Length).Where(i => splittedcategories[i] == "Venues").ToList());
            for (int i = 0; i < selectedvenuescategories.Split(',').Length; i++)
            {
                selectedvenues = selectedvenues +','+splittedsubcategories[int.Parse(selectedvenuescategories.Split(',')[i])].ToString();
            }
            //for (int i = 0; i < subcategory.Split(',').Length; i++)
            //{
            //    ViewBag.Venue = vendorProductsService.Getvendorproducts_Result("Venue").Where(m => m.businessname == subcategory.Split(',')[i]).ToList();
            //    ViewBag.Hotels = vendorProductsService.Getvendorproducts_Result("Hotel").Where(m => m.businessname == subcategory.Split(',')[i]).ToList(); // Hotel records
            //    ViewBag.Resorts = vendorProductsService.Getvendorproducts_Result("Resort").Where(m => m.businessname == subcategory.Split(',')[i]).ToList();// Resort records
            //    ViewBag.Conventions = vendorProductsService.Getvendorproducts_Result("Convention Hall").Where(m => m.businessname == subcategory.Split(',')[i]).ToList(); // Convention records
            //    ViewBag.Catering = vendorProductsService.Getvendorproducts_Result("Catering").Where(m => m.businessname == subcategory.Split(',')[i]).ToList();
            //    ViewBag.Photography = vendorProductsService.Getvendorproducts_Result("Photography").Where(m => m.businessname == subcategory.Split(',')[i]).ToList();
            //    ViewBag.Decorator = vendorProductsService.Getvendorproducts_Result("Decorator").Where(m => m.businessname == subcategory.Split(',')[i]).ToList();
            //    ViewBag.Mehendi = vendorProductsService.Getvendorproducts_Result("Mehendi").Where(m => m.businessname == subcategory.Split(',')[i]).ToList();
            //}
            return View("SearchResult");
        }
    }
}