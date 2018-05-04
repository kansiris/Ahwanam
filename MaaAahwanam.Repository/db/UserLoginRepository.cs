﻿using System.Linq;
using MaaAahwanam.Models;
using System;

namespace MaaAahwanam.Repository.db
{
    public class UserLoginRepository
    {
        readonly ApiContext _dbContext = new ApiContext();

        public UserLoginRepository()
        {

        }

        public UserLogin AddLoginCredentials(UserLogin userLogin)
        {
            _dbContext.UserLogin.Add(userLogin);
            _dbContext.SaveChanges();
            return userLogin;
        }

        public UserDetail GetLoginDetailsByUsername(UserLogin userLogin)
        {
            UserLogin list = null;
            if (userLogin.Password != null)
            {
                list = _dbContext.UserLogin.FirstOrDefault(p => p.UserName == userLogin.UserName && p.Password == userLogin.Password && p.UserType == userLogin.UserType);
            }
            if (userLogin.Password == null)
            {
                list = _dbContext.UserLogin.FirstOrDefault(p => p.UserName == userLogin.UserName);
            }
            UserDetail userDetail = new UserDetail();
            if (list != null)
            {
                userDetail = _dbContext.UserDetail.FirstOrDefault(m=>m.UserLoginId==list.UserLoginId);
            }
            return userDetail;
        }
        public UserLogin UpdatePassword(UserLogin userLogin, int UserloginID)
        {
            // Query the database for the row to be updated.
            var query =
                from ord in _dbContext.UserLogin
                where ord.UserLoginId == UserloginID
                select ord;

            // Execute the query, and change the column values
            // you want to change.
            foreach (UserLogin ord in query)
            {
                ord.Password = userLogin.Password;
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
            return userLogin;
        }
        public UserLogin Updatestatus(UserLogin userLogin, int UserloginID)
        {
            // Query the database for the row to be updated.
            var query =
                from ord in _dbContext.UserLogin
                where ord.UserLoginId == UserloginID
                select ord;

            // Execute the query, and change the column values
            // you want to change.
            foreach (UserLogin ord in query)
            {
                ord.Status = userLogin.Status;
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
            return userLogin;
        }

        public UserLogin AddVendorUserLogin(UserLogin userLogin)
        {
            _dbContext.UserLogin.Add(userLogin);
            _dbContext.SaveChanges();
            return userLogin;
        }
        public string username(long UserId)
        {
            return _dbContext.UserLogin.Where(p => p.UserLoginId == UserId).Select(u => u.UserName).FirstOrDefault();
        }

        public string password(long UserId)
        {
            return _dbContext.UserLogin.Where(p => p.UserLoginId == UserId).Select(u => u.Password).FirstOrDefault();
        }

        public int UpdateUserLogin(string email,string status)
        {
            // Query the database for the row to be updated.
            var query =
                from ord in _dbContext.UserLogin
                where ord.UserName == email
                select ord;

            // Execute the query, and change the column values
            // you want to change.
            foreach (UserLogin ord in query)
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
    }
}
