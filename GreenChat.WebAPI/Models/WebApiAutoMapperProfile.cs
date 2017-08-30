using AutoMapper;
using GreenChat.BLL.DTO;
using GreenChat.WebAPI.Models.AccountModels;

namespace GreenChat.WebAPI.Models
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