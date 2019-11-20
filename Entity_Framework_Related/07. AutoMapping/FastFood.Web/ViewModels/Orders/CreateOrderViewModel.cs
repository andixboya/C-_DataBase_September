namespace FastFood.Web.ViewModels.Orders
{
    using FastFood.Web.ViewModels.Orders.SubViews;
    using System.Collections.Generic;

    public class CreateOrderViewModel
    {
        public List<CreateOrderItemView> Items { get; set; }

        public List<CreateOrderEmployeeView> Employees { get; set; }
    }
}
