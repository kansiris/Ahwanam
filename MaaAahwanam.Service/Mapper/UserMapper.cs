using MaaAahwanam.Models;
using AutoMapper;

namespace MaaAahwanam.Service.Mapper
{
    public class UserMapper
    {
        public UserMapper()
        {

        }
        public UserLogin MapUserRequestToUserLogin(UserRequest userRequest)
        {
            AutoMapper.Mapper.CreateMap<UserRequest, UserLogin>();
            var userLogin = AutoMapper.Mapper.Map<UserRequest, UserLogin>(userRequest);
            return userLogin;
        }
        public UserResponse MapUserDetailToUserResponse(UserDetail userDetail)
        {
            AutoMapper.Mapper.CreateMap<UserDetail, UserResponse>();
            var userResponse = AutoMapper.Mapper.Map<UserDetail, UserResponse>(userDetail);
            return userResponse;
        }
    }
}
