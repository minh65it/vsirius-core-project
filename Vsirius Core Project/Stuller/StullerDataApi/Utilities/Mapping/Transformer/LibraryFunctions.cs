using StullerApiClient.Models;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Text;

namespace StullerDataApi.Utilities.Mapping
{
    public class LibraryFunctions
    {
        Stuller_Data_Maps DataMaps = new Stuller_Data_Maps();

        public string GetSearchKeyWordCommmon(string[] stringArr, string keyword)
        {
            string result = null;
            int bestMatch = 0;
            keyword = $" {keyword} ";
            if (stringArr == null)
                return null;
            foreach (var itemArr in stringArr)
            {
                var itemSplit = itemArr.Split(' ');
                var match = 0;
                foreach (var item in itemSplit)
                {
                    //$" {item} " để search theo từng từ
                    if (keyword.Contains($" {item} ", StringComparison.OrdinalIgnoreCase))
                        match += 1;
                }
                if (match > bestMatch)
                {
                    bestMatch = match;
                    result = itemArr;
                }
            }

            return result;
        }

        public string GetFeedProductType(string productType, List<DescriptiveElement> listElement)
        {
            /* choose one of four: bracelet  necklace  earring  charm(Pendants)  ring  cufflink  */

            string result = "";
            switch (productType.Trim())
            {
                case "Bracelet Slides":
                case "Bracelets":
                    result = "bracelet";
                    break;
                case "Pins":
                    result = "apparelpin";
                    break;
                case "Necklaces":
                    result = "necklace";
                    break;
                case "Earrings":
                case "Earring Components":
                case "Earring Jackets":
                    result = "earring";
                    break;
                case "Cuff Links":
                    result = "cufflink";
                    break;
                case "Rings":
                    result = "ring";
                    break;
                case "Charms":
                    result = "charm";
                    break;
            }
            foreach (var item in listElement)
            {
                if (item.Name.Contains("Jewelry State", StringComparison.OrdinalIgnoreCase) && item.DisplayValue == "Set")
                {
                    return "jewelryset";
                }
            }
            return result;
        }
        public string GetItemLengthNumeric(List<DescriptiveElement> listElement, string feed_product_type, bool isGetNumber)
        {
            string result = null;
            if (feed_product_type == "bracelet")
            {
                foreach (var item in listElement)
                {
                    if (item.Name.Contains("Length", StringComparison.OrdinalIgnoreCase))
                    {
                        if (isGetNumber)
                        {
                            result = item.DisplayValue.Split(" ")[0];
                            if (result.Contains("-"))
                            {
                                result = result.Split("-")[0];
                                break;
                            }
                            break;
                        }
                        else
                        {
                            result = "Inches";
                            break;
                        }
                    }
                }
                if (result == null)
                {
                    if (isGetNumber)
                    {
                        result = "0";
                    }
                    else
                    {
                        result = "Inches";
                    }
                }

            }
            return result;
        }
        public string GetUrlImage(List<ImageInfo> input, List<GroupImage> input2, int index)
        {
            if (input?.Count > index)
            {
                return input[index].ZoomUrl?.Replace(" ", "") ?? input[index].FullUrl?.Replace(" ", "");
            }
            if (input2?.Count > index)
            {
                return input2[index].ZoomUrl?.Replace(" ", "") ?? input2[index].FullUrl?.Replace(" ", "");
            }

            return null;
        }
        public string GetUrlImageStandard(List<ImageInfo> input, List<GroupImage> input2, int index)
        {
            if (input?.Count > index)
            {
                return input[index].ZoomUrl?.Replace(" ", "") ?? input[index].FullUrl?.Replace(" ", "");
            }
            if (input2?.Count > index)
            {
                return input2[index].ZoomUrl?.Replace(" ", "") ?? input2[index].FullUrl?.Replace(" ", "");
            }

            return null;
        }
        public string GetOnHand(double OnHand, string Status, bool Orderable)
        {
            if (OnHand >= 0 && Status != "Made To Order" && Orderable == true)
                return ((int)OnHand).ToString();
            return "0";
        }
        public string GetUnitOfSale(string UnitOfSale)
        {
            string result = "1";

            if (UnitOfSale != "Each" && UnitOfSale != "Pair" && UnitOfSale != "DWT")
            {
                result = UnitOfSale.Replace("P", "").Replace("H", "");
            }
            return result;
        }
        public string GetItemType(Product input, string feed_product_type)
        {
            string item_type = null;
            switch (feed_product_type)
            {
                case "bracelet":
                    if (input.ShortDescription.Contains(" charm ", StringComparison.OrdinalIgnoreCase) || input.GroupDescription.Contains("charm", StringComparison.OrdinalIgnoreCase))
                        item_type = "charm-bracelets";
                    else if (input.ShortDescription.Contains(" link ", StringComparison.OrdinalIgnoreCase) || input.GroupDescription.Contains("link", StringComparison.OrdinalIgnoreCase))
                        item_type = "link-bracelets";
                    else if (input.ShortDescription.Contains(" strand ", StringComparison.OrdinalIgnoreCase) || input.GroupDescription.Contains("strand", StringComparison.OrdinalIgnoreCase))
                        item_type = "strand-bracelets";
                    else if (input.ShortDescription.Contains(" tennis ", StringComparison.OrdinalIgnoreCase) || input.GroupDescription.Contains("tennis", StringComparison.OrdinalIgnoreCase))
                        item_type = "tennis-bracelets";
                    else if (input.ShortDescription.Contains(" wrap ", StringComparison.OrdinalIgnoreCase) || input.GroupDescription.Contains("wrap", StringComparison.OrdinalIgnoreCase))
                        item_type = "wrap-bracelets";
                    else if (input.ShortDescription.Contains(" cuff ", StringComparison.OrdinalIgnoreCase) || input.GroupDescription.Contains("cuff", StringComparison.OrdinalIgnoreCase))
                        item_type = "cuff-bracelets";
                    else
                        item_type = "bracelets";
                    break;
                case "apparelpin":
                    item_type = "tie-pins";
                    break;
                case "necklace":
                    if (input.ShortDescription.Contains(" chain ", StringComparison.OrdinalIgnoreCase) || input.GroupDescription.Contains("chain", StringComparison.OrdinalIgnoreCase))
                        item_type = "chain-necklaces";
                    else if (input.ShortDescription.Contains(" choker ", StringComparison.OrdinalIgnoreCase) || input.GroupDescription.Contains("choker", StringComparison.OrdinalIgnoreCase))
                        item_type = "choker-necklaces";
                    else if (input.ShortDescription.Contains(" pendant ", StringComparison.OrdinalIgnoreCase) || input.GroupDescription.Contains("pendant", StringComparison.OrdinalIgnoreCase))
                        item_type = "pendant-necklaces";
                    else if (input.ShortDescription.Contains(" y ", StringComparison.OrdinalIgnoreCase) || input.GroupDescription.Contains(" y ", StringComparison.OrdinalIgnoreCase))
                        item_type = "y-shaped-necklaces";
                    else
                        item_type = "necklaces";
                    break;
                case "earring":
                    if (input.ShortDescription.Contains(" stud ", StringComparison.OrdinalIgnoreCase) || input.GroupDescription.Contains("stud", StringComparison.OrdinalIgnoreCase))
                        item_type = "stud-earrings";
                    else if (input.ShortDescription.Contains(" ball ", StringComparison.OrdinalIgnoreCase) || input.GroupDescription.Contains("ball", StringComparison.OrdinalIgnoreCase))
                        item_type = "ball-earrings";
                    else if (input.ShortDescription.Contains(" hoop ", StringComparison.OrdinalIgnoreCase) || input.GroupDescription.Contains("hoop", StringComparison.OrdinalIgnoreCase))
                        item_type = "hoop-earrings";
                    else if (input.ShortDescription.Contains(" dangle ", StringComparison.OrdinalIgnoreCase) || input.GroupDescription.Contains("dangle", StringComparison.OrdinalIgnoreCase))
                        item_type = "dangle-earrings";
                    else
                        item_type = "earrings";
                    break;
                case "cufflink":
                    if (input.ShortDescription.Contains(" cuff ", StringComparison.OrdinalIgnoreCase) || input.GroupDescription.Contains("cuff", StringComparison.OrdinalIgnoreCase))
                        item_type = "cuff-links";
                    break;
                case "ring":
                    if (input.ShortDescription.Contains(" engagement ", StringComparison.OrdinalIgnoreCase) || input.GroupDescription.Contains("engagement", StringComparison.OrdinalIgnoreCase))
                        item_type = "engagement-rings";
                    else if (input.ShortDescription.Contains(" stackable ", StringComparison.OrdinalIgnoreCase) || input.GroupDescription.Contains("stackable", StringComparison.OrdinalIgnoreCase))
                        item_type = "stackable-rings";
                    else if (input.ShortDescription.Contains("statement", StringComparison.OrdinalIgnoreCase) || input.GroupDescription.Contains("statement", StringComparison.OrdinalIgnoreCase))
                        item_type = "statement-rings";
                    else
                        item_type = "rings";
                    break;
                case "charm":
                    if (input.ShortDescription.Contains(" necklaces ", StringComparison.OrdinalIgnoreCase) || input.GroupDescription.Contains("necklaces", StringComparison.OrdinalIgnoreCase))
                        item_type = "pendant-necklaces";
                    else
                        item_type = "pendants";
                    break;
                case "jewelryset":
                    item_type = "jewelry-sets";
                    break;
            }
            return item_type;
        }
        public string GetMetalType(List<DescriptiveElement> listElement, string feed_product_type, Dictionary<string, string> valid)
        {
            //var listString = DataMaps.BuildMetalTypeList(feed_product_type);
            var listString = valid.FirstOrDefault(x => x.Key.Contains("[ " + feed_product_type + " ]") && x.Key.Contains("Metal Type - ")).Value?.Split("|");
            foreach (var item in listElement)
            {
                if (item.Name.Contains("Quality", StringComparison.OrdinalIgnoreCase))
                {
                    var text = GetSearchKeyWordCommmon(listString, item.DisplayValue);
                    return text != null ? text : listString[0];
                }
            }
            return listString[0];
        }
        public string GetMaterialType(List<DescriptiveElement> listElement, string feed_product_type, Dictionary<string, string> valid)
        {
            //var listString = DataMaps.BuildMaterialTypeList(feed_product_type);
            var listString = valid.FirstOrDefault(x => x.Key.Contains("[ " + feed_product_type + " ]") && x.Key.Contains("Material Type - ")).Value?.Split("|");
            foreach (var item in listElement)
            {
                if (item.Name.Contains("Quality", StringComparison.OrdinalIgnoreCase))
                {
                    var text = GetSearchKeyWordCommmon(listString, item.DisplayValue);
                    return text != null ? text : listString[0];
                }
            }
            return listString[0];
        }
        public string GetGemType(List<DescriptiveElement> listElement, string feed_product_type)
        {
            var listString = DataMaps.BuildGemTypeList(feed_product_type);
            listString = new string[] { "No Gemstone", "Red Agate", "Pearl" };

            foreach (var item in listElement)
            {
                if (item.Name.Contains("Primary Stone Type", StringComparison.OrdinalIgnoreCase))
                {
                    var text = GetSearchKeyWordCommmon(listString, item.DisplayValue);
                    return text != null ? text : listString[0];
                }
            }
            return listString[0];
        }
        public string BuildProductDescription(Product input)
        {
            StringBuilder sb = new StringBuilder();
            if (input.Description != null)
                sb.Append(input.Description + "<br/>");
            if (input.ProductType != null)
            {
                sb.Append("<br/> Product Type: " + input.ProductType);
            }
            if (input.DescriptiveElementGroup?.DescriptiveElements.Count > 0)
            {
                foreach (var item in input.DescriptiveElementGroup.DescriptiveElements)
                {
                    sb.Append("<br/>" + item.Name + ": " + item.DisplayValue);
                }
            }
            sb.Append("<br/><br/>Customized items can not be returned<br/>");
            return sb.ToString();
        }
        public string GetColorMap(string description, Dictionary<string, string> valid)
        {
            //var listString = DataMaps.ColorMapList;
            var listString = valid.FirstOrDefault(x => x.Key.Contains("Color Map - ")).Value?.Split("|");
            for (int i = 0; i < listString.Length; i++)
            {
                if (description.Contains($" {listString[i]} ", StringComparison.OrdinalIgnoreCase))
                {
                    return listString[i];
                }
            }
            return "Multi";
        }
        public string GetColorName(string color_map)
        {
            if (!string.IsNullOrEmpty(color_map))
                return color_map;
            return "White";
        }
        public string BulletPoint(List<DescriptiveElement> input, int bullet_point)
        {
            string b1 = "";
            string b2 = "";
            string bullet_point2 = "Material quality is genuine fine-jewelry standard guarantee";
            if (input.Count > 0)
            {
                foreach (var item in input)
                {
                    if (!item.Name.Contains("Series"))
                    {
                        var s = " " + item.Name + ": " + item.DisplayValue;

                        if (b1.Length + s.Length < 250)
                            b1 += s;
                        else if (b2.Length + s.Length < 250)
                            b2 += s;
                    }
                }
            }
            if (!string.IsNullOrEmpty(b2) && b2.Length + bullet_point2.Length < 250)
                bullet_point2 = b2 + ". " + bullet_point2;
            if (bullet_point == 1)
                return b1;
            return bullet_point2;
        }
        public string GetItemTypeName(string item_type)
        {
            item_type = item_type.Replace("-", " ");
            TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
            if (item_type == "Statement Rings")
                return "Statement Ring";
            return textInfo.ToTitleCase(item_type);
        }
        public string GetCountryOfOrigin(string countryOfOrigin, string feed_product_type)
        {
            if (countryOfOrigin != null)
            {
                var result = DataMaps.CountryOfOriginDictionary.GetValueOrDefault(countryOfOrigin.Trim());

                var listString = DataMaps.BuildCountryOfOriginList(feed_product_type);
                return listString.Contains(result) ? result?.ToString() : listString[0];
            }
            return "Unknown";
        }
        public string GetRingSize(double ringSize, string feed_product_type, Dictionary<string, string> valid)
        {
            var listString = valid.FirstOrDefault(x => x.Key.Contains("[ " + feed_product_type + " ]") && x.Key.Contains("Ring Size - ")).Value?.Split("|");
            if (listString != null)
            {

                return listString.Contains($"{ringSize}") ? $"{ringSize}" : "Adjustable";
            }
            return "None";
        }
        public string GetStoneColor(List<DescriptiveElement> listElement, string feed_product_type, Dictionary<string, string> valid)
        {
            foreach (var item in listElement)
            {
                if (item.Name.Contains("color", StringComparison.OrdinalIgnoreCase))
                {
                    //var listString = DataMaps.BuildStoneColorList(feed_product_type);
                    var listString = valid.FirstOrDefault(x => x.Key.Contains("[ " + feed_product_type + " ]") && x.Key.Contains("Stone Color - ")).Value?.Split("|");
                    var text = GetSearchKeyWordCommmon(listString, item.DisplayValue);
                    return text != null ? text : listString[0];
                }
            }
            return null;
        }
        public string GetStyleName(string description, string feed_product_type, Dictionary<string, string> valid)
        {
            var listString = valid.FirstOrDefault(x => x.Key.Contains("[ " + feed_product_type + " ]") && x.Key.Contains("Style - ")).Value?.Split("|");
            string result = null;
            for (int i = 0; i < listString?.Length; i++)
            {
                if (description.Contains($" {listString[i]} ", StringComparison.OrdinalIgnoreCase))
                {
                    result = listString[i];
                }
            }
            if (listString != null)
            {
                if (result != null)
                    return result;
                if (feed_product_type != "earring")
                    return "Classic";
                return listString[0];
            }
            return "None";
        }
        public string GetClaspType(List<DescriptiveElement> input, string feed_product_type, Dictionary<string, string> valid)
        {
            foreach (var item in input)
            {
                if (item.Name.Contains("Clasp Type", StringComparison.OrdinalIgnoreCase))
                {
                    //var listString = DataMaps.BuildClaspTypeList(feed_product_type);
                    var listString = valid.FirstOrDefault(x => x.Key.Contains("[ " + feed_product_type + " ]") && x.Key.Contains("Clasp Type - ")).Value?.Split("|");
                    var text = GetSearchKeyWordCommmon(listString, item.DisplayValue);
                    return text;
                }

            }

            return null;

        }
        public string GetStonesCreationMethod(string ShortDescription, string feed_product_type)
        {
            if (feed_product_type == "ring")
            {
                string[] StonesCreationMethodListRing = new string[] { "Compressed", "Simulated", "Synthetic", "Natural" };
                return StonesCreationMethodListRing.FirstOrDefault(x => ShortDescription.Contains(x, StringComparison.OrdinalIgnoreCase));
            }
            return null;

        }
        public string GetStoneShape(string CenterStoneShape, string feed_product_type, Dictionary<string, string> valid)
        {
            if (feed_product_type == "jewelryset" || feed_product_type == "ring")
                return "";
            var listString = valid.FirstOrDefault(x => x.Key.Contains("[ " + feed_product_type + " ]") && x.Key.Contains("Stone Shape - ")).Value?.Split("|");
            if (listString == null)
                return null;
            string result = null;
            if (CenterStoneShape != null)
            {
                result = listString!.FirstOrDefault(x => CenterStoneShape.Contains(x, StringComparison.OrdinalIgnoreCase));
            }

            return result == null ? listString[0] : result;
        }
        public string GetStoneClarity(List<DescriptiveElement> listElement, string feed_product_type, Dictionary<string, string> valid)
        {
            foreach (var item in listElement)
            {
                if (item.Name.Contains("clarity", StringComparison.OrdinalIgnoreCase))
                {
                    //var listString = DataMaps.BuildStoneClarityList(feed_product_type);
                    var listString = valid.FirstOrDefault(x => x.Key.Contains("[ " + feed_product_type + " ]") && x.Key.Contains("Stone Clarity - ")).Value?.Split("|");
                    var text = GetSearchKeyWordCommmon(listString, item.DisplayValue.Replace(",", ""));
                    if (feed_product_type == "necklace" || feed_product_type == "earring" || feed_product_type == "ring")
                    {
                        if (text == null)
                        {
                            return null;
                        }
                        text = DataMaps.StoneClarityDictionary.GetValueOrDefault(text);
                    }
                    if (feed_product_type == "jewelryset")
                    {
                        if (text == null)
                        {
                            return null;
                        }
                        var check = DataMaps.StoneClarityDictionary.GetValueOrDefault(text);
                        if (check != null)
                        {
                            text = check;
                        }
                    }
                    return text;
                }
            }
            return null;
        }
        public string GetStonesClarity(string stone_clarity1, string feed_product_type)
        {
            if (feed_product_type == "jewelryset")
                return null;
            return stone_clarity1;
        }
        public string GetStonesType(List<DescriptiveElement> listElement)
        {
            foreach (var item in listElement)
            {
                if (item.Name.Contains("Stone Type", StringComparison.OrdinalIgnoreCase))
                {
                    return item.DisplayValue.Trim();
                }
            }
            return null;
        }
        public string GetStonesNumberOfStones(string Description, string GroupDescription, string feed_product_type)
        {
            if (feed_product_type == "jewelryset" || feed_product_type == "ring")
                return "";
            string[] stonesNumber = new string[] { "Side-Stone", "Two-Stone", "Three-Stone", "Four-Stone", "Five-Stone",
                "Six-Stone", "Seven-Stone", "Eight-Stone","Nine-Stone", "Multi-Stone"};
            foreach (var item in stonesNumber)
            {
                try
                {
                    if (Description.Contains(item, StringComparison.OrdinalIgnoreCase) || GroupDescription.Contains(item, StringComparison.OrdinalIgnoreCase))
                        return DataMaps.StonesNumberOfStonesDictionary.GetValueOrDefault(item);
                }
                catch (Exception)
                {
                    return null;
                }

            }
            return null;
        }
        public string GetSettingType(string Description, string GroupDescription, string feed_product_type, Dictionary<string, string> valid)
        {
            var listString = valid.FirstOrDefault(x => x.Key.Contains("[ " + feed_product_type + " ]") && x.Key.Contains("Setting Type - ")).Value?.Split("|");
            var input = Regex.Replace(GroupDescription + " " + Description, @"[^\w\s]", " ");
            var result = GetSearchKeyWordCommmon(listString, input);
            if (result == null)
            {
                if (feed_product_type == "ring" || feed_product_type == "cufflink"
                    || feed_product_type == "jewelryset" || feed_product_type == "charm")
                    return "Other Setting";
                return listString[0];
            }
            return result;
        }
        public string GetMetalsMetalStamp(List<DescriptiveElement> listElement)
        {
            string result = null;
            foreach (var item in listElement)
            {
                if (item.Name.Contains("Quality", StringComparison.OrdinalIgnoreCase))
                {
                    result = item.DisplayValue.Split(' ')[0].Trim();
                }
            }
            return result;
        }
        public string GetStonesShape(string stone_shape1)
        {
            return stone_shape1 != null ? $"{stone_shape1}" : null;
        }
        public string GetStandardPrice(Price price)
        {
            return price != null ? $"{Math.Round((decimal)(price.Value * 1.7))}" : "0";
        }
    }
}
