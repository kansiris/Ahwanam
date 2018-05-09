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
        public static List<string> services = new List<string> { "Hotel", "Resort", "Convention Hall", "Catering", "Photography", "Decorator", "Mehendi", "Pandit" };
        public static List<string> selectedservices = services;
        //string services1 = { "Hotel,Resort,Convention Hall,Catering,Photography,Decorator,Mehendi,Pandit" };
        // GET: NResults
        VendorProductsService vendorProductsService = new VendorProductsService();
        public ActionResult Index(string type, string loc, string budget, string stype, string date, string count) //string f1, string f2, string f3, string f4, string f5, string f6, string f7, string f8, string f9,
        {
            ViewBag.count = 6;
            return View();
        }

        public ActionResult BlockOnePartial(string type, string loc, string budget, string stype, string date, string count, string L1)  //string f1, string f2, string f3, string f4, string f5, string f6, string f7, string f8, string f9,
        {
            loc = (loc != "undefined" && loc != "") ? loc : "Hyderabad";
            budget = (budget != "undefined" && budget != "") ? budget : "100";
            count = (count != "undefined" && count != "") ? count : "10";
            int takecount = (L1 != null) ? int.Parse(L1) : 6;
            //string inputcategory = (takecount > 6) ? type : services[0];
            //if (new string[] { "Hotel", "Resort", "Convention Hall", }.Contains(type))
            //{

            //}
            //if (new string[] { "Wedding", "Party", "Corporate", "BabyFunction", "Birthday", "Engagement" }.Contains(type))
            if(stype != null && stype != "")
            {
                if (new string[] { "Hotel", "Resort", "Convention Hall","Convention-Hall" }.Contains(stype.Split(',')[0]))
                {
                    string selectedtype = stype.Split(',')[0];
                    selectedtype = (stype.Split(',')[0] == "Convention-Hall") ? "Convention Hall" : stype.Split(',')[0];
                    var data = vendorProductsService.Getfiltervendors_Result(selectedtype, loc, budget, count);
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
                    if (data.Count > 0)
                        ViewBag.results = data.Take(takecount).ToList();
                    else
                        ViewBag.results = vendorProductsService.Getfiltervendors_Result("Venue", loc, "0", "0").ToList();
                    string type1 = selectedtype;
                    type = (stype.Split(',')[0] == "Convention Hall" || stype.Split(',')[0] == "Convention-Hall" || stype.Split(',')[0] == "Convention" || stype.Split(',')[0] == "Convetion") ? "ConventionHall" : type;
                    ViewBag.type = type;
                    ViewBag.type1 = selectedtype;
                    int recordcount = data.Count();
                    ViewBag.count = (recordcount >= takecount) ? "1" : "0";
                    ViewBag.recordcount = recordcount;
                }
                else
                {
                    var data = vendorProductsService.Getfiltervendors_Result(stype.Split(',')[0], loc, budget, count);
                    if (data.Count > 0)
                        ViewBag.results = data.Take(takecount).ToList();
                    else
                        ViewBag.results = vendorProductsService.Getfiltervendors_Result(stype.Split(',')[0], loc, "0", "0").ToList();
                    ViewBag.type = stype.Split(',')[0];
                    int recordcount = data.Count();
                    ViewBag.count = (recordcount >= takecount) ? "1" : "0";
                    ViewBag.recordcount = recordcount;
                }
                //if (takecount > 6)
                //{
                if (new string[] { "Hotel", "Resort", "Convetion" }.Contains(stype.Split(',')[0]))
                {
                    selectedservices.Remove("Hotel");
                    selectedservices.Remove("Resort");
                    selectedservices.Remove("Convention Hall");
                }
                else { selectedservices.Remove(stype.Split(',')[0]); };
                //}
                return PartialView();
            }

            if (new string[] { "Hotel", "Resort", "Convetion", "Convention", "BanquetHall", "FunctionHall" ,"Banquet","Function", "Banquet Hall", "Function Hall" }.Contains(type))
            {
                string type1 = type;
                type1 = (type1 == "Convetion" || type1 == "Convention") ? "Convention Hall" : type1;
                type1 = (type1 == "BanquetHall" || type1 == "Banquet") ? "Banquet Hall" : type1;
                type1 = (type1 == "FunctionHall" || type1 == "Function") ? "Function Hall" : type1;
                var data = vendorProductsService.Getfiltervendors_Result(type1, loc, budget, count);
                if (data.Count > 0)
                    ViewBag.results = data.Take(takecount).ToList();
                else
                    ViewBag.results = vendorProductsService.Getfiltervendors_Result(type1, loc, "0", "0").ToList();
                ViewBag.type1 = type1;
                type = (type == "Banquet Hall" || type == "Banquet") ? "BanquetHall" : type;
                type = (type == "Function Hall" || type == "Function") ? "FunctionHall" : type;
                ViewBag.type = type;
                int recordcount = data.Count();
                ViewBag.count = (recordcount >= takecount) ? "1" : "0";
                ViewBag.recordcount = recordcount;
            }
            else
            {
                //type = (type == "Banquet") ? "Banquet Hall" : type;
                //type = (type == "Function") ? "Function Hall" : type;
                var data = vendorProductsService.Getfiltervendors_Result(type, loc, budget, count);
                if (data.Count > 0)
                    ViewBag.results = data.Take(takecount).ToList();
                else
                    ViewBag.results = vendorProductsService.Getfiltervendors_Result(type, loc, "0", "0").ToList();
                ViewBag.type = type;
                int recordcount = data.Count();
                ViewBag.count = (recordcount >= takecount) ? "1" : "0";
                ViewBag.recordcount = ViewBag.results.Count;
            }
            //if (takecount > 6)
            //{
            if (new string[] { "Hotel", "Resort", "Convetion" }.Contains(type))
            {
                services.Remove("Hotel");
                services.Remove("Resort");
                services.Remove("Convention Hall");
            }
            else { services.Remove(type); };
            //}

            return PartialView();
        }

        public ActionResult BlockTwoPartial(string type, string loc, string budget, string stype, string date, string count, string L2)
        {
            int takecount = (L2 != null) ? int.Parse(L2) : 6;
            string inputcategory = null;
            if (stype == "")
                inputcategory = (takecount > 6) ? type : services[0];
            else
                inputcategory = (takecount > 6) ? type : selectedservices[0];
            if (stype.Split(',').Length > 1) inputcategory = stype.Split(',')[1];
            loc = (loc != "undefined" && loc != "") ? loc : "Hyderabad";
            budget = (budget != "undefined" && budget != "") ? budget : "100";
            count = (count != "undefined" && count != "") ? count : "10";
            var data = vendorProductsService.Getfiltervendors_Result(inputcategory, loc, "0", "0");
            if (stype != "")
                data = vendorProductsService.Getfiltervendors_Result(inputcategory, loc, budget, count);
            //ViewBag.results = data.Take(takecount).ToList();
            //if (data.Count > 0)
            ViewBag.results = data.Take(takecount).ToList();
            //else
            //    ViewBag.results = vendorProductsService.Getfiltervendors_Result(inputcategory, loc, "0", "0").ToList();
            int recordcount = data.Count();
            ViewBag.count = (recordcount >= takecount) ? "1" : "0";
            ViewBag.type = inputcategory;
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
                else { services.Remove(selectedservices[0]); };
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

        public ActionResult BlockThreePartial(string type, string loc, string budget, string stype, string date, string count, string L3)
        {
            int takecount = (L3 != null) ? int.Parse(L3) : 6;
            string inputcategory = null;
            if (stype == "")
                inputcategory = (takecount > 6) ? type : services[0];
            else
                inputcategory = (takecount > 6) ? type : selectedservices[0];
            if (stype.Split(',').Length > 2) inputcategory = stype.Split(',')[2];
            loc = (loc != "undefined" && loc != "") ? loc : "Hyderabad";
            budget = (budget != "undefined" && budget != "") ? budget : "100";
            var data = vendorProductsService.Getfiltervendors_Result(inputcategory, loc, "0", "0");
            if (stype != "")
                data = vendorProductsService.Getfiltervendors_Result(inputcategory, loc, budget, count);
            ViewBag.results = data.Take(takecount).ToList();
            int recordcount = data.Count();
            ViewBag.count = (recordcount >= takecount) ? "1" : "0";
            ViewBag.type = inputcategory;
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
                else { services.Remove(selectedservices[0]); };
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

        public ActionResult BlockFourPartial(string type, string loc, string budget, string stype, string date, string count, string L4)
        {
            int takecount = (L4 != null) ? int.Parse(L4) : 6;
            string inputcategory = null;
            if (stype == "")
                inputcategory = (takecount > 6) ? type : services[0];
            else
                inputcategory = (takecount > 6) ? type : selectedservices[0];
            if (stype.Split(',').Length > 3) inputcategory = stype.Split(',')[3];
            loc = (loc != "undefined" && loc != "") ? loc : "Hyderabad";
            budget = (budget != "undefined" && budget != "") ? budget : "100";
            var data = vendorProductsService.Getfiltervendors_Result(inputcategory, loc, "0", "0");
            if (stype != "")
                data = vendorProductsService.Getfiltervendors_Result(inputcategory, loc, budget, count);
            ViewBag.results = data.Take(takecount).ToList();
            int recordcount = data.Count();
            ViewBag.count = (recordcount >= takecount) ? "1" : "0";
            ViewBag.type = inputcategory;
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
                else { services.Remove(selectedservices[0]); };
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

            services = null;
            services = new List<string> { "Hotel", "Resort", "Convention Hall", "Catering", "Photography", "Decorator", "Mehendi", "Pandit" };
            selectedservices = services;
            return PartialView();
        }

        public ActionResult BlockFivePartial(string type, string loc, string budget, string stype, string date, string count, string L5)
        {
            int takecount = (L5 != null) ? int.Parse(L5) : 6;
            string inputcategory = null;
            if (stype == "")
                inputcategory = (takecount > 6) ? type : services[0];
            else
                inputcategory = (takecount > 6) ? type : selectedservices[0];
            if (stype.Split(',').Length > 4) inputcategory = stype.Split(',')[4];
            budget = (budget != "undefined" && budget != "") ? budget : "100";
            var data = vendorProductsService.Getfiltervendors_Result(inputcategory, loc, "0", "0");
            if (stype != "")
                data = vendorProductsService.Getfiltervendors_Result(inputcategory, loc, budget, count);
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
            int recordcount = data.Count();
            ViewBag.results = data.Take(takecount).ToList();
            ViewBag.type = inputcategory;
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
                else { services.Remove(selectedservices[0]); };
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
            services = new List<string> { "Hotel", "Resort", "Convention Hall", "Catering", "Photography", "Decorator", "Mehendi", "Pandit" };
            selectedservices = services;
            return PartialView();
            //}
            //return PartialView();
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