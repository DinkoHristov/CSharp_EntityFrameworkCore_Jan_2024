namespace BookShop
{
    using BookShop.Models.Enums;
    using Data;
    using Initializer;
    using Microsoft.EntityFrameworkCore;
    using System.Globalization;
    using System.Text;

    public class StartUp
    {
        public static void Main()
        {
            using var db = new BookShopContext();
            //DbInitializer.ResetDatabase(db);

            //var booksByAge = GetBooksByAgeRestriction(db, "miNor");
            //Console.WriteLine(booksByAge);

            //var goldenBooks = GetGoldenBooks(db);
            //Console.WriteLine(goldenBooks);

            //var booksByPrice = GetBooksByPrice(db);
            //Console.WriteLine(booksByPrice);

            //var booksNotReleasedIn = GetBooksNotReleasedIn(db, 1998);
            //Console.WriteLine(booksNotReleasedIn);

            //var booksByCategory = GetBooksByCategory(db, "horror mystery drama");
            //Console.WriteLine(booksByCategory);

            //var booksReleasedBefore = GetBooksReleasedBefore(db, "12-04-1992");
            //Console.WriteLine(booksReleasedBefore);

            //var authorsEndingWith = GetAuthorNamesEndingIn(db, "e");
            //Console.WriteLine(authorsEndingWith);

            //var booksContain = GetBookTitlesContaining(db, "sK");
            //Console.WriteLine(booksContain);

            //var booksAndAuthor = GetBooksByAuthor(db, "po");
            //Console.WriteLine(booksAndAuthor);

            //var booksCount = CountBooks(db, 40);
            //Console.WriteLine(booksCount);

            //var booksCopies = CountCopiesByAuthor(db);
            //Console.WriteLine(booksCopies);

            //var booksProfit = GetTotalProfitByCategory(db);
            //Console.WriteLine(booksProfit);

            //var mostRecentBooks = GetMostRecentBooks(db);
            //Console.WriteLine(mostRecentBooks);

            //IncreasePrices(db);

            var removedBooks = RemoveBooks(db);
            Console.WriteLine(removedBooks);
        }

        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            var bookTitles = context.Books
                .OrderBy(b => b.Title)
                .ToList()
                .Select(b => new
                {
                    b.Title,
                    AgeRestriction = b.AgeRestriction.ToString().ToLower()
                })
                .Where(b => b.AgeRestriction == command.ToLower())
                .ToList();

            var result = new StringBuilder();

            foreach (var title in bookTitles)
            {
                result.AppendLine(title.Title);
            }

            return result.ToString().TrimEnd();
        }

        public static string GetGoldenBooks(BookShopContext context)
        {
            var goldenBooks = context.Books
                .Where(b => b.Copies < 5000 && b.EditionType == EditionType.Gold)
                .OrderBy(b => b.BookId)
                .Select(b => b.Title)
                .ToList();

            var result = new StringBuilder();

            foreach (var title in goldenBooks) 
            {
                result.AppendLine(title);
            }

            return result.ToString().TrimEnd();
        }

        public static string GetBooksByPrice(BookShopContext context)
        {
            var booksByPrice = context.Books
                .Where(b => b.Price > 40)
                .Select(b => new
                {
                    b.Title,
                    b.Price
                })
                .OrderByDescending(b => b.Price)
                .ToList();

            var result = new StringBuilder();

            foreach (var book in booksByPrice)
            {
                result.AppendLine($"{book.Title} - ${book.Price:F2}");
            }

            return result.ToString().TrimEnd();
        }

        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        {
            var booksNotReleasedIn = context.Books
                .Where(b => b.ReleaseDate.Value.Year != year)
                .OrderBy(b => b.BookId)
                .Select(b => b.Title)
                .ToList();

            var result = new StringBuilder();

            foreach (var title in booksNotReleasedIn)
            {
                result.AppendLine(title);
            }

            return result.ToString().TrimEnd();
        }

        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            var categories = input.Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(c => c.ToLower()).ToList();

            var booksByCategory = context.BooksCategories
                .Where(bc => categories.Contains(bc.Category.Name.ToLower()))
                .Select(bc => new
                {
                    BookTitle = bc.Book.Title
                })
                .OrderBy(b => b.BookTitle)
                .ToList();

            var result = new StringBuilder();

            foreach (var book in booksByCategory)
            {
                result.AppendLine(book.BookTitle);
            }

            return result.ToString().TrimEnd();
        }

        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            var dateTime = DateTime.ParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture);

            var booksReleasedBefore = context.Books
                .Where(b => b.ReleaseDate < dateTime)
                .OrderByDescending(b => b.ReleaseDate)
                .Select(b => new
                {
                    b.Title,
                    b.EditionType,
                    b.Price
                })
                .ToList();

            var result = new StringBuilder();

            foreach (var book in booksReleasedBefore)
            {
                result.AppendLine($"{book.Title} - {book.EditionType} - ${book.Price:F2}");
            }

            return result.ToString().TrimEnd();
        }

        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            var authors = context.Authors
                .Where(a => a.FirstName.EndsWith(input))
                .Select(a => new
                {
                    FullName = $"{a.FirstName} {a.LastName}"
                })
                .ToList()
                .OrderBy(a => a.FullName)
                .ToList();

            var result = new StringBuilder();

            foreach (var author in authors)
            {
                result.AppendLine(author.FullName);
            }

            return result.ToString().TrimEnd();
        }

        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {
            var books = context.Books
                .Where(b => b.Title.ToLower().Contains(input.ToLower()))
                .Select(b => b.Title)
                .OrderBy(b => b)
                .ToList();

            var result = new StringBuilder();

            foreach (var title in books)
            {
                result.AppendLine(title);
            }

            return result.ToString().TrimEnd();
        }

        public static string GetBooksByAuthor(BookShopContext context, string input)
        {
            var booksAndAuthor = context.Books
                .Where(b => b.Author.LastName.ToLower().StartsWith(input.ToLower()))
                .OrderBy(b => b.BookId)
                .Select(b => new
                {
                    b.Title,
                    AuthorFullName = $"{b.Author.FirstName} {b.Author.LastName}"
                })
                .ToList();

            var result = new StringBuilder();

            foreach (var book in booksAndAuthor)
            {
                result.AppendLine($"{book.Title} ({book.AuthorFullName})");
            }

            return result.ToString().TrimEnd();
        }

        public static int CountBooks(BookShopContext context, int lengthCheck)
        {
            return context.Books
                .Where(b => b.Title.Length > lengthCheck)
                .Count();
        }

        public static string CountCopiesByAuthor(BookShopContext context)
        {
            var booksCopies = context.Authors
                .Select(a => new
                {
                    FullName = $"{a.FirstName} {a.LastName}",
                    BookCopies = a.Books.Sum(b => b.Copies)
                })
                .OrderByDescending(a => a.BookCopies)
                .ToList();

            var result = new StringBuilder();

            foreach (var author in booksCopies)
            {
                result.AppendLine($"{author.FullName} - {author.BookCopies}");
            }

            return result.ToString().TrimEnd();
        }

        public static string GetTotalProfitByCategory(BookShopContext context)
        {
            var categories = context.Categories
                .Select(b => new
                {
                    CategoryName = b.Name,
                    Profit = b.CategoryBooks
                    .Select(cb => cb.Book.Copies * cb.Book.Price)
                    .Sum()
                })
                .OrderByDescending(b => b.Profit)
                .ThenBy(b => b.CategoryName)
                .ToList();

            var result = new StringBuilder();

            foreach (var category in categories)
            {
                result.AppendLine($"{category.CategoryName} ${category.Profit:F2}");
            }

            return result.ToString().TrimEnd();
        }

        public static string GetMostRecentBooks(BookShopContext context)
        {
            var mostRecentBooks = context.Categories
                .Select(bc => new
                {
                    CategoryName = bc.Name,
                    Books = bc.CategoryBooks
                    .OrderByDescending(cb => cb.Book.ReleaseDate)
                    .Select(c => new
                    {
                        BookTitle = c.Book.Title,
                        ReleaseYear = c.Book.ReleaseDate.Value.Year
                    })
                    .Take(3)
                    .ToList()
                })
                .OrderBy(bc => bc.CategoryName)
                .ToList();

            var result = new StringBuilder();

            foreach (var category in mostRecentBooks)
            {
                result.AppendLine($"--{category.CategoryName}");

                foreach (var book in category.Books)
                {
                    result.AppendLine($"{book.BookTitle} ({book.ReleaseYear})");
                }
            }

            return result.ToString().TrimEnd();
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

            int removedBooks = books.Count;

            context.Books.RemoveRange(books);

            context.SaveChanges();

            return removedBooks;
        }
    }
}


