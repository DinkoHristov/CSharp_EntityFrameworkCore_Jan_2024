using AutoMapper;
using Microsoft.EntityFrameworkCore.Internal;
using ProductShop.Data;
using ProductShop.DataTransferObjects;
using ProductShop.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace ProductShop
{
    public class StartUp
    {
        private static IMapper mapper;

        public static void Main(string[] args)
        {
            var context = new ProductShopContext();

            //context.Database.EnsureDeleted();
            //context.Database.EnsureCreated();

            InitializeMapper();

            // Query 1. Import Users
            //var inputXml = "../../../Datasets/users.xml";
            //var result = ImportUsers(context, inputXml);

            // Query 2. Import Products
            //var inputXml = "../../../Datasets/products.xml";
            //var result = ImportProducts(context, inputXml);

            // Query 3. Import Categories
            //var inputXml = "../../../Datasets/categories.xml";
            //var result = ImportCategories(context, inputXml);

            // Query 4. Import Categories and Products
            //var inputXml = "../../../Datasets/categories-products.xml";
            //var result = ImportCategoryProducts(context, inputXml);

            // Query 5. Products In Range
            //var result = GetProductsInRange(context);

            // Query 6. Sold Products
            //var result = GetSoldProducts(context);

            // Query 7. Categories By Products Count
            //var result = GetCategoriesByProductsCount(context);

            // Query 8. Users and Products
            var result = GetUsersWithProducts(context);

            Console.WriteLine(result);
        }

        private static void InitializeMapper()
        {
            var mapperConfiguration = new MapperConfiguration(mc =>
            {
                mc.AddProfile<ProductShopProfile>();
            });

            mapper = mapperConfiguration.CreateMapper();
        }

        public static string ImportUsers(ProductShopContext context, string inputXml)
        {
            var serializer = new XmlSerializer(typeof(List<UserDto>), new XmlRootAttribute("Users"));
            var usersDto = (List<UserDto>)serializer.Deserialize(File.OpenRead(inputXml));
            var users = mapper.Map<List<User>>(usersDto);

            context.Users.AddRange(users);
            context.SaveChanges();

            return $"Successfully imported {users.Count}";
        }

        public static string ImportProducts(ProductShopContext context, string inputXml)
        {
            var serializer = new XmlSerializer(typeof(List<ProductDto>), new XmlRootAttribute("Products"));
            var productsDto = (List<ProductDto>)serializer.Deserialize(File.OpenRead(inputXml));
            var products = mapper.Map<List<Product>>(productsDto);

            context.Products.AddRange(products);
            context.SaveChanges();

            return $"Successfully imported {products.Count}";
        }

        public static string ImportCategories(ProductShopContext context, string inputXml)
        {
            var serializer = new XmlSerializer(typeof(List<CategoryDto>), new XmlRootAttribute("Categories"));
            var categoriesDto = (List<CategoryDto>)serializer.Deserialize(File.OpenRead(inputXml));
            var categories = mapper.Map<List<Category>>(categoriesDto);

            context.Categories.AddRange(categories);
            context.SaveChanges();

            return $"Successfully imported {categories.Count}";
        }

        public static string ImportCategoryProducts(ProductShopContext context, string inputXml)
        {
            var serializer = new XmlSerializer(typeof(List<CategoryProductDto>), new XmlRootAttribute("CategoryProducts"));
            var categoryProductsDto = (List<CategoryProductDto>)serializer.Deserialize(File.OpenRead(inputXml));
            var categoryProducts = mapper.Map<List<CategoryProduct>>(categoryProductsDto);

            int count = 0;
            foreach (var catProd in categoryProducts)
            {
                if (!context.Categories.Any(c => c.Id == catProd.CategoryId) ||
                    !context.Products.Any(p => p.Id == catProd.ProductId))
                {
                    continue;
                }

                context.CategoryProducts.Add(catProd);
                count++;
            }

            context.SaveChanges();

            return $"Successfully imported {count}";
        }

        public static string GetProductsInRange(ProductShopContext context)
        {
            var products = context.Products
                .Where(p => p.Price >= 500 && p.Price <= 1000)
                .Select(p => new
                {
                    p.Name,
                    p.Price,
                    Buyer = p.Buyer.FirstName + " " + p.Buyer.LastName
                })
                .OrderBy(p => p.Price)
                .Take(10)
                .ToList();

            var doc = new XDocument();
            var root = new XElement("Products");
            doc.Add(root);

            foreach (var product in products)
            {
                var productElement = new XElement("Product");
                var name = new XElement("name", product.Name);
                var price = new XElement("price", product.Price);

                productElement.Add(name);
                productElement.Add(price);

                if (product.Buyer != null)
                {
                    var buyer = new XElement("buyer", product.Buyer);
                    productElement.Add(buyer);
                }

                root.Add(productElement);
            }

            doc.Save("../../../CreatedXmlFiles/products-in-range.xml");

            var xml = XDocument.Load(File.OpenRead("../../../CreatedXmlFiles/products-in-range.xml"));

            return xml.ToString();
        }

        public static string GetSoldProducts(ProductShopContext context)
        {
            var users = context.Users
                .Where(u => u.ProductsSold.Any())
                .Select(u => new
                {
                    u.FirstName,
                    u.LastName,
                    SoldProducts = u.ProductsSold.Select(sp => new
                    {
                        sp.Name,
                        sp.Price
                    })
                    .ToList()
                })
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                .Take(5)
                .ToList();

            var doc = new XDocument();
            var root = new XElement("Users");
            doc.Add(root);

            foreach (var user in users)
            {
                var userElement = new XElement("User");
                var firstName = new XElement("firstName", user.FirstName);
                var lastName = new XElement("lastName", user.LastName);
                var soldProductsElement = new XElement("soldProducts");

                root.Add(userElement);
                userElement.Add(firstName);
                userElement.Add(lastName);
                userElement.Add(soldProductsElement);

                foreach (var product in user.SoldProducts)
                {
                    var productElement = new XElement("Product");
                    var name = new XElement("name", product.Name);
                    var price = new XElement("price", product.Price);

                    soldProductsElement.Add(productElement);
                    productElement.Add(name);
                    productElement.Add(price);
                }
            }

            doc.Save("../../../CreatedXmlFiles/users-sold-products.xml");

            var xml = XDocument.Load("../../../CreatedXmlFiles/users-sold-products.xml");

            return xml.ToString();
        }

        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            var categories = context.Categories
                .Select(c => new
                {
                    c.Name,
                    c.CategoryProducts.Count,
                    AveragePrice = $"{c.CategoryProducts.Average(cp => cp.Product.Price):F2}",
                    TotalRevenue = $"{c.CategoryProducts.Sum(cp => cp.Product.Price):F2}"
                })
                .OrderByDescending(c => c.Count)
                .ThenBy(c => c.TotalRevenue)
                .ToList();

            var doc = new XDocument();
            var root = new XElement("Categories");
            doc.Add(root);

            foreach (var category in categories)
            {
                var categoryElement = new XElement("Category");
                var name = new XElement("name", category.Name);
                var count = new XElement("count", category.Count);
                var averagePrice = new XElement("averagePrice", category.AveragePrice);
                var totalRevenue = new XElement("totalRevenue", category.TotalRevenue);

                categoryElement.Add(name);
                categoryElement.Add(count);
                categoryElement.Add(averagePrice);
                categoryElement.Add(totalRevenue);

                root.Add(categoryElement);
            }

            doc.Save("../../../CreatedXmlFiles/categories-by-products.xml");

            var xml = XDocument.Load("../../../CreatedXmlFiles/categories-by-products.xml");

            return xml.ToString();
        }

        public static string GetUsersWithProducts(ProductShopContext context)
        {
            var users = context.Users
                .Where(u => u.ProductsSold.Any())
                .Select(u => new
                {
                    u.FirstName,
                    u.LastName,
                    u.Age,
                    u.ProductsSold.Count,
                    Products = u.ProductsSold.Select(ps => new
                    {
                        ps.Name,
                        ps.Price
                    })
                    .OrderByDescending(ps => ps.Price)
                    .Take(10)
                    .ToList()
                })
                .OrderByDescending(u => u.Count)
                .ToList();

            var doc = new XDocument();
            var root = new XElement("Users");
            doc.Add(root);

            var countElement = new XElement("count", users.Count);
            root.Add(countElement);

            var usersRootElement = new XElement("users");
            root.Add(usersRootElement);
            foreach (var user in users)
            {
                var userElement = new XElement("User");
                usersRootElement.Add(userElement);

                var firstName = new XElement("firstName", user.FirstName);
                var lastName = new XElement("lastName", user.LastName);
                var age = new XElement("age", user.Age);
                userElement.Add(firstName);
                userElement.Add(lastName);
                userElement.Add(age);

                var soldProductsElement = new XElement("SoldProducts");
                userElement.Add(soldProductsElement);
                var count = new XElement("count", user.Count);
                soldProductsElement.Add(count);
                var productRootElement = new XElement("products");
                soldProductsElement.Add(productRootElement);

                foreach (var product in user.Products)
                {
                    var productElement = new XElement("Product");
                    productRootElement.Add(productElement);

                    var name = new XElement("name", product.Name);
                    var price = new XElement("price", product.Price);

                    productElement.Add(name);
                    productElement.Add(price);
                }
            }

            doc.Save("../../../CreatedXmlFiles/users-and-products.xml");

            var xml = XDocument.Load("../../../CreatedXmlFiles/users-and-products.xml");

            return xml.ToString();
        }
    }
}