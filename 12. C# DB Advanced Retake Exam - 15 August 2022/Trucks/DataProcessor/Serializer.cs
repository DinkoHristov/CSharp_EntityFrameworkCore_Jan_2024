namespace Trucks.DataProcessor
{
    using System;
    using System.Linq;
    using System.Text;
    using System.Xml.Linq;
    using Data;
    using Newtonsoft.Json;
    using Formatting = Newtonsoft.Json.Formatting;

    public class Serializer
    {
        public static string ExportDespatchersWithTheirTrucks(TrucksContext context)
        {
            var despatchers = context.Despatchers
                .Where(d => d.Trucks.Any())
                .Select(d => new
                {
                    DespatcherName = d.Name,
                    TrucksCount = d.Trucks.Count,
                    Trucks = d.Trucks.Select(t => new
                    {
                        t.RegistrationNumber,
                        Make = t.MakeType.ToString()
                    })
                    .OrderBy(t => t.RegistrationNumber)
                    .ToList()
                })
                .OrderByDescending(d => d.TrucksCount)
                .ThenBy(d => d.DespatcherName)
                .ToList();

            var doc = new XDocument();
            var root = new XElement("Despatchers");
            doc.Add(root);

            foreach (var despatcher in despatchers)
            {
                var despatcherElement = new XElement("Despatcher");
                despatcherElement.SetAttributeValue("TrucksCount", despatcher.TrucksCount);
                root.Add(despatcherElement);

                var despatcherName = new XElement("DespatcherName", despatcher.DespatcherName);
                despatcherElement.Add(despatcherName);

                var trucks = new XElement("Trucks");
                despatcherElement.Add(trucks);

                foreach (var truck in despatcher.Trucks)
                {
                    var truckElement = new XElement("Truck");
                    trucks.Add(truckElement);

                    var registrationNumber = new XElement("RegistrationNumber", truck.RegistrationNumber);
                    truckElement.Add(registrationNumber);

                    var make = new XElement("Make", truck.Make);
                    truckElement.Add(make);
                }
            }

            return doc.ToString().TrimEnd();
        }

        public static string ExportClientsWithMostTrucks(TrucksContext context, int capacity)
        {
            var clients = context.Clients
                .Where(c => c.ClientsTrucks.Any(ct => ct.Truck.TankCapacity >= capacity))
                .Select(c => new
                {
                    c.Name,
                    Trucks = c.ClientsTrucks
                    .Where(ct => ct.Truck.TankCapacity >= capacity)
                    .OrderBy(t => t.Truck.MakeType)
                    .ThenByDescending(t => t.Truck.CargoCapacity)
                    .Select(t => new
                    {
                        TruckRegistrationNumber = t.Truck.RegistrationNumber,
                        t.Truck.VinNumber,
                        t.Truck.TankCapacity,
                        t.Truck.CargoCapacity,
                        CategoryType = t.Truck.CategoryType.ToString(),
                        MakeType = t.Truck.MakeType.ToString()
                    })
                    .ToList()
                })
                .ToList()
                .OrderByDescending(c => c.Trucks.Count)
                .ThenBy(c => c.Name)
                .Take(10)
                .ToList();

            var json = JsonConvert.SerializeObject(clients, Formatting.Indented);

            return json.ToString().TrimEnd();
        }
    }
}
