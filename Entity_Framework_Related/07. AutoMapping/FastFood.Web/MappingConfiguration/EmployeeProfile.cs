

namespace FastFood.Web.MappingConfiguration
{
    using AutoMapper;
    using FastFood.Models;
    using FastFood.Web.ViewModels.Employees;
    using FastFood.Web.ViewModels.Orders.SubViews;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class EmployeeProfile :Profile
    {

        public EmployeeProfile()
        {
            this.CreateMap<Position, RegisterEmployeeViewModel>()
                .ForMember(d => d.PositionId, y => y.MapFrom(s => s.Id))
                .ForMember(d => d.PositionName, y => y.MapFrom(s => s.Name));

            this.CreateMap<RegisterEmployeeInputModel, Employee>()
                .ForMember(d => d.PositionId, y => y.MapFrom(s => s.PositionId));
            
            this.CreateMap<Employee, EmployeesAllViewModel>()
                .ForMember(d => d.Position, y => y.MapFrom(s => s.Position.Name));

            this.CreateMap<Employee, CreateOrderEmployeeView>();

        }
    }
}
