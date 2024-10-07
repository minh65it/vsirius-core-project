using System;
using System.Collections.Generic;

namespace StullerDataApi.Dtos
{
    public class ProductsRequestPostFilterLookupDto
    {
        /// <summary>
        /// Excludes all products except those that are on a price list
        /// </summary>
        public const string OnPriceList = "0";

        /// <summary>
        /// Excludes all products except those that are flagged as finished goods (ready-to-wear)
        /// </summary>
        public const string Finished = "1";

        /// <summary>
        /// Excludes all products except those that are orderable
        /// </summary>
        public const string Orderable = "2";

        /// <summary>
        /// No filters are applied to the request
        /// </summary>
        public const string None = "3";

        /// <summary>
        /// Excludes all items that are not in stock.
        /// </summary>
        public const string InStock = "4";

        /// <summary>
        /// Excludes all items that are not flagged as best sellers.
        /// </summary>
        public const string BestSeller = "5";

    }

    [Serializable]

    public class ProductsRequestPostFilterDto
    {
        public List<string> Include { get; set; }
        public List<string> Filter { get; set; }
        public List<string> Series { get; set; }
        public List<string> SKU { get; set; }
        public List<int> ProductId { get; set; }
        public List<int> CategoryIds { get; set; }
        public List<AdvancedProductFilterDto> AdvancedProductFilters { get; set; }
        public string NextPage { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }

  


}
