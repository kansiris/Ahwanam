using MaaAahwanam.Models;
using MaaAahwanam.Repository;
using MaaAahwanam.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MaaAahwanam.Web.Controllers
{
    public class ProductController : Controller
    {
        //
        // GET: /CardSelect/
        public ActionResult Index()
        {
            ProductService productService = new ProductService();
            string servicetypeQuerystring = Request.QueryString["par"];
            string servicetypesType = Request.QueryString["sType"];
            string servicetypeloc = Request.QueryString["loc"];
            string servicetypeorder = Request.QueryString["a"];
            List<GetProducts_Result> Productlist = productService.GetProducts_Results(servicetypeQuerystring, 0, servicetypesType, servicetypeloc, servicetypeorder);
            List<getservicetype_Result> servicetypelist = productService.Getservicetype_Result(servicetypeQuerystring);
            ViewBag.ServiceType = servicetypeQuerystring;
            ViewBag.subservicetype = servicetypelist;
            return View(Productlist);
        }
        public JsonResult Loadmore(string servicetypeQuerystring, string VID, string servicetype, string subservicetype, string location, string order)
        {
            ProductService productService = new ProductService();
            List<GetProducts_Result> Productlist = productService.GetProducts_Results(servicetype, int.Parse(VID), subservicetype, location, order);
            ViewBag.ServiceType = servicetypeQuerystring;
            return Json(Productlist);
        }
    }
}