﻿using MaaAahwanam.Models;
using MaaAahwanam.Service;
using MaaAahwanam.Web.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MaaAahwanam.Web.Controllers
{
    public class ManageStaffController : Controller
    {
        newmanageuser newmanageuse = new newmanageuser();
        Vendormaster vendorMaster = new Vendormaster();
        private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        VendorDashBoardService mnguserservice = new VendorDashBoardService();

        // GET: ManageStaff
        public ActionResult Index()
        {
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
                string uid = user.UserId.ToString();
                string vemail = newmanageuse.Getusername(long.Parse(uid));
                vendorMaster = newmanageuse.GetVendorByEmail(vemail);
             string   VendorId = vendorMaster.Id.ToString();
                ViewBag.masterid = VendorId;
                ViewBag.Userlist = mnguserservice.getuser(VendorId);
               
            }
            return View();
        }

        [HttpPost]
        public JsonResult Index(StaffAccess Staffsccess, string id, string command,string kscadd)
        {
            string msg = string.Empty;
            Staffsccess.UpdatedDate = DateTime.Now;
            Staffsccess.RegisteredDate = DateTime.Now;
            if (command == "Save")
            {
                Staffsccess = newmanageuse.Savestaff(Staffsccess);
                msg = "Added New staff";
            }
            else if (command == "Update")
            {
                Staffsccess = newmanageuse.updatestaff(Staffsccess, int.Parse(id));
                msg = "Updated staff";
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
    }
}