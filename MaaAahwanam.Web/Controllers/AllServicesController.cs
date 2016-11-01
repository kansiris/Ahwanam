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
        public ActionResult Index(string l)
        {
            
            List <ProductsDisplay_Result> Productlist_Venue = productService.ProductsDisplay("Venue", 0,"%",l,"ASC");
            List<ProductsDisplay_Result> Productlist_Catering = productService.ProductsDisplay("Catering", 0, "%", l, "ASC");
            List<ProductsDisplay_Result> Productlist_Decorator = productService.ProductsDisplay("Decorator", 0, "%", l, "ASC");
            List<ProductsDisplay_Result> Productlist_Photography = productService.ProductsDisplay("Photography", 0, "%", l, "ASC");
            List<ProductsDisplay_Result> Productlist_InvitationCard = productService.ProductsDisplay("InvitationCard", 0, "%", l, "ASC");
            List<ProductsDisplay_Result> Productlist_Gift = productService.ProductsDisplay("Gift", 0, "%", l, "ASC");
            List<ProductsDisplay_Result> Productlist_Entertainment = productService.ProductsDisplay("Entertainment", 0, "%", l, "ASC");
            List<ProductsDisplay_Result> Productlist_Travel = productService.ProductsDisplay("Travel", 0, "%", l, "ASC");
            List<ProductsDisplay_Result> Productlist_Other = productService.ProductsDisplay("Other", 0, "%", l, "ASC");


            List<ProductsDisplay_Result> Productlist_BeautyServices = productService.ProductsDisplay("BeautyService", 0, "%", l, "ASC");
            List<ProductsDisplay_Result> Productlist_Eventorganiser = productService.ProductsDisplay("Eventorganiser", 0, "%", l, "ASC");
            List<ProductsDisplay_Result> Productlist_Weddingcollection = productService.ProductsDisplay("WeddingCollection", 0, "%", l, "ASC");


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

            ViewBag.BeautyServices = Productlist_BeautyServices;
            ViewBag.Eventorganiser = Productlist_Eventorganiser;
            ViewBag.Weddingcollection = Productlist_Weddingcollection;

            return View();
        }
    }
}