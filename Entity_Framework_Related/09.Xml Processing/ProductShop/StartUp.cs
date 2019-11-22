using AutoMapper;
using AutoMapper.QueryableExtensions;
using ProductShop.Data;
using ProductShop.Dtos;
using ProductShop.Dtos.Export;
using ProductShop.Dtos.Import;
using ProductShop.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace ProductShop
{
    public class StartUp
    {

        private static string defaultFormat = "Successfully imported {0}";
        public static void Main(string[] args)
        {
            Mapper.Initialize(cfg => cfg.AddProfile(new ProductShopProfile()));
            using (var db = new ProductShopContext())
            {
                //db.Database.EnsureDeleted();
                //db.Database.EnsureCreated();
                //Import(db);
                Export(db);
            }


        }

        private static void Export(ProductShopContext db)
        {
            var result = GetUsersWithProducts(db);
            Console.WriteLine(result);
        }

        private static void Import(ProductShopContext db)
        {
            string categoriesXml = "../../../Datasets/categories.xml";
            string usersXml = "../../../Datasets/users.xml";
            string productsXml = "../../../Datasets/products.xml";
            string categoriesAndProductsXml = "../../../Datasets/categories-products.xml";

            var categoriesInput = File.ReadAllText(categoriesXml);
            var usersInput = File.ReadAllText(usersXml);
            var productInput = File.ReadAllText(productsXml);
            var categoriesAndProductInput = File.ReadAllText(categoriesAndProductsXml);


            //ImportUsers(db, usersInput);
            //ImportCategories(db, categoriesInput);
            //ImportProducts(db, productInput);

            //var result = ImportCategoryProducts(db, categoriesAndProductInput);

            //Console.WriteLine(result);
        }

        // 1. Import Users
        public static string ImportUsers(ProductShopContext context, string inputXml)
        {
            var serializer = new XmlSerializer(typeof(ImportUserDto[]), new XmlRootAttribute("Users"));
            int affectedRows = -1;

            using (var reader = new StringReader(inputXml))
            {
                var importedUsers = (ImportUserDto[])serializer.Deserialize(reader);

                var usersToAdd = Mapper.Map<User[]>(importedUsers);
                context.Users.AddRange(usersToAdd);

                affectedRows = context.SaveChanges();
            }

            return string.Format(defaultFormat, affectedRows);
        }

        //02. Import Products (usually... you can`t import nulls , but it seems they `ve left the tests for this with 200..., which is weird (nullables cannot be inserted, even if within the table Products we got an option for a nullable user, iT STILLL points to AN USER who has PK (NOT NULLABLE)...
        public static string ImportProducts(ProductShopContext context, string inputXml)
        {
            var serializer = new XmlSerializer(typeof(ImportProductDto[]), new XmlRootAttribute("Products"));
            int affectedRows = -1;

            var validUsersId = context.Users.Select(u => u.Id).ToList();



            using (var reader = new StringReader(inputXml))
            {
                var importedProducts = (ImportProductDto[])serializer.Deserialize(reader);


                var productsToAdd = Mapper.Map<Product[]>(importedProducts);

                var validIds = context.Users.Select(u => u.Id).ToList();

                var result = new List<Product>();
                foreach (var p in productsToAdd)
                {
                    //buyer can be null, therefore we do not check it ? 
                    if (p.BuyerId != 0 && validIds.Contains(p.SellerId))
                    {
                        result.Add(p);
                    }

                }



                context.Products.AddRange(result);

                affectedRows = context.SaveChanges();
            }

            return string.Format(defaultFormat, affectedRows);
        }


        // 3. Import Categories
        public static string ImportCategories(ProductShopContext context, string inputXml)
        {
            var serializer = new XmlSerializer(typeof(CategoryImportDto[]), new XmlRootAttribute("Categories"));
            int affectedRows = -1;

            using (var reader = new StringReader(inputXml))
            {

                var importedCategories = (CategoryImportDto[])serializer.Deserialize(reader);

                var categoriesToAdd = Mapper.Map<Category[]>(importedCategories).Where(c => !string.IsNullOrEmpty(c.Name));
                context.Categories.AddRange(categoriesToAdd);

                affectedRows = context.SaveChanges();
            }



            return string.Format(defaultFormat, affectedRows);
        }

        //4. Import Categories and Products
        public static string ImportCategoryProducts(ProductShopContext context, string inputXml)
        {
            var serializer = new XmlSerializer(typeof(ImportCategoryProductDto[]), new XmlRootAttribute("CategoryProducts"));
            int affectedRows = -1;

            var validProductIds = context.Products.Select(p => p.Id).ToHashSet<int>();
            var validCategoriesIds = context.Categories.Select(c => c.Id).ToHashSet<int>();

            var result = new List<CategoryProduct>();
            using (var reader = new StringReader(inputXml))
            {
                var importedCategoryProducts = (ImportCategoryProductDto[])serializer.Deserialize(reader);

                var categoryProductsToAdd = Mapper.Map<CategoryProduct[]>(importedCategoryProducts);
                var uniqueCombos = new HashSet<string>();

                foreach (var cp in categoryProductsToAdd)
                {
                    string currentCombination = $"{cp.CategoryId}-{cp.ProductId}";

                    if (validProductIds.Contains(cp.ProductId) &&
                        validCategoriesIds.Contains(cp.CategoryId) &&
                        !uniqueCombos.Contains(currentCombination))
                    {
                        uniqueCombos.Add(currentCombination);
                        result.Add(cp);
                    }
                }

                context.CategoryProducts.AddRange(result);
                affectedRows = context.SaveChanges();
            }


            return string.Format(defaultFormat, affectedRows);
        }


        //5 Products In Range
        public static string GetProductsInRange(ProductShopContext context)
        {
            //note: add here the necessary dto object + root
            var serializer = new XmlSerializer(typeof(ProductInRangeDto[]), new XmlRootAttribute("Products"));

            StringBuilder sb = new StringBuilder();


            //get the objects from db into dto format
            var objectsToSerialize = context.Products
                .ProjectTo<ProductInRangeDto>()
                .Where(p => 500 <= p.Price && p.Price <= 1000)
                .OrderBy(p => p.Price)
                .Take(10)
                .ToArray();


            //for removal of namespace
            var nameSpaces = new XmlSerializerNamespaces
                (
                  new[] { XmlQualifiedName.Empty }
                );

            //and writer , who needs a sb, everyrthing is written in the sb!
            using (StringWriter writer = new StringWriter(sb))
            {
                serializer.Serialize(writer, objectsToSerialize, nameSpaces);
            };


            var result = sb.ToString();
            return result;
        }

        //6. Sold Products
        public static string GetSoldProducts(ProductShopContext context)
        {

            var serializer = new XmlSerializer(typeof(ExportUserDto[]), new XmlRootAttribute("Users"));

            StringBuilder sb = new StringBuilder();


            //get the objects from db into dto format
            var objectsToSerialize = context.Users
                .Where(u => u.ProductsSold.Any(ps => ps.Buyer != null))
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                .Take(5)
                .ProjectTo<ExportUserDto>()
                .ToArray();


            //for removal of namespace
            var nameSpaces = new XmlSerializerNamespaces
                (
                  new[] { XmlQualifiedName.Empty }
                );

            //and writer , who needs a sb, everyrthing is written in the sb!
            using (StringWriter writer = new StringWriter(sb))
            {
                serializer.Serialize(writer, objectsToSerialize, nameSpaces);
            };


            var result = sb.ToString();
            return result;
        }


        //7. Categories by Products Count
        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {

            var serializer = new XmlSerializer(typeof(List<ExportCategoryByProductCountDto>), new XmlRootAttribute("Categories"));

            StringBuilder sb = new StringBuilder();


            //get the objects from db into dto format
            var objectsToSerialize = context.Categories
                .Select(c => new ExportCategoryByProductCountDto
                {
                    Name = c.Name,
                    Count = c.CategoryProducts.Count,
                    AveragePrice = c.CategoryProducts.Average(cp => cp.Product.Price),
                    TotalRevenue = c.CategoryProducts.Sum(cp => cp.Product.Price)
                })
                //.ProjectTo<ExportCategoryByProductCountDto>()
                .OrderByDescending(c => c.Count)
                .ThenByDescending(c => c.TotalRevenue)
                .ToList();


            //for removal of namespace
            var nameSpaces = new XmlSerializerNamespaces
            (
              new[] { XmlQualifiedName.Empty }
            );


            //and writer , who needs a sb, everyrthing is written in the sb!
            using (StringWriter writer = new StringWriter(sb))
            {
                serializer.Serialize(writer, objectsToSerialize, nameSpaces);
            };


            return sb.ToString();
        }

        //8. Users and Products
        public static string GetUsersWithProducts(ProductShopContext context)
        {
            //var serializer = new XmlSerializer(typeof(ExportUsersWithProductsDto), new XmlRootAttribute("Users"));

            StringBuilder sb = new StringBuilder();

            var objectToSerialize = new ExportUsersWithProductsDto();

            //get the objects from db into dto format
            objectToSerialize.SingleUsers = context.Users
                .Where(u=> u.ProductsSold.Any())
                .OrderByDescending(u=> u.ProductsSold.Count)
                .Take(10)
                .ProjectTo<AnotherSingleUser>()
                //.Select(u => new AnotherSingleUser()
                //{
                //    FirstName = u.FirstName,
                //    LastName = u.LastName,
                //    Age = u.Age,
                //    SoldProduct = new SoldProductsType()
                //    {
                //        Count = u.ProductsSold.Count,
                //        SoldProducts = u.ProductsSold.Select(p => new SingleProduct
                //        {
                //            Name = p.Name,
                //            Price = p.Price
                //        })
                //        .OrderByDescending(p=> p.Price)
                //        .ToArray()
                //    }
                //})
                .ToArray();
            
            objectToSerialize.Count = context.Users.Where(u=> u.ProductsSold.Any()).Count();

            //for removal of namespace
            var nameSpaces = new XmlSerializerNamespaces
            (
              new[] { XmlQualifiedName.Empty }
            );


            //and writer , who needs a sb, everyrthing is written in the sb!
            using (StringWriter writer = new StringWriter(sb))
            {
                //serializer.Serialize(writer, objectToSerialize, nameSpaces);
            };



            return sb.ToString();
        }
    }
}