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
        //static List<GetProducts_Result> lst;
        //
        // GET: /CardSelect/
        public ActionResult Index()
        {
            ProductService productService = new ProductService();
            string servicetypeQuerystring = Request.QueryString["par"];
            //string servicetypesType = Request.QueryString["sType"].ToString();
            string abc = Request.Url.PathAndQuery.Split('=')[2].Replace("%20"," ");
            string second = abc.Remove(abc.Length - 4);
            string servicetypesType = second.Replace(", ",",");
            string servicetypeloc = Request.QueryString["loc"];
            string servicetypeorder = Request.QueryString["a"];
            List<GetProducts_Result> Productlist = productService.GetProducts_Results(servicetypeQuerystring, 0, servicetypesType, servicetypeloc, servicetypeorder);
            //lst = Productlist;
            List<getservicetype_Result> servicetypelist = productService.Getservicetype_Result(servicetypeQuerystring);
            var s = servicetypelist.GroupBy(m => m.vendortype).Select(vendortype => vendortype.First());
            long idlast=0;
            if (Productlist.Count!=0)
            { 
            idlast = Productlist.Max(i=>i.Id);
            }
            else
            {
                idlast = 0;
            }
            ViewBag.Lastrecordid = idlast;
            ViewBag.ServiceType = servicetypeQuerystring;
            ViewBag.subservicetype = s;
            return View(Productlist);
        }
        public JsonResult Loadmore(string servicetypeQuerystring, string VID, string servicetype, string subservicetype, string location, string order)
         {
            ProductService productService = new ProductService();
            string id = subservicetype.Replace("%20", " ");
            //List<getservicetype_Result> servicetypelist = productService.Getservicetype_Result(servicetypeQuerystring);
            //var s = servicetypelist.Where(m => m.vendortype == subservicetype.Replace("%20"," ")).Select(m => m.temp);
            //var a = lst.Select(m => m.Id).Distinct();
            //var b = s.Except(a).ToList().Take(4);
            //string c = string.Join(",", b.ToArray());
            //List<sampleproc_Result> Productlist = productService.sampleproc(c);
            List<GetProducts_Result> Productlist = productService.GetProducts_Results(servicetype, int.Parse(VID), id, location, order);
            ViewBag.ServiceType = servicetypeQuerystring;
            return Json(Productlist);
        }
    }
}