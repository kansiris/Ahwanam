using MaaAahwanam.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MaaAahwanam.Web.Controllers
{
    public class TestimonialsController : Controller
    {
        //
        // GET: /Testimonials/
        public ActionResult Index()
        {
            TestmonialService testmonialService = new TestmonialService();
            ViewBag.Testimonials = testmonialService.TestmonialServiceList();//Testimonials List
            return View();
        }
	}
}