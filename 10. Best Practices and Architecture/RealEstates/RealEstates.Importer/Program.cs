
using RealEstates.Data;
using RealEstates.Importer;
using RealEstates.Models;
using RealEstates.Services;
using System.Text.Json;

public class Program
{
    private static void Main(string[] args)
    {
        ImportProperties("../../../PropertiesInfo/properties.json");
        ImportProperties("../../../PropertiesInfo/houses.json");
    }

    private static void ImportProperties(string json)
    {
        var dbContext = new ApplicationDbContext();
        IPropertyService propertyService = new PropertyService(dbContext);

        var properties = JsonSerializer.Deserialize<List<PropertyAsJson>>(File.ReadAllText(json));

        foreach (var property in properties)
        {
            propertyService.Add(property.Size, property.YardSize, property.Floor,
                                property.TotalFloors, property.District, property.Year,
                                property.Type, property.BuildingType, property.Price);
        }
    }
}