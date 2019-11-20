namespace FastFood.Web.Controllers
{
    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Linq;

    using Data;
    using ViewModels.Orders;
    using AutoMapper.QueryableExtensions;
    using FastFood.Web.ViewModels.Orders.SubViews;
    using FastFood.Models.Enums;
    using FastFood.Models;
    using System.Globalization;

    public class OrdersController : Controller
    {
        private readonly FastFoodContext context;
        private readonly IMapper mapper;

        public OrdersController(FastFoodContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public IActionResult Create()
        {

            var viewOrder = new CreateOrderViewModel()
            {
                Items = this.context.Items.ProjectTo<CreateOrderItemView>(this.mapper.ConfigurationProvider).ToList(),
                Employees = this.context.Employees.ProjectTo<CreateOrderEmployeeView>(this.mapper.ConfigurationProvider).ToList()
            };

            return this.View(viewOrder);
        }

        [HttpPost]
        public IActionResult Create(CreateOrderInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.RedirectToAction("Error", "Home");
            }

            model.DateTime = DateTime.UtcNow;
            //total price and quantity... i guess will be skipped
            model.Type = OrderType.ForHere;//for here will be default

            var orderToAdd = this.mapper.Map<Order>(model);
            context.Add(orderToAdd);
            context.SaveChanges();

            return this.RedirectToAction("All", "Orders");
        }

        public IActionResult All()
        {
            var orders = this.context
                .Orders
                .ProjectTo<OrderAllViewModel>
                (this.mapper.ConfigurationProvider)
                .ToList();
           

            return this.View(orders);
        }
    }
}
