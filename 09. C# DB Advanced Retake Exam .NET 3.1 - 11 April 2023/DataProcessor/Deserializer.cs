namespace Invoices.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Reflection.PortableExecutable;
    using System.Text;
    using System.Xml.Serialization;
    using AutoMapper;
    using Invoices.Data;
    using Invoices.Data.Models;
    using Invoices.DataTransferObjects;
    using Newtonsoft.Json;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedClients
            = "Successfully imported client {0}.";

        private const string SuccessfullyImportedInvoices
            = "Successfully imported invoice with number {0}.";

        private const string SuccessfullyImportedProducts
            = "Successfully imported product - {0} with {1} clients.";


        public static string ImportClients(InvoicesContext context, string xmlString)
        {
            var serializer = new XmlSerializer(typeof(List<ClientDto>), new XmlRootAttribute("Clients"));
            var clientDtos = (List<ClientDto>)serializer.Deserialize(new StringReader(xmlString));
            var mapper = CreateMapper();

            var result = new StringBuilder();
            foreach (var clientDto in clientDtos)
            {
                if (!IsValid(clientDto))
                {
                    result.AppendLine(ErrorMessage);
                    continue;
                }

                var client = mapper.Map<Client>(clientDto);

                var validAddresses = new List<Address>();
                foreach (var addressDto in clientDto.Addresses)
                {
                    if (!IsValid(addressDto))
                    {
                        result.AppendLine(ErrorMessage);
                        continue;
                    }

                    var address = mapper.Map<Address>(addressDto);

                    validAddresses.Add(address);
                }
                
                context.Addresses.AddRange(validAddresses);
                client.Addresses = validAddresses;
                context.Clients.Add(client);
                result.AppendLine(string.Format(SuccessfullyImportedClients, client.Name));
            }

            context.SaveChanges();
            return result.ToString().TrimEnd();
        }


        public static string ImportInvoices(InvoicesContext context, string jsonString)
        {
            var invoiceDtos = JsonConvert.DeserializeObject<List<InvoiceDto>>(jsonString);
            var mapper = CreateMapper();

            var clients = context.Clients.ToList();
            var result = new StringBuilder();
            foreach (var invoiceDto in invoiceDtos)
            {
                if (!IsValid(invoiceDto))
                {
                    result.AppendLine(ErrorMessage);
                    continue;
                }

                if (invoiceDto.IssueDate == DateTime.ParseExact("01/01/0001", "dd/MM/yyyy", CultureInfo.InvariantCulture) ||
                    invoiceDto.DueDate == DateTime.ParseExact("01/01/0001", "dd/MM/yyyy", CultureInfo.InvariantCulture))
                {
                    result.AppendLine(ErrorMessage);
                    continue;
                }

                var invoice = mapper.Map<Invoice>(invoiceDto);
                if (DateTime.Compare(invoiceDto.IssueDate, invoiceDto.DueDate) > 0
                    || !clients.Any(c => c.Id == invoice.ClientId))
                {
                    result.AppendLine(ErrorMessage);
                    continue;
                }

                context.Invoices.Add(invoice);
                result.AppendLine(string.Format(SuccessfullyImportedInvoices, invoice.Number));
            }

            context.SaveChanges();

            return result.ToString().TrimEnd();
        }

        public static string ImportProducts(InvoicesContext context, string jsonString)
        {
            var productsDtos = JsonConvert.DeserializeObject<List<ProductDto>>(jsonString);
            var mapper = CreateMapper();

            var allClients = context.Clients.ToList();
            var result = new StringBuilder();
            foreach (var productDto in productsDtos)
            {
                if (!IsValid(productDto))
                {
                    result.AppendLine(ErrorMessage);
                    continue;
                }

                var product = mapper.Map<Product>(productDto);
                var uniqueClientsId = new List<int>();
                foreach (var id in productDto.Clients.Distinct())
                {
                    if (!allClients.Any(c => c.Id == id))
                    {
                        result.AppendLine(ErrorMessage);
                        continue;
                    }

                    if (!uniqueClientsId.Contains(id))
                    {
                        uniqueClientsId.Add(id);
                    }
                }

                foreach (var id in uniqueClientsId)
                {
                    product.ProductsClients.Add(new ProductClient
                    {
                        Client = allClients.FirstOrDefault(c => c.Id == id)
                    });
                }

                context.Products.Add(product);
                result.AppendLine(string.Format(SuccessfullyImportedProducts, product.Name, product.ProductsClients.Count));
            }

            context.SaveChanges();

            return result.ToString().TrimEnd();
        }

        public static bool IsValid(object dto)
        {
            var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }

        private static IMapper CreateMapper()
        {
            var configuration = new MapperConfiguration(config =>
            {
                config.AddProfile<ApplicationProfile>();
            });

            return configuration.CreateMapper();
        }
    } 
}
