using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ProductShop.Data;
using ProductShop.Models;
using ProductShop.ViewModels;
using System.Reflection;
using Microsoft.EntityFrameworkCore;


namespace ProductShop
{
    public class StartUp
    {



        public static void Main(string[] args)
        {
            //Import();
            //here we load all of the profiles
            LoadProfilesToAutoMapper();


            //Exports:
            Export();

        }
        //Exports
        private static void Export()
        {
            using (var db = new ProductShopContext())
            {
                //GetUsersWithProducts
                //GetSoldProducts
                var result = GetUsersWithProducts(db);
                Console.WriteLine(result);
            }
        }

        //Imports:
        private static void Import()
        {
            string usersJson = File.ReadAllText("../../../Datasets/users.json");
            string productsJson = File.ReadAllText("../../../Datasets/products.json");
            string categoriesJson = File.ReadAllText("../../../Datasets/categories.json");
            string categoryProductJson = File.ReadAllText("../../../Datasets/categories-products.json");




            using (var db = new ProductShopContext())
            {
                //db.Database.EnsureDeleted();
                //db.Database.EnsureCreated();


                //ImportUsers(db, usersJson);
                //ImportProducts(db, productsJson);
                //ImportCategories(db, categoriesJson);
                var result = ImportCategoryProducts(db, categoryProductJson);
                Console.WriteLine(result);
            }
        }

        private static void LoadProfilesToAutoMapper()
        {
            var profiles = Assembly
               .GetExecutingAssembly()
               .GetTypes()
               .Where(t => t.IsSubclassOf(typeof(Profile))).ToList();

            Mapper.Initialize(cfg =>
            {
                profiles.ForEach(p => cfg.AddProfile(p));
            });
        }

        //01. Import Users
        public static string ImportUsers(ProductShopContext context, string inputJson)
        {
            var users = JsonConvert.DeserializeObject<User[]>(inputJson);

            context.Users.AddRange(users);
            int affectedRows = context.SaveChanges();

            return $"Successfully imported {affectedRows}";
        }

        //02. Import Products
        public static string ImportProducts(ProductShopContext context, string inputJson)
        {
            var products = JsonConvert.DeserializeObject<Product[]>(inputJson);

            context.Products.AddRange(products);
            int affectedRows = context.SaveChanges();

            return $"Successfully imported {affectedRows}";
        }

        //03. Import Categories
        public static string ImportCategories(ProductShopContext context, string inputJson)
        {
            var categories = JsonConvert.DeserializeObject<Category[]>(inputJson)
                .Where(c => !string.IsNullOrEmpty(c.Name))
                .ToList();

            context.Categories.AddRange(categories);
            int affectedRows = context.SaveChanges();

            return $"Successfully imported {affectedRows}";
        }

        //04. Import Categories and Products
        public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
        {
            var categoryProducts = JsonConvert.DeserializeObject<CategoryProduct[]>(inputJson);

            var inputs = categoryProducts.Select(cp => $"{cp.CategoryId} {cp.ProductId}");

            HashSet<string> values = new HashSet<string>();
            var result = new List<CategoryProduct>();

            int counter = 0;
            foreach (var key in inputs)
            {
                if (!values.Contains(key))
                {
                    values.Add(key);
                    result.Add(categoryProducts[counter]);
                }

                counter++;
            }

            context.CategoryProducts.AddRange(result);
            int affectedRows = context.SaveChanges();

            return $"Successfully imported {affectedRows}";
        }

        //05. Export Products in range:
        public static string GetProductsInRange(ProductShopContext context)
        {
            var products = context
                .Products
                .Where(p => 500 <= p.Price && p.Price <= 1000)
                .OrderBy(p => p.Price)
                //.Select(p => new ProductInRangeVM()
                //{
                //    Name = p.Name,
                //    Price = p.Price,
                //    Seller = $"{p.Seller.FirstName} {p.Seller.LastName}"
                //})
                .ProjectTo<ProductInRangeVM>()
                .ToList();



            var resolver = new DefaultContractResolver()
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };

            var stringResult = JsonConvert.SerializeObject(products, new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented,
                ContractResolver = resolver
            });


            return stringResult;
        }

        //06. Export Successfully Sold Products
        public static string GetSoldProducts(ProductShopContext context)
        {
            //well it works! 
            var users = context.Users
                .Where(u => u.ProductsSold.Any(p => p.Buyer != null))
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
            //.Select(u => new SoldProductVM()
            //{
            //    FirstName = u.FirstName,
            //    LastName = u.LastName,
            //    SoldProducts = u.ProductsSold.Select(p => new ProductVM()
            //    {
            //        BuyerFirstName = p.Buyer.FirstName,
            //        BuyerLastName = p.Buyer.LastName,
            //        Name = p.Name,
            //        Price = p.Price
            //    })
            //    .ToList()
            //})
            //.ToList();
            .ProjectTo<SoldProductVM>()
            .ToList();


            var resolver = new DefaultContractResolver()
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };

            string result = JsonConvert.SerializeObject(users, new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented,
                ContractResolver = resolver
            });


            return result;
        }

        //07. Export Categories By Products Count
        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            var categories = context
                .Categories
                .ProjectTo<CategoryByProductVM>()
                .OrderByDescending(c => c.ProductsCount)
                .ToList();

            var resolver = new DefaultContractResolver()
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };

            string result = JsonConvert.SerializeObject(categories, new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented,
                ContractResolver = resolver
            });

            return result;
        }

        //08. Export Users and Products

        public static string GetUsersWithProducts(ProductShopContext context)
        {
            var result = new UsersAndProductsVM();

            result.users = context.
                Users
                .Where(u => u.ProductsSold.Any(ps=> ps.Buyer!=null))
                
                .Select(u => new SingleUser
                {
                    firstName = u.FirstName
                    ,
                    age = u.Age
                    ,
                    lastName = u.LastName
                    ,
                    soldProducts = new SoldProducts
                    {
                        Count = u.ProductsSold.Where(ps=> ps.Buyer!=null).Count()
                        ,
                        Products = u.ProductsSold.Where(ps => ps.Buyer != null).Select(p => new SingleProduct
                        {
                            Name = p.Name,
                            Price = p.Price
                        })
                        .ToList()
                    }

                })
                .OrderByDescending(u => u.soldProducts.Count)
                .ToList();

            result.usersCount = result.users.Count;

            var resolver = new DefaultContractResolver()
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };

            string strResult = JsonConvert.SerializeObject(result, new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented,
                ContractResolver = resolver
                ,NullValueHandling=NullValueHandling.Ignore
            });


            return strResult;
        }
    }
}