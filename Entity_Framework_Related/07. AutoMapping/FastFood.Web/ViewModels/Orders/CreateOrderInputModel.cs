using FastFood.Models.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FastFood.Web.ViewModels.Orders
{
    public class CreateOrderInputModel
    {
        [Required]
        public string Customer { get; set; }

        public int ItemId { get; set; }

        public int EmployeeId { get; set; }

        public OrderType Type { get; set; }

        public DateTime DateTime { get; set; }

        //[NotMapped]
        //public decimal TotalPrice { get; set; }

        [Range(0,50000)]
        public int Quantity { get; set; }

        
    }
}
