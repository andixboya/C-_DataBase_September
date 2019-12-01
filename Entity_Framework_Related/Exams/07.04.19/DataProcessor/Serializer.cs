namespace Cinema.DataProcessor
{
    using System;

    using Data;

    public class Serializer
    {
        public static string ExportTopMovies(CinemaContext context, int rating)
        {




            throw new NotImplementedException();
        }

        public static string ExportTopCustomers(CinemaContext context, int age)
        {

            //var carsToExport = context.Suppliers
            //  .Where(s => s.IsImporter == false)
            //  .ProjectTo<ExportLocalSupplierDto>()
            //  .ToArray();

            //var namespaces = new XmlSerializerNamespaces(new[] { new XmlQualifiedName() });

            //XmlRootAttribute root = new XmlRootAttribute("suppliers");

            //var serializer = new XmlSerializer(typeof(ExportLocalSupplierDto[]), root);

            //StringBuilder sb = new StringBuilder();

            //using (var writer = new StringWriter(sb))
            //{
            //    serializer.Serialize(writer, carsToExport, namespaces);
            //}

            return sb.ToString().TrimEnd();



            throw new NotImplementedException();
        }
    }
}