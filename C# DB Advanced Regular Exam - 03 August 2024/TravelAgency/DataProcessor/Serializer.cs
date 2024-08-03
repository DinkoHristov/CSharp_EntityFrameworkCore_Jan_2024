using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Xml.Serialization;
using TravelAgency.Data;
using TravelAgency.Data.Models.Enums;
using TravelAgency.DataProcessor.ExportDtos;

namespace TravelAgency.DataProcessor
{
	public class Serializer
    {
        public static string ExportGuidesWithSpanishLanguageWithAllTheirTourPackages(TravelAgencyContext context)
        {
            var guides = context.Guides
                .Where(g => g.Language == Language.Spanish)
                .Include(g => g.TourPackagesGuides)
                .OrderByDescending(g => g.TourPackagesGuides.Count)
                .ThenBy(g => g.FullName)
                .ToList()
                .Select(g => new ExportGuideModel
                {
                    FullName = g.FullName,
                    TourPackages = g.TourPackagesGuides
                    .Select(tp => new ExportTourPackageModel
                    {
                        Name = tp.TourPackage.PackageName,
                        Description = tp.TourPackage.Description,
                        Price = tp.TourPackage.Price
                    })
                    .OrderByDescending(tp => tp.Price)
                    .ThenBy(tp => tp.Name)
                    .ToList()
                })
                .ToList();

            var serializer = new XmlSerializer(typeof(List<ExportGuideModel>), new XmlRootAttribute("Guides"));

            var textWriter = new StringWriter();
            var xmlNamespace = new XmlSerializerNamespaces();
            xmlNamespace.Add("", "");

            serializer.Serialize(textWriter, guides, xmlNamespace);

            return textWriter.ToString().TrimEnd();
        }

        public static string ExportCustomersThatHaveBookedHorseRidingTourPackage(TravelAgencyContext context)
        {
            var customers = context.Customers
                .Where(c => c.Bookings.Any(b => b.TourPackage.PackageName == "Horse Riding Tour"))
                .ToList()
                .Select(c => new
                {
                    FullName = c.FullName,
                    PhoneNumber = c.PhoneNumber,
                    Bookings = c.Bookings
                    .Where(b => b.TourPackage.PackageName == "Horse Riding Tour")
                    .OrderBy(b => b.BookingDate)
                    .ToList()
                    .Select(b => new
                    {
                        TourPackageName = b.TourPackage.PackageName,
                        Date = b.BookingDate.ToString("yyyy-MM-dd")
                    })
                    .ToList()
                })
                .OrderByDescending(c => c.Bookings.Count)
                .ThenBy(c => c.FullName)
                .ToList();

            var json = JsonConvert.SerializeObject(customers, Formatting.Indented);

            return json.ToString().TrimEnd();
        }
    }
}
