

namespace FastFood.Web.MappingConfiguration
{
    using AutoMapper;
    using FastFood.Models;
    using FastFood.Web.ViewModels.Items;
    using FastFood.Web.ViewModels.Orders.SubViews;
    using FastFood.Web.ViewModels.Positions;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    public class ItemProfile :Profile
    {
        public ItemProfile()
        {
            this.CreateMap<Category, CreateItemViewModel>()
                .ForMember(d => d.CategoryId, y => y.MapFrom(s => s.Id));

            this.CreateMap<CreateItemInputModel, Item>()
                .ForMember(d => d.CategoryId, y => y.MapFrom(s => s.CategoryId));

            this.CreateMap<Item, ItemsAllViewModels>()
                .ForMember(d => d.Category, y => y.MapFrom(s => s.Category.Name));


            this.CreateMap<Item, CreateOrderItemView>();
        }
    }
}
