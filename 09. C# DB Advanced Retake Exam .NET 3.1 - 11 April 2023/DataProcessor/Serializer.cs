namespace Invoices.DataProcessor
{
    using Invoices.Data;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;
    using System;
    using System.Linq;
    using System.Xml.Linq;

    public class Serializer
    {
        public static string ExportClientsWithTheirInvoices(InvoicesContext context, DateTime date)
        {
            var clients = context.Clients
                .Include(c => c.Invoices)
                .Where(c => c.Invoices.Any(i => DateTime.Compare(i.IssueDate, date) > 0))
                .Select(c => new
                {
                    c.Name,
                    VatNumber = c.NumberVat,
                    Invoices = c.Invoices
                    .OrderBy(i => i.IssueDate)
                    .ThenByDescending(i => i.DueDate)
                    .Select(i => new
                    {
                        i.Number,
                        i.Amount,
                        Currency = i.CurrencyType.ToString(),
                        DueDate = i.DueDate.ToString("MM/dd/yyyy")
                    })
                    .ToList()
                })
                .ToList()
                .OrderByDescending(c => c.Invoices.Count)
                .ThenBy(c => c.Name)
                .ToList();

            var doc = new XDocument();
            var root = new XElement("Clients");
            doc.Add(root);

            foreach (var client in clients)
            {
                var clientElement = new XElement("Client");
                clientElement.SetAttributeValue("InvoicesCount", client.Invoices.Count);
                root.Add(clientElement);

                var name = new XElement("ClientName", client.Name);
                clientElement.Add(name);

                var vatNumber = new XElement("VatNumber", client.VatNumber);
                clientElement.Add(vatNumber);

                var invoicesElement = new XElement("Invoices");
                clientElement.Add(invoicesElement);

                foreach (var invoice in client.Invoices)
                {
                    var invoiceElement = new XElement("Invoice");
                    invoicesElement.Add(invoiceElement);

                    var number = new XElement("InvoiceNumber", invoice.Number);
                    invoiceElement.Add(number);

                    var amount = new XElement("InvoiceAmount", invoice.Amount);
                    invoiceElement.Add(amount);

                    var dueDate = new XElement("DueDate", invoice.DueDate);
                    invoiceElement.Add(dueDate);

                    var currency = new XElement("Currency", invoice.Currency);
                    invoiceElement.Add(currency);
                }
            }

            return doc.ToString().TrimEnd();
        }

        public static string ExportProductsWithMostClients(InvoicesContext context, int nameLength)
        {
            var products = context.Products
               .Where(p => p.ProductsClients.Any(pc => pc.Client.Name.Length >= nameLength))
               .Select(p => new
               {
                   p.Name,
                   p.Price,
                   Category = p.CategoryType.ToString(),
                   Clients = p.ProductsClients
                   .Where(pc => pc.Client.Name.Length >= nameLength)
                   .Select(pc => new
                   {
                       pc.Client.Name,
                       pc.Client.NumberVat
                   })
                   .OrderBy(pc => pc.Name)
                   .ToList()
               })
               .ToList()
               .OrderByDescending(p => p.Clients.Count)
               .ThenBy(p => p.Name)
               .Take(5)
               .ToList();

            return JsonConvert.SerializeObject(products, Formatting.Indented);
        }
    }
}