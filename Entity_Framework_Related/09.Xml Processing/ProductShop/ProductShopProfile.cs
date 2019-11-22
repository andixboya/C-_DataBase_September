using AutoMapper;
using ProductShop.Dtos;
using ProductShop.Dtos.Export;
using ProductShop.Dtos.Import;
using ProductShop.Models;
using System.Collections.Generic;
using System.Linq;

namespace ProductShop
{
    public class ProductShopProfile : Profile
    {
        public ProductShopProfile()
        {
            //for Import:
            this.CreateMap<CategoryImportDto, Category>();
            this.CreateMap<ImportUserDto, User>();
            this.CreateMap<ImportProductDto, Product>();
            this.CreateMap<ImportCategoryProductDto, CategoryProduct>();

            //for Export:
            this.CreateMap<Product, ProductInRangeDto>()
                .ForMember(s => s.FullName, y => y.MapFrom(s => $"{s.Buyer.FirstName} {s.Buyer.LastName}"));


            //for Export User
            this.CreateMap<User, ExportUserDto>()
                .ForMember(d => d.SoldProducts, x => x.MapFrom(s => s.ProductsSold));
            this.CreateMap<Product, SoldProductDto>();


            this.CreateMap<Category, ExportCategoryByProductCountDto>()
                .ForMember(d => d.Count, y => y.MapFrom(s => s.CategoryProducts.Count))
                .ForMember(d => d.AveragePrice, y => y.MapFrom(s => s.CategoryProducts.Average(pc => pc.Product.Price)))
                .ForMember(d => d.TotalRevenue, y => y.MapFrom(s => s.CategoryProducts.Sum(pc => pc.Product.Price)));



            this.CreateMap<User, AnotherSingleUser>()
                .ForPath(x => x.SoldProduct, y => y.MapFrom(s => s))
                .AfterMap((s, d) =>
                {
                    d.SoldProduct = new SoldProductsType()
                    {
                        Count = 5,
                        SoldProducts = Mapper.Map<SingleProduct[]>(s.ProductsSold.ToArray())
                    };

                }
                );

        }
    }
}
