using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaaAahwanam.Utility;
using MaaAahwanam.Models;
using MaaAahwanam.Service;
using MaaAahwanam.Repository;
using MaaAahwanam.Web.Custom;

namespace MaaAahwanam.Web.Controllers
{
    public class IndexController : Controller
    {
        public ActionResult Index()
        {
            //var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
            EventsService eventsService = new EventsService();
            ViewBag.EventsCount = eventsService.EventInformationCount();//Successful Events Count
            ticketsService ticketsService = new ticketsService();
            ViewBag.Ticketscount = ticketsService.TicketsCount();//Raised Tickets COunt
            TestmonialService testmonialService = new TestmonialService();
            ViewBag.Testimonials = testmonialService.TestmonialServiceList();//Testimonials List
            //Products List Index(4 Services Photography,Beautition,Decorators,Travels)
            EventsandtipsService eventsandtipsService = new EventsandtipsService();
            var Events = eventsandtipsService.EventsandTipsListUser("Event");
            var Beautytips = eventsandtipsService.EventsandTipsListUser("Beauty Tips");
            var Healthtips = eventsandtipsService.EventsandTipsListUser("Health Tips");
            ViewBag.UpcomingEvents = Events;
            ViewBag.HealthTips = Healthtips;
            ViewBag.BeautyTips = Beautytips;
            ProductService productService = new ProductService();
            List<SP_Deals_Result> Productlist_Photography = productService.GetSP_Deals_Result("Photography", 0, "%", "Hyderabad", "ASC");
            List<SP_Deals_Result> Productlist_BeautyService = productService.GetSP_Deals_Result("BeautyService", 0, "%", "Hyderabad", "ASC");
            List<SP_Deals_Result> Productlist_Decorator = productService.GetSP_Deals_Result("Decorator", 0, "%", "Hyderabad", "ASC");
            List<SP_Deals_Result> Productlist_Travel = productService.GetSP_Deals_Result("Travel", 0, "%", "Hyderabad", "ASC");
            ViewBag.PhotographersDetails = Productlist_Photography;
            ViewBag.Beautician = Productlist_BeautyService;
            ViewBag.Decorators = Productlist_Decorator;
            ViewBag.ToursandTravels = Productlist_Travel;
            return View();
        }


        [ChildActionOnly]
        public PartialViewResult ItemsCartViewBindingLayout()
        {
            CartService cartService = new CartService();
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
                ViewBag.cartCount = cartService.CartItemsCount((int)user.UserId);
            }
            else
            {
                ViewBag.cartCount = cartService.CartItemsCount(0);
            }
            return PartialView("ItemsCartViewBindingLayout");
        }

        [ChildActionOnly]
        public PartialViewResult TestimonialsBindingLayout(string view)
        {
            return PartialView("TestimonialsBindingLayout");
        }

        [HttpPost]
        public JsonResult SubmittingSubscriber(Subscription Subscription)
        {
            string message = string.Empty;
            try
            {
                SubscriptionService subscriptionService = new SubscriptionService();
                subscriptionService.addsubscription(Subscription);
                EmailSendingUtility EmailSend = new EmailSendingUtility();
                EmailSend.Email_maaaahwanam(Subscription.EmailId,"Thank you for subscribing to Maa-Aahwanam","Confirmation Subscription");
                message = "subscribed successfully";
            }
            catch
            {
                message = "subscription failed";
            }
            return Json(String.Format(message));
        }

        public JsonResult AutoCompleteCountry()
        {
            AllVendorsService allVendorsService = new AllVendorsService();
            var Listoflocations = allVendorsService.VendorsList();
            var builder = new TagBuilder("<br/>");

            string[] ListofEvents = { "Wedding", "Reception", "Engagement", "Birthday", "Wedding Anniversary", "Get Together", "Kitty Party", "Cocktail Party" };
            return Json(new { Listoflocations, ListofEvents }, JsonRequestBehavior.AllowGet);
        }
    }
}