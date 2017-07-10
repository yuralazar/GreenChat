using AutoMapper;
using CreenChat.WebAPI.Models.AccountModels;
using GreenChat.BLL.DTO;

namespace CreenChat.WebAPI.Models
{
    public class WebApiAutoMapperProfile : Profile
    {
        public WebApiAutoMapperProfile()
        {
            CreateMap<Login, UserDto>();
            CreateMap<UserDto, Login>();
            
            CreateMap<Register, UserDto>().ForMember(obj => obj.UserName, m=>m.MapFrom(s=>s.Email));
            CreateMap<UserDto, Register>();
        }
    }
}