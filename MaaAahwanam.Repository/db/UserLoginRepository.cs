using System.Linq;
using MaaAahwanam.Models;
using System;

namespace MaaAahwanam.Repository.db
{
    public class UserLoginRepository
    {
        readonly ApiContext _dbContext = new ApiContext();
        MaaAahwanamEntities maaAahwanamEntities = new MaaAahwanamEntities();
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
                userDetail = _dbContext.UserDetail.FirstOrDefault(m => m.UserLoginId == list.UserLoginId);
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
                ord.UpdatedDate = userLogin.UpdatedDate;
            }

            // Submit the changes to the database.
            try
            {
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {

            }
            return userLogin;
        }
        public UserLogin Updatestatus(UserLogin userLogin, UserDetail userDetails, int UserloginID)
        {
            // Query the database for the row to be updated.
            var query =
                from ord in _dbContext.UserLogin
                where ord.UserLoginId == UserloginID
                select ord;
            // Query the database for the row to be updated.
            var query1 =
                from ord1 in _dbContext.UserDetail
                where ord1.UserLoginId == UserloginID
                select ord1;

            // Execute the query, and change the column values
            // you want to change.
            foreach (UserLogin ord in query)
            {
                ord.Status = userLogin.Status;
                // Insert any additional changes to column values.
            }

            foreach (UserDetail ord1 in query1)
            {
                ord1.Status = userDetails.Status;
                // Insert any additional changes to column values.
            }
            // Submit the changes to the database.
            try
            {
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
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

        public int UpdateUserLogin(string email, string status)
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
            catch (Exception ex)
            {
                return 0;
            }
            return 1;
        }

        public UserLogin UpdateUserName(UserLogin userLogin, string email)
        {
            var GetMasterRecord = _dbContext.UserLogin.Where(m => m.UserType == "Vendor").SingleOrDefault(m => m.UserName == email);
            userLogin.UserName = userLogin.UserName;
            userLogin.UserLoginId = GetMasterRecord.UserLoginId;
            userLogin.UserType = "Vendor";
            _dbContext.Entry(GetMasterRecord).CurrentValues.SetValues(userLogin);
            _dbContext.SaveChanges();
            return userLogin;
        }

        public UserLogin UpdateActivationCode(UserLogin userlogin)
        {
            var GetMasterRecord = _dbContext.UserLogin.Where(m => m.UserName == userlogin.UserName).FirstOrDefault();
            //GetMasterRecord.ActivationCode = userlogin.ActivationCode;
            userlogin.UserLoginId = GetMasterRecord.UserLoginId;
            _dbContext.Entry(GetMasterRecord).CurrentValues.SetValues(userlogin);
            _dbContext.SaveChanges();
            return userlogin;
        }

        public int checktoken(string token, string IP)
        {
            return _dbContext.UserToken.Where(m => m.Token == token && m.IPAddress == IP).Count();
        }

        public UserToken addtoken(UserToken usertoken)
        {
            _dbContext.UserToken.Add(usertoken);
            _dbContext.SaveChanges();
            return usertoken;
        }

        public int RemoveToken(string token,long userloginid)
        {
            var getdata = _dbContext.UserToken.Where(m => m.UserLoginID == userloginid && m.Token == token).FirstOrDefault();
            _dbContext.UserToken.Remove(getdata);
            return _dbContext.SaveChanges();
        }

        public long userloginId(string token)
        {
            return _dbContext.UserToken.Where(t => t.Token == token).Select(u => u.UserLoginID).FirstOrDefault();
        }


        public Getmyprofile_Result Getmyprofile(string token)
        {
            return maaAahwanamEntities.Getmyprofile(token).FirstOrDefault();
        }

            public UserDetail updateprofile(UserDetail userdetail,long userloginid)
        {
            var getdetails = _dbContext.UserDetail.SingleOrDefault(u => u.UserLoginId == userloginid);
            UserDetail details = new UserDetail();
            details.UserDetailId = getdetails.UserDetailId;
            details.UserLoginId = getdetails.UserLoginId;
            details.AlternativeEmailID = getdetails.AlternativeEmailID;
            details.UserPhone = userdetail.UserPhone;
            details.FirstName = userdetail.FirstName;
            details.name = userdetail.name;
            details.UpdatedDate = userdetail.UpdatedDate;
            details.LastName = getdetails.LastName;
            details.Status = getdetails.Status;
            details.City = getdetails.City;
            details.Country = getdetails.Country;
            details.Address = getdetails.Address;
            details.Landmark = getdetails.Landmark;
            details.State = getdetails.State;
            details.ZipCode = getdetails.ZipCode;
            details.UserImgId = getdetails.UserImgId;
            details.UserImgName = getdetails.UserImgName;
            details.UpdatedBy = getdetails.UpdatedBy;
            _dbContext.Entry(getdetails).CurrentValues.SetValues(details);
            _dbContext.SaveChanges();
            return details;
        }
    }
}
