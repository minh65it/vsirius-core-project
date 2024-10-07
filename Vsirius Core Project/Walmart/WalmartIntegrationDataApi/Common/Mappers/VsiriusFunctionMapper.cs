using System.Text.RegularExpressions;
using WalmartIntegrationDataApi.Common.Extensions;
using WalmartIntegrationDataApi.Dtos.Vsirius;

namespace WalmartIntegrationDataApi.Common.Mappers
{
    public static class VsiriusFunctionMapper
    {
        public static string GetProductIdType(string? external_product_id_type, bool isExternal)
        {

            if (isExternal)
            {
                return "";
                //return external_product_id_type.ToUpper().Equals("ASIN") ? external_product_id_type : "GTIN";
            }
            else
            {
                if (string.IsNullOrEmpty(external_product_id_type)) return "GTIN";
                return
                DataMaps.ProductIdTypeList.Contains(external_product_id_type.ToUpper())
                && !external_product_id_type.Equals("ASIN")
                ? external_product_id_type : "GTIN";
            }
        }

        //review sau
        public static string GetProductId(string? external_product_id, string? external_product_id_type)
        {
            external_product_id ??= string.Empty;
            return external_product_id_type switch
            {
                //mac dinh
                "UPC" => external_product_id.PadLeft(12, '0'),
                "GTIN" => external_product_id.PadLeft(14, '0'),
                "ISBN" => external_product_id.Length < 10 && external_product_id.Length > 0 ? external_product_id.PadLeft(10, '0') : external_product_id,
                "EAN" => external_product_id.PadLeft(13, '0'),
                "ASIN" => string.Empty,
                _ => "CUSTOM",
            };
        }

        public static string? GetExternalProductId(string? item_sku, string? external_product_id_type)
        {
            if (string.IsNullOrEmpty(external_product_id_type))
                return string.Empty;
            return external_product_id_type.Contains("ASIN") ? item_sku : string.Empty;
        }

        public static string GetProductSecondaryImageURL(VsiriusProductDto src)
        {
            string productSecondaryImageURL = string.Empty;

            for (int i = 1; i <= 8; i++)
            {
                string? imageUrl = (string?)typeof(VsiriusProductDto).GetProperty("other_image_url" + i)?.GetValue(src);

                if (!string.IsNullOrWhiteSpace(imageUrl))
                {
                    productSecondaryImageURL += $"{imageUrl}|";
                }
            }

            return productSecondaryImageURL.TrimEnd('|');
        }

        public static string GetKeyFeatures(VsiriusProductDto src)
        {
            string keyFeatures = string.Empty;

            for (int i = 1; i <= 10; i++)
            {
                string? bulletPoint = (string?)typeof(VsiriusProductDto).GetProperty("bullet_point" + i)?.GetValue(src);

                if (!string.IsNullOrWhiteSpace(bulletPoint))
                {
                    keyFeatures += $"{bulletPoint}|";
                }
            }

            return keyFeatures.TrimEnd('|');
        }

        public static string GetProp65WarningText(VsiriusProductDto src)
        {
            string text = string.Empty;

            for (int i = 1; i <= 5; i++)
            {
                string? alifornia_proposition_65_chemical_names =
                    (string?)typeof(VsiriusProductDto).GetProperty("alifornia_proposition_65_chemical_names" + i)?.GetValue(src);

                if (!string.IsNullOrWhiteSpace(alifornia_proposition_65_chemical_names))
                {
                    text += $"{alifornia_proposition_65_chemical_names}|";
                }
            }

            return text.TrimEnd('|');
        }

        public static string GetShippingWeight(VsiriusProductDto src)
        {
            string value = "0.1";

            var key = src.website_shipping_weight_unit_of_measure;

            if (string.IsNullOrWhiteSpace(key))
            {
                return value;
            }
            else
            {
                try
                {
                    var shipping_weight =
                        Math.Round(double.Parse(src.website_shipping_weight)
                        * DataMaps.ShippingWeightDictionary.GetValueOrDefault(key)
                        , 2, MidpointRounding.ToPositiveInfinity);
                    value = shipping_weight.ToString();
                }
                catch
                {
                }
            }

            return value;
        }

        public static string? GetSmallPartsWarnings(string? cpsia_cautionary_statement1)
        {
            if (cpsia_cautionary_statement1 == null)
                return null;
            return DataMaps.SmallPartsWarningsDictionary!.GetValueOrDefault(cpsia_cautionary_statement1);
        }

        public static string? GetAgeGroup(string? department_name)
        {
            if (department_name == null) return null;
            return DataMaps.AgeGroupDictionary!.GetValueOrDefault(department_name);
        }

        public static string GetColorCategory(string? color)
        {
            if (string.IsNullOrWhiteSpace(color)) { return string.Empty; }
            var result = string.Empty;
            color = color.ToLower().ToSimpleString();

            foreach (var item in DataMaps.ColorCategoryList)
            {
                var itemNew = item.ToLower().ToSimpleString();
                if (itemNew == color)
                {
                    result = item;
                    return result;
                }
            }

            foreach (var item in DataMaps.ColorCategoryList)
            {
                var itemNew = item.ToLower().ToSimpleString();
                if (color.Contains(itemNew)
                    || itemNew.Contains(color))
                {
                    result = item; break;
                }
            }

            if (string.IsNullOrEmpty(result))
            {
                return "";
            }
            return result;
        }

        public static string GetChainType(string? chainType)
        {
            if (string.IsNullOrWhiteSpace(chainType)) { return string.Empty; }
            var result = string.Empty;
            chainType = chainType.ToLower().ToSimpleString();

            foreach (var item in DataMaps.ChainTypeList)
            {
                var itemNew = item.ToLower().ToSimpleString();
                if (itemNew == chainType)
                {
                    result = item; return result;
                }
            }

            foreach (var item in DataMaps.ChainTypeList)
            {
                var itemNew = item.ToLower().ToSimpleString();
                if (chainType.Contains(itemNew)
                    || itemNew.Contains(chainType))
                {
                    result = item; break;
                }
            }

            if (string.IsNullOrEmpty(result))
            {
                return "Other";
            }
            return result;
        }

        public static string GetJewelrySetting(string? settings)
        {
            if (string.IsNullOrWhiteSpace(settings)) { return string.Empty; }
            var result = string.Empty;
            settings = settings.ToLower().ToSimpleString();

            foreach (var item in DataMaps.JewelrySettingList)
            {
                var itemNew = item.ToLower().ToSimpleString();
                if (itemNew == settings)
                {
                    result = item; return result;
                }
            }

            foreach (var item in DataMaps.JewelrySettingList)
            {
                var itemNew = item.ToLower().ToSimpleString();
                if (settings.Contains(itemNew)
                    || itemNew.Contains(settings))
                {
                    result = item; break;
                }
            }

            if (string.IsNullOrEmpty(result))
            {
                return "other";
            }
            return result;
        }

        public static string GetGemStone(string? gem)
        {
            if (string.IsNullOrWhiteSpace(gem)) { return string.Empty; }
            var result = string.Empty;
            gem = gem.ToLower().ToSimpleString();

            foreach (var item in DataMaps.GemTypeList)
            {
                var itemNew = item.ToLower().ToSimpleString();
                if (itemNew == gem)
                {
                    result = item; return result;
                }
            }

            foreach (var item in DataMaps.GemTypeList)
            {
                var itemNew = item.ToLower().ToSimpleString();
                if (gem.Contains(itemNew)
                    || itemNew.Contains(gem))
                {
                    result = item;
                    break;
                }
            }

            if (string.IsNullOrEmpty(result))
            {
                return "Other Stone";
            }
            return result;
        }

        public static string GetDiamondClarity(string? stone_clarity)
        {
            if (string.IsNullOrWhiteSpace(stone_clarity)) { return string.Empty; }
            var result = string.Empty;
            stone_clarity = stone_clarity.ToLower().ToSimpleString();

            foreach (var item in DataMaps.StoneClarityList)
            {
                var itemNew = item.ToLower().ToSimpleString();
                if (itemNew == stone_clarity)
                {
                    result = item; return result;
                }
            }

            foreach (var item in DataMaps.StoneClarityList)
            {
                var itemNew = item.ToLower().ToSimpleString();
                if (stone_clarity.Contains(itemNew)
                    || itemNew.Contains(stone_clarity))
                {
                    result = item; break;
                }
            }

            if (string.IsNullOrEmpty(result))
            {
                return "Other";
            }
            return result;
        }

        public static string GetDiamondColor(string? stone_color)
        {
            if (string.IsNullOrWhiteSpace(stone_color)) { return string.Empty; }
            var result = string.Empty;
            stone_color = stone_color.ToLower().ToSimpleString();

            foreach (var item in DataMaps.StoneColorList)
            {
                var itemNew = item.ToLower().ToSimpleString();
                if (itemNew == stone_color)
                {
                    result = item; return result;
                }
            }

            foreach (var item in DataMaps.StoneColorList)
            {
                var itemNew = item.ToLower().ToSimpleString();
                if (stone_color.Contains(itemNew)
                    || itemNew.Contains(stone_color))
                {
                    result = item; break;
                }
            }

            if (string.IsNullOrEmpty(result))
            {
                return "Other";
            }
            return result;
        }

        public static string GetDiamondCut(string? stone_cut)
        {
            if (string.IsNullOrWhiteSpace(stone_cut)) { return string.Empty; }
            var result = string.Empty;
            stone_cut = stone_cut.ToLower().ToSimpleString();

            foreach (var item in DataMaps.StoneCutList)
            {
                var itemNew = item.ToLower().ToSimpleString();
                if (itemNew == stone_cut)
                {
                    result = item; return result;
                }
            }

            foreach (var item in DataMaps.StoneCutList)
            {
                var itemNew = item.ToLower().ToSimpleString();
                if (stone_cut.Contains(itemNew)
                    || itemNew.Contains(stone_cut))
                {
                    result = item; break;
                }
            }

            if (string.IsNullOrEmpty(result))
            {
                return "Other cut";
            }
            return result;
        }

        public static string GetGemstoneShape(string? stone_shape)
        {
            if (string.IsNullOrWhiteSpace(stone_shape)) { return string.Empty; }
            var result = string.Empty;
            stone_shape = stone_shape.ToLower().ToSimpleString();

            foreach (var item in DataMaps.GemstoneShapeList)
            {
                var itemNew = item.ToLower().ToSimpleString();
                if (itemNew == stone_shape)
                {
                    result = item; return result;
                }
            }

            foreach (var item in DataMaps.GemstoneShapeList)
            {
                var itemNew = item.ToLower().ToSimpleString();
                if (stone_shape.Contains(itemNew)
                    || itemNew.Contains(stone_shape))
                {
                    result = item; break;
                }
            }

            return result;
        }

        public static string GetClaspType(string? clasp_type)
        {
            if (string.IsNullOrWhiteSpace(clasp_type)) { return string.Empty; }
            var result = string.Empty;
            clasp_type = clasp_type.ToLower().ToSimpleString();

            foreach (var item in DataMaps.ClaspList)
            {
                var itemNew = item.ToLower().ToSimpleString();
                if (itemNew == clasp_type)
                {
                    result = item; return result;
                }
            }

            foreach (var item in DataMaps.ClaspList)
            {
                var itemNew = item.ToLower().ToSimpleString();
                if (clasp_type.Contains(itemNew)
                    || itemNew.Contains(clasp_type))
                {
                    result = item; break;
                }
            }
            if (string.IsNullOrEmpty(result))
            {
                return "Other";
            }
            return result;
        }

        public static string GetBackFinding(string? back_finding)
        {
            if (string.IsNullOrWhiteSpace(back_finding)) { return string.Empty; }
            var result = string.Empty;
            back_finding = back_finding.ToLower().ToSimpleString();

            foreach (var item in DataMaps.BackFindingList)
            {
                var itemNew = item.ToLower().ToSimpleString();
                if (itemNew == back_finding)
                {
                    result = item; return result;
                }
            }

            foreach (var item in DataMaps.BackFindingList)
            {
                var itemNew = item.ToLower().ToSimpleString();
                if (back_finding.Contains(itemNew)
                    || itemNew.Contains(back_finding))
                {
                    result = item;
                    break;
                }
            }

            if (string.IsNullOrEmpty(result))
            {
                return "Other";
            }
            return result;
        }

        public static string GetEarringStyle(string? earring_style)
        {
            if (string.IsNullOrWhiteSpace(earring_style)) { return string.Empty; }
            var result = string.Empty;
            earring_style = earring_style.ToLower().ToSimpleString();

            foreach (var item in DataMaps.EarringTypeList)
            {
                var itemNew = item.ToLower().ToSimpleString();
                if (itemNew == earring_style)
                {
                    result = item; return result;
                }
            }

            foreach (var item in DataMaps.EarringTypeList)
            {
                var itemNew = item.ToLower().ToSimpleString();
                if (earring_style.Contains(itemNew)
                    || itemNew.Contains(earring_style))
                {
                    result = item; break;
                }
            }
            return result;
        }

        public static string GetMetalType(string? metalType)
        {
            if (string.IsNullOrWhiteSpace(metalType)) { return string.Empty; }
            var result = string.Empty;
            metalType = metalType.ToLower().ToSimpleString();

            foreach (var item in DataMaps.MetalTypeList)
            {
                var itemNew = item.ToLower().ToSimpleString();
                if (itemNew == metalType)
                {
                    result = item; return result;
                }
            }

            foreach (var item in DataMaps.MetalTypeList)
            {
                var itemNew = item.ToLower().ToSimpleString();
                if (metalType.Contains(itemNew)
                    || itemNew.Contains(metalType))
                {
                    result = item; break;
                }
            }

            if (string.IsNullOrEmpty(result))
            {
                return "Other Metal";
            }

            return result;
        }

        public static string GetMaterialType(VsiriusProductDto src)
        {
            string materials = string.Empty;
            if (!string.IsNullOrWhiteSpace(src.material_type1))
            {
                materials += src.material_type1 + ";";
            }
            if (!string.IsNullOrWhiteSpace(src.material_type2))
            {
                materials += src.material_type2 + ";";
            }
            if (!string.IsNullOrWhiteSpace(src.material_type3))
            {
                materials += src.material_type3;
            }
            return materials;
        }

        public static string GetBraceletStyle(string? item_type)
        {
            if (string.IsNullOrWhiteSpace(item_type)) { return string.Empty; }
            var result = string.Empty;
            item_type = item_type.ToLower().ToSimpleString();

            foreach (var item in DataMaps.BraceletList)
            {
                var itemNew = item.ToLower().ToSimpleString();
                if (itemNew == item_type)
                {
                    result = item; return result;
                }
            }

            foreach (var item in DataMaps.BraceletList)
            {
                var itemNew = item.ToLower().ToSimpleString();
                if (item_type.Contains(itemNew)
                    || itemNew.Contains(item_type))
                {
                    result = item; break;
                }
            }

            if (string.IsNullOrEmpty(result))
            {
                return "Other";
            }

            return result;
        }

        public static string GetNecklaceStyle(string? item_type)
        {
            if (string.IsNullOrWhiteSpace(item_type)) { return string.Empty; }
            var result = string.Empty;
            item_type = item_type.ToLower().ToSimpleString();

            foreach (var item in DataMaps.NecklaceList)
            {
                var itemNew = item.ToLower().ToSimpleString();
                if (itemNew == item_type)
                {
                    result = item; return result;
                }
            }

            foreach (var item in DataMaps.NecklaceList)
            {
                var itemNew = item.ToLower().ToSimpleString();
                if (item_type.Contains(itemNew)
                    || itemNew.Contains(item_type))
                {
                    result = item; break;
                }
            }
            if (string.IsNullOrEmpty(result))
            {
                return "Other";
            }
            return result;
        }

        public static string? GetOccasion(VsiriusProductDto src)
        {
            string occasions = string.Empty;
            if (!string.IsNullOrWhiteSpace(src.occasion_type1))
            {
                occasions += src.occasion_type1 + ";";
            }
            if (!string.IsNullOrWhiteSpace(src.occasion_type2))
            {
                occasions += src.occasion_type2 + ";";
            }
            if (!string.IsNullOrWhiteSpace(src.occasion_type3))
            {
                occasions += src.occasion_type3 + ";";
            }
            if (!string.IsNullOrWhiteSpace(src.occasion_type4))
            {
                occasions += src.occasion_type4 + ";";
            }
            if (!string.IsNullOrWhiteSpace(src.occasion_type5))
            {
                occasions += src.occasion_type5;
            }

            return occasions;
        }

        public static string GetDiamondWeightTotal(VsiriusProductDto src)
        {
            switch (src.total_diamond_weight_unit_of_measure)
            {
                case "CARATS":
                    return src.total_diamond_weight;
                case "CTTW":
                    try
                    {
                        return Math.Round(double.Parse(src.total_diamond_weight) * 0.1286, 2, MidpointRounding.ToPositiveInfinity).ToString();
                    }
                    catch
                    {
                        return string.Empty;
                    }

                case "DWT":
                    return src.total_diamond_weight;
                default:
                    return string.Empty;
            }
        }

        public static string? GetSeasonCode(string? collection_name)
        {
            if (collection_name == null) return null;
            return DataMaps.SeasonDictionary.GetValueOrDefault(collection_name);
        }

        public static string? GetMeasureChain(VsiriusProductDto src)
        {
            try
            {
                return src.chain_length_unit switch
                {
                    "FT" => Math.Round(double.Parse(src.chain_length_derived) * 12, 2, MidpointRounding.ToPositiveInfinity).ToString(),
                    "CM" => Math.Round(double.Parse(src.chain_length_derived) * 0.393700787, 2, MidpointRounding.ToPositiveInfinity).ToString(),
                    "MM" => Math.Round(double.Parse(src.chain_length_derived) * 0.0393700787, 2, MidpointRounding.ToPositiveInfinity).ToString(),
                    "DM" => Math.Round(double.Parse(src.chain_length_derived) * 3.93700787, 2, MidpointRounding.ToPositiveInfinity).ToString(),
                    "Picometer" => Math.Round(double.Parse(src.chain_length_derived) * 3.93700787 * (10 ^ -11), 2, MidpointRounding.ToPositiveInfinity).ToString(),
                    "µM" => Math.Round(double.Parse(src.chain_length_derived) * 3.93700787 * (10 ^ -5), 2, MidpointRounding.ToPositiveInfinity).ToString(),
                    "M" => Math.Round(double.Parse(src.chain_length_derived) * 39.3701, 2, MidpointRounding.ToPositiveInfinity).ToString(),
                    "Yards" => Math.Round(double.Parse(src.chain_length_derived) * 36, 2, MidpointRounding.ToPositiveInfinity).ToString(),
                    "CTTW" => string.Empty,
                    "Miles" => Math.Round(double.Parse(src.chain_length_derived) * 63.360, 2, MidpointRounding.ToPositiveInfinity).ToString(),
                    "Mils" => Math.Round(double.Parse(src.chain_length_derived) / 1000, 2, MidpointRounding.ToPositiveInfinity).ToString(),
                    "IN" => src.chain_length_derived,
                    "Nanometer" => Math.Round(double.Parse(src.chain_length_derived) * 3.93700787 * (10 ^ -8), 2, MidpointRounding.ToPositiveInfinity).ToString(),
                    "Hundredths-Inches" => Math.Round(double.Parse(src.chain_length_derived) / 100, 2, MidpointRounding.ToPositiveInfinity).ToString(),
                    "Kilometers" => Math.Round(double.Parse(src.chain_length_derived) * 39370.1, 2, MidpointRounding.ToPositiveInfinity).ToString(),
                    "Angstrom" => Math.Round(double.Parse(src.chain_length_derived) * (10 ^ -8), 2, MidpointRounding.ToPositiveInfinity).ToString(),
                    _ => string.Empty,
                };
            }
            catch
            {
                return string.Empty;
            }

        }

        public static string? GetUnitDiameter(VsiriusProductDto vsirius)
        {
            if (string.IsNullOrWhiteSpace(vsirius.item_display_diameter))
            {
                return string.Empty;
            }
            else
            {
                if (DataMaps.DiameterUnitOfMeasureList.Contains(vsirius.item_display_diameter_unit_of_measure.ToLower()))
                {
                    return vsirius.item_display_diameter_unit_of_measure.ToLower();
                }
                else
                {
                    return vsirius.item_display_diameter_unit_of_measure switch
                    {
                        "DM" or "M" => "cm",
                        _ => string.Empty,
                    };
                }
            }
        }

        public static string? GetMeasureDiameter(VsiriusProductDto vsirius)
        {
            if (string.IsNullOrWhiteSpace(vsirius.item_display_diameter_unit_of_measure)) return string.Empty;
            if (!DataMaps.DiameterUnitOfMeasureList.Contains(vsirius.item_display_diameter_unit_of_measure.ToLower()))
            {
                return null;
            }
            else
            {
                try
                {
                    return vsirius.item_display_diameter_unit_of_measure.ToUpper() switch
                    {
                        "IN" or "FT" or "CM" or "MM" => vsirius.item_display_diameter,
                        "DM" => Math.Round(double.Parse(vsirius.item_display_diameter) * 10, 2, MidpointRounding.ToPositiveInfinity).ToString(),
                        "M" => Math.Round(double.Parse(vsirius.item_display_diameter) * 100, 2, MidpointRounding.ToPositiveInfinity).ToString(),
                        _ => string.Empty,
                    };
                }
                catch
                {
                    return string.Empty;
                }
            }
        }

        public static string? GetMeasureSize(string size_per_pearl)
        {
            if (!string.IsNullOrWhiteSpace(size_per_pearl))
            {
                string pattern = @"-?\d+(\.\d+)?";

                Match match = Regex.Match(size_per_pearl, pattern);

                return match.Value;
            }

            return string.Empty;
        }

        public static string GetEarringLengthUnit(VsiriusProductDto vsirius)
        {
            if (string.IsNullOrWhiteSpace(vsirius.item_display_length_unit_of_measure))
                return string.Empty;
            if (vsirius.item_display_length_unit_of_measure.ToLower().Equals("mm")
                || vsirius.item_display_length_unit_of_measure.ToLower().Equals("in"))
            {
                return vsirius.item_display_length_unit_of_measure.ToLower();
            }
            else
            {
                return "in";
            }
        }

        public static string GetEarringLength(VsiriusProductDto src)
        {
            try
            {
                return src.item_length_unit_of_measure switch
                {
                    "IN" => src.item_length,
                    "FT" => Math.Round(double.Parse(src.item_length) * 12, 2, MidpointRounding.ToPositiveInfinity).ToString(),
                    "CM" => Math.Round(double.Parse(src.item_length) * 0.393700787, 2, MidpointRounding.ToPositiveInfinity).ToString(),
                    "MM" => Math.Round(double.Parse(src.item_length) * 0.0393700787, 2, MidpointRounding.ToPositiveInfinity).ToString(),
                    "DM" => Math.Round(double.Parse(src.item_length) * 3.93700787, 2, MidpointRounding.ToPositiveInfinity).ToString(),
                    "Picometer" => Math.Round(double.Parse(src.item_length) * 3.93700787 * (10 ^ -11), 2, MidpointRounding.ToPositiveInfinity).ToString(),
                    "µM" => Math.Round(double.Parse(src.item_length) * 3.93700787 * (10 ^ -5), 2, MidpointRounding.ToPositiveInfinity).ToString(),
                    "M" => Math.Round(double.Parse(src.item_length) * 39.3701, 2, MidpointRounding.ToPositiveInfinity).ToString(),
                    "Yards" => Math.Round(double.Parse(src.item_length) * 36, 2, MidpointRounding.ToPositiveInfinity).ToString(),
                    "Miles" => Math.Round(double.Parse(src.item_length) * 63.360, 2, MidpointRounding.ToPositiveInfinity).ToString(),
                    "Mils" => Math.Round(double.Parse(src.item_length) / 1000, 2, MidpointRounding.ToPositiveInfinity).ToString(),
                    "Nanometer" => Math.Round(double.Parse(src.item_length) * 3.93700787 * (10 ^ -8), 2, MidpointRounding.ToPositiveInfinity).ToString(),
                    "Hundredths-Inches" => Math.Round(double.Parse(src.item_length) / 100, 2, MidpointRounding.ToPositiveInfinity).ToString(),
                    "Kilometers" => Math.Round(double.Parse(src.item_length) * 39370.1, 2, MidpointRounding.ToPositiveInfinity).ToString(),
                    "Angstrom" => Math.Round(double.Parse(src.item_length) * (10 ^ -8), 2, MidpointRounding.ToPositiveInfinity).ToString(),
                    _ => string.Empty,
                };
            }
            catch
            {
                return string.Empty;
            }

        }

        public static string GetCertifyingAgent(VsiriusProductDto vsirius)
        {
            string text = string.Empty;

            for (int i = 1; i <= 9; i++)
            {
                string? certificateType = (string?)typeof(VsiriusProductDto).GetProperty("certificate_type" + i)?.GetValue(vsirius);

                if (!string.IsNullOrWhiteSpace(certificateType))
                {
                    text += $"{certificateType}|";
                }
            }

            return text.TrimEnd('|');
        }

        public static string? GetMsrpForWalmart(string map_price)
        {
            try
            {
                return Math.Round(double.Parse(map_price) * 1.5, 2, MidpointRounding.ToPositiveInfinity).ToString();
            }
            catch
            {
                return string.Empty;
            }
        }

        public static string? GetPrice(VsiriusProductDto vsirius)
        {
            double price = double.Parse(vsirius.standard_price);

            return Math.Round(price, 2, MidpointRounding.ToPositiveInfinity).ToString();
        }

        public static string GetSKU(VsiriusProductDto vsirius)
        {
            //những product có brand VSirius thì sẽ bắt đầu bằng QU để phân biệt sản phẩm có thương hiệu và sản phẩm của mình
            if (vsirius.brand_name != null && vsirius.brand_name.Equals("VSIRIUS", StringComparison.OrdinalIgnoreCase))
            {
                if (vsirius.item_sku.StartsWith("Q"))
                {
                    return "QU-" + vsirius.item_sku[2..];
                }
                else
                {
                    return vsirius.item_sku;
                }
            }
            else
            {
                return vsirius.item_sku;
            }
        }
    }
}
