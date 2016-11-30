using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaaAahwanam.Models;
using MaaAahwanam.Utility;
using MaaAahwanam.Web.Custom;
using MaaAahwanam.Repository;
using MaaAahwanam.Service;

namespace MaaAahwanam.Web.Controllers
{
    public class ParticularOrderController : Controller
    {
        ReviewService reviewService = new ReviewService();
        DashBoardService dashBoardService = new DashBoardService();
        // GET: ParticularOrder
        public ActionResult Index()
        {
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
                ViewBag.Userloginstatus = user.UserId;
            }
            else
            {
                ViewBag.Userloginstatus = 0;
            }
            ProductInfoService productInfoService = new ProductInfoService();
            VendorVenueService vendorVenueService = new VendorVenueService();
            Review review = new Review();
            int oid = Convert.ToInt32(Request.QueryString["oid"]);
            string Servicetype = Request.QueryString["par"];
            int vid = Convert.ToInt32(Request.QueryString["VID"]);
            int Svid = Convert.ToInt32(Request.QueryString["subvid"]);
            string  dealid = Request.QueryString["did"];
            
            if (Servicetype == "Travel&Accomadation")
            {
                Servicetype = "Travel";
            }
            ViewBag.Subvid = Svid;
            ViewBag.servicetype = Servicetype;
            ViewBag.Reviewlist = reviewService.GetReview(vid);
            string type = Request.QueryString["par"];
            //Ratings count & avg rating 
            ViewBag.ratingscount = productInfoService.GetCount(vid, Svid,type).Count();
            ViewBag.rating = productInfoService.ratingscount(vid, Svid, type);
            ViewBag.price = dashBoardService.GetPrice(oid);
            if (dealid != null && dealid != "")
            {
                ViewBag.deal = "1";
                SP_dealsinfo_Result Dealinfo = productInfoService.getDealsInfo_Result(vid, Servicetype, Svid, int.Parse(dealid));
                ViewBag.discountvalue = 10.00;
                if (Dealinfo.ActualServiceprice != 0 && Dealinfo.DealServiceprice != 0)
                {
                    string discountvalue = (((Dealinfo.ActualServiceprice - Dealinfo.DealServiceprice) / Dealinfo.ActualServiceprice) * 100).Value.ToString("0.00");
                    ViewBag.discountvalue = discountvalue;
                }
                if (Dealinfo.image != null)
                {
                    string[] imagenameslist = Dealinfo.image.Replace(" ", "").Split(',');
                    ViewBag.Imagelist = imagenameslist;
                }
                if (Dealinfo.ServicType == "Venue")
                {
                    var list = vendorVenueService.GetVendorVenue(vid, Svid);
                    ViewBag.venuetype = list.VenueType;
                    ViewBag.servicecost = list.ServiceCost;
                }
                var tupleModel1 = new Tuple<SP_dealsinfo_Result, Review>(Dealinfo, review);
                return View(tupleModel1);
            }
            else
            {
                GetProductsInfo_Result Productinfo = productInfoService.getProductsInfo_Result(vid, Servicetype, Svid);
                if (Productinfo != null)
                {
                    if (Productinfo.image != null)
                    {
                        string[] imagenameslist = Productinfo.image.Replace(" ", "").Split(',');
                        ViewBag.Imagelist = imagenameslist;
                    }

                }
                
                if (Productinfo.ServicType == "Venue")
                {
                    var list = vendorVenueService.GetVendorVenue(vid, Svid);
                    ViewBag.venuetype = list.VenueType;
                    ViewBag.servicecost = list.ServiceCost;
                }
                var tupleModel = new Tuple<GetProductsInfo_Result, Review>(Productinfo, review);
                return View(tupleModel);
            }
            
        }
    }
}