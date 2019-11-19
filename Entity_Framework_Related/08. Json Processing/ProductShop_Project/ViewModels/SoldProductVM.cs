using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ProductShop.ViewModels
{
    
    public class SoldProductVM
    {
        public SoldProductVM()
        {
            this.SoldProducts = new List<ProductVM>();
        }
        
        
        public string FirstName { get; set; }
        
        public string LastName { get; set; }

        
        public ICollection<ProductVM> SoldProducts { get; set; }
    }

    public class ProductVM
    {
        public string Name { get; set; }

        public decimal Price { get; set; }

        public string BuyerFirstName { get; set; }

        public string BuyerLastName { get; set; }
    }
}
