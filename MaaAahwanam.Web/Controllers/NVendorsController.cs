using MaaAahwanam.Models;
using MaaAahwanam.Service;
using MaaAahwanam.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaaAahwanam.Web.Custom;

namespace MaaAahwanam.Web.Controllers
{
    public class NVendorsController : Controller
    {
        // GET: NVendors
        VendorProductsService vendorProductsService = new VendorProductsService();
        QuotationListsService quotationListsService = new QuotationListsService();

        public ActionResult Index(string servicetype)
        {
            ViewBag.service = servicetype;
            if (servicetype == "Hotels")
            { ViewBag.records = vendorProductsService.Getvendorproducts_Result("Hotel"); ViewBag.service = "Hotel"; }
            else if (servicetype == "Resorts")
            { ViewBag.records = vendorProductsService.Getvendorproducts_Result("Resort"); ViewBag.service = "Resort"; }
            else if (servicetype == "Conventions")
            { ViewBag.records = vendorProductsService.Getvendorproducts_Result("Convention Hall"); ViewBag.service = "Convention Hall"; }
            else if (servicetype == "BanquetHall")
            { ViewBag.records = vendorProductsService.Getvendorproducts_Result("Banquet Hall"); ViewBag.service = "Banquet Hall"; }
            else if (servicetype == "FunctionHall")
            { ViewBag.records = vendorProductsService.Getvendorproducts_Result("Function Hall"); ViewBag.service = "Function Hall"; }
            else
            { ViewBag.records = vendorProductsService.Getvendorproducts_Result(servicetype); }
            return View();
        }

        [HttpPost]
        public JsonResult GetQuote(QuotationsList quotationsList)
        {
            string ip = HttpContext.Request.UserHostAddress;
            //string hostName = Dns.GetHostName();
            quotationsList.IPaddress = ip;//Dns.GetHostByName(Dns.GetHostName()).AddressList[0].ToString();
            quotationsList.UpdatedTime = DateTime.UtcNow;
            quotationsList.Status = "Active";
            int count = quotationListsService.GetVendorVenue(quotationsList.IPaddress).Count;

            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                count = 0;
            }

            if (count < 6)
            {
                int quotation = quotationListsService.AddQuotationList(quotationsList);
                EmailSendingUtility emailSendingUtility = new EmailSendingUtility();
                string msg = "Email ID:"+quotationsList.EmailId+",Name:"+quotationsList.Name+",Phone:"+quotationsList.PhoneNo+",Check-IN Date:"+quotationsList.EventStartDate+",Check-IN Time:"+quotationsList.EventStartTime+",Check-OUT Date:"+quotationsList.EventEnddate+",Check-OUT Time:"+quotationsList.EventEndtime+",Persons:"+quotationsList.Persons+",Extra Persons:"+quotationsList.ExtraPersons+"";
                emailSendingUtility.Email_maaaahwanam("seema@xsilica.com", msg, "Mail From Ahwanam");
                emailSendingUtility.Email_maaaahwanam("amit.saxena@ahwanam.com", msg, "Mail From Ahwanam");
                emailSendingUtility.Email_maaaahwanam("rameshsai@xsilica.com", msg, "Mail From Ahwanam");
                if (quotation > 0)
                    return Json("Success");
                else
                    return Json("Fail");
            }
            else
                return Json("exceeded");
        }
    }
}