namespace EtsyIntegrationDataAPI.Dtos.ProductEtsyDto
{
    /// <summary>
    /// Request
    /// </summary>
    public class Offering
    {
        public decimal price { get; set; }
        public int quantity { get; set; }
        public bool is_enabled { get; set; }
    }

    public class Products
    {
        public string sku { get; set; }
        public List<PropertyValue> property_values { get; set; }
        public List<Offering> offerings { get; set; }
    }

    public class PropertyValue
    {
        public long property_id { get; set; }
        public List<int> value_ids { get; set; }
        public int scale_id { get; set; }
        public string property_name { get; set; }
        public List<string> values { get; set; }
    }

    public class ListingInventoryDto
    {
        public List<Products> products { get; set; }
        public List<int> price_on_property { get; set; }
        public List<int> quantity_on_property { get; set; }
        public List<int> sku_on_property { get; set; }
    }

    /// <summary>
    /// Response
    /// </summary>
    public class ListingInventoryResponseDto
    {
        public List<ProductResponse> Products { get; set; }
        public List<int> PriceOnProperty { get; set; }
        public List<int> QuantityOnProperty { get; set; }
        public List<int> SkuOnProperty { get; set; }
    }

    public class ProductResponse
    {
        public long ProductId { get; set; }
        public string Sku { get; set; }
        public bool IsDeleted { get; set; }
        public List<OfferingResponse> Offerings { get; set; }
        public List<PropertyValueResponse> PropertyValues { get; set; }
    }

    public class OfferingResponse
    {
        public int OfferingId { get; set; }
        public int Quantity { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsDeleted { get; set; }
        public PriceResponse Price { get; set; }
    }

    public class PriceResponse
    {
        public decimal Amount { get; set; }
        public decimal Divisor { get; set; }
        public string CurrencyCode { get; set; }
    }

    public class PropertyValueResponse
    {
        public int PropertyId { get; set; }
        public string PropertyName { get; set; }
        public int ScaleId { get; set; }
        public string ScaleName { get; set; }
        public List<int> ValueIds { get; set; }
        public List<string> Values { get; set; }
    }
}
