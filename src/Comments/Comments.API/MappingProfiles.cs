using AutoMapper;
using Comments.API.Dtos;
using Comments.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Comments.API
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<CommentDto, Comment>().ReverseMap();
        }
    }
}
