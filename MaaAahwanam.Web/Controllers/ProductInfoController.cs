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
    public class ProductInfoController : Controller
    {
        ReviewService reviewService = new ReviewService();
        //
        // GET: /CardInfo/
        public ActionResult Index()
        {

            ProductInfoService productInfoService = new ProductInfoService();
            Review review = new Review();
            string Servicetype = Request.QueryString["par"];
            int vid = Convert.ToInt32(Request.QueryString["VID"]);
            GetProductsInfo_Result Productinfo = productInfoService.getProductsInfo_Result(vid, Servicetype);
            string[] imagenameslist = Productinfo.image.Replace(" ", "").Split(',');
            ViewBag.Imagelist = imagenameslist;
            ViewBag.servicetype = Servicetype;
            ViewBag.Reviewlist = reviewService.GetReview(vid);

            var tupleModel = new Tuple<GetProductsInfo_Result, Review>(Productinfo, review);
            return View(tupleModel);
        }
        public ActionResult WriteaRiview([Bind(Prefix = "Item2")] Review review)
        {
            int a = ValidUserUtility.ValidUser();
            if (ValidUserUtility.ValidUser() != 0 && (ValidUserUtility.UserType() == "User"))
            {
                review.UpdatedBy = ValidUserUtility.ValidUser();
                review.Status = "Active";
                review.UpdatedDate = DateTime.Now;
                reviewService.InsertReview(review);
                return RedirectToAction("Index", new { par = review.Service, VID = review.ServiceId });
            }
            return RedirectToAction("Index", "Signin");
        }

        [Authorize]
        public JsonResult Addtocart(string VID, string servicetype, string amount)
        {
            var user = (CustomPrincipal)System.Web.HttpContext.Current.User;           
            CartItem cartItem = new CartItem();
            cartItem.VendorId = Int32.Parse(VID);
            cartItem.ServiceType = servicetype;
            cartItem.TotalPrice = decimal.Parse(amount);
            cartItem.Orderedby = ValidUserUtility.ValidUser();
            cartItem.UpdatedDate = DateTime.Now;
            CartService cartService = new CartService();
            string mesaage = cartService.AddCartItem(cartItem);
            return Json(mesaage);
        }

        //public JsonResult Addtocart(OrderRequest orderRequest)
        //{

        //}
    }
}