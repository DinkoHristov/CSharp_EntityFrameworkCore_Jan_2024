using System.ComponentModel.DataAnnotations;

namespace TravelAgency.DataProcessor.ImportDtos
{
	public class ImportBookingModel
	{
		[Required]
		public string BookingDate { get; set; }

		public string CustomerName { get; set; }

		public string TourPackageName { get; set; }
	}
}
