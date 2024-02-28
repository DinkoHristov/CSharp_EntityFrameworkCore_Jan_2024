using RealEstates.Models;

namespace RealEstates.Services
{
    public interface IPropertyService
    {
        void Add(int size, int yardSize, int floor, int totalFloors, string distrinctName, 
                 int year, string propertyTypeName, string buildingTypeName, decimal Price);

        IEnumerable<Property> SearchProperty(decimal minPrice, decimal maxPrice);
    }
}
