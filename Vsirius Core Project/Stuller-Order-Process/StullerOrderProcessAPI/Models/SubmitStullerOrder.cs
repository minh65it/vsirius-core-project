using MongoDB.Bson;
using StullerOrderProcessAPI.Dtos;

namespace StullerOrderProcessAPI.Models
{
    public class SubmitStullerOrder
    {
        public ObjectId _id { get; set; }
        public string? StullerOrderId {  get; set; }
        public string? ShipToName { get; set; }
        public string? ShipToAddress1 { get; set; }
        public string? ShipToAddress2 { get; set; }
        public string? ShipToAddress3 { get; set; }
        public string? ShipToCity { get; set; }
        public string? ShipToState { get; set; }
        public string? ShipToZip { get; set; }
        public string? ShipToCountryCode { get; set; }
        public string? ShipToPhone { get; set; }
        public string? ShipToProvince { get; set; }
        public List<ItemSubmit> Item { get; set; }
        public string? Description { get; set; }
        /// <summary>
        /// UPS_NEXT_DAY, UPS_NEXT_DAY_SAVR, UPS_2DAY, UPS_GROUND, FED_PRIORITY, FED_STD_OVERNIGHT, FED_2DAY, FED_GROUND, USPS_FIRST_CLASS,
        /// FED_INT_PRIORITY, UPS_WW_EXPR, UPS_STD_CANADA, FED_INT_ECONOMY, USPS_PRIORITY, FEDEX_EXPRESS_SAVER, USPS_EXPRESS, FEDEX_HOME_DELIVERY
        /// </summary>
        public string? ShipMethodCode { get; set; }
        public string? MarketplaceOrderId { get; set; }
        public string? Marketplace { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
