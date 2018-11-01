using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaaAahwanam.Utility;
using MaaAahwanam.Service;
using MaaAahwanam.Models;

namespace MaaAahwanam.Web.Controllers
{
    public class cateringController : Controller
    {
        // GET: catering
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult SendEmail(string fname, string lname, string emailid, string phoneno, string eventdate)
        {
            string ip = HttpContext.Request.UserHostAddress;

            // Saving Enquiry
            TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            EnquiryService enquiryService = new EnquiryService();
            Enquiry enquiry = new Enquiry();
            enquiry.PersonName = fname + " " + lname;
            enquiry.SenderEmailId = emailid;
            enquiry.SenderPhone = phoneno;
            enquiry.EnquiryDate = Convert.ToDateTime(eventdate);
            enquiry.EnquiryTitle = enquiry.EnquiryDetails = "Catering";
            enquiry.EnquiryStatus = enquiry.Status = "Active";
            enquiry.Country = ip;
            enquiry.CompanyName = enquiry.PersonName;
            enquiry.UpdatedDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
            string status = enquiryService.SaveEnquiries(enquiry);

            //Email Sending part
            string msg = "First Name: " + fname + ", Last Name : " + lname + ",Email ID : " + emailid + ",Phone Number:" + phoneno + ",Event date:" + eventdate + ",IP:" + ip;
            EmailSendingUtility emailSendingUtility = new EmailSendingUtility();
            string subject = "Ahwanam Catering Landing Page";
            string txtto = "info@ahwanam.com,seema@xsilica.com,amit.saxena@ahwanam.com,dedeepya@gmail.com,pravalika.b@xsilica.com"; // Mention Target Email ID's here
            emailSendingUtility.Email_maaaahwanam(txtto, msg.Replace(",", "<br/>"), subject);
            return Json("success", JsonRequestBehavior.AllowGet);
        }
    }
}