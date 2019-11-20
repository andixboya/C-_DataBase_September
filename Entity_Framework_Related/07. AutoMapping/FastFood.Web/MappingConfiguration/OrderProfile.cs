

namespace FastFood.Web.MappingConfiguration
{
    using AutoMapper;
    using FastFood.Models;
    using FastFood.Web.ViewModels.Orders;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class OrderProfile : Profile
    {

        public OrderProfile()
        {
            this.CreateMap<CreateOrderInputModel, Order>();

            this.CreateMap<Order, OrderAllViewModel>()
                .ForMember(d => d.Employee, y => y.MapFrom(src => src.Employee.Name))
                .ForMember(d=> d.OrderId,y=> y.MapFrom(src=> src.Id))
                .ForMember(d=> d.DateTime,y=> y.MapFrom(src=> src.DateTime.ToString("dd-MM-yyyy")));
        }
    }
}
