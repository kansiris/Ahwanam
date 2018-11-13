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
            var list = allvendors(6, selectedservices, "first", 6);
            ViewBag.count = 6;

            if (to_from == null)
            {
                ViewBag.venues = vendorlist(6, selectedservices, "first", 6); //allvendors(6, selectedservices, "first", 6);//vendorlist(6, selectedservices, "first", 6); 
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
            var slider1 = slider.Split(';');
            var min = slider1[0];
            var max = slider1[1];

            type = (type == null || type == "") ? "Venue" : type;
            var selectedservices = type.Split(',');
            int takecount = (count == "" || count == null) ? 6 : int.Parse(count) * 6;
            ViewBag.count = takecount;
            List<GetVendors_Result> vendorslist;
            if (takecount == 6)
                vendorslist = vendorlist(12, selectedservices, "first", takecount).Where(m => m.cost1 >= decimal.Parse(min) && m.cost1 <= decimal.Parse(max)).ToList();//resultsPageService.GetAllVendors(type).Take(12).ToList(); 
            else
                vendorslist = vendorlist(6, selectedservices, "next", takecount).Where(m => m.cost1 >= decimal.Parse(min) && m.cost1 <= decimal.Parse(max)).ToList(); //resultsPageService.GetAllVendors(type).Skip(takecount).Take(6).ToList(); 
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

        public List<searchresults> allvendors(int count, string[] selectedservices, string command, int takecount)
        {
            VenorVenueSignUpService vendorVenueSignUpService = new VenorVenueSignUpService();
            PartnerService partnerService = new PartnerService();
            List<searchresults> list = new List<searchresults>();
            int recordcount = 0;
            recordcount = count / selectedservices.Count();
            takecount = takecount / selectedservices.Count();
            //var data = resultsPageService.GetAllVendors(selectedservices[i]);
            var partnerpackages = partnerService.getallPartnerPackage();
            var allpkgs = vendorVenueSignUpService.GetAllPackages();
            for (int i = 0; i < selectedservices.Count(); i++)
            {
                selectedservices[i] = (selectedservices[i] == "Convention" || selectedservices[i] == "Banquet" || selectedservices[i] == "Function") ? selectedservices[i] + " Hall" : selectedservices[i];
                var getrecords = resultsPageService.GetAllVendors(selectedservices[i]);//.Take(recordcount).ToList();
                if (command == "next")
                    getrecords = getrecords.Skip(takecount).Take(recordcount).ToList();
                else if (command == "first")
                    getrecords = getrecords.Take(recordcount).ToList();
                for (int j = 0; j < getrecords.Count; j++)
                {
                    string pkgtype = "Normal Days";//GetType(getrecords[j].Id.ToString(), getrecords[j].subid.ToString());
                    var cpkgs = allpkgs.Where(m => m.VendorId == getrecords[j].Id && m.VendorSubId == getrecords[j].subid).ToList();
                    searchresults search = new searchresults();
                    search.ID = getrecords[j].Id;
                    search.SubID = getrecords[j].subid;
                    search.BusinessName = getrecords[j].BusinessName;
                    search.SubType = getrecords[j].subtype;
                    search.SubTypeName = getrecords[j].subtypename;
                    search.Landmark = getrecords[j].Landmark;
                    search.City = getrecords[j].City;
                    search.State = getrecords[j].State;
                    string id = "";//string category = "";
                    if (pkgtype == "Normal Days")
                    { search.VegPkgPrice = long.Parse(cpkgs.Select(m => m.normaldays).Min()); id = cpkgs.Select(m=>m.PackageID & m.normaldays.Min()).ToString();  }
                    //else if (pkgtype == "Peak Days")
                    //{ search.VegPkgPrice = cpkgs.Select(m => m.peakdays).Min(); }
                    //else if (pkgtype == "Holidays")
                    //{ search.VegPkgPrice = cpkgs.Select(m => m.holidays).Min(); }
                    //else if (pkgtype == "Choice Days")
                    //{ search.VegPkgPrice = cpkgs.Select(m => m.choicedays).Min(); }
                    search.PackageID = id;
                    var particularpackage = partnerpackages.Where(m => m.packageid == id).FirstOrDefault();
                    if (pkgtype == "Normal Days")
                    { search.SVegPkgPrice = long.Parse(particularpackage.Anormaldays); }
                    //else if (pkgtype == "Peak Days")
                    //{ search.SVegPkgPrice = particularpackage.Apeakdays; }
                    //else if (pkgtype == "Holidays")
                    //{ search.SVegPkgPrice = particularpackage.Aholidays; }
                    //else if (pkgtype == "Choice Days")
                    //{ search.SVegPkgPrice = particularpackage.Achoicedays; }
                    list.Add(search);
                }


            }
            return list;
        }

        public class searchresults
        {
            public long ID { get; set; }
            public long SubID { get; set; }
            public string BusinessName { get; set; }
            public string SubType { get; set; }
            public string SubTypeName { get; set; }
            public string Landmark { get; set; }
            public string City { get; set; }
            public string State { get; set; }
            public long VegPkgPrice { get; set; }
            public long NonVegPkgPrice { get; set; }
            public long SVegPkgPrice { get; set; }
            public long SNonVegPkgPrice { get; set; }
            public string MinSeating { get; set; }
            public string MaxSeating { get; set; }
            public string PackageID { get; set; }
        }

        public string GetType(string id, string vid)
        {
            VendorDatesService vendorDatesService = new VendorDatesService();
            string type = "Normal Days";
            var data = vendorDatesService.GetDates(long.Parse(id), long.Parse(vid)).Where(m => m.Type == "Package");
            var today = DateTime.Now;
            foreach (var item in data)
            {
                var startdate = Convert.ToDateTime(item.StartDate);
                var enddate = Convert.ToDateTime(item.EndDate);
                if (today < startdate && today > enddate)
                {
                    return item.Title;
                }
            }
            return type;
        }

        public void SendEmail(string type, string count, string loc, string eventtype, string date)
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