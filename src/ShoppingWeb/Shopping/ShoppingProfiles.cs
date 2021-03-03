using AuthorizationService.Dtos;
using AutoMapper;
using Shopping.DTOs;
using Shopping.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shopping
{
    public class ShoppingProfiles : Profile
    {
        public ShoppingProfiles()
        {
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<User, RegistrationUser>().ReverseMap();
            CreateMap<User, LoginUser>().ReverseMap();
            CreateMap<CommentDto, Comment>().ReverseMap();
        }
    }
}
