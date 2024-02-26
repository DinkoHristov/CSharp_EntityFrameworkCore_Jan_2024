using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Invoices.DataTransferObjects
{
    [XmlType("Address")]
    public class AddressDto
    {
        [Required]
        [MinLength(10)]
        [MaxLength(20)]
        public string StreetName { get; set; }

        [Required]
        public int StreetNumber { get; set; }

        [Required]
        public string PostCode { get; set; }

        [Required]
        [MinLength(5)]
        [MaxLength(15)]
        public string City { get; set; }

        [Required]
        [MinLength(5)]
        [MaxLength(15)]
        public string Country { get; set; }
    }
}
