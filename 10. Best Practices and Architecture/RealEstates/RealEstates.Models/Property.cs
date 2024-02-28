namespace RealEstates.Models
{
    public class Property
    {
        public Property()
        {
            Tags = new HashSet<Tag>();
        }

        public int Id { get; set; }

        public int Size { get; set; }

        public int? YardSize { get; set; }

        public int? Floor { get; set; }

        public int? TotalFloors { get; set; }

        public int DistrictId { get; set; }

        public virtual District District { get; set; }

        public int? Year { get; set; }

        public int TypeId { get; set; }

        public virtual PropertyType Type { get; set; }

        public int BuildingTypeId { get; set; }

        public virtual BuildingType BuildingType { get; set; }

        // Price is in EURO
        public decimal Price { get; set; }

        public virtual ICollection<Tag> Tags { get; set; }
    }
}
