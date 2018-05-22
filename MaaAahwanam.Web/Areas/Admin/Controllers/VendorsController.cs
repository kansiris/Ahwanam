using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaaAahwanam.Service;
using MaaAahwanam.Models;
using MaaAahwanam.Utility;
using System.IO;

namespace MaaAahwanam.Web.Areas.Admin.Controllers
{
    public class VendorsController : Controller
    {

        //VendorVenueService vendorVenueService = new VendorVenueService();
        //VendorImageService vendorImageService = new VendorImageService();
        VendorMasterService vendorMasterService = new VendorMasterService();
        VendorSetupService vendorSetupService = new VendorSetupService();
        UserLoginDetailsService userLoginDetailsService = new UserLoginDetailsService();
        public ActionResult AllVendors()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AllVendors(string dropstatus, string vid, string command, string id, string type, [Bind(Prefix = "Item2")] VendorVenue vendorVenue, [Bind(Prefix = "Item1")] Vendormaster vendorMaster)
        {
            if (dropstatus != null && dropstatus != "")
            {
                ViewBag.VendorList = vendorSetupService.AllVendorList(dropstatus);
            }
            if (command == "Edit")
            {
                return RedirectToAction(dropstatus, "CreateVendor", new { id = id, vid = vid });
            }
            if (command == "View")
            {
                return RedirectToAction(dropstatus, "CreateVendor", new { id = id, vid = vid, op = "display" });
            }
            if (command == "Add New")
            {
                return RedirectToAction(dropstatus, "CreateVendor", new { id = id, vid = vid, op = "add" });
            }
            if (command == "confirm")
            {
                TempData["confirm"] = 1;
                return RedirectToAction(dropstatus, "CreateVendor", new { id = id, vid = vid, op = "confirm" });
            }
            return View();
        }
        public ActionResult ActiveVendors()
        {
            return View();
        }
        public ActionResult PendingVendors()
        {
            return View();
        }
        public ActionResult SuspendedVendors()
        {
            return View();
        }
        public ActionResult VendorDetails(string id, [Bind(Prefix = "Item2")] VendorVenue vendorVenue, [Bind(Prefix = "Item1")] Vendormaster vendorMaster)
        {
            return View();
        }

        public ActionResult SearchVendor()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SearchVendor(string searchvendor, string command, Vendormaster vendormaster, string pemail, string id)
        {
            UserLogin userlogin = new UserLogin();
            UserDetail userdetails = new UserDetail();
            ViewBag.VendorList = vendorMasterService.SearchVendors().Where(m => m.BusinessName == searchvendor).FirstOrDefault();
            if (command == "Update")
            {
                var updatedetails = vendormaster;
                VenorVenueSignUpService venorVenueSignUpService = new VenorVenueSignUpService();
                vendormaster = vendorMasterService.GetVendorByEmail(pemail);
                vendormaster.EmailId = updatedetails.EmailId;
                vendormaster.ContactNumber = updatedetails.ContactNumber;
                vendormaster.ContactPerson = updatedetails.ContactPerson;
                vendormaster = vendorMasterService.UpdateVendorDetails(vendormaster, long.Parse(id)); // Updating Email ID in Vendor Master Table
                userlogin.UserName = pemail;
                userlogin = venorVenueSignUpService.GetUserLogdetails(userlogin);
                userlogin.UserName = vendormaster.EmailId;
                userlogin = userLoginDetailsService.UpdateUserName(userlogin, pemail); // Updating Email ID in User Login Table
                userdetails = userLoginDetailsService.GetUserDetailsByEmail(pemail);
                userdetails.AlternativeEmailID = vendormaster.EmailId;
                userdetails.FirstName = vendormaster.ContactPerson;
                userdetails.UserPhone = vendormaster.ContactNumber;
                userdetails = userLoginDetailsService.UpdateUserDetailEmail(userdetails, pemail); // Updating Email ID in User Detail Table
                TempData["Active"] = "Info Updated";
                return RedirectToAction("SearchVendor", "Vendors");
            }
            if (command == "Email")
            {
                VenorVenueSignUpService venorVenueSignUpService = new VenorVenueSignUpService();
                userlogin.UserName = vendormaster.EmailId;
                var userResponse = venorVenueSignUpService.GetUserLogdetails(userlogin);
                if (userResponse != null)
                {
                    string emailid = userlogin.UserName;
                    if (userResponse.ActivationCode == null)
                    {
                        userlogin = userResponse;
                        userlogin.ActivationCode= Guid.NewGuid().ToString();
                        userlogin = userLoginDetailsService.UpdateUserName(userlogin, emailid);
                        userResponse.ActivationCode = userlogin.ActivationCode;
                    }

                    string activationcode = userResponse.ActivationCode;
                    int userid = Convert.ToInt32(userResponse.UserLoginId);
                    var userdetail = userLoginDetailsService.GetUser(userid);
                    string name = userdetail.FirstName;

                    // vendor mail activation  begin
                    
                    string mailid = userlogin.UserName;
                    var userR = venorVenueSignUpService.GetUserdetails(mailid);
                    string pas1 = userR.Password;
                    string url = Request.Url.Scheme + "://" + Request.Url.Authority + "/NUserRegistration/ActivateEmail1?ActivationCode=" + activationcode + "&&Email=" + emailid;
                    FileInfo File = new FileInfo(Server.MapPath("/mailtemplate/WelcomeMessage.html"));
                    string readFile = File.OpenText().ReadToEnd();
                    readFile = readFile.Replace("[ActivationLink]", url);
                    readFile = readFile.Replace("[name]", name);
                    readFile = readFile.Replace("[username]", mailid);
                    readFile = readFile.Replace("[pass1]", pas1);
                    string txtto = userlogin.UserName;
                    string txtmessage = readFile;//readFile + body;
                    string subj = "Welcome to Ahwanam";

                    // vendor mail activation  end

                    EmailSendingUtility emailSendingUtility = new EmailSendingUtility();
                    emailSendingUtility.Email_maaaahwanam(txtto, txtmessage, subj);
                    TempData["Active"] = "Invitation Sent to " + txtto + "";
                    return RedirectToAction("SearchVendor", "Vendors");
                }
            }
            return View();
        }

    }
}