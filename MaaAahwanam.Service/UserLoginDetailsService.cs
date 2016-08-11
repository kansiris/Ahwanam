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
            userLogin.Status = "Active";
            userLogin.RegDate = DateTime.Now;
            userLogin.UpdatedDate = DateTime.Now;
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
        public UserLogin changepassword(UserLogin userLogin, int UserLoginId)
        {
            var changes = userLoginRepository.UpdatePassword(userLogin, UserLoginId);
            return changes;
        }


    }
}
