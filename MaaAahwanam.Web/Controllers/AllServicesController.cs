using MaaAahwanam.Repository;
using MaaAahwanam.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaaAahwanam.Models;
using MaaAahwanam.Utility;
using MaaAahwanam.Web.Custom;

namespace MaaAahwanam.Web.Controllers
{
    public class AllServicesController : Controller
    {
        ProductService productService = new ProductService();
        // GET: AllServices
        public ActionResult Index()
        {
            
            List <GetProducts_Result> Productlist_Venue = productService.GetProducts_Results("Venue", 0,"%","Hyderabad","ASC");
            List<GetProducts_Result> Productlist_Catering = productService.GetProducts_Results("Catering", 0, "%", "Hyderabad", "ASC");
            List<GetProducts_Result> Productlist_Decorator = productService.GetProducts_Results("Decorator", 0, "%", "Hyderabad", "ASC");
            List<GetProducts_Result> Productlist_Photography = productService.GetProducts_Results("Photography", 0, "%", "Hyderabad", "ASC");
            List<GetProducts_Result> Productlist_InvitationCard = productService.GetProducts_Results("InvitationCard", 0, "%", "Hyderabad", "ASC");
            List<GetProducts_Result> Productlist_Gift = productService.GetProducts_Results("Gift", 0, "%", "Hyderabad", "ASC");
            List<GetProducts_Result> Productlist_Entertainment = productService.GetProducts_Results("Entertainment", 0, "%", "Hyderabad", "ASC");
            //List<GetProducts_Result> Productlist_BeautyServices = productService.GetProducts_Results("BeautyServices", 0);
            List<GetProducts_Result> Productlist_Travel = productService.GetProducts_Results("Travel", 0, "%", "Hyderabad", "ASC");
            List<GetProducts_Result> Productlist_Other = productService.GetProducts_Results("Other", 0, "%", "Hyderabad", "ASC");
            ViewBag.Venue = Productlist_Venue;
            ViewBag.Catering = Productlist_Catering;
            ViewBag.Decorator = Productlist_Decorator;
            ViewBag.Photography = Productlist_Photography;

            ViewBag.InvitationCard = Productlist_InvitationCard;
            ViewBag.Gift = Productlist_Gift;
            ViewBag.Entertainment = Productlist_Entertainment;
            //ViewBag.BeautyServices = Productlist_BeautyServices;
            ViewBag.Travel = Productlist_Travel;
            ViewBag.Other = Productlist_Other;

            return View();
        }
    }
}