using MongoDB.Bson.Serialization.Attributes;

namespace StullerDataApi.Dtos
{
    [Serializable]
    public class ProductDto
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
        public List<WebCategoryDto> WebCategories { get; set; }
        public string Collection { get; set; }
        public double OnHand { get; set; }
        public string Status { get; set; }
        public bool Orderable { get; set; }
        public PriceDto Price { get; set; }
        public CustomerSpecificPriceDto CustomerSpecificPrice { get; set; }
        public ShowcasePriceDto ShowcasePrice { get; set; }
        public bool IsOnPriceList { get; set; }
        public string UnitOfSale { get; set; }
        public double Weight { get; set; }
        public string WeightUnitOfMeasure { get; set; }
        public double GramWeight { get; set; }
        public bool RingSizable { get; set; }
        public double RingSize { get; set; }
        public string RingSizeType { get; set; }
        public int LeadTime { get; set; }
        public DescriptiveElementGroupDto DescriptiveElementGroup { get; set; }
        public List<SetWithDto> SetWith { get; set; }
        public List<CanBeSetWithDto> CanBeSetWith { get; set; }
        public List<MadeWithDto> MadeWith { get; set; }
        public string StoneMapImage { get; set; }
        public List<ImageInfoDto> Images { get; set; }
        public List<GroupImageDto> GroupImages { get; set; }
        public List<VideoDto> Videos { get; set; }
        public List<GroupVideoDto> GroupVideos { get; set; }
        public ConfigurationModelDto ConfigurationModel { get; set; }
        public int ConfigurationModelId { get; set; }
        public DateTime CreationDate { get; set; }
        public bool ReadyToWear { get; set; }
        public double DiscountPercent { get; set; }
        public string AGTA { get; set; }
        public string EarringPostType { get; set; }
        public string ProductType { get; set; }
        public List<SpecificationDto> Specifications { get; set; }
        public List<PublicationDto> Publications { get; set; }
        public List<SearchFilterDto> SearchFilters { get; set; }
        public bool Hazardous { get; set; }
        public bool Heavy { get; set; }
        public int DefaultProductGroupId { get; set; }
        public string CountryOfOrigin { get; set; }
        public string CenterStoneShape { get; set; }
        public string CenterStoneSize { get; set; }
        public List<FullySetImageDto> FullySetImages { get; set; }
    }
}
