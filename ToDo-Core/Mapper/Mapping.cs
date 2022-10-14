using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDo_Common.Extentions;
using ToDo_MainProject.Model;
using ToDo_ModelView.ModelView;

namespace ToDo_Core.Mapper
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<User, LoginUserResponse>().ReverseMap();
            CreateMap<UserModel, User>().ReverseMap();
            CreateMap<ItemModelView, Item>().ReverseMap();
            CreateMap<PagedResult<ItemModelView>, PagedResult<Item>>().ReverseMap();
            CreateMap<UserResult, User>().ReverseMap();



        }
    }
}
