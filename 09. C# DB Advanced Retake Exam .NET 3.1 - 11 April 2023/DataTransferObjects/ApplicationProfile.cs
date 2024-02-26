using AutoMapper;
using Invoices.Data.Models;

namespace Invoices.DataTransferObjects
{
    public class ApplicationProfile : Profile
    {
        public ApplicationProfile()
        {
            this.CreateMap<ClientDto, Client>();
            this.CreateMap<AddressDto, Address>();
            this.CreateMap<InvoiceDto, Invoice>();
            this.CreateMap<ProductDto, Product>();
        }
    }
}
