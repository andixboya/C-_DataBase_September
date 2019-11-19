using AutoMapper;
using ProductShop.Data;
using ProductShop.Models;
using ProductShop.ViewModels;
using System.Linq;

namespace ProductShop
{
    public class ProductShopProfile : Profile
    {
        public ProductShopProfile()
        {
            // 1 layer
            this.CreateMap<User, SoldProductVM>()
                .ForMember(d => d.FirstName, y => y.MapFrom(s => s.FirstName))
                .ForMember(d => d.LastName, y => y.MapFrom(s => s.LastName))
                .ForMember(d => d.SoldProducts, y => y.MapFrom(s => s.ProductsSold));


            //0 nesting
            this.CreateMap<Product, ProductInRangeVM>()
                .ForMember(d => d.Seller, y =>
                y.MapFrom(src => $"{src.Seller.FirstName} {src.Seller.LastName}"));


            //0 nesting
            this.CreateMap<Category, CategoryByProductVM>()
                .ForMember(d => d.Category, y => y.MapFrom(s => s.Name))
                .ForMember(d => d.ProductsCount, y => y.MapFrom(s => s.CategoryProducts.Count))
                .ForMember(d => d.AveragePrice, y => y.MapFrom(s => s.CategoryProducts.Average(cp => cp.Product.Price).ToString("f2")))
                .ForMember(d => d.TotalRevenue, y => y.MapFrom(s => s.CategoryProducts.Sum(cp => cp.Product.Price).ToString("f2")));


            this.CreateMap<User, SingleUser>()
                .ForPath(s => s.soldProducts.Products, y => y.MapFrom(x => x.ProductsSold));

        }
    }
}
