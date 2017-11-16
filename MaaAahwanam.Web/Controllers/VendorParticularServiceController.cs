using MaaAahwanam.Repository;
using MaaAahwanam.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MaaAahwanam.Web.Controllers
{
    public class VendorParticularServiceController : Controller
    {
        ProductInfoService productInfoService = new ProductInfoService();
        // GET: VendorParticularService
        public ActionResult Index(string type,string id,string vid)
        {
            var data = productInfoService.getProductsInfo_Result(int.Parse(id), type, int.Parse(vid)); //GetProductsInfo_Result Productinfo
            ViewBag.image1 = data.image.Split(',')[0].Replace(" ","");
            ViewBag.image2 = data.image.Split(',')[1].Replace(" ", "");
            ViewBag.image3 = data.image.Split(',')[2].Replace(" ", "");
            ViewBag.Productinfo = data;
            return View();
        }
    }
}