
namespace Artillery.DataProcessor
{
    using Artillery.Data;
    using Artillery.Data.Models.Enums;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;
    using System;
    using System.Linq;
    using System.Text;
    using System.Xml.Linq;

    public class Serializer
    {
        public static string ExportShells(ArtilleryContext context, double shellWeight)
        {
            var shells = context.Shells
                .Where(s => s.ShellWeight > shellWeight)
                .Select(s => new
                {
                    s.ShellWeight,
                    s.Caliber,
                    Guns = s.Guns.Where(g => g.GunType == (GunType)Enum.Parse(typeof(GunType), "AntiAircraftGun"))
                    .Select(g => new
                    {
                        GunType = g.GunType.ToString(),
                        g.GunWeight,
                        g.BarrelLength,
                        Range = g.Range > 3000 ? "Long-range" : "Regular range"
                    })
                    .OrderByDescending(g => g.GunWeight)
                    .ToList()
                })
                .OrderBy(s => s.ShellWeight)
                .ToList();

            var json = JsonConvert.SerializeObject(shells, Formatting.Indented);

            return json.ToString();
        }

        public static string ExportGuns(ArtilleryContext context, string manufacturer)
        {
            var newg = context.Guns
                .Include(g => g.CountriesGuns)
                .ThenInclude(g => g.Country)
                .Where(g => g.Manufacturer.ManufacturerName == manufacturer)
                .OrderBy(g => g.BarrelLength)
                .ToList();

            var guns = context.Guns
                .Where(g => g.Manufacturer.ManufacturerName == manufacturer)
                .Select(g => new
                {
                    g.Manufacturer.ManufacturerName,
                    GunType = g.GunType.ToString(),
                    g.BarrelLength,
                    g.GunWeight,
                    g.Range,
                    Countries = g.CountriesGuns
                    .Where(cg => cg.Country.ArmySize > 4500000)
                    .Select(cg => new
                    {
                        cg.Country.CountryName,
                        cg.Country.ArmySize
                    })
                    .OrderBy(x => x.ArmySize)
                    .ToList()
                })
                .OrderBy(x => x.BarrelLength)
                .ToList();

            var doc = new XDocument();
            var root = new XElement("Guns");
            doc.Add(root);

            foreach (var gun in guns)
            {
                var gunElement = new XElement("Gun");
                root.Add(gunElement);

                gunElement.SetAttributeValue("ManufacturerName", gun.ManufacturerName);
                gunElement.SetAttributeValue("GunType", gun.GunType);
                gunElement.SetAttributeValue("GunWeight", gun.GunWeight);
                gunElement.SetAttributeValue("BarrelLength", gun.BarrelLength);
                gunElement.SetAttributeValue("Range", gun.Range);

                var countriesElement = new XElement("Countries");
                gunElement.Add(countriesElement);

                foreach (var country in gun.Countries)
                {
                    var countryElement = new XElement("Country");
                    countriesElement.Add(countryElement);

                    countryElement.SetAttributeValue("CountryName", country.CountryName);
                    countryElement.SetAttributeValue("ArmySize", country.ArmySize);
                }
            }

            return doc.ToString().TrimEnd();
        }
    }
}
