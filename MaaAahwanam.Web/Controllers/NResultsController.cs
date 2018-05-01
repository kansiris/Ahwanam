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
        string[] services = { };
        // GET: NResults
        VendorProductsService vendorProductsService = new VendorProductsService();
        public ActionResult Index(string type, string loc, string budget, string stype, string date, string count) //string f1, string f2, string f3, string f4, string f5, string f6, string f7, string f8, string f9,
        {
            ViewBag.count = 6;
            return View();
        }

        public ActionResult BlockOnePartial(string type, string loc, string budget, string stype, string date, string count, string L1)  //string f1, string f2, string f3, string f4, string f5, string f6, string f7, string f8, string f9,
        {
            int takecount = (L1 != null) ? int.Parse(L1) : 6;
            if (new string[] { "Wedding", "Party", "Corporate", "BabyFunction", "Birthday", "Engagement" }.Contains(type))
            {
                budget = (budget == "undefined" && budget == "") ? budget : "100";
                count = (count == "undefined" && count == "") ? count : "10";
                var data = vendorProductsService.Getfiltervendors_Result("Venue", loc, budget, count);
                string[] venuetypes = { "Convention-Hall", "Function Hall", "Banquet Hall", "Meeting Room", "Hotel", "Resort", "Convention Hall" };
                List<string> matchingvenues = venuetypes.Intersect(stype.Split(',')).ToList();//String.Join(",", venuetypes.Intersect(stype.Split(',')).ToList());
                if (stype.Split(',').Contains("Hotel") || stype.Split(',').Contains("Resort") || stype.Split(',').Contains("Convetion-Hall") || stype.Split(',').Contains("Convetion Hall"))
                {
                    var sortedlist = new List<filtervendors_Result>();
                    for (int i = 0; i < matchingvenues.Count; i++)
                    {
                        sortedlist.AddRange(data.Where(m => m.subtype == matchingvenues[i]).ToList());
                    }
                    data = sortedlist;
                }
                //data = vendorProductsService.Getfiltervendors_Result(matchingvenues, loc, budget, count);

                //else if (stype.Split(',').Contains("Resort"))
                //    data = vendorProductsService.Getfiltervendors_Result("Resort", loc, budget, count);
                //else if (stype.Split(',').Contains("Convetion Hall"))
                //    data = vendorProductsService.Getfiltervendors_Result("Convetion Hall", loc, budget, count);
                ViewBag.venues = data.Take(takecount).ToList();//.Where(m=>m.city == f1);
                ViewBag.type = "Venues";
                int recordcount = data.Count();
                ViewBag.count = (recordcount >= takecount) ? "1" : "0";
                return PartialView();
            }


            loc = (loc != "undefined" && loc != "") ? loc : "Hyderabad";
            budget = (budget != "undefined" && budget != "") ? budget : "100";
            count = (count != "undefined" && count != "") ? count : "10";

            if (new string[] { "Hotel", "Resort", "Convetion" }.Contains(type))
            {
                type = (type == "Convetion") ? "Convention Hall" : type;
                var data = vendorProductsService.Getfiltervendors_Result(type, loc, budget, count);
                ViewBag.venues = data.Take(takecount).ToList();
                ViewBag.type = type;
                int recordcount = data.Count();
                ViewBag.count = (recordcount >= takecount) ? "1" : "0";
            }
            else
            {
                var data = vendorProductsService.Getfiltervendors_Result("Venue", loc, budget, count);
                ViewBag.venues = data.Take(takecount).ToList();
                ViewBag.type = "Venues";
                int recordcount = data.Count();
                ViewBag.count = (recordcount >= takecount) ? "1" : "0";
            }

            return PartialView();
        }

        public ActionResult BlockTwoPartial(string type, string loc, string budget, string stype, string date, string count, string L2)
        {
            int takecount = (L2 != null) ? int.Parse(L2) : 6;
            loc = (loc != "undefined" && loc != "") ? loc : "Hyderabad";
            budget = (budget != "undefined" && budget != "") ? budget : "100";
            count = (count != "undefined" && count != "") ? count : "10";
            var data = vendorProductsService.Getfiltervendors_Result("Catering", loc, budget, count);
            //if (stype.Split(',').Contains("Catering"))
            //{
            //    data = vendorProductsService.Getfiltervendors_Result("Catering", loc, budget, count);
            //}

            ViewBag.Catering = data.Take(takecount).ToList();
            int recordcount = data.Count();
            ViewBag.count = (recordcount >= takecount) ? "1" : "0";
            return PartialView();
        }

        public ActionResult BlockThreePartial(string type, string loc, string budget, string stype, string date, string count, string L3)
        {
            int takecount = (L3 != null) ? int.Parse(L3) : 6;
            loc = (loc != "undefined" && loc != "") ? loc : "Hyderabad";
            budget = (budget != "undefined" && budget != "") ? budget : "100";
            var data = vendorProductsService.Getfiltervendors_Result("Decorator", loc, budget, "");
            //if (stype.Split(',').Contains("Decorator"))
            //{
            //    data = vendorProductsService.Getfiltervendors_Result("Decorator", loc, budget, "");
            //}
            ViewBag.Decorator = data.Take(takecount).ToList();
            int recordcount = data.Count();
            ViewBag.count = (recordcount >= takecount) ? "1" : "0";
            return PartialView();
        }

        public ActionResult BlockFourPartial(string type, string loc, string budget, string stype, string date, string count, string L4)
        {
            int takecount = (L4 != null) ? int.Parse(L4) : 6;
            loc = (loc != "undefined" && loc != "") ? loc : "Hyderabad";
            budget = (budget != "undefined" && budget != "") ? budget : "100";
            var data = vendorProductsService.Getfiltervendors_Result("Photography", loc, budget, "");
            //if (stype.Split(',').Contains("Photography"))
            //{
            //    data = vendorProductsService.Getfiltervendors_Result("Photography", loc, budget, "");
            //}
            ViewBag.Photography = data.Take(takecount).ToList();
            int recordcount = data.Count();
            ViewBag.count = (recordcount >= takecount) ? "1" : "0";
            return PartialView();
        }

        public ActionResult BlockFivePartial(string type, string loc, string budget, string stype, string date, string count, string L5)
        {
            int takecount = (L5 != null) ? int.Parse(L5) : 6;
            //if (new string[] { "Mehendi", "Pandit" }.Contains(type))
            //{
            //ViewBag.type = type;
            //if (budget != "") budget = budget; else budget = "100";
            budget = (budget != "undefined" && budget != "") ? budget : "100";
            //var data = vendorProductsService.Getfiltervendors_Result(stype, loc, budget, "").Where(m => m.subtype == stype);//.Where(m=>m.cit);
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
            ViewBag.others = data.Take(takecount).ToList();
            int recordcount = data.Count();
            ViewBag.count = (recordcount >= takecount) ? "1" : "0";
            return PartialView();
            //}
            //return PartialView();
        }



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