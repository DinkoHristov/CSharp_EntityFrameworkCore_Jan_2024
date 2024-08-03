using System.ComponentModel.DataAnnotations;

namespace TravelAgency.Data.Models
{
	public class Booking
	{
		[Key]
        public int Id { get; set; }

		[Required]
        public DateTime BookingDate { get; set; }

		[Required]
        public int CustomerId { get; set; }

        public virtual Customer Customer { get; set; }

		[Required]
        public virtual int TourPackageId { get; set; }

        public TourPackage TourPackage { get; set; }
	}
}
