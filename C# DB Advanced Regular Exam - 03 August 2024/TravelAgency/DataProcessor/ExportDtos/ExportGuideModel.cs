using System.Xml.Serialization;

namespace TravelAgency.DataProcessor.ExportDtos
{
	[XmlType("Guide")]
	public class ExportGuideModel
	{
        public string FullName { get; set; }

		[XmlArray(nameof(TourPackages))]
        public List<ExportTourPackageModel> TourPackages { get; set; }
	}

	[XmlType("TourPackage")]
	public class ExportTourPackageModel
	{
        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }
    }
}
