

namespace ProductShop.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    public class CategoryByProductVM
    {

        public string Category { get; set; }

        public int ProductsCount { get; set; }

        public string AveragePrice { get; set; }
        public string TotalRevenue { get; set; }

    }
}
