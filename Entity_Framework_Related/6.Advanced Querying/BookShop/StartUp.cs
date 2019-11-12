namespace BookShop
{
    using BookShop.Models.Enums;
    using Data;
    using Initializer;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Text;

    public class StartUp
    {
        private static StringBuilder sb = new StringBuilder();
        public static void Main()
        {
            using (var db = new BookShopContext())
            {
                //DbInitializer.ResetDatabase(db);

                //string input = Console.ReadLine();
                //int year = int.Parse(Console.ReadLine());
                //int count = int.Parse(Console.ReadLine());

                var result = RemoveBooks(db);
                Console.WriteLine(result);
            }
        }

        //1. Age Restriction
        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            var result = context
                .Books
                .Where(b => b.AgeRestriction.ToString().ToLower() == command.ToLower())
                .OrderBy(b=> b.Title)
                .Select(b => 
                    b.Title
                )
                .ToList();


            return string.Join(Environment.NewLine,result);
        }

        //2. GoldenBooks
        public static string GetGoldenBooks(BookShopContext context)
        {
            var result = context
                .Books
                .Where(b => b.EditionType == EditionType.Gold && b.Copies < 5000)
                .OrderBy(b => b.BookId)
                .Select(b => b.Title)
                .ToList();
                

            return string.Join(Environment.NewLine,result);
        }

        //3.Books By Price:
        public static string GetBooksByPrice(BookShopContext context)
        {
            var result = context
                .Books
                .Where(b => b.Price > 40)
                .OrderByDescending(b => b.Price)
                .Select(b => new
                {
                    b.Title,
                    b.Price
                })
                .ToList();

            return string.Join(Environment.NewLine, result.Select(b=> $"{b.Title} - ${b.Price:f2}"));
        }

        //4 Not released In:
        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        {

            var result =
                context
                .Books
                .Where(b => b.ReleaseDate.Value.Year != year)
                .OrderBy(b => b.BookId)
                .Select(b => b.Title)
                .ToArray();
                

            return string.Join(Environment.NewLine,result);
        }

        //5. Book Titles By Category:
        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            string[] categories = input.Split(" ", StringSplitOptions.RemoveEmptyEntries)
                .ToArray()
                .Select(x=> x.ToLower())
                .ToArray();
            
            var result =
                context
                .Books
                .Where(b => b.BookCategories
                .Any(bc => categories.Contains(bc.Category.Name.ToLower())))
                .OrderBy(b=> b.Title)
                .Select(b=> b.Title)
                .ToList();

            return string.Join(Environment.NewLine,result);
        }

        
        //6. Released Before Date
        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            var dateAsDt = DateTime.ParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture);

            var result =
                context
                .Books
                .Where(b => b.ReleaseDate < dateAsDt)
                .OrderByDescending(b => b.ReleaseDate)
                .Select(b => $"{b.Title} - {b.EditionType} - ${b.Price:f2}")
                .ToList();

            return string.Join(Environment.NewLine,result);
        }

        //7. Author Search
        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            var result =
                  context
                  .Authors
                  //well i`m silly, switched the two places :( 
                  .Where(a => EF.Functions.Like(a.FirstName, $@"%{input}"))
                  //.Where(a=> a.FirstName.EndsWith(input))
                  .Select(a => $"{a.FirstName} {a.LastName}")
                  .OrderBy(n=> n)
                  .ToArray();


            return string.Join(Environment.NewLine,result);
        }

    }
}
