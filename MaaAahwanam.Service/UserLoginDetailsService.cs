using System;
using MaaAahwanam.Models;
using MaaAahwanam.Repository.db;
using MaaAahwanam.Service.Mapper;

namespace MaaAahwanam.Service
{
    public class UserLoginDetailsService
    {
        UserLoginRepository userLoginRepository = new UserLoginRepository();
        UserDetailsRepository userDetailsRepository = new UserDetailsRepository();
        public string AddUserDetails(UserLogin userLogin, UserDetail userDetails)
        {
            string response;
            //userLogin.Status = "Active";
            string updateddate = DateTime.UtcNow.ToShortDateString();
            userLogin.RegDate = Convert.ToDateTime(updateddate);
            userLogin.UpdatedDate = userDetails.UpdatedDate = Convert.ToDateTime(updateddate);
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
        public UserLogin changepassword(UserLogin userLogin, int UserLoginId)
        {
            var changes = userLoginRepository.UpdatePassword(userLogin, UserLoginId);
            return changes;
        }
        public UserLogin changestatus(UserLogin userLogin, int UserLoginId)
        {
            var changes = userLoginRepository.Updatestatus(userLogin, UserLoginId);
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
    }
}
