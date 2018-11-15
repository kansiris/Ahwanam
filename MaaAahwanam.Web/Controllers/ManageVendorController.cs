using MaaAahwanam.Models;
using MaaAahwanam.Service;
using MaaAahwanam.Web.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MaaAahwanam.Web.Controllers
{
    public class ManageVendorController : Controller
    {
        Vendormaster vendorMaster = new Vendormaster();
        private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        VendorDashBoardService mngvendorservice = new VendorDashBoardService();
        newmanageuser newmanageuse = new newmanageuser();

        // GET: ManageVendor
        [HttpGet]
        public ActionResult Index(string VendorId)
        {
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
                string uid = user.UserId.ToString();
                string vemail = newmanageuse.Getusername(long.Parse(uid));
                vendorMaster = newmanageuse.GetVendorByEmail(vemail);
                VendorId = vendorMaster.Id.ToString();
                ViewBag.masterid = VendorId;
                ViewBag.vendorlist = mngvendorservice.getvendor(VendorId);
                string S = "Services";
                ViewBag.s = S;
                
                 ViewBag.SupplierServicesLst = mngvendorservice.getsupplierservices(VendorId); 
                               
                
            }
            return View();
        }

        [HttpPost]
        public JsonResult Index(ManageVendor mngvendor, string id, string command)
        {
            string msg = string.Empty;
            mngvendor.registereddate = DateTime.Now;
            mngvendor.updateddate = DateTime.Now;
            if (command == "Save")
            {
                mngvendor = mngvendorservice.SaveVendor(mngvendor);
                msg = "Added New vendor";
            }
            else if (command == "Update")
            {
                mngvendor = mngvendorservice.UpdateVendor(mngvendor, int.Parse(id));
                msg = "Updated vendor";
            }
            //return Content("<script language='javascript' type='text/javascript'>alert('" + msg + "');location.href='/ManageVendor'</script>");
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetVendorDetails(string id)
        {
            var data = mngvendorservice.getvendorbyid(int.Parse(id));
            return Json(data);
        }
        //[HttpPost]
        //public JsonResult UpdateVendorDetails(ManageVendor mngvendor, string id)
        //{
        //    mngvendor.updateddate = DateTime.Now;
        //    mngvendor = mngvendorservice.UpdateVendor(mngvendor, int.Parse(id));
        //    return Json("Sucess", JsonRequestBehavior.AllowGet);
        //}
        public JsonResult checkVendoremail(string email, string id)
        {
            int query = mngvendorservice.checkvendoremail(email, id);
            if (query == 0)
                return Json("success");
            else
                return Json("sucess1");
        }
        [HttpPost]
        public JsonResult SupplierServices(AllSupplierServices supplierservices,string id, string command)
        {
            string msg = string.Empty;
            supplierservices.UpdatedDate = DateTime.Now;
            if (command == "Save") {
                supplierservices = mngvendorservice.AddSupplierServices(supplierservices);
                msg = "Added New Services";
            }
            else if (command == "Update")
            {
                supplierservices = mngvendorservice.updatesupplierservices(supplierservices, Convert.ToInt32(id));
                msg = "Updated Services";
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        public JsonResult checksupplierservices(string servicename,string vid)
        {
           int services = mngvendorservice.checksupplierservices(servicename, vid);
            if(services == 0)
            {
                return Json("success");
            }
            else { return Json("sucess1"); }
        }

        public JsonResult Getsupplierservice(string id)
        {
            var data = mngvendorservice.getsuplierservicesbyid(Convert.ToInt32(id));
            return Json(data);
        }
    }
}