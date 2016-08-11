﻿using MaaAahwanam.Repository;
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
        public JsonResult Addtocart(OrderRequest orderRequest)
        {
            var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
            CartItem cartItem = new CartItem();
            cartItem.VendorId = orderRequest.VendorId;
            cartItem.ServiceType = orderRequest.ServiceType;
            cartItem.TotalPrice = orderRequest.TotalPrice;
            cartItem.Orderedby = user.UserId;
            cartItem.UpdatedDate = DateTime.Now;

            EventInformation eventInformation = new EventInformation();
            eventInformation.EventName = orderRequest.EventName;
            eventInformation.Email = orderRequest.Email;
            eventInformation.Address = orderRequest.Address;
            eventInformation.Location = orderRequest.Location;
            eventInformation.Phone = orderRequest.Phone;
            eventInformation.PostalCode = orderRequest.PostalCode;
            eventInformation.State = orderRequest.State;
            eventInformation.City = orderRequest.City;

            EventDate eventDate = new EventDate();
            foreach (var item in orderRequest.EventDates)
            {
                eventDate.StartDate = item.StartDate;
                eventDate.StartTime = item.StartTime;
                eventDate.EndDate = item.EndDate;
                eventDate.EndTime = item.EndTime;
            }
            CartService cartService = new CartService();
            string mesaage = cartService.AddCartItem(cartItem);
            EventsService eventsService = new EventsService();
            string mesaage1 = eventsService.SaveEventinformation(eventInformation);
            EventDatesServices eventDatesServices = new EventDatesServices();
            string message3 = eventDatesServices.SaveEventDates(eventDate);
            return Json(mesaage);
        }

        //public JsonResult Addtocart(OrderRequest orderRequest)
        //{

        //}
    }
}