using System;
using MaaAahwanam.Models;
using MaaAahwanam.Repository.db;
using MaaAahwanam.Service.Mapper;
using System.Collections.Generic;

namespace MaaAahwanam.Service
{
    public class UserLoginDetailsService
    {
        private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        UserLoginRepository userLoginRepository = new UserLoginRepository();
        UserDetailsRepository userDetailsRepository = new UserDetailsRepository();
        public string AddUserDetails(UserLogin userLogin, UserDetail userDetails)
        {
            string response;
            //userLogin.Status = "Active";
            DateTime updateddate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);//DateTime.UtcNow.ToShortDateString();
            userLogin.RegDate = updateddate;
            userLogin.UpdatedDate = userDetails.UpdatedDate = updateddate;
            try
            {
                UserLogin l1 = userLoginRepository.AddLoginCredentials(userLogin);
                userDetails.UserLoginId = l1.UserLoginId;
                UserDetail l2 = userDetailsRepository.AddUserDetails(userDetails);
                response = "sucess";
            }
            catch (Exception ex)
            {
                response = "failure";
            }
            return response;
        }
        public UserResponse AuthenticateUser(UserLogin UserLogin)
        {
            UserMapper userMapper = new UserMapper();
            var userDetail = userLoginRepository.GetLoginDetailsByUsername(UserLogin);
            var userResponse = userMapper.MapUserDetailToUserResponse(userDetail);
            return userResponse;
        }
        public UserLogin GetUserId(int userid)
        {
            string response = string.Empty;
            UserLogin list = userDetailsRepository.GetLoginDetails(userid);
            return list;
        }

        public Vendormaster getvendor(int vendorid)
        {
            string response = string.Empty;
            Vendormaster list = userDetailsRepository.getvendor(vendorid);
            return list;

        }
        public UserDetail GetUser(int userid)
        {
            string response = string.Empty;
            UserDetail list = userDetailsRepository.GetLoginDetailsByUsername(userid);
            return list;
        }
        public UserDetail UpdateUserdetails(UserDetail userDetail, Int64 UserloginID)
        {
            var userdetail = userDetailsRepository.UpdateUserdetails(userDetail, UserloginID);
            return userdetail;
        }
        public UserDetail UpdateUserdetailsnew(UserDetail userDetail, Int64 UserloginID)
        {
            var userdetail = userDetailsRepository.UpdateUserdetailsnew(userDetail, UserloginID);
            return userdetail;
        }
        public Vendormaster Updatevendordetailsnew(Vendormaster vendor,string  email)
        {
            var vendordetail = userDetailsRepository.Updatevendordetailsnew(vendor, email);
            return vendordetail;
        }
        public UserLogin changepassword(UserLogin userLogin, int UserLoginId)
        {
            var changes = userLoginRepository.UpdatePassword(userLogin, UserLoginId);
            return changes;
        }
        public UserLogin changestatus(UserLogin userLogin, UserDetail userDetails, int UserLoginId)
        {
            var changes = userLoginRepository.Updatestatus(userLogin,userDetails, UserLoginId);
            return changes;
        }
        public string SetUserDP(int userloginid)
        {
            return userDetailsRepository.GetUserDP(userloginid);
        }
        public void ChangeDP(int userloginID, string imagename)
        {
            userDetailsRepository.UpdateDP(userloginID, imagename);
        }
        public string Getusername(long UserId)
        {
            return userLoginRepository.username(UserId);
        }
        public string Getpassword(long UserId)
        {
            return userLoginRepository.password(UserId);
        }

        public long GetLoginDetailsByEmail(string username)
        {
            return userDetailsRepository.GetLoginDetailsByEmail(username);
        }
        public int Updatestatus(string email, string status)
        {
            userDetailsRepository.UpdateUserDetail(email, status);
            return userLoginRepository.UpdateUserLogin(email, status);
        }

        public UserLogin UpdateUserName(UserLogin userLogin, string email)
        {
            return userLoginRepository.UpdateUserName(userLogin, email);
        }

        public UserDetail UpdateUserDetailEmail(UserDetail userDetail, string email)
        {
            return userDetailsRepository.UpdateUserDetailEmail(userDetail, email);
        }

        public UserDetail GetUserDetailsByEmail(string email)
        {
            return userDetailsRepository.GetUserDetailsByEmail(email);
        }

        public List<UserLogin> GetUserLoginTypes(string email)
        {
            return userDetailsRepository.GetUserLoginTypes(email);
        }

        public UserLogin UpdateActivationCode(UserLogin userlogin)
        {
            return userLoginRepository.UpdateActivationCode(userlogin);
        }

        //Token Part
        public int checktoken(string token, string IP)
        {
            return userLoginRepository.checktoken(token, IP);
        }

        public UserToken addtoken(UserToken usertoken)
        {
            return userLoginRepository.addtoken(usertoken);
        }
    }
}
