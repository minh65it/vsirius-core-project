
using EtsyIntegrationDataAPI.Dtos.OrderEtsy;
using EtsyIntegrationDataAPI.Dtos.ProductEtsyDto;
using EtsyIntegrationDataAPI.Models;

namespace EtsyIntegrationDataAPI.Helpers.OrderEtsy
{
    public static class MapperOrder
    {
        public static List<QgoldFtpOrderObject> ConvertOrderQgold(ref List<Order> orderDB, List<ProductEtsyDto> productDB)
        {
            List<QgoldFtpOrderObject> orders = new List<QgoldFtpOrderObject>();
            if (orderDB == null || productDB == null) return orders;
            foreach (var item in orderDB)
            {
                try
                {
                    List<QgoldFtpOrderItem> item1 = new List<QgoldFtpOrderItem>();
                    foreach (var i in item.transactions)
                    {
                        var checkSKU = productDB.Where(x => x.listing_id == i.listing_id).FirstOrDefault();
                        if (checkSKU != null)
                        {
                            item1.Add(new QgoldFtpOrderItem()
                            {
                                Item = checkSKU.item_sku[2..],
                                Price = i.price_amount.ToString(),
                                Qty = i.quantity.ToString(),
                                //Size = null
                            });
                        }
                    }
                    if (item1.Count > 0)
                    {
                        orders.Add(new QgoldFtpOrderObject()
                        {
                            PO = null,
                            ShipToName = item.name,
                            ShipToAddress1 = item.first_line,
                            ShipToAddress2 = item.second_line,
                            ShipToAddress3 = null,
                            ShipToCity = item.city,
                            ShipToState_Province = GetStateCode(item.state, item.country_iso),
                            ShipToZip = item.zip,
                            ShipToCountryCode = item.country_iso,
                            ShipToPhone = null,
                            VSiriusItem = null,
                            Description = null,
                            Ship_Method_Code = GetShippingCode(item.transactions.FirstOrDefault()?.shipping_profile_id, item.country_iso),
                            Ship_Option_Code = "100",
                            Gift_Message = null,
                            Cust_Order_Number = null,
                            Amz_Order_ID = item.receipt_id.ToString(),
                            Amz_Item_ID = null,
                            Item = item1,
                            TimeStamp = DateTime.Now,
                            Marketplace = "Esty"
                        });
                        //cap nhat is_submit = true;
                        item.is_submit = true;
                    }
                }
                catch (Exception ex)
                {

                    continue;
                }

            }
            return orders;
        }
        private static string GetStateCode(string state, string country)
        {
            if (string.IsNullOrWhiteSpace(country) || string.IsNullOrWhiteSpace(state))
            {
                return string.Empty;
            }
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
        private static string GetShippingCode(long? shipping_profile_id, string country)
        {

            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict["Standard".ToUpper()] = "1102";
            dict["SecondDay".ToUpper()] = "220";
            dict["Expedited".ToUpper()] = "1100";


            return null;
        }

        //Stuller
        public static List<SubmitOrderStuller> ConvertOrderStuller(ref List<Order> orderDB, List<ProductEtsyDto> productDB)
        {
            List<SubmitOrderStuller> orders = new List<SubmitOrderStuller>();
            if (orderDB == null || productDB == null) return orders;
            foreach (var item in orderDB)
            {
                try
                {
                    List<ItemSubmit> item1 = new List<ItemSubmit>();
                    foreach (var i in item.transactions)
                    {
                        var checkSKU = productDB.Where(x => x.listing_id == i.listing_id).FirstOrDefault();
                        if (checkSKU != null)
                        {
                            item1.Add(new ItemSubmit()
                            {
                                Item = checkSKU.item_sku[2..],
                                Price = i.price_amount.ToString(),
                                Qty = i.quantity.ToString(),
                                //Size = null
                            });
                        }
                    }
                    if (item1.Count > 0)
                    {
                        orders.Add(new SubmitOrderStuller()
                        {
                            ShipToName = item.name,
                            ShipToAddress1 = item.first_line,
                            ShipToAddress2 = item.second_line,
                            ShipToAddress3 = "",
                            ShipToCity = item.city,
                            ShipToState = GetStateCode(item.state, item.country_iso),
                            ShipToZip = item.zip,
                            ShipToCountryCode = item.country_iso,
                            ShipToProvince = null,
                            ShipToPhone = null,
                            Item = item1,
                            Description = null,
                            ShipMethodCode = GetShippingCode(item.transactions.FirstOrDefault()?.shipping_profile_id, item.country_iso),
                            MarketplaceOrderId = item.receipt_id.ToString(),
                            Marketplace = "Etsy"
                        });
                        //cap nhat is_submit = true;
                        item.is_submit = true;
                    }
                }
                catch (Exception ex)
                {
                    continue;
                }
            }
            return orders;
        }
    }
}
