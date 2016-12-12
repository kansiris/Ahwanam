using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaaAahwanam.Models;

namespace MaaAahwanam.Repository.db
{
    public class UserAddBookRepository
    {
        ApiContext _Dbcontext = new ApiContext();
        public List<UserAddBook> GetUserAddBook(int userid)
        {
            return _Dbcontext.UserAddBook.Where(i=>i.UserLoginId==userid && i.Status == "Active").ToList();
        }
        public UserAddBook InsertUserAddBook(UserAddBook userAddBook)
        {
            _Dbcontext.UserAddBook.Add(userAddBook);
            _Dbcontext.SaveChanges();
            return userAddBook;
        }

        public string DeleteAddressBook(string[] id)
        {
            //_Dbcontext.UserAddBook.Remove(m => m.id == item.id);
            _Dbcontext.UserAddBook.RemoveRange(_Dbcontext.UserAddBook.Where(m=>id.Contains(m.AddBookId.ToString())));
            _Dbcontext.SaveChanges();
            return "sucess";
        }
    }
}
