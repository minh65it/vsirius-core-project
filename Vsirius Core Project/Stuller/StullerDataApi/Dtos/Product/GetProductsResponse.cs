using System;
using System.Collections.Generic;
using System.Text;

namespace StullerDataApi.Dtos
{
    [Serializable]
    public class GetProductsResponseDto
    { 
        public List<ProductDto> Products { get; set; }
        public List<MetalMarketDto> MetalMarkets { get; set; }
        public double ExchangeRate { get; set; }
        public string NextPage { get; set; }
        public int PageSize { get; set; }
        public int TotalNumberOfProducts { get; set; }
    }

}
