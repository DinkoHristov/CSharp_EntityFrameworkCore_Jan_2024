using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text;
using System.Xml.Serialization;
using TravelAgency.Data;
using TravelAgency.Data.Models;
using TravelAgency.DataProcessor.ImportDtos;

namespace TravelAgency.DataProcessor
{
	public class Deserializer
    {
        private const string ErrorMessage = "Invalid data format!";
        private const string DuplicationDataMessage = "Error! Data duplicated.";
        private const string SuccessfullyImportedCustomer = "Successfully imported customer - {0}";
        private const string SuccessfullyImportedBooking = "Successfully imported booking. TourPackage: {0}, Date: {1}";

        public static string ImportCustomers(TravelAgencyContext context, string xmlString)
        {
            var result = new StringBuilder();

            var serializer = new XmlSerializer(typeof(List<ImportCustomerModel>), new XmlRootAttribute("Customers"));

            var customerDtos = (List<ImportCustomerModel>)serializer.Deserialize(new StringReader(xmlString));

            foreach (var dto in customerDtos) 
            {
                if (!IsValid(dto))
                {
                    result.AppendLine(ErrorMessage);

                    continue;
                }

				if (context.Customers.Any(c => c.FullName == dto.FullName) ||
					context.Customers.Any(c => c.Email == dto.Email) ||
					context.Customers.Any(c => c.PhoneNumber == dto.PhoneNumber))
                {
                    result.AppendLine(DuplicationDataMessage);

                    continue;
                }

                var customer = new Customer()
                {
                    FullName = dto.FullName,
                    Email = dto.Email,
                    PhoneNumber = dto.PhoneNumber
                };

                context.Customers.Add(customer);

				context.SaveChanges();

				result.AppendLine(string.Format(SuccessfullyImportedCustomer, customer.FullName));
            }

			return result.ToString().TrimEnd();
        }

        public static string ImportBookings(TravelAgencyContext context, string jsonString)
        {
            var result = new StringBuilder();

            var bookingDtos = JsonConvert.DeserializeObject<List<ImportBookingModel>>(jsonString);

            foreach (var dto in bookingDtos)
            {
                if (!IsValid(dto) || dto.CustomerName == null || dto.TourPackageName == null)
                {
                    result.AppendLine(ErrorMessage);

                    continue;
                }

                DateTime.TryParseExact(dto.BookingDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime bookingDate);

                if (bookingDate == DateTime.ParseExact("0001-01-01", "yyyy-MM-dd", CultureInfo.InvariantCulture))
                {
					result.AppendLine(ErrorMessage);

					continue;
				}

                var booking = new Booking()
                {
                    BookingDate = bookingDate,
					Customer = context.Customers.FirstOrDefault(c => c.FullName == dto.CustomerName),
				    TourPackage = context.TourPackages.FirstOrDefault(tp => tp.PackageName == dto.TourPackageName)
			    };

                context.Bookings.Add(booking);

				context.SaveChanges();

				result.AppendLine(string.Format(SuccessfullyImportedBooking, dto.TourPackageName, booking.BookingDate.ToString("yyyy-MM-dd")));
			}

			return result.ToString().TrimEnd();
        }

        public static bool IsValid(object dto)
        {
            var validateContext = new ValidationContext(dto);
            var validationResults = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(dto, validateContext, validationResults, true);

            foreach (var validationResult in validationResults)
            {
                string currValidationMessage = validationResult.ErrorMessage;
            }

            return isValid;
        }
    }
}
