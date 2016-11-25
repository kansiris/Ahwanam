using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaaAahwanam.Models;
using MaaAahwanam.Utility;
using System.Configuration;
using System.Web.Security;
using MaaAahwanam.Service;

namespace MaaAahwanam.Web.Controllers
{
    public class ContactUsController : Controller
    {
        //
        // GET: /ContactUs/
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(Enquiry enquiry)
        {
            
                EnquiryService enquiryService = new EnquiryService();
                string respnonse = enquiryService.SaveEnquiries(enquiry);
                if (respnonse == "Success")
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Submitted Successfully');location.href='" + @Url.Action("Index", "ContactUs") + "'</script>");
                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Submission failed');location.href='" + @Url.Action("Index", "ContactUs") + "'</script>");
                }
            }
           
    }
}