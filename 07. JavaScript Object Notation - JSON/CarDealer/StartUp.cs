using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CarDealer.Data;
using CarDealer.Data.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace CarDealer
{
    public class StartUp
    {
        static void Main(string[] args)
        {
            var context = new CarDealerContext();

            //context.Database.EnsureDeleted();
            //context.Database.EnsureCreated();

            // Query 8. Import Suppliers
            //string inputJson = File.ReadAllText("../../../Datasets/suppliers.json");
            //var result = ImportSuppliers(context, inputJson);

            // Query 9. Import Parts
            //var inputJson = File.ReadAllText("../../../Datasets/parts.json");
            //var result = ImportParts(context, inputJson);

            // Query 10. Import Cars
            //var inputJson = File.ReadAllText("../../../Datasets/cars.json");
            //var result = ImportCars(context, inputJson);

            // Query 11. Import Customers
            //var inputJson = File.ReadAllText("../../../Datasets/customers.json");
            //var result = ImportCustomers(context, inputJson);

            // Query 12. Import Sales
            //var inputJson = File.ReadAllText("../../../Datasets/sales.json");
            //var result = ImportSales(context, inputJson);

            // Query 13. Export Ordered Customers
            //var result = GetOrderedCustomers(context);

            // Export Cars from Make Toyota
            //var result = GetCarsFromMakeToyota(context);

            // Query 14. Export Local Suppliers
            //var result = GetLocalSuppliers(context);

            // Query 15. Export Cars with Their List of Parts
            //var result = GetCarsWithTheirListOfParts(context);

            // Query 16. Export Total Sales by Customer
            //var result = GetTotalSalesByCustomer(context);

            // Query 17. Export Sales with Applied Discount
            var result = GetSalesWithAppliedDiscount(context);

            Console.WriteLine(result);
        }

        public static string ImportSuppliers(CarDealerContext context, string inputJson)
        {

            var suppliers = JsonConvert.DeserializeObject<List<Supplier>>(inputJson);

            context.Suppliers.AddRange(suppliers);
            context.SaveChanges();

            return $"Successfully imported {suppliers.Count}."; ;
        }

        public static string ImportParts(CarDealerContext context, string inputJson)
        {
            var parts = JsonConvert.DeserializeObject<List<Part>>(inputJson);

            foreach (var part in parts)
            {
                if (context.Suppliers.Any(sup => sup.Id == part.SupplierId))
                {
                    context.Parts.Add(part);
                }
            }

            context.SaveChanges();

            return $"Successfully imported {context.Parts.Count()}."; ;
        }

        public static string ImportCars(CarDealerContext context, string inputJson)
        {
            var cars = JsonConvert.DeserializeObject<List<Car>>(inputJson);

            context.Cars.AddRange(cars);
            context.SaveChanges();

            return $"Successfully imported {cars.Count}."; ;
        }

        public static string ImportCustomers(CarDealerContext context, string inputJson)
        {
            var customers = JsonConvert.DeserializeObject<List<Customer>>(inputJson);

            context.Customers.AddRange(customers);
            context.SaveChanges();

            return $"Successfully imported {customers.Count}.";
        }

        public static string ImportSales(CarDealerContext context, string inputJson)
        {
            var sales = JsonConvert.DeserializeObject<List<Sale>>(inputJson);

            context.Sales.AddRange(sales);
            context.SaveChanges();

            return $"Successfully imported {sales.Count}.";
        }

        public static string GetOrderedCustomers(CarDealerContext context)
        {
            var customers = context.Customers
                .OrderBy(c => c.BirthDate)
                .ThenBy(c => c.IsYoundDriver == true)
                .Select(c => new
                {
                    c.Name,
                    c.BirthDate,
                    c.IsYoundDriver
                })
                .ToList();

            var settings = new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            var json = JsonConvert.SerializeObject(customers, settings);

            return json;
        }

        public static string GetCarsFromMakeToyota(CarDealerContext context)
        {
            var cars = context.Cars
                .Where(c => c.Make == "Toyota")
                .Select(c => new
                {
                    c.Id,
                    c.Make,
                    c.Model,
                    c.TravelledDistance
                })
                .OrderBy(c => c.Model)
                .ThenByDescending(c => c.TravelledDistance)
                .ToList();

            var settings = new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            var json = JsonConvert.SerializeObject(cars, settings);

            return json;
        }

        public static string GetLocalSuppliers(CarDealerContext context)
        {
            var suppliers = context.Suppliers
                .Where(s => s.IsImported == false)
                .Select(s => new
                {
                    s.Id,
                    s.Name,
                    PartsCount = s.Parts.Count
                })
                .ToList();

            var settings = new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            var json = JsonConvert.SerializeObject(suppliers, settings);

            return json;
        }

        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            var cars = context.Cars
                .Select(c => new
                {
                    Car = new
                    {
                        c.Make,
                        c.Model,
                        c.TravelledDistance
                    },
                    Parts = c.PartCars.Select(p => new
                    {
                        p.Part.Name,
                        Price = $"{p.Part.Price:F2}"
                    })
                    .ToList()
                })
                .ToList();

            var settings = new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            var json = JsonConvert.SerializeObject(cars, settings);

            return json;
        }

        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            var customers = context.Customers
                .Where(c => c.Sales.Any())
                .Select(c => new
                {
                    FullName = c.Name,
                    BoughtCars = c.Sales.Count,
                    SpentMoney = c.Sales.SelectMany(s => s.Car.PartCars).Sum(pc => pc.Part.Price)
                })
                .OrderByDescending(c => c.SpentMoney)
                .ThenByDescending(c => c.BoughtCars)
                .ToList();

            var settings = new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            var json = JsonConvert.SerializeObject(customers, settings);

            return json;
        }

        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            var sales = context.Sales
                .Select(s => new
                {
                    Car = new
                    {
                        s.Car.Make,
                        s.Car.Model,
                        s.Car.TravelledDistance
                    },
                    s.Customer.Name,
                    s.Discount,
                    Price = $"{s.Car.PartCars.Sum(pc => pc.Part.Price):F2}",
                    PriceWithDiscount = $"{s.Car.PartCars.Sum(pc => pc.Part.Price) - (s.Car.PartCars.Sum(pc => pc.Part.Price) * s.Discount)}"
                })
                .Take(10);

            var settings = new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            var json = JsonConvert.SerializeObject(sales, settings);

            return json;
        }
    }
}
