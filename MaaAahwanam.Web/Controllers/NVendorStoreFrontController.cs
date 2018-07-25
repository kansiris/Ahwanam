using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaaAahwanam.Models;
using MaaAahwanam.Service;
using MaaAahwanam.Utility;

namespace MaaAahwanam.Web.Controllers
{
    public class NVendorStoreFrontController : Controller
    {
        VendorImageService vendorImageService = new VendorImageService();
        VendorMasterService vendorMasterService = new VendorMasterService();
        VenorVenueSignUpService vendorVenueSignUpService = new VenorVenueSignUpService();
        // GET: NVendorStoreFront
        public ActionResult Index(string ks)
        {
            try
            {
                if (TempData["Active"] != "")
                {
                    ViewBag.msg = TempData["Active"];
                }
                ViewBag.id = ks;
                string strReq = "";
                encptdecpt encript = new encptdecpt();
                strReq = encript.Decrypt(ks);
                //Parse the value... this is done is very raw format.. you can add loops or so to get the values out of the query string...
                string[] arrMsgs = strReq.Split('&');
                string[] arrIndMsg;
                string id = "";
                arrIndMsg = arrMsgs[0].Split('='); //Get the id
                id = arrIndMsg[1].ToString().Trim();
                ViewBag.Vendor = vendorMasterService.GetVendor(long.Parse(id));
                var venues = vendorVenueSignUpService.GetVendorVenue(long.Parse(id)).ToList();
                var catering = vendorVenueSignUpService.GetVendorCatering(long.Parse(id)).ToList();
                var photography = vendorVenueSignUpService.GetVendorPhotography(long.Parse(id));
                var decorators = vendorVenueSignUpService.GetVendorDecorator(long.Parse(id));
                var others = vendorVenueSignUpService.GetVendorOther(long.Parse(id));
                ViewBag.venues = venues;
                ViewBag.catering = catering;
                ViewBag.photography = photography;
                ViewBag.decorators = decorators;
                ViewBag.others = others;
                return View();
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Nhomepage");
            }
        }

        public ActionResult deleteservice(string id, string vid, string type)
        {
            try
            {
                int count = 0;
                if (type == "Venue")
                    count = vendorVenueSignUpService.GetVendorVenue(long.Parse(id)).ToList().Count;
                if (type == "Catering")
                    count = vendorVenueSignUpService.GetVendorCatering(long.Parse(id)).ToList().Count;
                if (type == "Photography")
                    count = vendorVenueSignUpService.GetVendorPhotography(long.Parse(id)).Count;
                if (type == "EventManagement")
                    count = vendorVenueSignUpService.GetVendorEventOrganiser(long.Parse(id)).Count;
                if (type == "Decorator")
                    count = vendorVenueSignUpService.GetVendorDecorator(long.Parse(id)).Count;
                if (type == "Other")
                    count = vendorVenueSignUpService.GetVendorOther(long.Parse(id)).Count;
                if (count > 1)
                {
                    string msg = vendorVenueSignUpService.RemoveVendorService(vid, type);
                    string message = vendorImageService.DeleteAllImages(long.Parse(id), long.Parse(vid));

                    //  TempData["Active"] = "Service " + msg + "";
                    // return RedirectToAction("Index", "NVendorStoreFront", new { id = id });
                    return Content("<script language='javascript' type='text/javascript'>alert('Service " + msg + "');location.href='/NVendorStoreFront/Index?id=" + id + "'</script>");
                }
                else
                {
                    long value = vendorVenueSignUpService.UpdateVendorService(id, vid, type);
                    string message = vendorImageService.DeleteAllImages(long.Parse(id), long.Parse(vid));
                    if (value > 0)
                    {
                        //TempData["Active"] = "Service Removed";
                        // return RedirectToAction("Index", "NVendorStoreFront", new { id = id });
                        return Content("<script language='javascript' type='text/javascript'>alert('Service Removed');location.href='/NVendorStoreFront/Index?id=" + id + "'</script>");

                    }
                    else

                        // TempData["Active"] = "Something Went Wrong!!! Try Again After Some Time";
                        //return RedirectToAction("Index", "NVendorStoreFront", new { id = id });
                        return Content("<script language='javascript' type='text/javascript'>alert('Something Went Wrong!!! Try Again After Some Time');location.href='/NVendorStoreFront/Index?id=" + id + "'</script>");
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Nhomepage");
            }
        }
    }
}