using EtsyIntegrationDataAPI.Dtos.MapPIMToEtsyDB;

namespace EtsyIntegrationDataAPI.Helpers.MapPIMToEtsyDB
{
    public static class ConvertPIMToEtsyDB
    {
        public static int TaxonomyId(string feed_product_type)
        {
            switch (feed_product_type)
            {
                case "bracelet":
                    return 1193;
                case "keychain":
                    return 164;
                case "necklace":
                    return 1217;
                case "earring":
                    return 1203;
                case "watches":
                    return 1249;
                case "ring":
                    return 1231;
                case "charm":
                    return 1197;
                default:
                    return 1179;
            }
        }
        public static long ShippingProfileId(string title)
        {
            return 163193277485;
        }
        public static string GenTitle(string title)
        {
            string[] words = title.Split(' ');
            return string.Join(" ", words, 0, words.Length - 1);
        }
        public static string GenDescription(ProductPIMDto dto)
        {
            return $"{dto.item_name} \n {dto.bullet_point1} \n {dto.bullet_point2} \n {dto.bullet_point3} \n {dto.bullet_point4} \n {dto.bullet_point5}";

        }
        public static long ReturnPolicyId(string title)
        {
            return 1266753980904;
        }
        public static float GetItemWidth(string? item_width)
        {
            if(string.IsNullOrWhiteSpace(item_width))
            {
                return 5;
            }
            return float.Parse(item_width);
        }
        public static float GetItemLength(string? item_length, string? item_width)
        {
            if (item_length == null && item_width != null)
            {
                return float.Parse(item_width);
            }
            else if(item_length != null && item_width != null)
            {
                float length = float.Parse(item_length);
                float width = float.Parse(item_width);


                if (length >= width)
                {
                    return length;
                }
                else
                {
                    return width;
                }
            }
            return 5;
        }
        public static float GetItemHeight(string? item_height, string? item_width)
        {
            if (item_height == null && item_width != null)
            {
                return float.Parse(item_width);
            }
            else if (item_height != null && item_width != null)
            {
                float height = float.Parse(item_height);
                float width = float.Parse(item_width);


                if (width >= height)
                {
                    return height;
                }
                else
                {
                    return width;
                }
            }
            return 5;
        }
        public static float ItemWeigh(string item_weight)
        {
            if(!string.IsNullOrWhiteSpace(item_weight) && item_weight != "0")
            {
                var tmp =  float.Parse(item_weight);
                return tmp >= 1 ? tmp : 1;
            }
            else
            {
                //Nếu bằng 0 hoặc null sẽ gắn bằng 0.22 oz
                return 0.22f;
            }
        }
        public static string ItemWeightUnit(string item_weight_unit_of_measure, string item_weight)
        {
            if (!string.IsNullOrWhiteSpace(item_weight) && item_weight != "0")
            {
                var lstWeight = new List<KeyValuePair<string, string>>();
                lstWeight.Add(new KeyValuePair<string, string>("gr", "g"));
                lstWeight.Add(new KeyValuePair<string, string>("kg", "kg"));
                lstWeight.Add(new KeyValuePair<string, string>("lb", "lb"));
                lstWeight.Add(new KeyValuePair<string, string>("oz", "oz"));
                return lstWeight.FirstOrDefault(x => x.Key == item_weight_unit_of_measure?.ToLower()).Value != null ?
                    lstWeight.FirstOrDefault(x => x.Key == item_weight_unit_of_measure?.ToLower()).Value : "oz";
            }
            else
            {
                //Nếu bằng 0 hoặc null sẽ gắn bằng 0.22 oz
                return "oz";
            }
            
        }
        public static string ItemDimensionsUnit(string item_width_unit_of_measure)
        {
            var lstWeight = new List<KeyValuePair<string, string>>();
            lstWeight.Add(new KeyValuePair<string, string>("inches", "inches"));
            lstWeight.Add(new KeyValuePair<string, string>("yd", "yd"));
            lstWeight.Add(new KeyValuePair<string, string>("m", "m"));
            lstWeight.Add(new KeyValuePair<string, string>("cm", "cm"));
            lstWeight.Add(new KeyValuePair<string, string>("mm", "mm"));
            lstWeight.Add(new KeyValuePair<string, string>("ft", "ft"));
            lstWeight.Add(new KeyValuePair<string, string>("in", "in"));
            return lstWeight.FirstOrDefault(x => x.Key == item_width_unit_of_measure?.ToLower()).Value != null ?
                lstWeight.FirstOrDefault(x => x.Key == item_width_unit_of_measure?.ToLower()).Value : "mm";
        }
    }
}
