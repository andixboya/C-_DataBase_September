

namespace ProductShop.ViewModels
{
    using ProductShop.Data;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    public class UsersAndProductsVM
    {
        public int usersCount { get; set; }

        public UsersAndProductsVM()
        {
            this.users = new List<SingleUser>();
        }
        
        public ICollection<SingleUser> users { get; set; }
    }

    public class SingleUser
    {
        public SingleUser()
        {
            //TODO: this is a must, true!
            this.soldProducts = new SoldProducts();
        }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public int? age { get; set; }

        public SoldProducts soldProducts { get; set; }
    }



    public class SoldProducts
    {
        public SoldProducts()
        {
            this.Products = new List<SingleProduct>();
        }
        public int Count { get; set; }

        public ICollection<SingleProduct> Products { get; set; }

    }

    public class SingleProduct
    {
        public string Name { get; set; }

        public decimal Price { get; set; }
    }
}
