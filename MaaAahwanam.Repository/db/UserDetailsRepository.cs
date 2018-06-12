using System.Linq;
using MaaAahwanam.Models;
using System;
using System.Collections.Generic;

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


        public UserLogin GetLoginDetails(int userId)
        {
            UserLogin list1 = new UserLogin();
            if (userId != 0)
                list1 = _dbContext.UserLogin.SingleOrDefault(p => p.UserLoginId == userId);
            return list1;
        }
        //To Show the login user details in all pages at the top

        public Vendormaster getvendor(int vendorid)
        {
            Vendormaster list = new Vendormaster();
            if (vendorid != 0)
                list = _dbContext.Vendormaster.SingleOrDefault(p => p.Id == vendorid);
            return list;
        }

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
        public UserDetail UpdateUserdetailsnew(UserDetail userDetail, Int64 UserloginID)
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
                ord.FirstName = userDetail.FirstName;
                ord.UserPhone = userDetail.UserPhone;
                // Insert any additional changes to column values.
            }

            // Submit the changes to the database.
            try
            {
                _dbContext.SaveChanges();
            }
            catch (Exception Ex)
            {

            }
            return userDetail;
        }


        public Vendormaster Updatevendordetailsnew(Vendormaster vendor,  string email)
        {
            // Query the database for the row to be updated.
            var query =
                from ord in _dbContext.Vendormaster
                where ord.EmailId == email
                select ord;

            // Execute the query, and change the column values
            // you want to change.
            foreach (Vendormaster ord in query)
            {
                ord.BusinessName = vendor.BusinessName;
                ord.Address = vendor.Address;
                ord.City = vendor.City;
                ord.State = vendor.State;
                ord.Landmark = vendor.Landmark;
                ord.ContactPerson = vendor.ContactPerson;
                ord.ContactNumber = vendor.ContactNumber;
                //ord.Description = vendor.Description;
                ord.LandlineNumber = vendor.LandlineNumber;
                ord.ZipCode = vendor.ZipCode;
                // Insert any additional changes to column values.
            }

            // Submit the changes to the database.
            try
            {
                _dbContext.SaveChanges();
            }
            catch (Exception Ex)
            {

            }
            return vendor;
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

       

        public int UpdateUserDetail(string email, string status)
        {
            // Query the database for the row to be updated.
            var query1 = _dbContext.UserLogin.Where(m => m.UserName == email).FirstOrDefault();
            var query =
                from ord in _dbContext.UserDetail
                where ord.UserLoginId == query1.UserLoginId
                select ord;

            // Execute the query, and change the column values
            // you want to change.
            foreach (UserDetail ord in query)
            {
                ord.Status = status;
                // Insert any additional changes to column values.
            }

            // Submit the changes to the database.
            try
            {
                _dbContext.SaveChanges();
            }
            catch (Exception Ex)
            {
                return 0;
            }
            return 1;
        }

        public UserDetail UpdateUserDetailEmail(UserDetail userDetail, string email)
        {
            var GetMasterRecord = _dbContext.UserDetail.SingleOrDefault(m => m.AlternativeEmailID == email);
            userDetail.AlternativeEmailID = userDetail.AlternativeEmailID;
            _dbContext.Entry(GetMasterRecord).CurrentValues.SetValues(userDetail);
            _dbContext.SaveChanges();
            return userDetail;
        }

        public UserDetail GetUserDetailsByEmail(string email)
        {
            return _dbContext.UserDetail.SingleOrDefault(m => m.AlternativeEmailID == email);
        }

        public List<UserLogin> GetUserLoginTypes(string email)
        {
            return _dbContext.UserLogin.Where(m => m.UserName == email).ToList();
        }
    }
}
