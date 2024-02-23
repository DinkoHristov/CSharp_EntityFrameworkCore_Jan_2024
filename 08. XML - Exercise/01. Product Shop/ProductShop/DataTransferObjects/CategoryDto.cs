using System.Xml.Serialization;

namespace ProductShop.DataTransferObjects
{
    [XmlType("Category")]
    public class CategoryDto
    {
        [XmlElement("name")]
        public string Name { get; set; }
    }
}
