using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaaAahwanam.Models;
using MaaAahwanam.Repository.db;

namespace MaaAahwanam.Service
{
    public class UserAddBookService
    {
        UserAddBookRepository userAddBookRepository = new UserAddBookRepository();
        public List<UserAddBook> GetUserAddBook(int userid)
        {
            return userAddBookRepository.GetUserAddBook(userid);
        }
        public string InsertUserAddBook(UserAddBook userAddBook)
        {
            string message;
            try {
                string updateddate = DateTime.UtcNow.ToShortDateString();
                userAddBook.UpdatedDate = Convert.ToDateTime(updateddate);
            userAddBookRepository.InsertUserAddBook(userAddBook);
            message = "Success";
            }
            catch
            {
                message = "Failed To Insert";
            }
                return message;
        }
    }
}
