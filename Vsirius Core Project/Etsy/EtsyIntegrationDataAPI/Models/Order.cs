using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EtsyIntegrationDataAPI.Models
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long receipt_id { get; set; }
        public long? receipt_type { get; set; }
        public long? seller_user_id { get; set; }
        public string? seller_email { get; set; }
        public long? buyer_user_id { get; set; }
        public string? buyer_email { get; set; }
        public string? name { get; set; }
        public string? first_line { get; set; }
        public string? second_line { get; set; }
        public string? city { get; set; }
        public string? state { get; set; }
        public string? zip { get; set; }
        public string? status { get; set; }
        public string? formatted_address { get; set; }
        public string? country_iso { get; set; }
        public string? payment_method { get; set; }
        public string? payment_email { get; set; }
        public string? message_from_seller { get; set; }
        public string? message_from_buyer { get; set; }
        public string? message_from_payment { get; set; }
        public bool? is_paid { get; set; }
        public bool? is_shipped { get; set; }
        public long? create_timestamp { get; set; }
        public long? created_timestamp { get; set; }
        public long? update_timestamp { get; set; }
        public long? updated_timestamp { get; set; }
        public bool? is_gift { get; set; }
        public string? gift_message { get; set; }
        public string? gift_sender { get; set; }
        public long? grandtotal_amount { get; set; }
        public long? grandtotal_divisor { get; set; }
        public string? grandtotal_currency_code { get; set; }
        public long? subtotal_amount { get; set; }
        public long? subtotal_divisor { get; set; }
        public string? subtotal_currency_code { get; set; }
        public long? total_price_amount { get; set; }
        public long? total_price_divisor { get; set; }
        public string? total_price_currency_code { get; set; }
        public long? total_shipping_cost_amount { get; set; }
        public long? total_shipping_cost_divisor { get; set; }
        public string? total_shipping_cost_currency_code { get; set; }
        public long? total_tax_cost_amount { get; set; }
        public long? total_tax_cost_divisor { get; set; }
        public string? total_tax_cost_currency_code { get; set; }
        public long? total_vat_cost_amount { get; set; }
        public long? total_vat_cost_divisor { get; set; }
        public string? total_vat_cost_currency_code { get; set; }
        public long? discount_amt_amount { get; set; }
        public long? discount_amt_divisor { get; set; }
        public string? discount_amt_currency_code { get; set; }
        public long? gift_wrap_price_amount { get; set; }
        public long? gift_wrap_price_divisor { get; set; }
        public string? gift_wrap_price_currency_code { get; set; }
        public bool? is_submit { get; set; }
        [ForeignKey("receipt_id")]
        public ICollection<Shipment> shipments { get; set; }
        [ForeignKey("receipt_id")]
        public ICollection<Transaction> transactions { get; set; }
        [ForeignKey("receipt_id")]
        public ICollection<Refund> refunds { get; set; }
    }

    public class Shipment
    {
        [Key]
        public long id { get; set; }
        public long? receipt_shipping_id { get; set; }
        public long? shipment_notification_timestamp { get; set; }
        public string? carrier_name { get; set; }
        public string? tracking_code { get; set; }
        public long receipt_id { get; set; }
    }
    public class Transaction
    {
        [Key]
        public long id { get; set; }
        public long? transaction_id { get; set; }
        public string? title { get; set; }
        public string? description { get; set; }
        public long? seller_user_id { get; set; }
        public long? buyer_user_id { get; set; }
        public long? create_timestamp { get; set; }
        public long? created_timestamp { get; set; }
        public long? paid_timestamp { get; set; }
        public long? shipped_timestamp { get; set; }
        public long? quantity { get; set; }
        public long? listing_image_id { get; set; }
        public long receipt_id { get; set; }
        public bool? is_digital { get; set; }
        public string? file_data { get; set; }
        public decimal? listing_id { get; set; }
        public string? transaction_type { get; set; }
        public long? product_id { get; set; }
        public string? sku { get; set; }
        public long? price_amount { get; set; }
        public long? price_divisor { get; set; }
        public string? price_currency_code { get; set; }
        public long? shipping_cost_amount { get; set; }
        public long? shipping_cost_divisor { get; set; }
        public string? shipping_cost_currency_code { get; set; }
        [ForeignKey("transaction_id")]
        public ICollection<Variation> variation { get; set; }
        [ForeignKey("transaction_id")]
        public ICollection<ProductDatum> product_data { get; set; }
        public long? shipping_profile_id { get; set; }
        public long? min_processing_days { get; set; }
        public long? max_processing_days { get; set; }
        public string? shipping_method { get; set; }
        public string? shipping_upgrade { get; set; }
        public long? expected_ship_date { get; set; }
        public long? buyer_coupon { get; set; }
        public long? shop_coupon { get; set; }
    }
    public class Refund
    {
        [Key]
        public long id { get; set; }
        public long? amount { get; set; }
        public long? amount_divisor { get; set; }
        public string? amount_currency_code { get; set; }
        public long? created_timestamp { get; set; }
        public string? reason { get; set; }
        public string? note_from_issuer { get; set; }
        public string? status { get; set; }
        public long receipt_id { get; set; }
    }

    public class Variation
    {
        [Key]
        public long id { get; set; }
        public long? property_id { get; set; }
        public long? value_id { get; set; }
        public string? formatted_name { get; set; }
        public string? formatted_value { get; set; }
        public long transaction_id { get; set; }
    }

    public class ProductDatum
    {
        [Key]
        public long id { get; set; }
        public long? property_id { get; set; }
        public string? property_name { get; set; }
        public long? scale_id { get; set; }
        public string? scale_name { get; set; }
        public string? value_ids { get; set; }
        public string? values { get; set; }
        public long transaction_id { get; set; }
    }
}
