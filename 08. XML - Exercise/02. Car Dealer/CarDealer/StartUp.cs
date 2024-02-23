using AutoMapper;
using CarDealer.Data;
using CarDealer.DataTransferObjects;
using CarDealer.Models;
using Castle.Core.Resource;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace CarDealer
{
    public class StartUp
    {
        private static IMapper mapper;

        public static void Main(string[] args)
        {
            var context = new CarDealerContext();

            //context.Database.EnsureDeleted();
            //context.Database.EnsureCreated();

            InitializeMapper();

            // Query 9. Import Suppliers
            //var inputXml = "../../../Datasets/suppliers.xml";
            //var result = ImportSuppliers(context, inputXml);

            // Query 10. Import Parts
            //var inputXml = "../../../Datasets/parts.xml";
            //var result = ImportParts(context, inputXml);

            // Query 11. Import Cars
            //var inputXml = "../../../Datasets/cars.xml";
            //var result = ImportCars(context, inputXml);

            // Query 12. Import Customers
            //var inputXml = "../../../Datasets/customers.xml";
            //var result = ImportCustomers(context, inputXml);

            // Query 13. Import Sales
            //var inputXml = "../../../Datasets/sales.xml";
            //var result = ImportSales(context, inputXml);

            // Query 14. Cars With Distance
            //var result = GetCarsWithDistance(context);

            // Query 15. Cars from make BMW
            //var result = GetCarsFromMakeBmw(context);

            // Query 16. Local Suppliers
            //var result = GetLocalSuppliers(context);

            // Query 17. Cars with Their List of Parts
            //var result = GetCarsWithTheirListOfParts(context);

            // Query 18. Total Sales by Customer
            //var result = GetTotalSalesByCustomer(context);

            // Query 19. Sales with Applied Discount
            var result = GetSalesWithAppliedDiscount(context);

            Console.WriteLine(result);
        }

        private static void InitializeMapper()
        {
            var configuration = new MapperConfiguration(config =>
            {
                config.AddProfile<CarDealerProfile>();
            });

            mapper = configuration.CreateMapper();
        }

        public static string ImportSuppliers(CarDealerContext context, string inputXml)
        {
            var serializer = new XmlSerializer(typeof(List<SupplierDto>), new XmlRootAttribute("Suppliers"));
            var supplierDto = (List<SupplierDto>)serializer.Deserialize(File.OpenRead(inputXml));
            var suppliers = mapper.Map<List<Supplier>>(supplierDto);

            context.Suppliers.AddRange(suppliers);
            context.SaveChanges();

            return $"Successfully imported {suppliers.Count}";
        }

        public static string ImportParts(CarDealerContext context, string inputXml)
        {
            var serializer = new XmlSerializer(typeof(List<PartDto>), new XmlRootAttribute("Parts"));
            var partsDto = (List<PartDto>)serializer.Deserialize(File.OpenRead(inputXml));
            var parts = mapper.Map<List<Part>>(partsDto);

            foreach (var part in parts)
            {
                if (!context.Suppliers.Any(s => s.Id == part.SupplierId))
                {
                    continue;
                }

                context.Parts.Add(part);
            }

            context.SaveChanges();

            return $"Successfully imported {parts.Count}";
        }

        public static string ImportCars(CarDealerContext context, string inputXml)
        {
            var ccc = context.Cars;

            var serializer = new XmlSerializer(typeof(List<CarDto>), new XmlRootAttribute("Cars"));
            var carsDto = (List<CarDto>)serializer.Deserialize(File.OpenRead(inputXml));

            var cars = mapper.Map<List<Car>>(carsDto);

            var parts = new List<List<PartCar>>();
            foreach (var car in cars)
            {
                var setOfParts = new List<PartCar>();

                foreach (var part in car.PartCars)
                {
                    if (!context.Parts.Any(p => p.Id == part.PartId) ||
                        setOfParts.Any(p => p.PartId == part.PartId))
                    {
                        continue;
                    }

                    setOfParts.Add(part);
                }

                car.PartCars = new List<PartCar>();
                parts.Add(setOfParts);
            }

            context.Cars.AddRange(cars);
            context.SaveChanges();

            var allParts = context.Parts.ToList();
            var contextCars = context.Cars.ToList();
            foreach (var car in contextCars)
            {
                foreach (var part in parts)
                {
                    foreach (var carPart in part)
                    {
                        carPart.Car = car;
                        carPart.CarId = car.Id;
                        carPart.Part = allParts.FirstOrDefault(p => p.Id == carPart.PartId);
                    }

                    car.PartCars = part;
                }
            }

            context.SaveChanges();

            return $"Successfully imported {cars.Count}";
        }

        public static string ImportCustomers(CarDealerContext context, string inputXml)
        {
            var serializer = new XmlSerializer(typeof(List<CustomerDto>), new XmlRootAttribute("Customers"));
            var customersDto = (List<CustomerDto>)serializer.Deserialize(File.OpenRead(inputXml));
            var customers = mapper.Map<List<Customer>>(customersDto);

            context.Customers.AddRange(customers);
            context.SaveChanges();

            return $"Successfully imported {customers.Count}"; ;
        }

        public static string ImportSales(CarDealerContext context, string inputXml)
        {
            var serializer = new XmlSerializer(typeof(List<SaleDto>), new XmlRootAttribute("Sales"));
            var salesDto = (List<SaleDto>)serializer.Deserialize(File.OpenRead(inputXml));
            var sales = mapper.Map<List<Sale>>(salesDto);

            var allCars = context.Cars.ToList();
            int count = 0;
            foreach (var sale in sales)
            {
                if (!allCars.Any(c => c.Id == sale.CarId))
                {
                    continue;
                }

                context.Sales.Add(sale);
                count++;
            }

            context.SaveChanges();

            var allCustomers = context.Customers.ToList();
            foreach (var sale in sales)
            {
                var car = allCars.FirstOrDefault(c => c.Id == sale.CarId);

                if (car != null)
                {
                    car.Sales.Add(sale);
                }

                var customer = allCustomers.FirstOrDefault(c => c.Id == sale.CustomerId);

                if (customer != null)
                {
                    customer.Sales.Add(sale);
                }
            }

            return $"Successfully imported {count}";
        }

        public static string GetCarsWithDistance(CarDealerContext context)
        {
            var cars = context.Cars
                .Where(c => c.TravelledDistance > 2_000_000)
                .OrderBy(c => c.Make)
                .ThenBy(c => c.Model)
                .Take(10)
                .ToList();

            var doc = new XDocument();
            var root = new XElement("cars");
            doc.Add(root);

            foreach (var car in cars)
            {
                var carElement = new XElement("car");
                root.Add(carElement);

                var make = new XElement("make", car.Make);
                var model = new XElement("model", car.Model);
                var travelledDistance = new XElement("travelled-distance", car.TravelledDistance);

                carElement.Add(make);
                carElement.Add(model);
                carElement.Add(travelledDistance);
            }

            doc.Save("../../../CreatedXmlFiles/cars.xml");

            var xml = XDocument.Load("../../../CreatedXmlFiles/cars.xml");

            return xml.ToString();
        }

        public static string GetCarsFromMakeBmw(CarDealerContext context)
        {
            var cars = context.Cars
                .Where(c => c.Make == "BMW")
                .OrderBy(c => c.Model)
                .ThenByDescending(c => c.TravelledDistance)
                .ToList();

            var doc = new XDocument();
            var root = new XElement("cars");
            doc.Add(root);

            foreach (var car in cars)
            {
                var carElement = new XElement("car");
                carElement.SetAttributeValue("id", car.Id);
                carElement.SetAttributeValue("model", car.Model);
                carElement.SetAttributeValue("travelled-distance", car.TravelledDistance);
                root.Add(carElement);
            }

            doc.Save("../../../CreatedXmlFiles/bmw-cars.xml");

            var xml = XDocument.Load("../../../CreatedXmlFiles/bmw-cars.xml");

            return xml.ToString();
        }

        public static string GetLocalSuppliers(CarDealerContext context)
        {
            var suppliers = context.Suppliers
                .Where(s => s.IsImporter == false)
                .Select(s => new
                {
                    s.Id,
                    s.Name,
                    PartsCount = s.Parts.Count
                })
                .ToList();

            var doc = new XDocument();
            var root = new XElement("suppliers");
            doc.Add(root);

            foreach (var supplier in suppliers)
            {
                var supplierElement = new XElement("supplier");
                supplierElement.SetAttributeValue("id", supplier.Id);
                supplierElement.SetAttributeValue("name", supplier.Name);
                supplierElement.SetAttributeValue("parts-count", supplier.PartsCount);
                root.Add(supplierElement);
            }

            doc.Save("../../../CreatedXmlFiles/local-suppliers.xml");

            var xml = XDocument.Load("../../../CreatedXmlFiles/local-suppliers.xml");

            return xml.ToString();
        }

        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            var cars = context.Cars
                .Select(c => new
                {
                    c.Make,
                    c.Model,
                    c.TravelledDistance,
                    Parts = c.PartCars.Select(pc => new
                    {
                        pc.Part.Name,
                        pc.Part.Price
                    })
                    .OrderByDescending(pc => pc.Price)
                    .ToList()
                })
                .OrderByDescending(c => c.TravelledDistance)
                .ThenBy(c => c.Model)
                .Take(5)
                .ToList();

            var doc = new XDocument();
            var root = new XElement("cars");
            doc.Add(root);

            foreach (var car in cars)
            {
                var carElement = new XElement("car");
                carElement.SetAttributeValue("make", car.Make);
                carElement.SetAttributeValue("model", car.Model);
                carElement.SetAttributeValue("travelled-distance", car.TravelledDistance);

                var parts = new XElement("parts");
                foreach (var part in car.Parts)
                {
                    var partElemennt = new XElement("part");
                    partElemennt.SetAttributeValue("name", part.Name);
                    partElemennt.SetAttributeValue("price", part.Price);
                    parts.Add(partElemennt);
                }

                carElement.Add(parts);
                root.Add(carElement);
            }

            doc.Save("../../../CreatedXmlFiles/cars-and-parts.xml");

            var xml = XDocument.Load("../../../CreatedXmlFiles/cars-and-parts.xml");

            return xml.ToString();
        }

        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            var customers = context.Customers
                .Where(c => c.Sales.Any())
                .Select(c => new
                {
                    FullName = c.Name,
                    BoughtCarsCount = c.Sales.Count,
                    TotalSpendMoney = c.Sales.SelectMany(s => s.Car.PartCars).Sum(pc => pc.Part.Price)
                })
                .OrderByDescending(c => c.TotalSpendMoney)
                .ToList();

            var doc = new XDocument();
            var root = new XElement("customers");
            doc.Add(root);

            foreach (var customer in customers)
            {
                var customerElement = new XElement("customer");
                customerElement.SetAttributeValue("full-name", customer.FullName);
                customerElement.SetAttributeValue("bought-cars", customer.BoughtCarsCount);
                customerElement.SetAttributeValue("spent-money", customer.TotalSpendMoney);
                root.Add(customerElement);
            }

            doc.Save("../../../CreatedXmlFiles/customers-total-sales.xml");

            var xml = XDocument.Load("../../../CreatedXmlFiles/customers-total-sales.xml");

            return xml.ToString();
        }

        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            var sales = context.Sales
                .Select(s => new
                {
                    s.Car,
                    s.Customer,
                    s.Discount,
                    Price = s.Car.PartCars.Sum(pc => pc.Part.Price),
                    PriceWithDiscount = s.Car.PartCars.Sum(pc => pc.Part.Price) - (s.Car.PartCars.Sum(pc => pc.Part.Price) * (s.Discount / 100.0M))
                })
                .ToList();

            var doc = new XDocument();
            var root = new XElement("sales");
            doc.Add(root);

            foreach (var sale in sales)
            {
                var salesElement = new XElement("sale");
                var carElement = new XElement("car");
                carElement.SetAttributeValue("make", sale.Car.Make);
                carElement.SetAttributeValue("model", sale.Car.Model);
                carElement.SetAttributeValue("travelled-distance", sale.Car.TravelledDistance);
                salesElement.Add(carElement);
                salesElement.Add("discount", sale.Discount);
                salesElement.Add("customer-name", sale.Customer.Name);
                salesElement.Add("price", sale.Price);
                salesElement.Add("price-with-discount", sale.PriceWithDiscount);
            }

            doc.Save("../../../CreatedXmlFiles/sales-discounts.xml");

            var xml = XDocument.Load("../../../CreatedXmlFiles/sales-discounts.xml");

            return xml.ToString();
        }
    }
}