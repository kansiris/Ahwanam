using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaaAahwanam.Service;
using MaaAahwanam.Repository;
using MaaAahwanam.Web.Custom;
using System.IO;
using MaaAahwanam.Utility;
using MaaAahwanam.Models;
using System.Text;

namespace MaaAahwanam.Web.Controllers
{
    public class resultsController : Controller
    {
        UserLoginDetailsService userLoginDetailsService = new UserLoginDetailsService();
        Vendormaster vendorMaster = new Vendormaster();
        ResultsPageService resultsPageService = new ResultsPageService();
        // Homeco
        // GET: results
        public ActionResult Index(string type, string loc, string eventtype, string count, string date)
        {
            type = (type == null) ? "Venue" : type;
            var data = resultsPageService.GetAllVendors(type);
            ViewBag.venues = data.Take(6).ToList();
            ViewBag.minprice = data.Select(m => m.cost1).Min();
            ViewBag.maxprice = data.Select(m => m.cost1).Max();
            ViewBag.count = 6;
            SendEmail(type, count, loc, eventtype, date); // An Email will be sent here...
            return View();
        }

        public PartialViewResult Loadmore(string count, string type, string to_from)

        {
            type = (type == null || type == "") ? "Venue" : type;
            var selectedservices = type.Split(',');
            ViewBag.count = 6;

            if (to_from == null) {
                ViewBag.venues = vendorlist(6, selectedservices, "first", 6); //list; //resultsPageService.GetAllVendors(type).Take(takecount).ToList();

            }
            else
            {
                var to_from1 = to_from.Split(',');
                var min = to_from1[0];
                var max = to_from1[1];
                var data = vendorlist(6, selectedservices, "first", 6).Where(m => m.cost1 >= decimal.Parse(min) && m.cost1 <= decimal.Parse(max)).ToList(); //list; //resultsPageService.GetAllVendors(type).Take(takecount).ToList();
                ViewBag.venues = data;
            }
            return PartialView("Loadmore");
        }


       
        public JsonResult LazyLoad(string count, string type, string slider)
        {
            type = (type == null || type == "") ? "Venue" : type;
            var selectedservices = type.Split(',');
            int takecount = (count == "" || count == null) ? 6 : int.Parse(count) * 6;
            ViewBag.count = takecount;
            List<GetVendors_Result> vendorslist;
            if (takecount == 6)
                vendorslist = vendorlist(12, selectedservices, "first", takecount);//resultsPageService.GetAllVendors(type).Take(12).ToList(); 
            else
                vendorslist = vendorlist(6, selectedservices, "next", takecount);//resultsPageService.GetAllVendors(type).Skip(takecount).Take(6).ToList(); 
            return Json(vendorslist);
        }

        public List<GetVendors_Result> vendorlist(int count, string[] selectedservices, string command, int takecount)
        {
            List<GetVendors_Result> list = new List<GetVendors_Result>();
            int recordcount = 0;
            recordcount = count / selectedservices.Count();
            takecount = takecount / selectedservices.Count();
            for (int i = 0; i < selectedservices.Count(); i++)
            {
                selectedservices[i] = (selectedservices[i] == "Convention") ? "Convention Hall" : selectedservices[i];
                selectedservices[i] = (selectedservices[i] == "Banquet") ? "Banquet Hall" : selectedservices[i];
                selectedservices[i] = (selectedservices[i] == "Function") ? "Function Hall" : selectedservices[i];
                var getrecords = resultsPageService.GetAllVendors(selectedservices[i]);//.Take(recordcount).ToList();
                if (command == "next")
                    getrecords = getrecords.Skip(takecount).Take(recordcount).ToList();
                else if (command == "first")
                    getrecords = getrecords.Take(recordcount).ToList();
                list.AddRange(getrecords);
            }
            return list;
        }

        public void SendEmail(string type,string count, string loc, string eventtype, string date)
        {
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
                if (user.UserType == "User")
                {
                    string url = Request.Url.AbsoluteUri;
                    string Email = user.Username;
                    string txtto = "amit.saxena@ahwanam.com,rameshsai@xsilica.com,sireesh.k@xsilica.com";
                    int id = Convert.ToInt32(user.UserId);
                    var userdetails = userLoginDetailsService.GetUser(id);
                    string ipaddress = HttpContext.Request.UserHostAddress;
                    string username = userdetails.FirstName;
                    HomeController home = new HomeController();
                    username = home.Capitalise(username);
                    string emailid = user.Username;
                    FileInfo File = new FileInfo(Server.MapPath("/mailtemplate/login.html"));
                    string readFile = File.OpenText().ReadToEnd();
                    StringBuilder cds = new StringBuilder();
                    cds.Append("<table ><tbody><tr><td> type </td><td style = 'width: 75px;'> " + type + "</td></tr><tr><td> guest </td><td style = 'width: 75px;' > " + count + " </td></tr><tr><td style = 'width: 50px;'> loction </td><td style = 'width: 75px;'> " + loc + " </td></tr><tr><td style = 'width: 50px;'> eventtype </td><td style = 'width: 50px;'> " + eventtype + " </td></tr><tr><td style = 'width: 50px;'> date </td><td style = 'width: 50px;'> " + date + " </td></tr></table></tbody>");
                    readFile = readFile.Replace("[ActivationLink]", url);
                    readFile = readFile.Replace("[name]", username);
                    readFile = readFile.Replace("[Ipaddress]", ipaddress);
                    readFile = readFile.Replace("[email]", Email);
                    readFile = readFile.Replace("[carttable]", cds.ToString());
                    string txtmessage = readFile;//readFile + body;
                    string subj = "User search from ahwanam";
                    EmailSendingUtility emailSendingUtility = new EmailSendingUtility();
                    emailSendingUtility.Email_maaaahwanam(txtto, txtmessage, subj);
                }
            }
        }

        //public List<string[]> vendorlist(int count, string[] selectedservices, string command,int takecount)
        //{
        //    List<string[]> mixedrecords = new List<string[]>();
        //    List<GetVendors_Result> vlist = new List<GetVendors_Result>();
        //    List<GetPhotographers_Result> plist = new List<GetPhotographers_Result>();
        //    List<GetDecorators_Result> dlist = new List<GetDecorators_Result>();
        //    int recordcount = 0;
        //    recordcount = count / selectedservices.Count();
        //    takecount = takecount / selectedservices.Count();
        //    if (selectedservices.Contains("Venue"))
        //    {
        //        for (int i = 0; i < selectedservices.Count(); i++)
        //        {
        //            selectedservices[i] = (selectedservices[i] == "Convention") ? "Convention Hall" : selectedservices[i];
        //            selectedservices[i] = (selectedservices[i] == "Banquet") ? "Banquet Hall" : selectedservices[i];
        //            selectedservices[i] = (selectedservices[i] == "Function") ? "Function Hall" : selectedservices[i];
        //            var getrecords = resultsPageService.GetAllVendors(selectedservices[i]);//.Take(recordcount).ToList();
        //            if (command == "next")
        //                getrecords = getrecords.Skip(takecount).Take(recordcount).ToList();
        //            else if (command == "first")
        //                getrecords = getrecords.Take(recordcount).ToList();
        //            vlist.AddRange(getrecords);
        //        }
        //    }
        //    if (selectedservices.Contains("Photography"))
        //    {
        //        for (int i = 0; i < selectedservices.Count(); i++)
        //        {
        //            var getrecords = resultsPageService.GetAllPhotographers();//.Take(recordcount).ToList();
        //            if (command == "next")
        //                getrecords = getrecords.Skip(takecount).Take(recordcount).ToList();
        //            else if (command == "first")
        //                getrecords = getrecords.Take(recordcount).ToList();
        //            plist.AddRange(getrecords);
        //        }
        //    }
        //    if (selectedservices.Contains("Decorator"))
        //    {
        //        for (int i = 0; i < selectedservices.Count(); i++)
        //        {
        //            var getrecords = resultsPageService.GetAllDecorators();//.Take(recordcount).ToList();
        //            if (command == "next")
        //                getrecords = getrecords.Skip(takecount).Take(recordcount).ToList();
        //            else if (command == "first")
        //                getrecords = getrecords.Take(recordcount).ToList();
        //            dlist.AddRange(getrecords);
        //        }
        //    }
        //    //mixedrecords.AddRange(vlist);
        //    //mixedrecords.AddRange(plist);
        //    //mixedrecords.AddRange(dlist);
        //    return mixedrecords;
        //}
    }
}