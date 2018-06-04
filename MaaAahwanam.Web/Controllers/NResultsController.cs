using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaaAahwanam.Service;
using System.Net;
using MaaAahwanam.Models;
using MaaAahwanam.Utility;
using MaaAahwanam.Repository;

namespace MaaAahwanam.Web.Controllers
{
    public class NResultsController : Controller
    {
        OrderService orderService = new OrderService();
        VendorDatesService vendorDatesService = new VendorDatesService();
        int stypecount = 0;
        public static List<string> services = new List<string> { "Hotel", "Resort", "Convention Hall", "Catering", "Photography", "Decorator", "Mehendi", "Pandit" };
        public static List<string> selectedservices = services;
        //string services1 = { "Hotel,Resort,Convention Hall,Catering,Photography,Decorator,Mehendi,Pandit" };
        // GET: NResults
        VendorProductsService vendorProductsService = new VendorProductsService();
        public ActionResult Index(string type, string loc, string budget, string stype, string date, string count) //string f1, string f2, string f3, string f4, string f5, string f6, string f7, string f8, string f9,
        {
            try
            {
                stypecount = stype.Split(',').Count();
                ViewBag.stype = stype.Split(',');
                string idslist = "BlockOnePartial,BlockTwoPartial,BlockThreePartial,BlockFourPartial,BlockFivePartial";
                List<string[]> list = new List<string[]>();
                if (stype != "")
                {
                    for (int i = 0; i < stypecount; i++)
                    {
                        list.Add(new string[] { idslist.Split(',')[i], stype.Split(',')[i] });
                    }
                }


                ViewBag.stypecount = stypecount;
                ViewBag.count = 6;
                ViewBag.stype = list;
                return View();
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Nhomepage");
            }
        }

        public ActionResult BlockOnePartial(string type, string loc, string budget, string stype, string date, string count, string L1)  //string f1, string f2, string f3, string f4, string f5, string f6, string f7, string f8, string f9,
        {
            try
            {
                loc = (loc != "undefined" && loc != "") ? loc : "Hyderabad";
                budget = (budget != "undefined" && budget != "") ? budget : "100";
                count = (count != "undefined" && count != "") ? count : "10";
                int takecount = (L1 != null) ? int.Parse(L1) : 6;
                //if (new string[] { "Wedding", "Party", "Corporate", "BabyFunction", "Birthday", "Engagement" }.Contains(type))
                if (stype != null && stype != "")
                {
                    if (new string[] { "Hotel", "Resort", "Convention Hall", "Convention-Hall", "ConventionHall" }.Contains(stype.Split(',')[0]))
                    {
                        string selectedtype = stype.Split(',')[0];
                        selectedtype = (stype.Split(',')[0] == "Convention-Hall" || stype.Split(',')[0] == "ConventionHall") ? "Convention Hall" : stype.Split(',')[0];
                        var data = vendorProductsService.Getfiltervendors_Result(selectedtype, loc, budget, count);
                        int recordcount = data.Count();
                        if (data.Count > 0)
                            data = data.Take(takecount).ToList();
                        else
                        { data = vendorProductsService.Getfiltervendors_Result("Venue", loc, "0", "0").ToList(); recordcount = data.Count(); }
                        string type1 = selectedtype;
                        ViewBag.results = data;
                        type = (stype.Split(',')[0] == "Convention Hall" || stype.Split(',')[0] == "Convention-Hall" || stype.Split(',')[0] == "Convention" || stype.Split(',')[0] == "Convetion") ? "ConventionHall" : type;
                        ViewBag.type = selectedtype;
                        ViewBag.type1 = selectedtype;
                        //int recordcount = data.Count();
                        ViewBag.count = (recordcount >= takecount) ? "1" : "0";
                        ViewBag.recordcount = recordcount;
                    }
                    else
                    {
                        var data = vendorProductsService.Getfiltervendors_Result(stype.Split(',')[0], loc, budget, count);
                        if (stype.Split(',')[0] == "Mehendi" || stype.Split(',')[0] == "Pandit")
                            data = data.Where(m => m.subtype == stype.Split(',')[0]).ToList();
                        int recordcount = data.Count();
                        if (data.Count > 0)
                            data = data.Take(takecount).ToList();
                        else
                        {
                            data = vendorProductsService.Getfiltervendors_Result(stype.Split(',')[0], loc, "0", "0").ToList();
                            if (stype.Split(',')[0] == "Mehendi" || stype.Split(',')[0] == "Pandit")
                                data = data.Where(m => m.subtype == stype.Split(',')[0]).ToList();
                            recordcount = data.Count();
                        }
                        ViewBag.results = data;
                        ViewBag.type = stype.Split(',')[0];
                        ViewBag.type1 = stype.Split(',')[0];

                        ViewBag.count = (recordcount >= takecount) ? "1" : "0";
                        ViewBag.recordcount = recordcount;
                    }
                    if (new string[] { "Hotel", "Resort", "Convetion" }.Contains(stype.Split(',')[0]))
                    {
                        selectedservices.Remove("Hotel");
                        selectedservices.Remove("Resort");
                        selectedservices.Remove("Convention Hall");
                    }
                    else { selectedservices.Remove(stype.Split(',')[0]); };
                    return PartialView();
                }

                if (new string[] { "Hotel", "Resort", "Convetion", "Convention", "BanquetHall", "FunctionHall", "Banquet", "Function", "Banquet Hall", "Function Hall", "ConventionHall" }.Contains(type))
                {
                    string type1 = type;
                    type1 = (type1 == "Convetion" || type1 == "Convention" || type1 == "ConventionHall") ? "Convention Hall" : type1;
                    type1 = (type1 == "BanquetHall" || type1 == "Banquet") ? "Banquet Hall" : type1;
                    type1 = (type1 == "FunctionHall" || type1 == "Function") ? "Function Hall" : type1;
                    var data = vendorProductsService.Getfiltervendors_Result(type1, loc, budget, count);
                    int recordcount = data.Count();
                    if (data.Count > 0)
                        data = data.Take(takecount).ToList();
                    else
                    { data = vendorProductsService.Getfiltervendors_Result(type1, loc, "0", "0").Take(takecount).ToList(); recordcount = data.Count(); }
                    ViewBag.type1 = type1;
                    ViewBag.results = data;
                    type = (type == "Banquet Hall" || type == "Banquet") ? "BanquetHall" : type;
                    type = (type == "Function Hall" || type == "Function") ? "FunctionHall" : type;
                    type = (type == "Convetion" || type == "Convention") ? "ConventionHall" : type;
                    ViewBag.type = type;

                    ViewBag.count = (recordcount >= takecount) ? "1" : "0";
                    ViewBag.recordcount = recordcount;
                }
                else
                {
                    //type = (type == "Banquet") ? "Banquet Hall" : type;
                    //type = (type == "Function") ? "Function Hall" : type;
                    string type1 = type;
                    var data = vendorProductsService.Getfiltervendors_Result(type, loc, budget, count);
                    if (type == "Mehendi" || type == "Pandit")
                        data = data.Where(m => m.subtype == type).ToList();
                    int recordcount = data.Count();
                    if (data.Count == 0)
                    {
                        data = vendorProductsService.Getfiltervendors_Result(type, loc, "0", "0").ToList();
                        if (type == "Mehendi" || type == "Pandit")
                            data = data.Where(m => m.subtype == type).ToList();
                        recordcount = data.Count();
                    }
                    ViewBag.type = type;
                    ViewBag.type1 = type;
                    ViewBag.results = data.Take(takecount).ToList();

                    ViewBag.count = (recordcount >= takecount) ? "1" : "0";
                    ViewBag.recordcount = data.Count;
                }
                if (new string[] { "Hotel", "Resort", "Convetion" }.Contains(type))
                {
                    services.Remove("Hotel");
                    services.Remove("Resort");
                    services.Remove("Convention Hall");
                }
                else { services.Remove(type); };
                return PartialView();
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Nhomepage");
            }
        }

        public ActionResult BlockTwoPartial(string type, string loc, string budget, string stype, string date, string count, string L2)
        {
            try
            {
                string type1 = "";
                int takecount = (L2 != null) ? int.Parse(L2) : 6;
                string inputcategory = null;
                if (stype == "")
                    inputcategory = (takecount > 6) ? type : services[0];
                else
                    inputcategory = (takecount > 6) ? type : selectedservices[0];
                if (stype.Split(',').Length > 1) inputcategory = stype.Split(',')[1];
                //if (takecount == 6) { if (stype.Split(',').Length > 2) inputcategory = stype.Split(',')[2]; }
                type1 = (inputcategory == "Convetion" || inputcategory == "Convention-Hall" || inputcategory == "Convention" || inputcategory == "ConventionHall") ? "Convention Hall" : inputcategory;
                if (takecount > 6) inputcategory = type1;
                loc = (loc != "undefined" && loc != "") ? loc : "Hyderabad";
                budget = (budget != "undefined" && budget != "") ? budget : "100";
                count = (count != "undefined" && count != "") ? count : "10";
                var data = vendorProductsService.Getfiltervendors_Result(inputcategory, loc, "0", "0");
                if (inputcategory == "Mehendi" || inputcategory == "Pandit")
                    data = data.Where(m => m.subtype == inputcategory).ToList();
                int recordcount = data.Count();
                if (stype != "")
                {
                    data = vendorProductsService.Getfiltervendors_Result(inputcategory, loc, budget, count);
                    recordcount = data.Count();
                    if (data.Count > 0)
                        data = data.Take(takecount).ToList();
                    //else
                    //    data = vendorProductsService.Getfiltervendors_Result(inputcategory, loc, "0", "0").ToList();
                    else
                    {
                        data = vendorProductsService.Getfiltervendors_Result(inputcategory, loc, "0", "0").ToList();
                        if (inputcategory == "Mehendi" || inputcategory == "Pandit")
                            data = data.Where(m => m.subtype == inputcategory).ToList();
                        recordcount = data.Count();
                    }
                }
                //ViewBag.results = data.Take(takecount).ToList();
                //if (data.Count > 0)
                ViewBag.results = data.Take(takecount).ToList();
                //else
                //    ViewBag.results = vendorProductsService.Getfiltervendors_Result(inputcategory, loc, "0", "0").ToList();

                ViewBag.count = (recordcount >= takecount) ? "1" : "0";
                inputcategory = (inputcategory == "Convention Hall" || inputcategory == "Convention-Hall" || inputcategory == "Convention" || inputcategory == "Convetion") ? "ConventionHall" : inputcategory;
                ViewBag.type = inputcategory;
                ViewBag.type1 = type1;
                ViewBag.recordcount = recordcount;
                //if (takecount > 6)
                //{
                if (stype != null && stype != "")
                {
                    if (new string[] { "Hotel", "Resort", "Convetion" }.Contains(selectedservices[0]))
                    {
                        selectedservices.Remove("Hotel");
                        selectedservices.Remove("Resort");
                        selectedservices.Remove("Convention Hall");
                    }
                    else { selectedservices.Remove(selectedservices[0]); };
                }
                else
                {
                    if (new string[] { "Hotel", "Resort", "Convetion" }.Contains(services[0]))
                    {
                        services.Remove("Hotel");
                        services.Remove("Resort");
                        services.Remove("Convention Hall");
                    }
                    else { services.Remove(services[0]); };
                }
                //}

                return PartialView();
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Nhomepage");
            }
        }

        public ActionResult BlockThreePartial(string type, string loc, string budget, string stype, string date, string count, string L3)
        {
            try
            {
                string type1 = "";
                int takecount = (L3 != null) ? int.Parse(L3) : 6;
                string inputcategory = null;
                if (stype == "")
                    inputcategory = (takecount > 6) ? type : services[0];
                else
                    inputcategory = (takecount > 6) ? type : selectedservices[0];
                if (stype.Split(',').Length > 2) inputcategory = stype.Split(',')[2];
                //if (takecount == 6) { if (stype.Split(',').Length > 2) inputcategory = stype.Split(',')[2]; }
                type1 = (inputcategory == "Convetion" || inputcategory == "Convention-Hall" || inputcategory == "Convention" || inputcategory == "ConventionHall") ? "Convention Hall" : inputcategory;
                if (takecount > 6) inputcategory = type1;
                loc = (loc != "undefined" && loc != "") ? loc : "Hyderabad";
                budget = (budget != "undefined" && budget != "") ? budget : "100";
                var data = vendorProductsService.Getfiltervendors_Result(inputcategory, loc, "0", "0");
                if (inputcategory == "Mehendi" || inputcategory == "Pandit")
                    data = data.Where(m => m.subtype == inputcategory).ToList();
                int recordcount = data.Count();
                //if (stype != "")
                //    data = vendorProductsService.Getfiltervendors_Result(inputcategory, loc, budget, count);
                if (stype != "")
                {
                    data = vendorProductsService.Getfiltervendors_Result(inputcategory, loc, budget, count);
                    recordcount = data.Count();
                    if (data.Count > 0)
                        data = data.Take(takecount).ToList();
                    else
                    {
                        data = vendorProductsService.Getfiltervendors_Result(inputcategory, loc, "0", "0").ToList();
                        if (inputcategory == "Mehendi" || inputcategory == "Pandit")
                            data = data.Where(m => m.subtype == inputcategory).ToList();
                        recordcount = data.Count();
                    }
                }
                ViewBag.results = data.Take(takecount).ToList();

                ViewBag.count = (recordcount >= takecount) ? "1" : "0";
                inputcategory = (inputcategory == "Convention Hall" || inputcategory == "Convention-Hall" || inputcategory == "Convention" || inputcategory == "Convetion") ? "ConventionHall" : inputcategory;
                ViewBag.type = inputcategory;
                ViewBag.type1 = type1;
                ViewBag.recordcount = recordcount;
                //if (takecount > 6)
                //{
                if (stype != null && stype != "")
                {
                    if (new string[] { "Hotel", "Resort", "Convetion" }.Contains(selectedservices[0]))
                    {
                        selectedservices.Remove("Hotel");
                        selectedservices.Remove("Resort");
                        selectedservices.Remove("Convention Hall");
                    }
                    else { selectedservices.Remove(selectedservices[0]); };
                }
                else
                {
                    if (new string[] { "Hotel", "Resort", "Convetion" }.Contains(services[0]))
                    {
                        services.Remove("Hotel");
                        services.Remove("Resort");
                        services.Remove("Convention Hall");
                    }
                    else { services.Remove(services[0]); };
                }
                //}

                return PartialView();
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Nhomepage");
            }
        }

        public ActionResult BlockFourPartial(string type, string loc, string budget, string stype, string date, string count, string L4)
        {
            try
            {
                string type1 = "";
                stypecount = stype.Split(',').Count();
                int takecount = (L4 != null) ? int.Parse(L4) : 6;
                string inputcategory = null;
                if (stype == "")
                    inputcategory = (takecount > 6) ? type : services[0];
                else
                    inputcategory = (takecount > 6) ? type : selectedservices[0];
                if (stype.Split(',').Length > 3) inputcategory = stype.Split(',')[3];
                //if (takecount == 6) { if (stype.Split(',').Length > 2) inputcategory = stype.Split(',')[2]; }
                type1 = (inputcategory == "Convetion" || inputcategory == "Convention-Hall" || inputcategory == "Convention" || inputcategory == "ConventionHall") ? "Convention Hall" : inputcategory;
                if (takecount > 6) inputcategory = type1;
                loc = (loc != "undefined" && loc != "") ? loc : "Hyderabad";
                budget = (budget != "undefined" && budget != "") ? budget : "100";
                var data = vendorProductsService.Getfiltervendors_Result(inputcategory, loc, "0", "0");
                if (inputcategory == "Mehendi" || inputcategory == "Pandit")
                    data = data.Where(m => m.subtype == inputcategory).ToList();
                int recordcount = data.Count();
                if (stype != "")
                {
                    data = vendorProductsService.Getfiltervendors_Result(inputcategory, loc, budget, count);
                    recordcount = data.Count();
                    if (data.Count > 0)
                        data = data.Take(takecount).ToList();
                    else
                    {
                        data = vendorProductsService.Getfiltervendors_Result(inputcategory, loc, "0", "0").ToList();
                        if (inputcategory == "Mehendi" || inputcategory == "Pandit")
                            data = data.Where(m => m.subtype == inputcategory).ToList();
                        recordcount = data.Count();
                    }
                }
                ViewBag.results = data.Take(takecount).ToList();

                ViewBag.count = (recordcount >= takecount) ? "1" : "0";
                inputcategory = (inputcategory == "Convention Hall" || inputcategory == "Convention-Hall" || inputcategory == "Convention" || inputcategory == "Convetion") ? "ConventionHall" : inputcategory;
                ViewBag.type = inputcategory;
                ViewBag.type1 = type1;
                ViewBag.recordcount = recordcount;
                if (stype != null && stype != "")
                {
                    if (new string[] { "Hotel", "Resort", "Convetion" }.Contains(selectedservices[0]))
                    {
                        selectedservices.Remove("Hotel");
                        selectedservices.Remove("Resort");
                        selectedservices.Remove("Convention Hall");
                    }
                    else { selectedservices.Remove(selectedservices[0]); };
                }
                else
                {
                    if (new string[] { "Hotel", "Resort", "Convetion" }.Contains(services[0]))
                    {
                        services.Remove("Hotel");
                        services.Remove("Resort");
                        services.Remove("Convention Hall");
                    }
                    else { services.Remove(services[0]); };
                }
                if (stypecount <= 4)
                {
                    services = null;
                    services = new List<string> { "Hotel", "Resort", "Convention Hall", "Catering", "Photography", "Decorator", "Mehendi", "Pandit" };
                    selectedservices = services;
                }
                return PartialView();
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Nhomepage");
            }
        }

        public ActionResult BlockFivePartial(string type, string loc, string budget, string stype, string date, string count, string L5)
        {
            try
            {
                string type1 = "";
                stypecount = stype.Split(',').Count();
                int takecount = (L5 != null) ? int.Parse(L5) : 6;
                string inputcategory = null;
                if (stype == "")
                    inputcategory = (takecount > 6) ? type : services[0];
                else
                    inputcategory = (takecount > 6) ? type : selectedservices[0];
                if (stype.Split(',').Length > 4) inputcategory = stype.Split(',')[4];
                //if (takecount == 6) { if (stype.Split(',').Length > 2) inputcategory = stype.Split(',')[2]; }
                type1 = (inputcategory == "Convetion" || inputcategory == "Convention-Hall" || inputcategory == "Convention" || inputcategory == "ConventionHall") ? "Convention Hall" : inputcategory;
                if (takecount > 6) inputcategory = type1;
                budget = (budget != "undefined" && budget != "") ? budget : "100";
                var data = vendorProductsService.Getfiltervendors_Result(inputcategory, loc, "0", "0");
                if (inputcategory == "Mehendi" || inputcategory == "Pandit")
                    data = data.Where(m => m.subtype == inputcategory).ToList();
                int recordcount = data.Count();
                if (stype != "")
                {
                    data = vendorProductsService.Getfiltervendors_Result(inputcategory, loc, budget, count);
                    recordcount = data.Count();
                    if (data.Count > 0)
                        data = data.Take(takecount).ToList();
                    else
                    {
                        data = vendorProductsService.Getfiltervendors_Result(inputcategory, loc, "0", "0").ToList();
                        if (inputcategory == "Mehendi" || inputcategory == "Pandit")
                            data = data.Where(m => m.subtype == inputcategory).ToList();
                        recordcount = data.Count();
                    }
                }


                ViewBag.results = data.Take(takecount).ToList();
                inputcategory = (inputcategory == "Convention Hall" || inputcategory == "Convention-Hall" || inputcategory == "Convention" || inputcategory == "Convetion") ? "ConventionHall" : inputcategory;
                ViewBag.type = inputcategory;
                ViewBag.type1 = type1;
                ViewBag.recordcount = recordcount;
                if (stype != null && stype != "")
                {
                    if (new string[] { "Hotel", "Resort", "Convetion" }.Contains(selectedservices[0]))
                    {
                        selectedservices.Remove("Hotel");
                        selectedservices.Remove("Resort");
                        selectedservices.Remove("Convention Hall");
                    }
                    else { selectedservices.Remove(selectedservices[0]); };
                }
                else
                {
                    if (new string[] { "Hotel", "Resort", "Convetion" }.Contains(services[0]))
                    {
                        services.Remove("Hotel");
                        services.Remove("Resort");
                        services.Remove("Convention Hall");
                    }
                    else { services.Remove(services[0]); };
                }
                if (stypecount > 4)
                {
                    services = null;
                    services = new List<string> { "Hotel", "Resort", "Convention Hall", "Catering", "Photography", "Decorator", "Mehendi", "Pandit" };
                    selectedservices = services;
                }
                return PartialView();
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Nhomepage");
            }
        }

        public ActionResult GetVendorDates(string id,string vid,string type)
        {
            ProductInfoService productInfoService = new ProductInfoService();
            string orderdates = productInfoService.disabledate(long.Parse(id), long.Parse(vid), type);

            //Blocking Dates
            //var vendorid = userLoginDetailsService.GetLoginDetailsByEmail(Productinfo.EmailId);

            var betweendates = new List<string>();
            var Gettotaldates = vendorDatesService.GetDates(long.Parse(id), long.Parse(vid));
            int recordcount = Gettotaldates.Count();
            foreach (var item in Gettotaldates)
            {
                var startdate = Convert.ToDateTime(item.StartDate);
                var enddate = Convert.ToDateTime(item.EndDate);
                if (startdate != enddate)
                {
                    for (var dt = startdate; dt <= enddate; dt = dt.AddDays(1))
                    {
                        betweendates.Add(dt.ToString("dd-MM-yyyy"));
                    }
                }
                else
                {
                    betweendates.Add(startdate.ToString("dd-MM-yyyy"));
                }

                //ViewBag.vendoravailabledates = String.Join(",", betweendates,orderdates);
                var vendoravailabledates = String.Join(",", betweendates);
                vendoravailabledates = vendoravailabledates + "," + String.Join(",", orderdates);
                ViewBag.vendoravailabledates = vendoravailabledates;
                return Json(vendoravailabledates, JsonRequestBehavior.AllowGet);
            }
            return Json("Failed", JsonRequestBehavior.AllowGet);
        }

        #region Reference Code

        public void venue(string type, string loc, string budget, string count, int takecount)
        {

            if (new string[] { "Hotel", "Resort", "Convetion" }.Contains(type))
            {
                type = (type == "Convetion") ? "Convention Hall" : type;
                var data = vendorProductsService.Getfiltervendors_Result(type, loc, budget, count);
                ViewBag.results = data.Take(takecount).ToList();
                ViewBag.type = type;
                int recordcount = data.Count();
                ViewBag.count = (recordcount >= takecount) ? "1" : "0";
            }
            else
            {
                var data = vendorProductsService.Getfiltervendors_Result("Venue", loc, budget, count);
                ViewBag.results = data.Take(takecount).ToList();
                ViewBag.type = "Venue";
                int recordcount = data.Count();
                ViewBag.count = (recordcount >= takecount) ? "1" : "0";
            }
        }

        public void catering(string type, string loc, string budget, string count, int takecount)
        {

            var data = vendorProductsService.Getfiltervendors_Result("Catering", loc, budget, count);
            ViewBag.results = data.Take(takecount).ToList();
            int recordcount = data.Count();
            ViewBag.count = (recordcount >= takecount) ? "1" : "0";
        }

        public void Decorator(string type, string loc, string budget, string count, int takecount)
        {
            ViewBag.type = "Decorator";
            var data = vendorProductsService.Getfiltervendors_Result("Decorator", loc, budget, "");
            ViewBag.results = data.Take(takecount).ToList();
            int recordcount = data.Count();
            ViewBag.count = (recordcount >= takecount) ? "1" : "0";
        }

        public void photography(string type, string loc, string budget, string count, int takecount)
        {
            ViewBag.type = "Photography";
            var data = vendorProductsService.Getfiltervendors_Result("Photography", loc, budget, "");
            ViewBag.results = data.Take(takecount).ToList();
            int recordcount = data.Count();
            ViewBag.count = (recordcount >= takecount) ? "1" : "0";
        }

        public void other(string type, string loc, string budget, string count, int takecount, string stype)
        {
            ViewBag.type = type;
            var data = vendorProductsService.Getfiltervendors_Result("Other", loc, budget, count);
            if (stype != "")
            {
                string[] venuetypes = { "Mehendi", "Pandit" };
                List<string> matchingvenues = venuetypes.Intersect(stype.Split(',')).ToList();//String.Join(",", venuetypes.Intersect(stype.Split(',')).ToList());
                if (stype.Split(',').Contains("Mehendi") || stype.Split(',').Contains("Pandit"))
                {
                    var sortedlist = new List<filtervendors_Result>();
                    for (int i = 0; i < matchingvenues.Count; i++)
                    {
                        sortedlist.AddRange(data.Where(m => m.subtype == matchingvenues[i]).ToList());
                    }
                    data = sortedlist;
                }

                ViewBag.type = String.Join(",", matchingvenues);

            }
            else
            {
                data = data.Where(m => m.subtype == type).ToList();
                ViewBag.type = type;
            }
            ViewBag.results = data.Take(takecount).ToList();
            int recordcount = data.Count();
            ViewBag.count = (recordcount >= takecount) ? "1" : "0";
        }

        #endregion

        #region previous code

        //public ActionResult BlockOnePartial(string type, string f1, string f2, string f3, string f4, string f5, string f6, string f7, string f8, string f9, string L1, string loc)
        //{
        //    int takecount = (L1 != null) ? int.Parse(L1) : 6;
        //    if (new string[] { "Wedding", "Party", "Corporate", "BabyFunction", "Birthday", "Engagement" }.Contains(type))
        //    {
        //        f4 = (f4 == "undefined" && f4 == "") ? f4 : "100";
        //        f5 = (f5 == "undefined" && f5 == "") ? f5 : "10";
        //        var data = vendorProductsService.Getfiltervendors_Result(type, "", "", "", f4, f5, "", "", "", "");
        //        ViewBag.venues = data.Take(takecount);//.Where(m=>m.city == f1);
        //        ViewBag.type = type;
        //        int count = data.Count();
        //        ViewBag.count = (count >= takecount) ? "1" : "0";
        //        return PartialView();
        //    }

        //    if (new string[] { "Mehendi", "Pandit" }.Contains(type))
        //    {
        //        f4 = (f4 == "undefined" && f4 == "" && int.Parse(f4) > 0) ? f4 : "1";
        //        ViewBag.type = type;
        //        if (f5 != "") f5 = f5.Split('-')[1].Trim(); else f5 = "100";
        //        var data = vendorProductsService.Getfiltervendors_Result(type, "", "", "", f4, f5, "", "", "", "");//.Where(m=>m.cit);
        //        ViewBag.others = data.Take(takecount);
        //        int count = data.Count();
        //        ViewBag.count = (count >= takecount) ? "1" : "0";
        //        return PartialView();
        //    }
        //    f1 = (f1 == "undefined" && f1 == "") ? f1 : "Yes";
        //    f2 = (f2 == "undefined" && f2 == "") ? f2 : "Yes";
        //    f3 = (f3 == "undefined" && f3 == "") ? f3 : "Yes";
        //    f4 = (f4 == "undefined" && f4 == "") ? f4 : "100";
        //    f5 = (f5 == "undefined" && f5 == "") ? f5.Split('-')[1].Trim() : "100";
        //    f6 = (f6 == "undefined" && f6 == "") ? f6 : "Yes";
        //    f7 = (f7 == "undefined" && f7 == "") ? f7 : "Yes";
        //    f8 = (f8 == "undefined" && f8 == "") ? f8 : "Yes";
        //    f9 = (f9 == "undefined" && f9 == "") ? f9 : "Yes";
        //    //count = 
        //    if (new string[] { "Hotel", "Resort", "Convetion" }.Contains(type))
        //    {
        //        type = (type == "Convetion") ? "Convention Hall" : type;
        //        var data = vendorProductsService.Getfiltervendors_Result(type, f1, f2, f3, f4, f5, f6, f7, f8, f9);
        //        ViewBag.venues = data.Take(takecount);
        //        ViewBag.type = type;
        //        int count = data.Count();
        //        ViewBag.count = (count >= takecount) ? "1" : "0";
        //    }
        //    else
        //    {
        //        var data = vendorProductsService.Getfiltervendors_Result("Venue", f1, f2, f3, f4, f5, f6, f7, f8, f9);
        //        ViewBag.venues = data.Take(takecount);
        //        ViewBag.type = "Venues";
        //        int count = data.Count();
        //        ViewBag.count = (count >= takecount) ? "1" : "0";
        //    }

        //    return PartialView();
        //}

        //public ActionResult BlockTwoPartial(string type, string f1, string f2, string f3, string f4, string f5, string f6, string f7, string f8, string f9, string L2, string loc)
        //{
        //    int takecount = (L2 != null) ? int.Parse(L2) : 6;
        //    f2 = (f2 == "undefined" && f2 == "") ? f2.Split('-')[1].Trim() : "100";
        //    f3 = (f3 == "undefined" && f3 == "") ? f3 : "Yes";
        //    f5 = (f5 == "undefined" && f5 == "") ? f5 : "Yes";
        //    f6 = (f6 == "undefined" && f6 == "") ? f6 : "Yes";
        //    var data = vendorProductsService.Getfiltervendors_Result("Catering", "", f2, f3, "", f5, f6, "", "", "");
        //    ViewBag.Catering = data.Take(takecount);
        //    int count = data.Count();
        //    ViewBag.count = (count >= takecount) ? "1" : "0";
        //    return PartialView();
        //}

        //public ActionResult BlockThreePartial(string type, string f1, string f2, string f3, string f4, string f5, string f6, string f7, string f8, string f9, string L3, string loc)
        //{
        //    int takecount = (L3 != null) ? int.Parse(L3) : 6;
        //    f2 = (f2 == "undefined" && f2 == "") ? f2 : "Yes";
        //    f3 = (f3 == "undefined" && f3 == "") ? f3 : "Yes";
        //    f4 = (f4 == "undefined" && f4 == "") ? f4 : "100";
        //    f5 = (f5 == "undefined" && f5 == "") ? f5 : "Yes";
        //    f6 = (f6 == "undefined" && f6 == "") ? f6 : "Yes";
        //    var data = vendorProductsService.Getfiltervendors_Result("Decorator", "", f2, f3, f4, f5, f6, "", "", "");
        //    ViewBag.Decorator = data.Take(takecount);
        //    int count = data.Count();
        //    ViewBag.count = (count >= takecount) ? "1" : "0";
        //    return PartialView();
        //}

        //public ActionResult BlockFourPartial(string type, string f1, string f2, string f3, string f4, string f5, string f6, string f7, string f8, string f9, string L4, string loc)
        //{
        //    int takecount = (L4 != null) ? int.Parse(L4) : 6;
        //    f2 = (f2 == "undefined" && f2 == "") ? f2 : "Yes";
        //    f3 = (f3 == "undefined" && f3 == "") ? f3 : "Yes";
        //    f4 = (f4 == "undefined" && f4 == "") ? f4 : "100";
        //    var data = vendorProductsService.Getfiltervendors_Result("Photography", "", f2, f3, f4, "", "", "", "", "");
        //    ViewBag.Photography = data.Take(takecount);
        //    int count = data.Count();
        //    ViewBag.count = (count >= takecount) ? "1" : "0";
        //    return PartialView();
        //}

        #endregion

        public void parameterdescription()
        {
            //f1 ---> Location 
            //f2 ---> Budget
            //f3 ---> Seating Capacity
            //f4 ---> date
            //f5 ---> venuetype or types
        }
    }
}

//BlockOnePartial
//List Add Range Example
//string[] venuetypes = { "Convention-Hall", "Function Hall", "Banquet Hall", "Meeting Room", "Hotel", "Resort", "Convention Hall" };
//List<string> matchingvenues = venuetypes.Intersect(stype.Split(',')).ToList();
//if (stype.Split(',').Contains("Hotel") || stype.Split(',').Contains("Resort") || stype.Split(',').Contains("Convetion-Hall") || stype.Split(',').Contains("Convetion Hall"))
//{
//    var sortedlist = new List<filtervendors_Result>();
//    for (int i = 0; i < matchingvenues.Count; i++)
//    {
//        sortedlist.AddRange(data.Where(m => m.subtype == matchingvenues[i]).ToList());
//    }
//    data = sortedlist;
//}
//ViewBag.results = data.Take(takecount).ToList();//.Where(m=>m.city == f1);

//BlockFivePartial
//if (stype != "")
//{
//    string[] venuetypes = { "Mehendi", "Pandit" };
//    List<string> matchingvenues = venuetypes.Intersect(stype.Split(',')).ToList();//String.Join(",", venuetypes.Intersect(stype.Split(',')).ToList());
//    if (stype.Split(',').Contains("Mehendi") || stype.Split(',').Contains("Pandit"))
//    {
//        var sortedlist = new List<filtervendors_Result>();
//        for (int i = 0; i < matchingvenues.Count; i++)
//        {
//            sortedlist.AddRange(data.Where(m => m.subtype == matchingvenues[i]).ToList());
//        }
//        data = sortedlist;
//    }

//    ViewBag.type = String.Join(",", matchingvenues);

//}
//else
//{
//    data = data.Where(m => m.subtype == type).ToList();
//    ViewBag.type = type;
//}