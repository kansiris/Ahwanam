using MaaAahwanam.Repository;
using MaaAahwanam.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MaaAahwanam.Web.Controllers
{
    public class DealsController : Controller
    {
        // GET: Deals
        public ActionResult Index()
        {
            ProductService productService = new ProductService();
            string servicetypeQuerystring = Request.QueryString["par"];
            //string servicetypesType = Request.QueryString["sType"];
            //string abc = Request.Url.PathAndQuery.Split('=')[2].Replace("%20", " ").Replace("%25", "%");
            string query = System.Web.HttpUtility.UrlDecode(Request.Url.PathAndQuery.Split('=')[2]);
            string second = query.Remove(query.Length - 4);
            string servicetypesType = second.Replace(", ", ",");
            string servicetypeloc = Request.QueryString["loc"];
            string servicetypeorder = Request.QueryString["a"];
            List<SP_Deals_Result> Productlist = productService.GetSP_Deals_Result(servicetypeQuerystring, 0, servicetypesType, servicetypeloc, servicetypeorder);
            List<GetDealServiceType_Result> servicetypelist = productService.GetDealsservicetype_Result(servicetypeQuerystring);
            var s = servicetypelist.GroupBy(m => m.vendortype).Select(vendortype => vendortype.First()).OrderBy(m=>m.vendortype);
            long idlast = 0;
            if (Productlist.Count != 0)
            {
                idlast = Productlist.Max(i => i.Id);
            }
            else
            {
                idlast = 0;
            }
            ViewBag.servicetypesType = servicetypesType;
            ViewBag.Lastrecordid = idlast;
            ViewBag.ServiceType = servicetypeQuerystring;
            ViewBag.subservicetype = s;
            return View(Productlist);
        }
        public JsonResult Loadmore(string servicetypeQuerystring, string VID, string servicetype, string subservicetype, string location, string order)
        {
            ProductService productService = new ProductService();
            string id = subservicetype.Replace("%20", " ");
            List<GetProducts_Result> Productlist = productService.GetProducts_Results(servicetype, int.Parse(VID), id, location, order);
            ViewBag.ServiceType = servicetypeQuerystring;
            return Json(Productlist);
        }
    }
}