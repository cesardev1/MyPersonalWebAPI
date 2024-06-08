using AutoMapper;
using MyPersonalWebAPI.Models;
using MyPersonalWebAPI.Models.Request;

namespace MyPersonalWebAPI
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<NewUserRequest, User>();
        }
    }
}
