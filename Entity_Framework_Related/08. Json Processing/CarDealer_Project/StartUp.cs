using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using AutoMapper;
using CarDealer.Data;
using CarDealer.DTO.Import;
using CarDealer.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace CarDealer
{
    public class StartUp
    {

        private static string defaultMessage = "Successfully imported {0}.";
        public static void Main(string[] args)
        {
            //Import();
            Export();

        }

        private static void Export()
        {
            using (var db = new CarDealerContext())
            {
                var result = GetSalesWithAppliedDiscount(db);
                Console.WriteLine(result);


            }
        }

        private static void Import()
        {
            string customersJson = File.ReadAllText("../../../Datasets/customers.json");
            string suppliersJson = File.ReadAllText("../../../Datasets/suppliers.json");
            string partsJson = File.ReadAllText("../../../Datasets/parts.json");
            string carsJson = File.ReadAllText("../../../Datasets/cars.json");
            string salesJson = File.ReadAllText("../../../Datasets/sales.json");



            using (var db = new CarDealerContext())
            {

                //db.Database.EnsureDeleted();
                //db.Database.EnsureCreated();
                //ImportSuppliers(db, suppliersJson);
                //ImportParts(db, partsJson);
                //ImportCars(db, carsJson);
                //ImportCustomers(db, customersJson);
                //ImportSales(db, salesJson);

                //var result = ImportSales(db, salesJson);

                //Console.WriteLine(result);
            }
        }

        //09. Import Suppliers
        public static string ImportSuppliers(CarDealerContext context, string inputJson)
        {
            var result = JsonConvert.DeserializeObject<Supplier[]>(inputJson);

            context.Suppliers.AddRange(result);
            int affectedRows = context.SaveChanges();

            return string.Format(defaultMessage, affectedRows);
        }

        //10. Import Parts
        public static string ImportParts(CarDealerContext context, string inputJson)
        {
            var suppliersId = context.Suppliers
                .Select(s => s.Id)
                .ToHashSet<int>();

            var parts = JsonConvert.DeserializeObject<Part[]>(inputJson)
                .Where(p => suppliersId.Contains(p.SupplierId))
                .ToList();

            context.Parts.AddRange(parts);
            int affectedRows = context.SaveChanges();

            return string.Format(defaultMessage, affectedRows);
        }

        //11. Import Cars
        public static string ImportCars(CarDealerContext context, string inputJson)
        {
            var realParts = context.Parts.Select(x => x.Id).ToHashSet<int>();

            var cars = JsonConvert.DeserializeObject<CarImportDto[]>(inputJson)
                .ToList();

            var carsToAdd = cars.Select(c => new Car
            {
                Make = c.Make,
                TravelledDistance = c.TravelledDistance,
                Model = c.Model
            })
                .ToList();

            context.AddRange(carsToAdd);


            int affectedRows = context.SaveChanges();

            var otherCombos = new HashSet<string>();

            var result = new List<PartCar>();
            var uniqueRelations = new HashSet<string>();

            int counter = 0;

            foreach (var c in cars)
            {
                foreach (var pId in c.PartCars.Where(p => realParts.Contains(p)))
                {
                    var idOfCar = carsToAdd[counter].Id;
                    var currentStr = $"{idOfCar}-{pId}";

                    if (!uniqueRelations.Contains(currentStr))
                    {
                        uniqueRelations.Add(currentStr);
                        var current = new PartCar()
                        {
                            CarId = idOfCar,
                            PartId = pId
                        };

                        result.Add(current);
                    }

                }

                counter++;
            }
            context.AddRange(result);
            context.SaveChanges();

            return string.Format(defaultMessage, affectedRows);
        }


        //12. Import Customers
        public static string ImportCustomers(CarDealerContext context, string inputJson)
        {
            var customersToAdd = JsonConvert.DeserializeObject<Customer[]>(inputJson);
            context.Customers.AddRange(customersToAdd);
            int affectedRows = context.SaveChanges();

            return string.Format(defaultMessage, affectedRows);
        }

        //13. Import Sales
        public static string ImportSales(CarDealerContext context, string inputJson)
        {
            //var validCars = context.Cars.Select(x => x.Id).ToHashSet<int>();
            //var validCustomers = context.Customers.Select(x => x.Id).ToHashSet<int>();
            //.Where(c => validCars.Contains(c.CarId) && validCustomers.Contains(c.CustomerId)) 
            //skip the ones... without cars !?!!?!?, yet this works
            var sales = JsonConvert.DeserializeObject<Sale[]>(inputJson).ToList();


            context.Sales.AddRange(sales);

            int affectedRows = context.SaveChanges();
            return string.Format(defaultMessage, affectedRows);
        }


        //14.  Export Ordered Customers
        public static string GetOrderedCustomers(CarDealerContext context)
        {
            var customers = context.Customers
                .OrderBy(c => c.BirthDate)
                .ThenBy(c => c.IsYoungDriver)
                .Select(c => new
                {
                    c.Name,
                    BirthDate = c.BirthDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)
                    ,
                    c.IsYoungDriver
                })
                .ToList();

            string result = JsonConvert.SerializeObject(customers, Formatting.Indented);

            return result;
        }

        //15. Export Cars from make Toyota
        public static string GetCarsFromMakeToyota(CarDealerContext context)
        {
            var cars = context.
                Cars.Where(c => c.Make == "Toyota")
                .OrderBy(c => c.Model)
                .ThenByDescending(c => c.TravelledDistance)
                .Select(c => new
                {
                    c.Id,
                    c.Make,
                    c.Model,
                    c.TravelledDistance
                })
                .ToList();


            var result = string.Empty;

            string answer = JsonConvert.SerializeObject(cars, Formatting.Indented);

            return answer;
        }

        //16. Export Local Suppliers
        public static string GetLocalSuppliers(CarDealerContext context)
        {
            var suppliers = context
                .Suppliers
                .Where(s => !s.IsImporter)
                .Select(s => new
                {
                    s.Id,
                    s.Name,
                    PartsCount = s.Parts.Count
                })
                .ToList();
            var result = string.Empty;

            string answer = JsonConvert.SerializeObject(suppliers, Formatting.Indented);

            return answer;
        }

        // 17.  Export Cars with Their List of Parts
        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            var cars = context
                .Cars
                .Select(c => new CarWithPartsVM
                {
                    Car = new SingleCar
                    {
                        Make = c.Make,
                        Model = c.Model,
                        TravelledDistance = c.TravelledDistance
                    },
                    Parts = c.PartCars.Select(p => new SinglePart
                    {
                        Name = p.Part.Name,
                        Price = p.Part.Price.ToString("f2")
                    })
                    .ToList()
                })
                .ToList();

            var result = string.Empty;

            string answer = JsonConvert.SerializeObject(cars, Formatting.Indented);

            return answer;
        }

        //18. Export Total Sales by Customer
        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            var carOwners = context.Customers
                .Where(c => c.Sales.Any())
                .Select(c => new
                {
                    fullName = c.Name,
                    boughtCars = c.Sales.Count,
                    spentMoney = c.Sales.Select(s => s.Car.PartCars.Sum(pc => pc.Part.Price)).Sum()
                })
                .OrderByDescending(c => c.spentMoney)
                .ThenByDescending(c => c.boughtCars)
                .ToList();


            var result = string.Empty;

            string answer = JsonConvert.SerializeObject(carOwners, Formatting.Indented);

            return answer;
        }

        //19. Export Sales with Applied Discount
        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            var sales = context
                .Sales
                .Take(10)
                .Select(s => new
                {
                    car = new
                    {
                        s.Car.Make,
                        s.Car.Model,
                        s.Car.TravelledDistance
                    },
                    customerName = s.Customer.Name,
                    Discount = s.Discount.ToString("f2"),
                    price = s.Car.PartCars.Sum(cp => cp.Part.Price).ToString("f2"),
                    priceWithDiscount = (s.Car.PartCars.Sum(cp => cp.Part.Price) *((100m- s.Discount)/100)).ToString("f2")
                });



            var result = string.Empty;

            string answer = JsonConvert.SerializeObject(sales, Formatting.Indented);

            return answer;
        }
    }
}