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
            //AutoMapper.Mapper.CreateMap<UserRequest, UserLogin>();
            var userLogin = AutoMapper.Mapper.Map<UserRequest, UserLogin>(userRequest);
            return userLogin;
        }
        public UserResponse MapUserDetailToUserResponse(UserDetail userDetail)
        {
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<UserDetail, UserResponse>();
            });
            IMapper mapper = config.CreateMapper();
            var source = userDetail;
            var dest = mapper.Map<UserDetail, UserResponse>(source);
            return dest;
        }
    }
}
