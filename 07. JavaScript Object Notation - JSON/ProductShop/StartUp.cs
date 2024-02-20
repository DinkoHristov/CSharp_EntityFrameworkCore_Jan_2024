using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ProductShop.Data;
using ProductShop.Data.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace ProductShop
{
    public class StartUp
    {
        static void Main(string[] args)
        {
            var db = new ProductShopContext();

            //db.Database.EnsureDeleted();
            //db.Database.EnsureCreated();

            // Query 2. Import Users
            //string inputJson = File.ReadAllText("../../../Datasets/users.json");

            //var result = ImportUsers(db, inputJson);

            // Query 3. Import Products
            //string inputJson = File.ReadAllText("../../../Datasets/products.json");

            //var result = ImportProducts(db, inputJson);

            //// Query 4. Import Categories
            //string inputJson = File.ReadAllText("../../../Datasets/categories.json");

            //var result = ImportCategories(db, inputJson);

            //// Query 5. Import Categories and Products
            //string inputJson = File.ReadAllText("../../../Datasets/categories-products.json");

            //var result = ImportCategoryProducts(db, inputJson);

            // Export Products in Range
            //var result = GetProductsInRange(db);

            // Query 6. Export Successfully Sold Products
            //var result = GetSoldProducts(db);

            // Export Categories by Products Count
            //var result = GetCategoriesByProductsCount(db);

            // Query 7. Export Users and Products
            var result = GetUsersWithProducts(db);

            Console.WriteLine(result);
        }

        public static string ImportUsers(ProductShopContext context, string inputJson)
        {
            var users = JsonConvert.DeserializeObject<List<User>>(inputJson);

            context.Users.AddRange(users);
            context.SaveChanges();

            return $"Successfully imported {users.Count}";
        }

        public static string ImportProducts(ProductShopContext context, string inputJson)
        {
            var products = JsonConvert.DeserializeObject<List<Product>>(inputJson);

            context.Products.AddRange(products);
            context.SaveChanges();

            return $"Successfully imported {products.Count}";
        }

        public static string ImportCategories(ProductShopContext context, string inputJson)
        {
            var categories = JsonConvert.DeserializeObject<List<Category>>(inputJson);

            foreach (var category in categories)
            {
                if (category.Name == null)
                {
                    continue;
                }

                context.Categories.Add(category);
            }
            context.SaveChanges();

            return $"Successfully imported {context.Categories.Count()}";
        }

        public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
        {
            var categoryProducts = JsonConvert.DeserializeObject<List<CategoryProduct>>(inputJson);

            context.CategoryProducts.AddRange(categoryProducts);
            context.SaveChanges();

            return $"Successfully imported {categoryProducts.Count}";
        }

        public static string GetProductsInRange(ProductShopContext context)
        {
            var products = context.Products
                .Where(p => p.Price >= 500 && p.Price <= 1000)
                .OrderBy(p => p.Price)
                .Select(p => new
                {
                    p.Name,
                    p.Price,
                    Seller = p.Seller.FirstName + " " + p.Seller.LastName
                })
                .ToHashSet();

            var settings = new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Formatting = Formatting.Indented
            };

            var json = JsonConvert.SerializeObject(products, settings);

            File.WriteAllText("../../../CreatedDatasets/products-in-range.json", json);

            return json;
        }

        public static string GetSoldProducts(ProductShopContext context)
        {
            var users = context.Users
                .Where(u => u.ProductsSold.Any(ps => ps.BuyerId != null))
                .Select(u => new
                {
                    u.FirstName,
                    u.LastName,
                    SoldProducts = u.ProductsSold
                    .Where(ps => ps.BuyerId != null)
                    .Select(ps => new
                    {
                        ps.Name,
                        ps.Price,
                        BuyerFirstName = ps.Buyer.FirstName,
                        BuyerLastName = ps.Buyer.LastName
                    })
                    .ToList()
                })
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                .ToList();

            var settings = new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            var json = JsonConvert.SerializeObject(users, settings);

            return json;
        }

        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            var categories = context.Categories
                .Select(c => new
                {
                    Category = c.Name,
                    ProductsCount = c.CategoryProducts.Count(),
                    AveragePrice = c.CategoryProducts.Average(cp => cp.Product.Price),
                    TotalRevenue = c.CategoryProducts.Sum(cp => cp.Product.Price)
                })
                .OrderByDescending(c => c.ProductsCount)
                .ToList();

            var settings = new JsonSerializerSettings() 
            { 
                Formatting = Formatting.Indented,
                ContractResolver= new CamelCasePropertyNamesContractResolver()
            };

            var json = JsonConvert.SerializeObject(categories, settings);

            return json;
        }

        public static string GetUsersWithProducts(ProductShopContext context)
        {
            var users = context.Users
                .Where(u => u.ProductsSold.Any(b => b.BuyerId != null))
                .Select(u => new
                {
                    u.FirstName,
                    u.LastName,
                    u.Age,
                    SoldProducts = new
                    {
                        Count = u.ProductsSold.Count,
                        Products = u.ProductsSold
                        .Where(ps => ps.BuyerId != null)
                        .Select(ps => new
                        {
                            ps.Name,
                            ps.Price
                        })
                    }
                })
                .OrderByDescending(u => u.SoldProducts.Count)
                .ToList();

            var result = new
            {
                UsersCount = users.Count,
                Users = users
            };

            var settings = new JsonSerializerSettings() 
            {
                Formatting = Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            var json = JsonConvert.SerializeObject(result, settings);

            return json;
        }
    }
}
