

namespace FastFood.Web.MappingConfiguration
{
    using AutoMapper;
    using FastFood.Models;
    using FastFood.Web.ViewModels.Categories;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    public class CategoryProfile :Profile
    {
        public CategoryProfile()
        {
            this.CreateMap<CreateCategoryInputModel, Category>()
                .ForMember(d=> d.Name,y=> y.MapFrom(s=> s.CategoryName));

            this.CreateMap<Category, CategoryAllViewModel>();


        }
    }
}
