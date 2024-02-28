using RealEstates.Data;
using RealEstates.Models;

namespace RealEstates.Services
{
    public class DistrictService : IDistrictService
    {
        private static ApplicationDbContext dbContext;

        public DistrictService(ApplicationDbContext context)
        {
            dbContext = context;
        }

        public IEnumerable<DistrictInfoDto> GetMostExpensiveDistricts(int count)
        {
            var districts = dbContext.Districts
                .Select(d => new DistrictInfoDto
                {
                    Name = d.Name,
                    PropertiesCount = d.Properties.Count,
                    AveragePricePerSquareMeter = d.Properties.Where(p => p.Price != 0).Average(p => p.Price / p.Size)
                })
                .OrderByDescending(d => d.AveragePricePerSquareMeter)
                .Take(count)
                .ToList();

            return districts;
        }
    }
}
