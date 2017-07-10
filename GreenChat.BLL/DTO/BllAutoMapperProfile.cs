using AutoMapper;
using GreenChat.DAL.Models;

namespace GreenChat.BLL.DTO
{
    public class BllAutoMapperProfile : Profile
    {
        public BllAutoMapperProfile()
        {
            CreateMap<ApplicationUser, UserDto>().ForMember(dto=> dto.User, m=>m.MapFrom(user=>user));
            CreateMap<UserDto, ApplicationUser>().AfterMap((dto, user) => dto.User = user);
        }
    }
}