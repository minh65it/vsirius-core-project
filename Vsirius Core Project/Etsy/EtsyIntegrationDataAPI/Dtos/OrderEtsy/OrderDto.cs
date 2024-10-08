namespace EtsyIntegrationDataAPI.Dtos.OrderEtsy
{
    public class AmountDto
    {
        public long amount { get; set; }
        public long divisor { get; set; }
        public string currency_code { get; set; }
    }

    public class DiscountAmtDto
    {
        public long amount { get; set; }
        public long divisor { get; set; }
        public string currency_code { get; set; }
    }

    public class GiftWrapPriceDto
    {
        public long amount { get; set; }
        public long divisor { get; set; }
        public string currency_code { get; set; }
    }

    public class GrandtotalDto
    {
        public long amount { get; set; }
        public long divisor { get; set; }
        public string currency_code { get; set; }
    }

    public class PriceDto
    {
        public long amount { get; set; }
        public long divisor { get; set; }
        public string currency_code { get; set; }
    }

    public class ProductDatumDto
    {
        public long property_id { get; set; }
        public string property_name { get; set; }
        public long scale_id { get; set; }
        public string scale_name { get; set; }
        public List<long> value_ids { get; set; }
        public List<string> values { get; set; }
    }

    public class RefundDto
    {
        public AmountDto amount { get; set; }
        public long created_timestamp { get; set; }
        public string reason { get; set; }
        public string note_from_issuer { get; set; }
        public string status { get; set; }
    }

    public class OrderDto
    {
        public long receipt_id { get; set; }
        public long receipt_type { get; set; }
        public long seller_user_id { get; set; }
        public string seller_email { get; set; }
        public long buyer_user_id { get; set; }
        public string buyer_email { get; set; }
        public string name { get; set; }
        public string first_line { get; set; }
        public string second_line { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zip { get; set; }
        public string status { get; set; }
        public string supplier { get; set; }
        public string formatted_address { get; set; }
        public string country_iso { get; set; }
        public string payment_method { get; set; }
        public string payment_email { get; set; }
        public string message_from_seller { get; set; }
        public string message_from_buyer { get; set; }
        public string message_from_payment { get; set; }
        public bool is_paid { get; set; }
        public bool is_shipped { get; set; }
        public long create_timestamp { get; set; }
        public long created_timestamp { get; set; }
        public long update_timestamp { get; set; }
        public long updated_timestamp { get; set; }
        public bool is_gift { get; set; }
        public string gift_message { get; set; }
        public string gift_sender { get; set; }
        public GrandtotalDto grandtotal { get; set; }
        public SubtotalDto subtotal { get; set; }
        public TotalPriceDto total_price { get; set; }
        public TotalShippingCostDto total_shipping_cost { get; set; }
        public TotalTaxCostDto total_tax_cost { get; set; }
        public TotalVatCostDto total_vat_cost { get; set; }
        public DiscountAmtDto discount_amt { get; set; }
        public GiftWrapPriceDto gift_wrap_price { get; set; }
        public List<ShipmentDto> shipments { get; set; }
        public List<TransactionDto> transactions { get; set; }
        public List<RefundDto> refunds { get; set; }
    }

    public class ShopReceiptsDto
    {
        public long count { get; set; }
        public List<OrderDto> results { get; set; }
    }

    public class ShipmentDto
    {
        public long receipt_shipping_id { get; set; }
        public long shipment_notification_timestamp { get; set; }
        public string carrier_name { get; set; }
        public string tracking_code { get; set; }
    }

    public class ShippingCostDto
    {
        public long amount { get; set; }
        public long divisor { get; set; }
        public string currency_code { get; set; }
    }

    public class SubtotalDto
    {
        public long amount { get; set; }
        public long divisor { get; set; }
        public string currency_code { get; set; }
    }

    public class TotalPriceDto
    {
        public long amount { get; set; }
        public long divisor { get; set; }
        public string currency_code { get; set; }
    }

    public class TotalShippingCostDto
    {
        public long amount { get; set; }
        public long divisor { get; set; }
        public string currency_code { get; set; }
    }

    public class TotalTaxCostDto
    {
        public long amount { get; set; }
        public long divisor { get; set; }
        public string currency_code { get; set; }
    }

    public class TotalVatCostDto
    {
        public long amount { get; set; }
        public long divisor { get; set; }
        public string currency_code { get; set; }
    }

    public class TransactionDto
    {
        public long transaction_id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public long seller_user_id { get; set; }
        public long buyer_user_id { get; set; }
        public long create_timestamp { get; set; }
        public long created_timestamp { get; set; }
        public long paid_timestamp { get; set; }
        public long? shipped_timestamp { get; set; }
        public long quantity { get; set; }
        public long listing_image_id { get; set; }
        public long receipt_id { get; set; }
        public bool is_digital { get; set; }
        public string file_data { get; set; }
        public long listing_id { get; set; }
        public string transaction_type { get; set; }
        public long? product_id { get; set; }
        public string sku { get; set; }
        public PriceDto price { get; set; }
        public ShippingCostDto shipping_cost { get; set; }
        public List<VariationDto> variations { get; set; }
        public List<ProductDatumDto> product_data { get; set; }
        public long shipping_profile_id { get; set; }
        public long min_processing_days { get; set; }
        public long max_processing_days { get; set; }
        public string shipping_method { get; set; }
        public string shipping_upgrade { get; set; }
        public long expected_ship_date { get; set; }
        public long buyer_coupon { get; set; }
        public long shop_coupon { get; set; }
    }

    public class VariationDto
    {
        public long property_id { get; set; }
        public long value_id { get; set; }
        public string formatted_name { get; set; }
        public string formatted_value { get; set; }
    }


}
