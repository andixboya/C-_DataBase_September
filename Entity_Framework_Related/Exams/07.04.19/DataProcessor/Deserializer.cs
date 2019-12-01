namespace Cinema.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;
    using Cinema.Data.Models;
    using Cinema.Data.Models.Enums;
    using Cinema.DataProcessor.ImportDto;
    using Data;
    using Newtonsoft.Json;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";
        private const string SuccessfulImportMovie
            = "Successfully imported {0} with genre {1} and rating {2}!";
        private const string SuccessfulImportHallSeat
            = "Successfully imported {0}({1}) with {2} seats!";
        private const string SuccessfulImportProjection
            = "Successfully imported projection {0} on {1}!";
        private const string SuccessfulImportCustomerTicket
            = "Successfully imported customer {0} {1} with bought tickets: {2}!";

        public static string ImportMovies(CinemaContext context, string jsonString)
        {
            var movieDtos = JsonConvert.DeserializeObject<MovieImportDto[]>(jsonString);


            List<Movie> validMovies = new List<Movie>();
            var movieList = new HashSet<string>();
            StringBuilder listResult = new StringBuilder();
            foreach (var importedMovie in movieDtos)
            {

                var isContained = movieList.Contains(importedMovie.Title);
                var isValid = IsValid(importedMovie);
                var IsEnum = Enum.TryParse(importedMovie.Genre, out Genre genre);

                if (isContained is true || isValid is false || IsEnum is false)
                {
                    listResult.AppendLine(ErrorMessage);
                    continue;
                }

                movieList.Add(importedMovie.Title);
                var movieToAdd = new Movie()
                {
                    Director = importedMovie.Director,
                    Duration = importedMovie.Duration,
                    Genre = genre,
                    Rating = importedMovie.Rating,
                    Title = importedMovie.Title
                };
                validMovies.Add(movieToAdd);
                listResult.AppendLine(string.Format(SuccessfulImportMovie, movieToAdd.Title, movieToAdd.Genre, movieToAdd.Rating.ToString("f2")));
            }

            context.AddRange(validMovies);
            context.SaveChanges();

            var x = listResult.ToString().TrimEnd();

            return x;
        }

        public static string ImportHallSeats(CinemaContext context, string jsonString)
        {
            var importedHalls = JsonConvert.DeserializeObject<HallImportDto[]>(jsonString);

            List<Hall> hallsToAdd = new List<Hall>();
            List<Seat> seatsToAdd = new List<Seat>();
            StringBuilder sb = new StringBuilder();


            foreach (var imHall in importedHalls)
            {
                var isValid = IsValid(imHall);

                if (isValid is false)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                string currentProjType = imHall.Is3D is true && imHall.Is4Dx is true ? "4Dx/3D"
                    : imHall.Is3D is true ? "3D"
                    : imHall.Is4Dx is true ? "4Dx"
                    : "Normal";


                var curHall = new Hall()
                {
                    Is3D = imHall.Is3D,
                    Is4Dx = imHall.Is4Dx,
                    Name = imHall.Name,
                    Seats = new List<Seat>()
                };

                for (int i = 0; i < imHall.Seats; i++)
                {
                    var currentSeat = new Seat()
                    {
                        Hall = curHall,
                    };
                    seatsToAdd.Add(currentSeat);
                    curHall.Seats.Add(currentSeat);
                }

                hallsToAdd.Add(curHall);
                sb.AppendLine(string.Format(SuccessfulImportHallSeat, curHall.Name, currentProjType, curHall.Seats.Count));
            }

            context.Seats.AddRange(seatsToAdd);
            context.Halls.AddRange(hallsToAdd);
            context.SaveChanges();

            var x = sb.ToString().TrimEnd();

            return x;
        }

        public static string ImportProjections(CinemaContext context, string xmlString)
        {
            XmlRootAttribute root = new XmlRootAttribute("Projections");

            XmlSerializerNamespaces xmlNamespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

            XmlSerializer serializer = new XmlSerializer(typeof(ProjectionImportDto[]), root);

            StringBuilder sb = new StringBuilder();

            var validHallIds = context.Halls.Select(h => h.Id).ToHashSet<int>();
            var validMovies = context.Movies.Select(m => new
            {
                Id = m.Id,
                Title = m.Title
            }).ToList();

            HashSet<int> validMovieIds = validMovies.Select(m => m.Id).ToHashSet<int>();


            var projectsToAdd = new List<Projection>();
            using (var stream = new StringReader(xmlString))
            {
                var importedProjections = (ProjectionImportDto[])serializer.Deserialize(stream);

                foreach (var imProject in importedProjections)
                {
                    var isValid = validHallIds.Contains(imProject.HallId) is true && validMovieIds.Contains(imProject.MovieId) is true;

                    if (!isValid)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    var movieTitle = validMovies.FirstOrDefault(m => m.Id == imProject.MovieId);
                    var parsedDateTime = DateTime.ParseExact(imProject.DateTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

                    sb.AppendLine(string.Format(SuccessfulImportProjection, movieTitle.Title, parsedDateTime.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)));

                    var currentProj = new Projection()
                    {
                        DateTime = parsedDateTime,
                        HallId = imProject.HallId,
                        MovieId = imProject.MovieId
                    };

                    projectsToAdd.Add(currentProj);
                }
            }

            context.AddRange(projectsToAdd);
            context.SaveChanges();


            var x = sb.ToString().TrimEnd();

            return x;

        }

        public static string ImportCustomerTickets(CinemaContext context, string xmlString)
        {

            XmlRootAttribute root = new XmlRootAttribute("Customers");

            XmlSerializerNamespaces xmlNamespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

            XmlSerializer serializer = new XmlSerializer(typeof(CustomerTicketImportDto[]), root);

            StringBuilder sb = new StringBuilder();


            List<Customer> customersToAdd = new List<Customer>();
            List<Ticket> ticketsToAdd = new List<Ticket>();
            using (var stream = new StringReader(xmlString))
            {
                var importedCustomers = (CustomerTicketImportDto[])serializer.Deserialize(stream);
                var validProjectIds = context.Projections.Select(p => p.Id).ToHashSet<int>();

                foreach (var cus in importedCustomers)
                {
                    var isValid = IsValid(cus);
                    if (!isValid)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    var currentCustomer = new Customer()
                    {
                        Age = cus.Age,
                        Balance = cus.Balance,
                        FirstName = cus.FirstName,
                        LastName = cus.LastName,
                        Tickets = new List<Ticket>()
                    };


                    foreach (var tick in cus.Tickets)
                    {
                        if (!validProjectIds.Contains(tick.ProjectionId))
                        {
                            continue;
                        }
                        var currentTicket = new Ticket()
                        {
                            Price = tick.Price,
                            ProjectionId = tick.ProjectionId,
                            Customer = currentCustomer
                        };

                        currentCustomer.Tickets.Add(currentTicket);
                        ticketsToAdd.Add(currentTicket);
                    }


                    sb.AppendLine(string.Format(SuccessfulImportCustomerTicket, cus.FirstName, cus.LastName, currentCustomer.Tickets.Count));
                }
            }


            context.AddRange(ticketsToAdd);
            context.AddRange(customersToAdd);

            context.SaveChanges();

            var x = sb.ToString().TrimEnd();

            return x;
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }

    }
}