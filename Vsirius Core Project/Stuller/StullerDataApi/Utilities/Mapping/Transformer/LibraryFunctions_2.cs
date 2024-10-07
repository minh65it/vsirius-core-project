using StullerApiClient.Models;

namespace StullerDataApi.Utilities.Mapping
{
    public class LibraryFunctions_2
    {
        LibraryFunctions library = new LibraryFunctions();
        Stuller_Data_Maps_2 DataMaps_2 = new Stuller_Data_Maps_2();
        public string GetPearlType(string ShortDescription)
        {
            var listString = DataMaps_2.PearlTypeNecklaceList;
            var result = library.GetSearchKeyWordCommmon(listString, ShortDescription);
            return result;
        }
        public string GetPearlShape(string ShortDescription)
        {
            var listString = DataMaps_2.PearlShapeNecklaceList;
            var result = library.GetSearchKeyWordCommmon(listString, ShortDescription);
            return result;
        }
        public string GetNumberOfPearls(string Description, string GroupDescription, string CenterStoneShape)
        {
            if (CenterStoneShape == "Pearl")
            {
                string[] stonesNumber = new string[] { "Side-Stone", "Two-Stone", "Three-Stone", "Four-Stone", "Five-Stone",
                    "Six-Stone", "Seven-Stone", "Eight-Stone","Nine-Stone", "Multi-Stone"};
                foreach (var item in stonesNumber)
                {
                    if (Description.Contains(item, StringComparison.OrdinalIgnoreCase) || GroupDescription.Contains(item, StringComparison.OrdinalIgnoreCase))
                        return DataMaps_2.NumberOfPearlDictionary.GetValueOrDefault(item);
                }
            }
            return null;
        }
        public string GetValueListElementFirst(List<DescriptiveElement> listElement, string name)
        {
            string result = null;
            foreach (var item in listElement)
            {
                if (item.Name.Contains(name, StringComparison.OrdinalIgnoreCase))
                {
                    try
                    {
                        var data = item.DisplayValue.Split(' ')[0];
                        result = Double.Parse(data).ToString();
                    }
                    catch (Exception)
                    {
                        return null;
                    }

                }
            }
            return result;
        }
        public string GetValueLength(List<DescriptiveElement> listElement, string feed_product_type, List<Specification> specification)
        {
            string result = null;
            foreach (var item in listElement)
            {
                if (item.Name.Contains("Length", StringComparison.OrdinalIgnoreCase))
                {
                    try
                    {
                        var data = item.DisplayValue.Split(' ')[0];
                        if (data.Contains("-"))
                        {
                            data = data.Split("-")[0];
                        }
                        result = Double.Parse(data).ToString();
                        break;
                    }
                    catch (Exception)
                    {
                        result = null;
                    }

                }
            }
            if (result == null || result == "")
            {
                foreach (var item in listElement)
                {
                    if (item.Name.Contains("Width", StringComparison.OrdinalIgnoreCase))
                    {
                        try
                        {
                            var data = item.DisplayValue.Split(' ')[0];
                            if (data.Contains("-"))
                            {
                                data = data.Split("-")[0];
                            }
                            result = Double.Parse(data).ToString();
                            break;
                        }
                        catch (Exception)
                        {
                            result = null;
                        }

                    }
                }
                if(specification != null) {
                    foreach (var item in specification)
                    {
                        if (item.Name.Contains("Width", StringComparison.OrdinalIgnoreCase))
                        {
                            try
                            {
                                var data = item.Value.Split(' ')[0];
                                if (data.Contains("-"))
                                {
                                    data = data.Split("-")[0];
                                }
                                result = Double.Parse(data).ToString();
                                break;
                            }
                            catch (Exception)
                            {
                                result = null;
                            }

                        }
                    }
                }
            }
            if (feed_product_type == "bracelet" && result == null)
            {
                return "0";
            }
            return result ?? "5";
        }

        public string GetValueWidth(List<DescriptiveElement> listElement, List<Specification> specification)
        {
            string result = null;
            foreach (var item in listElement)
            {
                if (item.Name.Contains("Width", StringComparison.OrdinalIgnoreCase))
                {
                    try
                    {
                        var data = item.DisplayValue.Split(' ')[0];
                        result = Double.Parse(data).ToString();
                    }
                    catch (Exception)
                    {
                        result = null;
                    }

                }
            }
            if(result == null && specification != null)
            {
                foreach (var item in specification)
                {
                    if (item.Name.Contains("Width", StringComparison.OrdinalIgnoreCase))
                    {
                        try
                        {
                            var data = item.Value.Split(' ')[0];
                            result = Double.Parse(data).ToString();
                        }
                        catch (Exception)
                        {
                            result = null;
                        }

                    }
                }
            }
            return result ?? "5";
        }
        public string GetValueHeight(List<DescriptiveElement> listElement, List<Specification> specification)
        {
            string result = null;
            foreach (var item in listElement)
            {
                if (item.Name.Contains("Height", StringComparison.OrdinalIgnoreCase))
                {
                    try
                    {
                        var data = item.DisplayValue.Split(' ')[0];
                        result = Double.Parse(data).ToString();
                    }
                    catch (Exception)
                    {
                        result = null;
                    }

                }
            }
            if(result == null && specification != null)
            {
                foreach (var item in specification)
                {
                    if (item.Name.Contains("Height", StringComparison.OrdinalIgnoreCase))
                    {
                        try
                        {
                            var data = item.Value.Split(' ')[0];
                            result = Double.Parse(data).ToString();
                        }
                        catch (Exception)
                        {
                            result = null;
                        }

                    }
                }
            }
            return result ?? "5";
        }
        public string GetStoneWeight(List<DescriptiveElement> listElement, string name)
        {
            string result = null;
            foreach (var item in listElement)
            {
                if (item.Name.Contains(name, StringComparison.OrdinalIgnoreCase))
                {
                    try
                    {
                        result = item.DisplayValue.Split("::")[1]?.Trim().Split(" ")[0];

                        if (result.Contains("/"))
                        {
                            int numerator = int.Parse(result.Split("/")[0]);
                            int denominator = int.Parse(result.Split("/")[1]);
                            double result1 = (double)numerator / denominator;
                            result = Math.Round(result1, 2).ToString();
                        }
                    }
                    catch (Exception)
                    {

                        return null;
                    }
                }
            }
            return result;
        }
        public string GetSizePerPearl(List<DescriptiveElement> listElement, string name)
        {
            string result = null;
            foreach (var item in listElement)
            {
                if (item.Name.Contains(name, StringComparison.OrdinalIgnoreCase))
                {
                    result = item.DisplayValue;
                }
            }
            return result;
        }
        public string GetUnitCountType(string UnitOfSale)
        {
            string result = "Count";

            if (UnitOfSale != "Each" && UnitOfSale != "Pair" && UnitOfSale != "DWT")
            {
                result = "Pound";
            }
            return result;
        }
        public string GetValueListElementLast(List<DescriptiveElement> listElement, string name, string feed_product_type, Dictionary<string, string> valid)
        {
            var listString = valid.FirstOrDefault(x => x.Key.Contains("[ " + feed_product_type + " ]") && x.Key.Contains("Item Width Unit Of Measure")).Value?.Split("|");
            string result = null;
            foreach (var item in listElement)
            {
                if (item.Name.Contains(name, StringComparison.OrdinalIgnoreCase))
                {
                    var count = item.DisplayValue.Split(' ').Count();
                    result = item.DisplayValue.Split(' ')[count - 1];

                    var text = library.GetSearchKeyWordCommmon(listString, result);
                    return text;
                }
            }
            return null;
        }
        public string GetValueLengthUnit(List<DescriptiveElement> listElement, string feed_product_type, Dictionary<string, string> valid, List<Specification> specification)
        {
            try
            {
                var listString = valid.FirstOrDefault(x => x.Key.Contains("[ " + feed_product_type + " ]") && x.Key.Contains("Item Length Unit Of Measure")).Value?.Split("|");
                if (listString == null)
                {
                    return "";
                }
                string result = null;
                string text = null;
                foreach (var item in listElement)
                {
                    if (item.Name.Contains("Length", StringComparison.OrdinalIgnoreCase))
                    {
                        var count = item.DisplayValue.Split(' ').Count();
                        result = item.DisplayValue.Split(' ')[count - 1];

                        text = library.GetSearchKeyWordCommmon(listString, result);
                        if (text != null)
                            if (text.Length <= 2)
                            {
                                text = text.ToUpper();
                            }
                    }
                }
                if (text == null && specification != null)
                {
                    foreach (var item in specification)
                    {
                        if (item.Name.Contains("Width", StringComparison.OrdinalIgnoreCase))
                        {
                            var count = item.Value.Split(' ').Count();
                            result = item.Value.Split(' ')[count - 1];

                            text = library.GetSearchKeyWordCommmon(listString, result);
                            if (text != null)
                                if (text.Length <= 2)
                                {
                                    text = text.ToUpper();
                                }
                        }
                    }
                }
                return text == null ? "MM" : text;
            }
            catch (Exception)
            {

                return "MM";
            }
            
        }

        public string GetValueHeightUnit(List<DescriptiveElement> listElement, string feed_product_type, Dictionary<string, string> valid, List<Specification> specification)
        {
            try
            {
                var listString = valid.FirstOrDefault(x => x.Key.Contains("[ " + feed_product_type + " ]") && x.Key.Contains("Item Height Unit Of Measure")).Value?.Split("|");
                if (listString == null)
                {
                    return "";
                }
                string result = null;
                string text = null;
                foreach (var item in listElement)
                {
                    if (item.Name.Contains("Height", StringComparison.OrdinalIgnoreCase))
                    {
                        var count = item.DisplayValue.Split(' ').Count();
                        result = item.DisplayValue.Split(' ')[count - 1];

                        text = library.GetSearchKeyWordCommmon(listString, result);
                        if (text != null)
                            if (text.Length <= 2)
                            {
                                text = text.ToUpper();
                            }
                    }
                }
                if (text == null && specification != null)
                {
                    foreach (var item in specification)
                    {
                        if (item.Name.Contains("Height", StringComparison.OrdinalIgnoreCase))
                        {
                            var count = item.Value.Split(' ').Count();
                            result = item.Value.Split(' ')[count - 1];

                            text = library.GetSearchKeyWordCommmon(listString, result);
                            if (text != null)
                                if (text.Length <= 2)
                                {
                                    text = text.ToUpper();
                                }
                        }
                    }
                }
                return text == null ? "MM" : text;
            }
            catch (Exception)
            {

                return "MM";
            }
            
        }
        public string GetValueWidthUnit(List<DescriptiveElement> listElement, string feed_product_type, Dictionary<string, string> valid, List<Specification> specification)
        {
            try
            {
                var listString = valid.FirstOrDefault(x => x.Key.Contains("[ " + feed_product_type + " ]") && x.Key.Contains("Item Width Unit Of Measure")).Value?.Split("|");
                if (listString == null)
                {
                    return "";
                }
                string result = null;
                string text = null;
                foreach (var item in listElement)
                {
                    if (item.Name.Contains("Width", StringComparison.OrdinalIgnoreCase))
                    {
                        var count = item.DisplayValue.Split(' ').Count();
                        result = item.DisplayValue.Split(' ')[count - 1];

                        text = library.GetSearchKeyWordCommmon(listString, result);
                        if (text != null)
                            if (text.Length <= 2)
                            {
                                text = text.ToUpper();
                            }
                    }
                }
                if (text == null && specification != null)
                {
                    foreach (var item in specification)
                    {
                        if (item.Name.Contains("Width", StringComparison.OrdinalIgnoreCase))
                        {
                            var count = item.Value.Split(' ').Count();
                            result = item.Value.Split(' ')[count - 1];

                            text = library.GetSearchKeyWordCommmon(listString, result);
                            if (text != null)
                                if (text.Length <= 2)
                                {
                                    text = text.ToUpper();
                                }
                        }
                    }
                }
                return text == null ? "MM" : text;
            }
            catch (Exception)
            {

                return "MM";
            }
            
        }
        public string GetValueLengthPackageUnit(List<DescriptiveElement> listElement, string feed_product_type, Dictionary<string, string> valid)
        {
            var listString = valid.FirstOrDefault(x => x.Key.Contains("[ " + feed_product_type + " ]") && x.Key.Contains("Package Length Unit")).Value?.Split("|");
            if (listString == null)
            {
                return "";
            }
            string result = null;
            string text = null;
            foreach (var item in listElement)
            {
                if (item.Name.Contains("Length", StringComparison.OrdinalIgnoreCase))
                {
                    var count = item.DisplayValue.Split(' ').Count();
                    result = item.DisplayValue.Split(' ')[count - 1];

                    text = library.GetSearchKeyWordCommmon(listString, result);
                    if (text != null)
                        if (text.Length <= 2)
                        {
                            text = text.ToUpper();
                        }
                }
            }
            return text == null ? listString[0] : text;
        }

        public string GetValueHeightPackageUnit(List<DescriptiveElement> listElement, string feed_product_type, Dictionary<string, string> valid)
        {
            var listString = valid.FirstOrDefault(x => x.Key.Contains("[ " + feed_product_type + " ]") && x.Key.Contains("Package Height Unit - ")).Value?.Split("|");
            if (listString == null)
            {
                return "";
            }
            string result = null;
            string text = null;
            foreach (var item in listElement)
            {
                if (item.Name.Contains("Height", StringComparison.OrdinalIgnoreCase))
                {
                    var count = item.DisplayValue.Split(' ').Count();
                    result = item.DisplayValue.Split(' ')[count - 1];

                    text = library.GetSearchKeyWordCommmon(listString, result);
                    if (text != null)
                        if (text.Length <= 2)
                        {
                            text = text.ToUpper();
                        }
                }
            }
            return text == null ? listString[0] : text;
        }
        public string GetValueWidthPackageUnit(List<DescriptiveElement> listElement, string feed_product_type, Dictionary<string, string> valid)
        {
            var listString = valid.FirstOrDefault(x => x.Key.Contains("[ " + feed_product_type + " ]") && x.Key.Contains("Package Width Unit - ")).Value?.Split("|");
            if (listString == null)
            {
                return "";
            }
            string result = null;
            string text = null;
            foreach (var item in listElement)
            {
                if (item.Name.Contains("Width", StringComparison.OrdinalIgnoreCase))
                {
                    var count = item.DisplayValue.Split(' ').Count();
                    result = item.DisplayValue.Split(' ')[count - 1];

                    text = library.GetSearchKeyWordCommmon(listString, result);
                    if (text != null)
                        if (text.Length <= 2)
                        {
                            text = text.ToUpper();
                        }

                }
            }
            return text == null ? listString[0] : text;
        }

        public string GetWebsiteShippingWeight(string item_display_weight)
        {
            if (!string.IsNullOrWhiteSpace(item_display_weight))
            {
                return Math.Round((double.Parse(item_display_weight) + 28.3495231), 2).ToString();
            }
            return null;
        }
        public string GetBackFinding(List<DescriptiveElement> listElement, string feed_product_type, Dictionary<string, string> valid)
        {
            var listString = valid.FirstOrDefault(x => x.Key.Contains("[ " + feed_product_type + " ]") && x.Key.Contains("Back Finding - ")).Value?.Split("|");
            if (listString == null)
            {
                return "";
            }
            string result = null;
            foreach (var item in listElement)
            {
                if (item.Name.Contains("Back Finding", StringComparison.OrdinalIgnoreCase))
                {
                    var count = item.DisplayValue.Split(' ').Count();
                    result = item.DisplayValue.Split(' ')[count - 1];

                    var text = library.GetSearchKeyWordCommmon(listString, result);
                    return text == null ? "Fish Hook" : text;
                }
            }
            return "Fish Hook";
        }

        public string GetTheme(List<DescriptiveElement> listElement, string feed_product_type, Dictionary<string, string> valid)
        {
            var listString = valid.FirstOrDefault(x => x.Key.Contains("[ " + feed_product_type + " ]") && x.Key.Contains("theme - ")).Value?.Split("|");
            if (listString == null)
            {
                return "";
            }
            string result = null;
            foreach (var item in listElement)
            {
                if (item.Name.Contains("theme", StringComparison.OrdinalIgnoreCase))
                {
                    var count = item.DisplayValue.Split(' ').Count();
                    result = item.DisplayValue.Split(' ')[count - 1];

                    var text = library.GetSearchKeyWordCommmon(listString, result);
                    return text == null ? "Holidays" : text;
                }
            }
            return "Holidays";
        }
        public string GetAgeRange(List<DescriptiveElement> listElement, string feed_product_type, Dictionary<string, string> valid)
        {
            var listString = valid.FirstOrDefault(x => x.Key.Contains("[ " + feed_product_type + " ]") && x.Key.Contains("Age Range Description - ")).Value?.Split("|");
            if (listString == null)
            {
                return "";
            }
            string result = null;
            foreach (var item in listElement)
            {
                if (item.Name.Contains("Age Range Description", StringComparison.OrdinalIgnoreCase))
                {
                    var count = item.DisplayValue.Split(' ').Count();
                    result = item.DisplayValue.Split(' ')[count - 1];

                    var text = library.GetSearchKeyWordCommmon(listString, result);
                    return text == null ? "Adult" : text;
                }
            }
            return "Adult";
        }
        public decimal GetPrice(Price orgPrice)
        {
            if (orgPrice == null)
                return 1000;
            decimal margin = (decimal)1.1; // profit margin of all investment    
            if ((decimal)orgPrice.Value < 100)
            {
                margin = (decimal)1.3;
            }
            else if (300 >= (decimal)orgPrice.Value && (decimal)orgPrice.Value >= 200)
            {
                margin = (decimal)1.2;
            }
            decimal ship1 = 0, // ship from qgold to VS
                ship2 = 3, // ship from VS to customer 
                operation_cost_per_item = 0, // email, crm, software ...
                marketing_per_item = 0; // according to initial research, it cost $12.00 per sale in PPC  

            decimal total_fix_cost_per_sale = (decimal)orgPrice.Value + ship1 + ship2 + marketing_per_item + operation_cost_per_item;
            decimal price = total_fix_cost_per_sale * margin;
            decimal amazon_fee_percentage = (decimal).2;
            price = price / (1 - amazon_fee_percentage);

            return decimal.Round(price, 2);
        }
        public string GetChainType(List<DescriptiveElement> listElement, string feed_product_type, Dictionary<string, string> valid)
        {
            var listString = valid.FirstOrDefault(x => x.Key.Contains("[ " + feed_product_type + " ]") && x.Key.Contains("Chain Type - ")).Value?.Split("|");
            if (listString == null)
            {
                return "";
            }
            string result = null;
            string text = null;
            foreach (var item in listElement)
            {
                if (item.Name.Contains("Chain Type", StringComparison.OrdinalIgnoreCase))
                {
                    var count = item.DisplayValue.Split(' ').Count();
                    result = item.DisplayValue.Split(' ')[count - 1];

                    text = library.GetSearchKeyWordCommmon(listString, result);

                }
            }
            return text == null ? listString[0] : text;
        }
    }
}
