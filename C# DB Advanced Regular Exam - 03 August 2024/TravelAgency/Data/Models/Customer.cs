using System.ComponentModel.DataAnnotations;

namespace TravelAgency.Data.Models
{
	public class Customer
	{
        public Customer()
        {
			Bookings = new HashSet<Booking>();
        }

        [Key]
        public int Id { get; set; }

		[Required]
		[StringLength(60, MinimumLength = 4)]
        public string FullName { get; set; }

		[Required]
		[StringLength(50, MinimumLength = 6)]
        public string Email { get; set; }

		[Required]
		[StringLength(13, MinimumLength = 13)]
		[RegularExpression("(\\+[0-9]{12})")]
        public string PhoneNumber { get; set; }

        public virtual ICollection<Booking> Bookings { get; set; }
	}
}
