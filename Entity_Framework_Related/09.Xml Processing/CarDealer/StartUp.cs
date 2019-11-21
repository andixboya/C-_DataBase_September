using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using AutoMapper;
using CarDealer.Data;
using CarDealer.Dtos.Import;
using CarDealer.Models;

namespace CarDealer
{
    public class StartUp
    {
        private  const string DefaultLayout = @"Successfully imported {0}";

        public static void Main(string[] args)
        {

            string xmlInputSuppliers = File.ReadAllText(@"..\..\..\Datasets\suppliers.xml");

            string xmlInputParts = File.ReadAllText(@"..\..\..\Datasets\parts.xml");

            string xmlInputCar = File.ReadAllText(@"..\..\..\Datasets\cars.xml");


            Mapper.Initialize(c=> c.AddProfile(new CarDealerProfile()));

            using (var context= new CarDealerContext())
            {
                

                string result = ImportCars(context, xmlInputCar);

                Console.WriteLine(result);
            }

        }



        public static string ImportSuppliers(CarDealerContext context, string inputXml)
        {

            XmlSerializer serializer= new XmlSerializer(typeof(ImportSupplierDto[]) ,new XmlRootAttribute("Suppliers"));

            var supplierDtos = (ImportSupplierDto[]) serializer.Deserialize(new StringReader(inputXml));

            var suppliers= new List<Supplier>();
            foreach (var dto in supplierDtos)
            {

                var supplier = Mapper.Map<Supplier>(dto);
                suppliers.Add(supplier);
            }

            context.Suppliers.AddRange(suppliers);

            int affectedRows = context.SaveChanges();

            return string.Format(DefaultLayout,affectedRows);
        }


        public static string ImportParts(CarDealerContext context, string inputXml)
        {

            XmlSerializer serializer= new XmlSerializer(typeof(ImportPartDto[]),new XmlRootAttribute("Parts"));


            var partsDtos =  (ImportPartDto[])serializer.Deserialize(new StringReader(inputXml));

            List<Part> parts
                = new List<Part>();

            foreach (var dto in partsDtos)
            {
                var supplier = context.Suppliers.Find(dto.SupplierId);



                if (supplier is null)
                {
                    continue;
                }

                var partToAdd= Mapper.Map<Part>(dto);

                parts.Add(partToAdd);
            }


            context.Parts.AddRange(parts);

            int affectedRows = context.SaveChanges();

            return string.Format(DefaultLayout,affectedRows);


        }

        public static string ImportCars(CarDealerContext context, string inputXml)
        {
            XmlSerializer serializer= new XmlSerializer(typeof(ImportCarDto[]), new XmlRootAttribute("Cars"));

            var carDtos = (ImportCarDto[]) serializer.Deserialize(new StringReader(inputXml));


            var carsToAdd = new List<Car>();



            return null;
        }

    }
}