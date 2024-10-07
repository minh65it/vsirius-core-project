using MongoDB.Bson.Serialization.Attributes;

namespace StullerApiClient.Models
{
    [Serializable]
    public class Product
    {
        public int Id { get; set; }

        [BsonId]
        public string SKU { get; set; }
        public string Description { get; set; }
        public string ShortDescription { get; set; }
        public string GroupDescription { get; set; }
        public string LongGroupDescription { get; set; }
        public string MerchandisingArea { get; set; }
        public string MerchandisingCategory1 { get; set; }
        public string MerchandisingCategory2 { get; set; }
        public string MerchandisingCategory3 { get; set; }
        public string MerchandisingCategory4 { get; set; }
        public string MerchandisingCategory5 { get; set; }
        public List<WebCategory> WebCategories { get; set; }
        public string Collection { get; set; }
        public double OnHand { get; set; }
        public string Status { get; set; }
        public bool Orderable { get; set; }
        public Price Price { get; set; }
        public CustomerSpecificPrice CustomerSpecificPrice { get; set; }
        public ShowcasePrice ShowcasePrice { get; set; }
        public bool IsOnPriceList { get; set; }
        public string UnitOfSale { get; set; }
        public double Weight { get; set; }
        public string WeightUnitOfMeasure { get; set; }
        public double GramWeight { get; set; }
        public bool RingSizable { get; set; }
        public double RingSize { get; set; }
        public string RingSizeType { get; set; }
        public int LeadTime { get; set; }
        public DescriptiveElementGroup DescriptiveElementGroup { get; set; }
        public List<SetWith> SetWith { get; set; }
        public List<CanBeSetWith> CanBeSetWith { get; set; }
        public List<MadeWith> MadeWith { get; set; }
        public string StoneMapImage { get; set; }
        public List<ImageInfo> Images { get; set; }
        public List<GroupImage> GroupImages { get; set; }
        public List<Video> Videos { get; set; }
        public List<GroupVideo> GroupVideos { get; set; }
        public ConfigurationModel ConfigurationModel { get; set; }
        public int ConfigurationModelId { get; set; }
        public DateTime CreationDate { get; set; }
        public bool ReadyToWear { get; set; }
        public double DiscountPercent { get; set; }
        public string AGTA { get; set; }
        public string EarringPostType { get; set; }
        public string ProductType { get; set; }
        public List<Specification> Specifications { get; set; }
        public List<Publication> Publications { get; set; }
        public List<SearchFilter> SearchFilters { get; set; }
        public bool Hazardous { get; set; }
        public bool Heavy { get; set; }
        public int DefaultProductGroupId { get; set; }
        public string CountryOfOrigin { get; set; }
        public string CenterStoneShape { get; set; }
        public string CenterStoneSize { get; set; }
        public List<FullySetImage> FullySetImages { get; set; }
    }
}
