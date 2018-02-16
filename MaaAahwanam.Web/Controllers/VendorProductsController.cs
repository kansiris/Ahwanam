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
            var selectedhotelscategories = ""; var selectedhotels = "";
            var selectedresortscategories = ""; var selectedresorts = "";
            var selectedconventionscategories = ""; var selectedconventions = "";
            var selectedcateringscategories = ""; var selectedcatering = "";
            var selectedPhotographycategories = ""; var selectedPhotography = "";
            var selectedDecoratorcategories = ""; var selectedDecorator = "";
            var selectedMehendicategories = ""; var selectedMehendi = "";
            selectedvenuescategories = String.Join(",", Enumerable.Range(0, splittedcategories.Length).Where(i => splittedcategories[i] == "Venues").ToList());
            selectedhotelscategories = String.Join(",", Enumerable.Range(0, splittedcategories.Length).Where(i => splittedcategories[i] == "Hotels").ToList());
            selectedresortscategories = String.Join(",", Enumerable.Range(0, splittedcategories.Length).Where(i => splittedcategories[i] == "Resorts").ToList());
            selectedconventionscategories = String.Join(",", Enumerable.Range(0, splittedcategories.Length).Where(i => splittedcategories[i] == "Conventions").ToList());
            selectedcateringscategories = String.Join(",", Enumerable.Range(0, splittedcategories.Length).Where(i => splittedcategories[i] == "Caterers").ToList());
            selectedPhotographycategories = String.Join(",", Enumerable.Range(0, splittedcategories.Length).Where(i => splittedcategories[i] == "Photographers").ToList());
            selectedDecoratorcategories = String.Join(",", Enumerable.Range(0, splittedcategories.Length).Where(i => splittedcategories[i] == "Decorator").ToList());
            selectedMehendicategories = String.Join(",", Enumerable.Range(0, splittedcategories.Length).Where(i => splittedcategories[i] == "Mehendi").ToList());
            if (selectedvenuescategories != "")
            {
                for (int i = 0; i < selectedvenuescategories.Split(',').Length; i++)
                {
                    selectedvenues = selectedvenues + ',' + splittedsubcategories[int.Parse(selectedvenuescategories.Split(',')[i])].ToString();
                }
            }
            if (selectedhotelscategories != "")
            {
                for (int i = 0; i < selectedhotelscategories.Split(',').Length; i++)
                {
                    selectedhotels = selectedhotels + ',' + splittedsubcategories[int.Parse(selectedhotelscategories.Split(',')[i])].ToString();
                }
            }
            if (selectedresortscategories != "")
            {
                for (int i = 0; i < selectedresortscategories.Split(',').Length; i++)
                {
                    selectedresorts = selectedresorts + ',' + splittedsubcategories[int.Parse(selectedresortscategories.Split(',')[i])].ToString();
                }
            }
            if (selectedconventionscategories != "")
            {
                for (int i = 0; i < selectedconventionscategories.Split(',').Length; i++)
                {
                    selectedconventions = selectedconventions + ',' + splittedsubcategories[int.Parse(selectedconventionscategories.Split(',')[i])].ToString();
                }
            }
            if (selectedcateringscategories != "")
            {
                for (int i = 0; i < selectedcateringscategories.Split(',').Length; i++)
                {
                    selectedcatering = selectedcatering + ',' + splittedsubcategories[int.Parse(selectedcateringscategories.Split(',')[i])].ToString();
                }
            }
            if (selectedPhotographycategories != "")
            {
                for (int i = 0; i < selectedPhotographycategories.Split(',').Length; i++)
                {
                    selectedPhotography = selectedPhotography + ',' + splittedsubcategories[int.Parse(selectedPhotographycategories.Split(',')[i])].ToString();
                }
            }
            if (selectedDecoratorcategories != "")
            {
                for (int i = 0; i < selectedDecoratorcategories.Split(',').Length; i++)
                {
                    selectedDecorator = selectedDecorator + ',' + splittedsubcategories[int.Parse(selectedDecoratorcategories.Split(',')[i])].ToString();
                }
            }
            if (selectedMehendicategories != "")
            {
                for (int i = 0; i < selectedMehendicategories.Split(',').Length; i++)
                {
                    selectedMehendi = selectedMehendi + ',' + splittedsubcategories[int.Parse(selectedMehendicategories.Split(',')[i])].ToString();
                }
            }
            
            ViewBag.Venue = vendorProductsService.GetSearchedVendorRecords("Venue",selectedvenues+",").ToList();
            ViewBag.Hotels = vendorProductsService.GetSearchedVendorRecords("Hotel", selectedhotels+",").ToList(); // Hotel records
            ViewBag.Resorts = vendorProductsService.GetSearchedVendorRecords("Resort", selectedresorts+",").ToList();// Resort records
            ViewBag.Conventions = vendorProductsService.GetSearchedVendorRecords("Convention Hall", selectedconventions+",").ToList(); // Convention records
            ViewBag.Catering = vendorProductsService.GetSearchedVendorRecords("Catering", selectedcatering + ",").ToList();
            ViewBag.Photography = vendorProductsService.GetSearchedVendorRecords("Photography", selectedPhotography + ",").ToList();
            ViewBag.Decorator = vendorProductsService.GetSearchedVendorRecords("Decorator", selectedDecorator + ",").ToList();
            ViewBag.Mehendi = vendorProductsService.GetSearchedVendorRecords("Mehendi", selectedMehendi + ",").ToList();
            return View("SearchResult");
        }
    }
}