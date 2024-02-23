using System.Xml.Serialization;

namespace ProductShop.DataTransferObjects
{
    [XmlType("CategoryProduct")]
    public class CategoryProductDto
    {
        public int CategoryId { get; set; }

        public int ProductId { get; set; }
    }
}
