using System;
using System.Collections.Generic;
using System.Text;

namespace StullerApiClient.Models
{
    [Serializable]
    public class GetProductsResponse
    { 
        public List<Product> Products { get; set; }
        public List<MetalMarket> MetalMarkets { get; set; }
        public double ExchangeRate { get; set; }
        public string NextPage { get; set; }
        public int PageSize { get; set; }
        public int TotalNumberOfProducts { get; set; }
    }

}
