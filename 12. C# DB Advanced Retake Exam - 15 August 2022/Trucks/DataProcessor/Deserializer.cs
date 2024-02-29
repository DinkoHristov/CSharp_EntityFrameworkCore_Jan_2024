namespace Trucks.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Data;
    using Newtonsoft.Json;
    using Trucks.Data.Models;
    using Trucks.Data.Models.Enums;
    using Trucks.DataProcessor.ImportDto;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedDespatcher
            = "Successfully imported despatcher - {0} with {1} trucks.";

        private const string SuccessfullyImportedClient
            = "Successfully imported client - {0} with {1} trucks.";

        public static string ImportDespatcher(TrucksContext context, string xmlString)
        {
            var serializer = new XmlSerializer(typeof(List<DespatcherDto>), new XmlRootAttribute("Despatchers"));
            var despatcherDtos = (List<DespatcherDto>)serializer.Deserialize(new StringReader(xmlString));

            var result = new StringBuilder();
            foreach (var despatcherDto in despatcherDtos)
            {
                if (!IsValid(despatcherDto))
                {
                    result.AppendLine(ErrorMessage);
                    continue;
                }

                if (string.IsNullOrEmpty(despatcherDto.Position))
                {
                    result.AppendLine(ErrorMessage);
                    continue;
                }

                var despatcher = new Despatcher
                {
                    Name = despatcherDto.Name,
                    Position = despatcherDto.Position
                };

                foreach (var truckDto in despatcherDto.Trucks)
                {
                    if (!IsValid(truckDto))
                    {
                        result.AppendLine(ErrorMessage);
                        continue;
                    }

                    var truck = new Truck
                    {
                        RegistrationNumber = truckDto.RegistrationNumber,
                        VinNumber = truckDto.VinNumber,
                        TankCapacity = truckDto.TankCapacity,
                        CargoCapacity = truckDto.CargoCapacity,
                        CategoryType = (CategoryType)truckDto.CategoryType,
                        MakeType = (MakeType)truckDto.MakeType
                    };

                    despatcher.Trucks.Add(truck);
                }

                context.Despatchers.Add(despatcher);
                result.AppendLine(string.Format(SuccessfullyImportedDespatcher, despatcher.Name, despatcher.Trucks.Count));
            }

            context.SaveChanges();
            return result.ToString().TrimEnd();
        }
        public static string ImportClient(TrucksContext context, string jsonString)
        {
            var clientDtos = JsonConvert.DeserializeObject<List<ClientDto>>(jsonString);
            var allTrucks = context.Trucks.ToList();

            var result = new StringBuilder();
            foreach (var clientDto in clientDtos)
            {
                if (!IsValid(clientDto))
                {
                    result.AppendLine(ErrorMessage);
                    continue;
                }

                if (clientDto.Type.ToLower() == "usual")
                {
                    result.AppendLine(ErrorMessage);
                    continue;
                }

                var client = new Client
                {
                    Name = clientDto.Name,
                    Nationality = clientDto.Nationality,
                    Type = clientDto.Type
                };

                foreach (var id in clientDto.TrucksId.Distinct())
                {
                    if (!IsValid(id))
                    {
                        result.AppendLine(ErrorMessage);
                        continue;
                    }

                    if (!allTrucks.Any(t => t.Id == id))
                    {
                        result.AppendLine(ErrorMessage);
                        continue;
                    }

                    var clientTruck = new ClientTruck
                    {
                        Truck = allTrucks.FirstOrDefault(t => t.Id == id)
                    };

                    client.ClientsTrucks.Add(clientTruck);
                }

                context.Clients.Add(client);
                result.AppendLine(string.Format(SuccessfullyImportedClient, client.Name, client.ClientsTrucks.Count));
            }

            context.SaveChanges();
            return result.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}
