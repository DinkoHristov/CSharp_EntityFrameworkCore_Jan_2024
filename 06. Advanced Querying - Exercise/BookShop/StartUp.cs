using BookShop.Data;
using BookShop.Models.Enums;
using System;
using System.Globalization;
using System.Linq;
using System.Text;

namespace BookShop
{
    public class StartUp
    {
        static void Main(string[] args)
        {
            var db = new BookShopContext();

            //DbInitializer.Seed(db);

            //var result = GetBooksByAgeRestriction(db, "teEN");

            //var result = GetGoldenBooks(db);

            //var result = GetBooksByPrice(db);

            //var result = GetBooksNotReleasedIn(db, 2000);

            //var result = GetBooksByCategory(db, "horror mystery drama");

            //var result = GetBooksReleasedBefore(db, "12-04-1992");

            //var result = GetAuthorNamesEndingIn(db, "dy");

            //var result = GetBookTitlesContaining(db, "WOR");

            //var result = GetBooksByAuthor(db, "po");

            //var result = CountBooks(db, 40);

            //var result = CountCopiesByAuthor(db);

            //var result = GetTotalProfitByCategory(db);

            //var result = GetMostRecentBooks(db);

            //IncreasePrices(db);

            var result = RemoveBooks(db);

            Console.WriteLine(result);
        }

        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            var books = context.Books
                .Select(b => new { b.Title, AgeRestriction = b.AgeRestriction.ToString().ToLower() })
                .ToHashSet()
                .Where(b => b.AgeRestriction.Equals(command.ToLower()))
                .Select(b => new { b.Title })
                .OrderBy(b => b.Title)
                .ToList();

            var sb = new StringBuilder();

            foreach (var book in books)
            {
                sb.AppendLine(book.Title);
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetGoldenBooks(BookShopContext context)
        {
            var books = context.Books
                .Where(b => b.EditionType == EditionType.Gold && b.Copies < 5_000)
                .OrderBy(b => b.BookId)
                .Select(b => new { b.Title })
                .ToList();

            var sb = new StringBuilder();

            foreach (var book in books)
            {
                sb.AppendLine(book.Title);
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetBooksByPrice(BookShopContext context)
        {
            var books = context.Books
                .Where(b => b.Price > 40)
                .Select(b => new
                {
                    b.Title,
                    b.Price
                })
                .OrderByDescending(b => b.Price)
                .ToHashSet();

            var sb = new StringBuilder();

            foreach (var book in books)
            {
                sb.AppendLine($"{book.Title} - ${book.Price:F2}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        {
            var books = context.Books
                .Where(b => b.ReleaseDate.Value.Year != year)
                .OrderBy(b => b.BookId)
                .Select(b => new
                {
                    b.Title
                })
                .ToList();

            var sb = new StringBuilder();

            foreach (var book in books)
            {
                sb.AppendLine(book.Title);
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            var categories = input.Split(" ", StringSplitOptions.RemoveEmptyEntries).ToList();

            var books = context.Books
                .Where(b => categories.Contains(b.BookCategories.Select(bc => bc.Category.Name).FirstOrDefault()))
                .Select(b => new
                {
                    b.Title
                })
                .OrderBy(b => b.Title)
                .ToHashSet();

            var sb = new StringBuilder();

            foreach (var book in books)
            {
                sb.AppendLine(book.Title);
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            var dateTime = DateTime.ParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture);

            var books = context.Books
                .Where(b => b.ReleaseDate < dateTime)
                .OrderByDescending(b => b.ReleaseDate)
                .Select(b => new
                {
                    b.Title,
                    b.EditionType,
                    b.Price
                })
                .ToHashSet();

            var sb = new StringBuilder();

            foreach (var book in books)
            {
                sb.AppendLine($"{book.Title} - {book.EditionType} - ${book.Price:F2}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            var authors = context.Authors
                .Where(a => a.FirstName.EndsWith(input))
                .Select(a => new
                {
                    FullName = a.FirstName + " " + a.LastName
                })
                .OrderBy(a => a.FullName)
                .ToHashSet();

            var sb = new StringBuilder();

            foreach (var author in authors)
            {
                sb.AppendLine(author.FullName);
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {
            var books = context.Books
                .Where(b => b.Title.Contains(input))
                .Select(b => new
                {
                    b.Title
                })
                .OrderBy(b => b.Title)
                .ToHashSet();

            var sb = new StringBuilder();

            foreach (var book in books)
            {
                sb.AppendLine(book.Title);
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetBooksByAuthor(BookShopContext context, string input)
        {
            var books = context.Books
                .Where(b => b.Author.LastName.StartsWith(input))
                .OrderBy(b => b.BookId)
                .Select(b => new
                {
                    b.Title,
                    AuthorFullName = b.Author.FirstName + " " + b.Author.LastName
                })
                .ToHashSet();

            var sb = new StringBuilder();

            foreach(var book in books)
            {
                sb.AppendLine($"{book.Title} ({book.AuthorFullName})");
            }

            return sb.ToString().TrimEnd();
        }

        public static int CountBooks(BookShopContext context, int lengthCheck)
        {
            var booksCount = context.Books
                .Count(b => b.Title.Length > lengthCheck);

            return booksCount;
        }

        public static string CountCopiesByAuthor(BookShopContext context)
        {
            var books = context.Books
                .Select(b => new
                {
                    AuthorFullName = b.Author.FirstName + " " + b.Author.LastName,
                    Copies = b.Copies
                })
                .GroupBy(b => b.AuthorFullName)
                .Select(b => new
                {
                    AuthorFullName = b.Key,
                    CountCopies = b.Sum(x => x.Copies)
                })
                .OrderByDescending(b => b.CountCopies)
                .ToList();

            var sb = new StringBuilder();

            foreach (var book in books)
            {
                sb.AppendLine($"{book.AuthorFullName} - {book.CountCopies}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetTotalProfitByCategory(BookShopContext context)
        {
            var books = context.Books
                .SelectMany(b => b.BookCategories.Select(bc => new
                {
                    CategoryName = bc.Category.Name,
                    Copies = bc.Book.Copies,
                    Price = bc.Book.Price
                }))
                .GroupBy(b => b.CategoryName)
                .Select(b => new
                {
                    CategoryName = b.Key,
                    Profit = b.Sum(b => b.Copies * b.Price)
                })
                .OrderByDescending(b => b.Profit)
                .ThenBy(b => b.CategoryName)
                .ToList();

            var sb = new StringBuilder();

            foreach (var book in books)
            {
                sb.AppendLine($"{book.CategoryName} ${book.Profit:F2}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetMostRecentBooks(BookShopContext context)
        {
            var books = context.Books
                .SelectMany(b => b.BookCategories.Select(bc => new
                {
                    CategoryName = bc.Category.Name,
                    BookTitle = bc.Book.Title,
                    BookReleaseDate = bc.Book.ReleaseDate.Value.Year
                }))
                .ToHashSet()
                .OrderBy(b => b.CategoryName)
                .GroupBy(b => b.CategoryName)
                .Select(b => new
                {
                    CategoryName = b.Key,
                    Books = b.OrderByDescending(x => x.BookReleaseDate).ThenBy(b => b.BookTitle).Take(3).ToList()
                })
                .ToList();

            var sb = new StringBuilder();

            foreach (var category in books)
            {
                sb.AppendLine($"--{category.CategoryName}");

                foreach (var book in category.Books)
                {
                    sb.AppendLine($"{book.BookTitle} ({book.BookReleaseDate})");
                }
            }

            return sb.ToString().TrimEnd();
        }

        public static void IncreasePrices(BookShopContext context)
        {
            var books = context.Books
                .Where(b => b.ReleaseDate.Value.Year < 2010)
                .ToList();

            foreach (var book in books)
            {
                book.Price += 5;
            }

            context.SaveChanges();
        }

        public static int RemoveBooks(BookShopContext context)
        {
            var books = context.Books
                .Where(b => b.Copies < 4200)
                .ToList();

            foreach (var book in books)
            {
                context.Remove(book);
            }

            context.SaveChanges();

            return books.Count;
        }
    }
}
