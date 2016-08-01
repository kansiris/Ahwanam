﻿using MaaAahwanam.Models;
using MaaAahwanam.Service;
using MaaAahwanam.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MaaAahwanam.Web.Controllers
{
    public class AvailableDatesController : Controller
    {
        AvailabledatesService availabledatesService = new AvailabledatesService();
        // GET: AvailableDates
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(Availabledates availabledates)
        {
            availabledates.vendorId = ValidUserUtility.ValidUser();
            string a=availabledatesService.saveavailabledates(availabledates);
            if(a== "Success")
            {
                return Content("<script language='javascript' type='text/javascript'>alert('Dates Submitted Successfully');location.href='" + @Url.Action("Index", "AvailableDates") + "'</script>");
            }
            else
            {
                return Content("<script language='javascript' type='text/javascript'>alert('Failed to Submitted dates');location.href='" + @Url.Action("Index", "AvailableDates") + "'</script>");
            }
            return View();
        }
    }
}