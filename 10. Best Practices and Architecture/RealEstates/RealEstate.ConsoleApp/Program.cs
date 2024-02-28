using Microsoft.EntityFrameworkCore;
using RealEstates.Data;
using RealEstates.Services;
using System.Text;

public class StartUp
{
    private static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.Unicode;

        var dbContext = new ApplicationDbContext();
        dbContext.Database.Migrate();

        while (true)
        {
            Console.Clear();
            Console.WriteLine("Choose an option:");
            Console.WriteLine("1. Property search.");
            Console.WriteLine("2. Most expensive districts.");
            Console.WriteLine("0. Exit");

            bool parsed = int.TryParse(Console.ReadLine(), out int option);

            if (parsed && option >= 1 && option <= 2)
            {
                switch (option)
                {
                    case 1:
                        PropertySearch(dbContext);
                        break;

                    case 2:
                        MostExpensiveDistricts(dbContext);
                        break;

                    default:
                        break;
                }

                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
            else if (parsed && option == 0)
            {
                break;
            }
        }
    }

    private static void PropertySearch(ApplicationDbContext dbContext)
    {
        IPropertyService propertyService = new PropertyService(dbContext);

        Console.Write("Enter minimal price you can give: ");
        decimal minPrice = decimal.Parse(Console.ReadLine());

        Console.Write("Enter maximum price you can give: ");
        decimal maxPrice = decimal.Parse(Console.ReadLine());

        var properties = propertyService.SearchProperty(minPrice, maxPrice);
        foreach (var property in properties)
        {
            Console.WriteLine($"{property.District.Name}; {property.Type.Name}; {property.BuildingType.Name} => {property.Price:F2}€ => {property.Size}m2");
        }
    }

    private static void MostExpensiveDistricts(ApplicationDbContext dbContext)
    {
        Console.Write("Enter how many districts to take: ");
        int count = int.Parse(Console.ReadLine());

        IDistrictService districtService = new DistrictService(dbContext);
        var districts = districtService.GetMostExpensiveDistricts(count);

        foreach (var district in districts)
        {
            Console.WriteLine($"{district.Name} => {district.AveragePricePerSquareMeter:F2}€ ({district.PropertiesCount})");
        }
    }
}