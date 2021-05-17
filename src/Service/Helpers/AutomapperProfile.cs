using AutoMapper;
using Models.Entities;
using Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Helpers
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<Story, StoryResponse>();
        }
    }
}
