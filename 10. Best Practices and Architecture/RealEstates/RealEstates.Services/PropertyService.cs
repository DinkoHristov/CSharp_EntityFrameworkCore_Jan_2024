using Microsoft.EntityFrameworkCore;
using RealEstates.Data;
using RealEstates.Models;

namespace RealEstates.Services
{
    public class PropertyService : IPropertyService
    {
        private static ApplicationDbContext dbContext;

        public PropertyService(ApplicationDbContext context)
        {
            dbContext = context;
        }

        public void Add(int size, int yardSize, int floor, int totalFloors, string districtName, 
                        int year, string propertyTypeName, string buildingTypeName, decimal price)
        {
            var property = new Property
            {
                Size = size,
                YardSize = yardSize == 0 ? null : yardSize,
                Floor = floor == 0 ? null : floor,
                TotalFloors = totalFloors == 0 ? null : totalFloors,
                Year = year == 0 ? null : year,
                Price = price
            };

            var district = dbContext.Districts.FirstOrDefault(d => d.Name == districtName);
            if (district == null)
            {
                district = new District
                {
                    Name = districtName
                };
            }

            property.District = district;

            var propertyType = dbContext.PropertyTypes.FirstOrDefault(pt => pt.Name == propertyTypeName);
            if (propertyType == null)
            {
                propertyType = new PropertyType
                {
                    Name = propertyTypeName
                };
            }

            property.Type = propertyType;

            var buildingType = dbContext.BuildingTypes.FirstOrDefault(bt => bt.Name == buildingTypeName);
            if (buildingType == null)
            {
                buildingType = new BuildingType
                {
                    Name = buildingTypeName
                };
            }

            property.BuildingType = buildingType;

            dbContext.Properties.Add(property);
            dbContext.SaveChanges();
        }

        public IEnumerable<Property> SearchProperty(decimal minPrice, decimal maxPrice)
        {
            var properties = dbContext.Properties
                .Where(p => p.Price >= minPrice && p.Price <= maxPrice)
                .Select(p => new Property
                {
                    Size = p.Size,
                    Price = p.Price,
                    District = p.District,
                    Type = p.Type,
                    BuildingType = p.BuildingType
                })
                .ToList();

            return properties;
        }
    }
}
