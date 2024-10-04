using AmazonIntegrationDataApi.Models.OrderAmazonProcessor;

namespace AmazonIntegrationDataApi.Helpers.OrderAmazonProcessor.SubmitOrderToQgold
{
    public interface IMappingOrder
    {
        public List<QgoldFtpOrderObject> ConvertOrder(List<OrderAmazon>? orderDB);
    }
    public class MappingOrder : IMappingOrder
    {
        public List<QgoldFtpOrderObject> ConvertOrder(List<OrderAmazon>? orderDB)
        {
            List<QgoldFtpOrderObject> orders = new List<QgoldFtpOrderObject>();
            if (orderDB == null) return orders;
            foreach (var item in orderDB)
            {
                try
                {
                    List<QgoldFtpOrderItem> item1 = new List<QgoldFtpOrderItem>();
                    foreach (var i in item.OrderItemList)
                    {
                        if (i.SellerSKU.StartsWith("Q-"))
                        {
                            item1.Add(new QgoldFtpOrderItem()
                            {
                                Item = i.SellerSKU[2..],
                                Price = i.ItemPrice.Amount,
                                Qty = i.QuantityOrdered.ToString(),
                                //Size = null
                            });
                        }
                    }
                    if (item1.Count > 0)
                    {
                        orders.Add(new QgoldFtpOrderObject()
                        {
                            PO = null,
                            ShipToName = item.ShippingAddress.Name,
                            ShipToAddress1 = item.ShippingAddress.AddressLine1,
                            ShipToAddress2 = item.ShippingAddress.AddressLine2,
                            ShipToAddress3 = item.ShippingAddress.AddressLine3,
                            ShipToCity = item.ShippingAddress.City,
                            ShipToState_Province = GetStateCode(item.ShippingAddress.StateOrRegion, item.ShippingAddress.CountryCode),
                            ShipToZip = item.ShippingAddress.PostalCode,
                            ShipToCountryCode = item.ShippingAddress.CountryCode,
                            ShipToPhone = item.ShippingAddress.Phone,
                            VSiriusItem = null,
                            Description = null,
                            Ship_Method_Code = GetShippingCode(item.ShipmentServiceLevelCategory, item.ShippingAddress.CountryCode),
                            Ship_Option_Code = "100",
                            Gift_Message = null,
                            Cust_Order_Number = null,
                            Amz_Order_ID = item.AmazonOrderId,
                            Amz_Item_ID = null,
                            Item = item1,
                            TimeStamp = DateTime.Now,
                            Marketplace = "Amazon"
                        });
                    }
                }
                catch (Exception ex)
                {

                    continue;
                }

            }
            return orders;
        }
        private string GetStateCode(string state, string country)
        {
            if (country.ToUpper() == "CA") return state;
            state = state.Trim().Replace(".", "").Replace(",", "");
            if (state.Length == 2) return state.ToUpper();
            else
            {
                Dictionary<string, string> dict = new Dictionary<string, string>();

                dict["Alabama".ToUpper()] = "AL";
                dict["Alaska".ToUpper()] = "AK";
                dict["Arizona".ToUpper()] = "AZ";
                dict["Arkansas".ToUpper()] = "AR";
                dict["California".ToUpper()] = "CA";
                dict["Colorado".ToUpper()] = "CO";
                dict["Connecticut".ToUpper()] = "CT";
                dict["Delaware".ToUpper()] = "DE";
                dict["Florida".ToUpper()] = "FL";
                dict["Georgia".ToUpper()] = "GA";
                dict["Hawaii".ToUpper()] = "HI";
                dict["Idaho".ToUpper()] = "ID";
                dict["Illinois".ToUpper()] = "IL";
                dict["Indiana".ToUpper()] = "IN";
                dict["Iowa".ToUpper()] = "IA";
                dict["Kansas".ToUpper()] = "KS";
                dict["Kentucky".ToUpper()] = "KY";
                dict["Louisiana".ToUpper()] = "LA";
                dict["Maine".ToUpper()] = "ME";
                dict["Maryland".ToUpper()] = "MD";
                dict["Massachusetts".ToUpper()] = "MA";
                dict["Michigan".ToUpper()] = "MI";
                dict["Minnesota".ToUpper()] = "MN";
                dict["Mississippi".ToUpper()] = "MS";
                dict["Missouri".ToUpper()] = "MO";
                dict["Montana".ToUpper()] = "MT";
                dict["Nebraska".ToUpper()] = "NE";
                dict["Nevada".ToUpper()] = "NV";
                dict["New Hampshire".ToUpper()] = "NH";
                dict["New Jersey".ToUpper()] = "NJ";
                dict["New Mexico".ToUpper()] = "NM";
                dict["New York".ToUpper()] = "NY";
                dict["North Carolina".ToUpper()] = "NC";
                dict["North Dakota".ToUpper()] = "ND";
                dict["Ohio".ToUpper()] = "OH";
                dict["Oklahoma".ToUpper()] = "OK";
                dict["Oregon".ToUpper()] = "OR";
                dict["Pennsylvania".ToUpper()] = "PA";
                dict["Puerto Rico".ToUpper()] = "PR";
                dict["Rhode Island".ToUpper()] = "RI";
                dict["South Carolina".ToUpper()] = "SC";
                dict["South Dakota".ToUpper()] = "SD";
                dict["Tennessee".ToUpper()] = "TN";
                dict["Texas".ToUpper()] = "TX";
                dict["Utah".ToUpper()] = "UT";
                dict["Vermont".ToUpper()] = "VT";
                dict["Virginia".ToUpper()] = "VA";
                dict["Washington".ToUpper()] = "WA";
                dict["West Virginia".ToUpper()] = "WV";
                dict["Wisconsin".ToUpper()] = "WI";
                dict["Wyoming".ToUpper()] = "WY";

                var statekey = state.Trim().ToUpper();

                if (dict.ContainsKey(statekey)) return dict[statekey];
                else return "";
            }
        }
        private string GetShippingCode(string shipping_service, string country)
        {

            if (country.ToUpper() == "CA") return "1701";

            var amz_shipping_service = shipping_service.ToUpper().Trim();

            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict["Standard".ToUpper()] = "1102";
            dict["SecondDay".ToUpper()] = "220";
            dict["Expedited".ToUpper()] = "1100";

            if (dict.ContainsKey(amz_shipping_service)) return dict[amz_shipping_service];

            return null;
        }
    }
}
