using AuthorizationService.Dtos;
using AuthorizationService.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorizationService.Services
{
    public class MappingService : Profile
    {
        public MappingService()
        {
            CreateMap<User, RegistrationUserDto>().ReverseMap();
            CreateMap<User, LoginUserDto>().ReverseMap();
            CreateMap<User, UserDto>().ReverseMap();
        }
    }
}
