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


        //8. Book Search:
        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {

            var result =
                context
                .Books
                .Where(b => b.Title.ToLower().Contains(input.ToLower()))
                .OrderBy(b => b.Title)
                .Select(b => b.Title)
                .ToArray();

            return string.Join(Environment.NewLine,result);
        }

        //9. Book Search by Author:
        public static string GetBooksByAuthor(BookShopContext context, string input)
        {

            var result =
                context
                .Books
                .Where(b => EF.Functions.Like(b.Author.LastName.ToLower(), $"{input.ToLower()}%"))
                .OrderBy(b => b.BookId)
                .Select(b => $"{b.Title} ({b.Author.FirstName} {b.Author.LastName})")
                .ToArray();
            

            return string.Join(Environment.NewLine,result);
        }

        //10. Count Books:
        public static int CountBooks(BookShopContext context, int lengthCheck)
        {
            var result =
                context
                .Books
                .Where(b => b.Title.Length > lengthCheck)
                .Select(b => b.Title)
                .Count();

               
            return result;
        }

        //11. Total Book Copies:
        public static string CountCopiesByAuthor(BookShopContext context)
        {
            var result =
                context
                .Authors
                .Select(a => new
                {
                    a.FirstName
                    ,
                    a.LastName
                    ,
                    Count = (a.Books.Select(b=> b.Copies)).Sum()
                })
                .OrderByDescending(a=> a.Count)
                .ToList();
                

            return string.Join(Environment.NewLine,result.Select(r=> $"{r.FirstName} {r.LastName} - {r.Count}"));
        }

        //12. Profit by Category:
        public static string GetTotalProfitByCategory(BookShopContext context)
        {
            var result =
                context
                .Categories
                .Select(c => new
                {
                    c.Name
                    ,
                    Profit = c.CategoryBooks.Sum(x => x.Book.Price * x.Book.Copies)
                })
                .OrderByDescending(c => c.Profit)
                .ThenBy(c => c.Name)
                .ToArray();



            return string.Join(Environment.NewLine,result.Select(r=> $"{r.Name} ${r.Profit:f2}"));
        }

        //13. Most Recent Books:
        public static string GetMostRecentBooks(BookShopContext context)
        {

            var categories =
                context
                .Categories
                .OrderBy(c => c.Name)
                .Select(c => new
                {
                    c.Name
                    ,
                    Books = c.CategoryBooks
                    .Select(cb => cb.Book)
                    .OrderByDescending(b => b.ReleaseDate)
                    .Take(3)
                    .Select(b=> $"{b.Title} ({b.ReleaseDate.Value.Year})")
                    .ToList()
                });


            foreach (var category in categories)
            {
                sb.AppendLine($"--{category.Name}");

                foreach (var b in category.Books)
                {
                    sb.AppendLine(b);
                }
            }

            var result = sb.ToString().TrimEnd();
            sb.Clear();
            
            return result; 
        }

        //14 Increase Prices:
        public static void IncreasePrices(BookShopContext context)
        {
            var booksToIncrease =
                context
                .Books
                .Where(b => b.ReleaseDate.Value.Year < 2010)
                .ToList();

            booksToIncrease.ForEach(x => x.Price +=5);
            context.SaveChanges();
        }

        //15 Remove Books
        public static int RemoveBooks(BookShopContext context)
        {

            var booksToRemove =
                context
                .Books
                .Where(b => b.Copies < 4200)
                .ToList();

            context.Books.RemoveRange(booksToRemove);
            context.SaveChanges();

            return booksToRemove.Count;
        }

    }
}
