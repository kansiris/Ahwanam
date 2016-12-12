using System.Web.Mvc;
using MaaAahwanam.Models;
using MaaAahwanam.Utility;
using MaaAahwanam.Service;
using MaaAahwanam.Web.Custom;

namespace MaaAahwanam.Web.Controllers
{
    [Authorize]
    public class AddressBookController : Controller
    {
        
        UserAddBookService userAddBookService = new UserAddBookService();
        //
        // GET: /AddressBook/
        [HttpGet]
        public ActionResult Index()
        {
            var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
            //ViewBag.Type = ValidUserUtility.UserType();
            ViewBag.Type = user.UserType;
            ViewBag.Addressbooklist = userAddBookService.GetUserAddBook((int)user.UserId);
            return View();
        }

        [HttpPost]
        public ActionResult Index(UserAddBook userAddBook)
        {
            var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
            userAddBook.UserLoginId = (int)user.UserId;
            userAddBook.UpdatedBy = (int)user.UserId;
            string message = userAddBookService.InsertUserAddBook(userAddBook);
            if (message == "Success")
            {
                return Content("<script language='javascript' type='text/javascript'>alert('Address Inserted successfully');location.href='" + @Url.Action("Index", "AddressBook") + "'</script>");
            }
            else
            {
                return Content("<script language='javascript' type='text/javascript'>alert('Error Occured');location.href='" + @Url.Action("Index", "AddressBook") + "'</script>");
            }
        }

        public JsonResult delete(string[] customerIDs)
        {
            
            string msg = userAddBookService.DeleteAddressBook(customerIDs);
            return Json("Selected Addresses deleted successfully!");
        }
    }
}