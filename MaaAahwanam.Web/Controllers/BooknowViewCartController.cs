﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaaAahwanam.Service;

namespace MaaAahwanam.Web.Controllers
{
    public class BooknowViewCartController : Controller
    {
        DashBoardService dashBoardService = new DashBoardService();
        public ActionResult Index(string id)
        {
            if (id != null)
            {
                var data  = dashBoardService.GetOrderDetailService(long.Parse(id));
                ViewBag.OrderDetail = data;
                ViewBag.subtotal = data.Sum(m=>m.TotalPrice);
            }
            return View();
        }
	}
}