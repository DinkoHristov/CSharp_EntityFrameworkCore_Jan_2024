namespace Artillery.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Artillery.Data;
    using Artillery.Data.Models;
    using Artillery.Data.Models.Enums;
    using Artillery.DataProcessor.ImportDto;
    using Newtonsoft.Json;

    public class Deserializer
    {
        private const string ErrorMessage =
                "Invalid data.";
        private const string SuccessfulImportCountry =
            "Successfully import {0} with {1} army personnel.";
        private const string SuccessfulImportManufacturer =
            "Successfully import manufacturer {0} founded in {1}.";
        private const string SuccessfulImportShell =
            "Successfully import shell caliber #{0} weight {1} kg.";
        private const string SuccessfulImportGun =
            "Successfully import gun {0} with a total weight of {1} kg. and barrel length of {2} m.";

        public static string ImportCountries(ArtilleryContext context, string xmlString)
        {
            var serializer = new XmlSerializer(typeof(List<CountryDto>), new XmlRootAttribute("Countries"));
            var countriesDto = (List<CountryDto>)serializer.Deserialize(new StringReader(xmlString));

            var result = new StringBuilder();
            foreach (var countryDto in countriesDto)
            {
                if (!IsValid(countryDto))
                {
                    result.AppendLine(ErrorMessage);
                    continue;
                }

                var country = new Country
                {
                    CountryName = countryDto.CountryName,
                    ArmySize = countryDto.ArmySize
                };

                context.Countries.Add(country);
                result.AppendLine(string.Format(SuccessfulImportCountry, country.CountryName, country.ArmySize));
            }
            context.SaveChanges();

            return result.ToString().TrimEnd();
        }

        public static string ImportManufacturers(ArtilleryContext context, string xmlString)
        {
            var serializer = new XmlSerializer(typeof(List<ManufacturerDto>), new XmlRootAttribute("Manufacturers"));
            var manufacturersDto = (List<ManufacturerDto>)serializer.Deserialize(new StringReader(xmlString));

            var result = new StringBuilder();
            foreach (var manufacturerDto in manufacturersDto.Distinct())
            {
                if (!IsValid(manufacturerDto))
                {
                    result.AppendLine(ErrorMessage); 
                    continue;
                }

                var manufacturer = new Manufacturer
                {
                    ManufacturerName = manufacturerDto.ManufacturerName,
                    Founded = manufacturerDto.Founded
                };

                var countryInfo = manufacturer.Founded.Split(", ", StringSplitOptions.RemoveEmptyEntries);
                var country = countryInfo[countryInfo.Length - 2] + ", " + countryInfo[countryInfo.Length - 1];

                if (!context.Manufacturers.ToList().Any(m => m.ManufacturerName == manufacturer.ManufacturerName))
                {
                    context.Manufacturers.Add(manufacturer);
                    result.AppendLine(string.Format(SuccessfulImportManufacturer, manufacturer.ManufacturerName, country));
                }
                else
                {
                    result.AppendLine(ErrorMessage);
                }

                context.SaveChanges();
            }

            return result.ToString().TrimEnd();
        }

        public static string ImportShells(ArtilleryContext context, string xmlString)
        {
            var serializer = new XmlSerializer(typeof(List<ShellDto>), new XmlRootAttribute("Shells"));
            var shellsDto = (List<ShellDto>)serializer.Deserialize(new StringReader(xmlString));

            var result = new StringBuilder();
            foreach (var shellDto in shellsDto)
            {
                if (!IsValid(shellDto))
                {
                    result.AppendLine(ErrorMessage);
                    continue;
                }

                var shell = new Shell
                {
                    ShellWeight = shellDto.ShellWeight,
                    Caliber = shellDto.Caliber
                };

                context.Shells.Add(shell);
                result.AppendLine(string.Format(SuccessfulImportShell, shell.Caliber, shell.ShellWeight));
            }
            context.SaveChanges();

            return result.ToString().TrimEnd();
        }

        public static string ImportGuns(ArtilleryContext context, string jsonString)
        {
            var gunsDto = JsonConvert.DeserializeObject<List<GunDto>>(jsonString);

            var result = new StringBuilder();
            foreach (var gunDto in gunsDto)
            {
                if (!IsValid(gunDto))
                {
                    result.AppendLine(ErrorMessage);
                    continue;
                }

                var isTypeFound = Enum.TryParse(typeof(GunType), gunDto.GunType, out object gunType);

                if (!isTypeFound)
                {
                    result.AppendLine(ErrorMessage);
                    continue;
                }

                var gun = new Gun
                {
                    Manufacturer = context.Manufacturers.FirstOrDefault(m => m.Id == gunDto.ManufacturerId),
                    GunWeight = gunDto.GunWeight,
                    BarrelLength = gunDto.BarrelLength,
                    NumberBuild = gunDto.NumberBuild,
                    Range = gunDto.Range,
                    GunType = (GunType)gunType,
                    Shell = context.Shells.FirstOrDefault(s => s.Id == gunDto.ShellId)
                };

                foreach (var id in gunDto.Ids)
                {
                    var country = context.Countries.FirstOrDefault(c => c.Id == id.Id);
                    var countryGun = new CountryGun
                    {
                        Country = country
                    };

                    gun.CountriesGuns.Add(countryGun);
                }

                context.Guns.Add(gun);
                result.AppendLine(string.Format(SuccessfulImportGun, gun.GunType, gun.GunWeight, gun.BarrelLength));
            }
            context.SaveChanges();

            return result.ToString().TrimEnd();
        }
        private static bool IsValid(object obj)
        {
            var validator = new ValidationContext(obj);
            var validationRes = new List<ValidationResult>();

            var result = Validator.TryValidateObject(obj, validator, validationRes, true);
            return result;
        }
    }
}
