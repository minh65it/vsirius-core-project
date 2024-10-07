using System;

namespace StullerDataApi.Dtos
{
    [Serializable]
    public static class ProductFilterIndexDto
    {
        public static string OnPriceList = "0";
        public static string Finished = "1";
        public static string Orderable = "2";
        public static string None = "3";
        public static string InStock = "4";
        public static string BestSeller = "5";
    }

  


}
