using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace TravelAgency.DataProcessor.ImportDtos
{
	[XmlType("Customer")]
	public class ImportCustomerModel
	{
		[Required]
		[StringLength(60, MinimumLength = 4)]
		public string FullName { get; set; }

		[Required]
		[StringLength(50, MinimumLength = 6)]
		public string Email { get; set; }

		[XmlAttribute("phoneNumber")]
		[Required]
		[StringLength(13, MinimumLength = 13)]
		[RegularExpression("(\\+[0-9]{12})")]
		public string PhoneNumber { get; set; }
	}
}
