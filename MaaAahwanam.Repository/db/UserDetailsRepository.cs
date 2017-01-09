using System.Linq;
using MaaAahwanam.Models;
using System;


namespace MaaAahwanam.Repository.db
{
    public class UserDetailsRepository
    {
        readonly ApiContext _dbContext = new ApiContext();

        public UserDetailsRepository()
        {

        }

        //User Registration
        public UserDetail AddUserDetails(UserDetail userDetails)
        {
            _dbContext.UserDetail.Add(userDetails);
            _dbContext.SaveChanges();
            return userDetails;
        }
        //To Show the login user details in all pages at the top
        public UserDetail GetLoginDetailsByUsername(int userId)
        {
            UserDetail list = new UserDetail();
            if (userId != 0)
                list = _dbContext.UserDetail.SingleOrDefault(p => p.UserLoginId == userId);
            return list;
        }
        public UserDetail UpdateUserdetails(UserDetail userDetail,Int64 UserloginID)
        {
            // Query the database for the row to be updated.
            var query =
                from ord in _dbContext.UserDetail
                where ord.UserLoginId == UserloginID
                select ord;

            // Execute the query, and change the column values
            // you want to change.
            foreach (UserDetail ord in query)
            {
                ord.FirstName= userDetail.FirstName;
                ord.LastName = userDetail.LastName;
                ord.AlternativeEmailID = userDetail.AlternativeEmailID;
                ord.Gender = userDetail.Gender;
                ord.UserPhone = userDetail.UserPhone;
                ord.City = userDetail.City;
                ord.Country = userDetail.Country;
                ord.Landmark = userDetail.Landmark;
                ord.Address = userDetail.Address;
                ord.ZipCode = userDetail.ZipCode;
                ord.State = userDetail.State;
                ord.Url = userDetail.Url;
                // Insert any additional changes to column values.
            }

            // Submit the changes to the database.
            try
            {
                _dbContext.SaveChanges();
            }
            catch(Exception Ex)
            {

            }
            return userDetail;
        }

        public void UpdateDP(int UserloginsID,string imagename)
        {
            var list = _dbContext.UserDetail.SingleOrDefault(o=>o.UserLoginId==UserloginsID);
            list.UserImgName = imagename;
            _dbContext.SaveChanges();
        }
        public string GetUserDP(int UserloginsID)
        {
            var list = _dbContext.UserDetail.SingleOrDefault(o => o.UserLoginId == UserloginsID);
            string imagename = list.UserImgName;
            return imagename;
        }

        public long GetLoginDetailsByEmail(string username)
        {
            var count = _dbContext.UserLogin.Where(m => m.UserName == username).FirstOrDefault();
            if (count != null)
                return count.UserLoginId;
            else
                //count.UserLoginId = 0;
                return 0;
        }
    }
}
