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
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<UserRequest, UserLogin>();
            });
            IMapper mapper = config.CreateMapper();
            var source = userRequest;
            var dest = mapper.Map<UserRequest, UserLogin>(source);
            return dest;
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
