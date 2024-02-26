using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Invoices.DataTransferObjects
{
    [XmlType("Client")]
    public class ClientDto
    {
        [Required]
        [MinLength(10)]
        [MaxLength(25)]
        public string Name { get; set; }

        [Required]
        [MinLength(10)]
        [MaxLength(15)]
        public string NumberVat { get; set; }

        [XmlArray("Addresses")]
        public List<AddressDto> Addresses { get; set; }
    }
}
