namespace Cinema.DataProcessor
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;
    using Cinema.DataProcessor.ExportDto;
    using Data;
    using Newtonsoft.Json;

    public class Serializer
    {
        public static string ExportTopMovies(CinemaContext context, int rating)
        {

            var moviewViews = context
                .Movies
                .Where(m => m.Rating >= rating && m.Projections.Any(p => p.Tickets.Count > 0))
                .Select(m => new MovieExportDto()
                {
                    Title = m.Title,
                    Rating = m.Rating.ToString("f2"),
                    TotalIncomes = m.Projections.Where(p => p.Tickets.Count > 0).Sum(p => p.Tickets.Sum(t => t.Price)).ToString("f2"),
                    Customers = m.Projections.Where(p => p.Tickets.Count > 0).SelectMany(p => p.Tickets).Select(t => new SingleCustomerView()
                    {
                        Balance = t.Customer.Balance.ToString("f2"),
                        FirstName = t.Customer.FirstName,
                        LastName = t.Customer.LastName
                    })
                    .OrderByDescending(c => c.Balance)
                    .ThenBy(c => c.FirstName)
                    .ThenBy(c => c.LastName)
                    .ToList()
                })
                .OrderByDescending(m => decimal.Parse(m.Rating))
                .ThenByDescending(m => decimal.Parse(m.TotalIncomes))
                .Take(10)
                .ToList();

            var result = JsonConvert.SerializeObject(moviewViews, Newtonsoft.Json.Formatting.Indented);


            return result;
        }

        public static string ExportTopCustomers(CinemaContext context, int age)
        {

            var customersToExport = context.Customers
                .Where(c => c.Age >= age)
                .Select(c => new CustomerExportDto()
                {
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    SpentTime = TimeSpan.FromMilliseconds(c.Tickets.Sum(t => t.Projection.Movie.Duration.TotalMilliseconds))
                    .ToString(@"hh\:mm\:ss", CultureInfo.InvariantCulture),
                    SpentMoney = c.Tickets.Sum(t => t.Price).ToString("f2")
                })
                .OrderByDescending(c => decimal.Parse(c.SpentMoney))
                .Take(10)
              .ToArray();

            var namespaces = new XmlSerializerNamespaces(new[] { new XmlQualifiedName() });

            XmlRootAttribute root = new XmlRootAttribute("Customers");

            var serializer = new XmlSerializer(typeof(CustomerExportDto[]), root);

            StringBuilder sb = new StringBuilder();

            using (var writer = new StringWriter(sb))
            {
                serializer.Serialize(writer, customersToExport, namespaces);
            }


            var x = sb.ToString().TrimEnd();
            return x;W

        }
    }
}