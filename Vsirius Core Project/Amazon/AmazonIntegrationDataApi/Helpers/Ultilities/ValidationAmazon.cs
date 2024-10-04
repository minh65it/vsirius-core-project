using AmazonIntegrationDataApi.Data;
using AmazonIntegrationDataApi.Dtos;
using System.Text.RegularExpressions;

namespace AmazonIntegrationDataApi.Helpers.Ultilities
{
    public class ValidationAmazon
    {
        private readonly DBContext _context;
        public ValidationAmazon(DBContext context)
        {
            _context = context;
        }
        public AmazonJewelryDataFeedItemV3_Dto IsValid(AmazonJewelryDataFeedItemV3_Dto dto)
        {
            //var fileName = Path.Combine(Directory.GetCurrentDirectory(), @"Resources\AmazonTemplate.xlsm");
            Dictionary<string, List<object>> listModelfromExcel = Utilities.listModelfromExcel;
            AmazonJewelryDataFeedItemV3_Dto msgDto = FiedRequired.FiedRequireds(listModelfromExcel, dto);

            //age_range_description
            if (!string.IsNullOrEmpty(dto.age_range_description))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.age_range_description) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.age_range_description) + "_" + dto.feed_product_type].Contains(dto.age_range_description))
                    {
                        msgDto.age_range_description = "Select one of the following options: Adult, Big Kid, Little Kid, Toddler, Infant.";
                    }
                }
            }

            //other_image_url1
            if (!string.IsNullOrEmpty(dto.other_image_url1))
            {
                if (!Utilities.IsValidUrl(dto.other_image_url1))
                {
                    msgDto.other_image_url1 = "A valid URL, including leading.";
                }
            }
            ////other_image_url2
            //if (!string.IsNullOrEmpty(dto.other_image_url2))
            //{
            //    if (!Ultilities.IsValidUrl(dto.other_image_url2))
            //    {
            //        msgDto.other_image_url2 = "A valid URL, including leading.";
            //    }
            //}
            ////other_image_url3
            //if (!string.IsNullOrEmpty(dto.other_image_url3))
            //{
            //    if (!Ultilities.IsValidUrl(dto.other_image_url3))
            //    {
            //        msgDto.other_image_url3 = "A valid URL, including leading.";
            //    }
            //}
            ////other_image_url4
            //if (!string.IsNullOrEmpty(dto.other_image_url4))
            //{
            //    if (!Ultilities.IsValidUrl(dto.other_image_url4))
            //    {
            //        msgDto.other_image_url4 = "A valid URL, including leading.";
            //    }
            //}
            ////other_image_url5
            //if (!string.IsNullOrEmpty(dto.other_image_url5))
            //{
            //    if (!Ultilities.IsValidUrl(dto.other_image_url5))
            //    {
            //        msgDto.other_image_url5 = "A valid URL, including leading.";
            //    }
            //}
            ////other_image_url6
            //if (!string.IsNullOrEmpty(dto.other_image_url6))
            //{
            //    if (!Ultilities.IsValidUrl(dto.other_image_url6))
            //    {
            //        msgDto.other_image_url6 = "A valid URL, including leading.";
            //    }
            //}
            ////other_image_url7
            //if (!string.IsNullOrEmpty(dto.other_image_url7))
            //{
            //    if (!Ultilities.IsValidUrl(dto.other_image_url7))
            //    {
            //        msgDto.other_image_url7 = "A valid URL, including leading.";
            //    }
            //}
            ////other_image_url8
            //if (!string.IsNullOrEmpty(dto.other_image_url8))
            //{
            //    if (!Ultilities.IsValidUrl(dto.other_image_url8))
            //    {
            //        msgDto.other_image_url8 = "A valid URL, including leading.";
            //    }
            //}
            //swatch_image_url
            if (!string.IsNullOrEmpty(dto.swatch_image_url))
            {
                if (!Utilities.IsValidUrl(dto.swatch_image_url))
                {
                    msgDto.swatch_image_url = "A valid URL, including leading.";
                }
            }
            //pt1_image_url 
            if (!string.IsNullOrEmpty(dto.pt1_image_url))
            {
                if (!Utilities.IsValidUrl(dto.pt1_image_url))
                {
                    msgDto.pt1_image_url = "Upload an additional image with a different view of your product.";
                }
            }
            //pt2_image_url 
            if (!string.IsNullOrEmpty(dto.pt2_image_url))
            {
                if (!Utilities.IsValidUrl(dto.pt2_image_url))
                {
                    msgDto.pt2_image_url = "Upload an additional image with a different view of your product.";
                }
            }
            //pt3_image_url 
            if (!string.IsNullOrEmpty(dto.pt3_image_url))
            {
                if (!Utilities.IsValidUrl(dto.pt3_image_url))
                {
                    msgDto.pt3_image_url = "Upload an additional image with a different view of your product.";
                }
            }
            //pt4_image_url 
            if (!string.IsNullOrEmpty(dto.pt4_image_url))
            {
                if (!Utilities.IsValidUrl(dto.pt4_image_url))
                {
                    msgDto.pt4_image_url = "Upload an additional image with a different view of your product.";
                }
            }
            //pt5_image_url 
            if (!string.IsNullOrEmpty(dto.pt5_image_url))
            {
                if (!Utilities.IsValidUrl(dto.pt5_image_url))
                {
                    msgDto.pt5_image_url = "Upload an additional image with a different view of your product.";
                }
            }
            //pt6_image_url 
            if (!string.IsNullOrEmpty(dto.pt6_image_url))
            {
                if (!Utilities.IsValidUrl(dto.pt6_image_url))
                {
                    msgDto.pt6_image_url = "Upload an additional image with a different view of your product.";
                }
            }
            //pt7_image_url 
            if (!string.IsNullOrEmpty(dto.pt7_image_url))
            {
                if (!Utilities.IsValidUrl(dto.pt7_image_url))
                {
                    msgDto.pt7_image_url = "Upload an additional image with a different view of your product.";
                }
            }
            //pt8_image_url 
            if (!string.IsNullOrEmpty(dto.pt8_image_url))
            {
                if (!Utilities.IsValidUrl(dto.pt8_image_url))
                {
                    msgDto.pt8_image_url = "Upload an additional image with a different view of your product.";
                }
            }
            //parent_child
            if (!string.IsNullOrEmpty(dto.parent_child))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.parent_child) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.parent_child) + "_" + dto.feed_product_type].Contains(dto.parent_child))
                    {
                        msgDto.parent_child = "Select one of the following options: parent or child.";
                    }
                }
            }

            //parent_sku
            if (!string.IsNullOrEmpty(dto.parent_sku))
            {
                if (!Utilities.IsValidAlphanumericString(dto.parent_sku, 40))
                {
                    msgDto.parent_sku = "An alphanumeric string, 1 character minimum in length and 40 characters maximum in length.";
                }
            }

            //relationship_type
            if (!string.IsNullOrEmpty(dto.relationship_type))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.relationship_type) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.relationship_type) + "_" + dto.feed_product_type].Contains(dto.relationship_type))
                    {
                        msgDto.relationship_type = "Select one of the following options Variation or Accessory.";
                    }
                }
            }

            //variation_theme
            if (!string.IsNullOrEmpty(dto.variation_theme))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.variation_theme) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.variation_theme) + "_" + dto.feed_product_type].Contains(dto.variation_theme))
                    {
                        msgDto.variation_theme = "Select one of the following options Variation or Accessory.";
                    }
                }
            }

            //package_level
            if (!string.IsNullOrEmpty(dto.package_level))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.package_level) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.package_level) + "_" + dto.feed_product_type].Contains(dto.package_level))
                    {
                        msgDto.package_level = "Select one of the following options Variation or Accessory.";
                    }
                }
            }

            //update_delete
            if (!string.IsNullOrEmpty(dto.update_delete))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.update_delete) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.update_delete) + "_" + dto.feed_product_type].Contains(dto.update_delete))
                    {
                        msgDto.update_delete = "Select one of the following options Update or PartialUpdate or Delete";
                    }
                }

            }

            //certificate_number1
            if (!string.IsNullOrEmpty(dto.certificate_number1))
            {
                if (!Utilities.IsValidAlphanumericString(dto.certificate_number1, 250))
                {
                    msgDto.certificate_number1 = "An alphanumeric string, 1 character minimum in length and 250 characters maximum in length.";
                }
            }
            //certificate_number2
            if (!string.IsNullOrEmpty(dto.certificate_number2))
            {
                if (!Utilities.IsValidAlphanumericString(dto.certificate_number2, 250))
                {
                    msgDto.certificate_number2 = "An alphanumeric string, 1 character minimum in length and 250 characters maximum in length.";
                }
            }
            //certificate_number3
            if (!string.IsNullOrEmpty(dto.certificate_number3))
            {
                if (!Utilities.IsValidAlphanumericString(dto.certificate_number3, 250))
                {
                    msgDto.certificate_number3 = "An alphanumeric string, 1 character minimum in length and 250 characters maximum in length.";
                }
            }
            //certificate_number4
            if (!string.IsNullOrEmpty(dto.certificate_number4))
            {
                if (!Utilities.IsValidAlphanumericString(dto.certificate_number4, 250))
                {
                    msgDto.certificate_number4 = "An alphanumeric string, 1 character minimum in length and 250 characters maximum in length.";
                }
            }
            //certificate_number5
            if (!string.IsNullOrEmpty(dto.certificate_number5))
            {
                if (!Utilities.IsValidAlphanumericString(dto.certificate_number5, 250))
                {
                    msgDto.certificate_number5 = "An alphanumeric string, 1 character minimum in length and 250 characters maximum in length.";
                }
            }
            //certificate_number6
            if (!string.IsNullOrEmpty(dto.certificate_number6))
            {
                if (!Utilities.IsValidAlphanumericString(dto.certificate_number6, 250))
                {
                    msgDto.certificate_number6 = "An alphanumeric string, 1 character minimum in length and 250 characters maximum in length.";
                }
            }
            //certificate_number7
            if (!string.IsNullOrEmpty(dto.certificate_number7))
            {
                if (!Utilities.IsValidAlphanumericString(dto.certificate_number7, 250))
                {
                    msgDto.certificate_number7 = "An alphanumeric string, 1 character minimum in length and 250 characters maximum in length.";
                }
            }
            //certificate_number8
            if (!string.IsNullOrEmpty(dto.certificate_number8))
            {
                if (!Utilities.IsValidAlphanumericString(dto.certificate_number8, 250))
                {
                    msgDto.certificate_number8 = "An alphanumeric string, 1 character minimum in length and 250 characters maximum in length.";
                }
            }
            //certificate_number9
            if (!string.IsNullOrEmpty(dto.certificate_number9))
            {
                if (!Utilities.IsValidAlphanumericString(dto.certificate_number9, 250))
                {
                    msgDto.certificate_number9 = "An alphanumeric string, 1 character minimum in length and 250 characters maximum in length.";
                }
            }
            //certificate_type1
            if (!string.IsNullOrEmpty(dto.certificate_type1))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.certificate_type1) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.certificate_type1) + "_" + dto.feed_product_type].Contains(dto.certificate_type1))
                    {
                        msgDto.certificate_type1 = "Assign a value from the list on the valid values tab.";
                    }
                }
            }

            //certificate_type2
            if (!string.IsNullOrEmpty(dto.certificate_type2))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.certificate_type2) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.certificate_type2) + "_" + dto.feed_product_type].Contains(dto.certificate_type2))
                    {
                        msgDto.certificate_type2 = "Assign a value from the list on the valid values tab.";
                    }
                }
            }
            //certificate_type3
            if (!string.IsNullOrEmpty(dto.certificate_type3))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.certificate_type3) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.certificate_type3) + "_" + dto.feed_product_type].Contains(dto.certificate_type3))
                    {
                        msgDto.certificate_type3 = "Assign a value from the list on the valid values tab.";
                    }
                }
            }
            //certificate_type4
            if (!string.IsNullOrEmpty(dto.certificate_type4))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.certificate_type4) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.certificate_type4) + "_" + dto.feed_product_type].Contains(dto.certificate_type4))
                    {
                        msgDto.certificate_type4 = "Assign a value from the list on the valid values tab.";
                    }
                }
            }
            //certificate_type5
            if (!string.IsNullOrEmpty(dto.certificate_type5))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.certificate_type5) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.certificate_type5) + "_" + dto.feed_product_type].Contains(dto.certificate_type5))
                    {
                        msgDto.certificate_type5 = "Assign a value from the list on the valid values tab.";
                    }
                }
            }
            //certificate_type6
            if (!string.IsNullOrEmpty(dto.certificate_type6))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.certificate_type6) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.certificate_type6) + "_" + dto.feed_product_type].Contains(dto.certificate_type6))
                    {
                        msgDto.certificate_type6 = "Assign a value from the list on the valid values tab.";
                    }
                }
            }
            //certificate_type7
            if (!string.IsNullOrEmpty(dto.certificate_type7))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.certificate_type7) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.certificate_type7) + "_" + dto.feed_product_type].Contains(dto.certificate_type7))
                    {
                        msgDto.certificate_type7 = "Assign a value from the list on the valid values tab.";
                    }
                }
            }
            //certificate_type8
            if (!string.IsNullOrEmpty(dto.certificate_type8))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.certificate_type8) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.certificate_type8) + "_" + dto.feed_product_type].Contains(dto.certificate_type8))
                    {
                        msgDto.certificate_type8 = "Assign a value from the list on the valid values tab.";
                    }
                }
            }
            //certificate_type9
            if (!string.IsNullOrEmpty(dto.certificate_type9))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.certificate_type9) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.certificate_type9) + "_" + dto.feed_product_type].Contains(dto.certificate_type9))
                    {
                        msgDto.certificate_type9 = "Assign a value from the list on the valid values tab.";
                    }
                }
            }

            if (!string.IsNullOrEmpty(dto.model_year))
            {
                if (!Utilities.IsValidTextString(dto.model_year))
                {
                    msgDto.model_year = "Numerical String";
                }
            }

            //production_method
            if (!string.IsNullOrEmpty(dto.production_method))
            {
                Regex regex = new Regex(@"^.{25,2000}$");
                if (!regex.IsMatch(dto.production_method))
                {
                    msgDto.production_method = "Describe how this product is made using a minimum of 25 characters and maximum of 2,000 characters.";
                }
            }


            //stone_color1
            if (!string.IsNullOrEmpty(dto.stone_color1))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.stone_color1) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.stone_color1) + "_" + dto.feed_product_type].Contains(dto.stone_color1))
                    {
                        msgDto.stone_color1 = "Assign a value from the list on the Valid Values tab.";
                    }
                }
            }
            //stone_color2
            if (!string.IsNullOrEmpty(dto.stone_color2))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.stone_color2) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.stone_color2) + "_" + dto.feed_product_type].Contains(dto.stone_color2))
                    {
                        msgDto.stone_color2 = "Assign a value from the list on the Valid Values tab.";
                    }
                }
            }
            //stone_color3
            if (!string.IsNullOrEmpty(dto.stone_color3))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.stone_color3) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.stone_color3) + "_" + dto.feed_product_type].Contains(dto.stone_color3))
                    {
                        msgDto.stone_color3 = "Assign a value from the list on the Valid Values tab.";
                    }
                }
            }

            //metals_metal_weight_unit_of_measure
            if (!string.IsNullOrEmpty(dto.metals_metal_weight_unit_of_measure))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.metals_metal_weight_unit_of_measure) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.metals_metal_weight_unit_of_measure) + "_" + dto.feed_product_type].Contains(dto.metals_metal_weight_unit_of_measure))
                    {
                        msgDto.metals_metal_weight_unit_of_measure = "Provide the corresponding weight unit.";
                    }
                }
            }

            //stones_color
            if (!string.IsNullOrEmpty(dto.stones_color))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.stones_color) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.stones_color) + "_" + dto.feed_product_type].Contains(dto.stones_color))
                    {
                        msgDto.stones_color = "Assign a value from the list on the Valid Values tab.";
                    }
                }
            }

            //occasion_type1
            if (!string.IsNullOrEmpty(dto.occasion_type1))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.occasion_type1) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.occasion_type1) + "_" + dto.feed_product_type].Contains(dto.occasion_type1))
                    {
                        msgDto.occasion_type1 = "Select from the list of valid values.";
                    }
                }
            }

            //occasion_type2
            if (!string.IsNullOrEmpty(dto.occasion_type2))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.occasion_type2) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.occasion_type2) + "_" + dto.feed_product_type].Contains(dto.occasion_type2))
                    {
                        msgDto.occasion_type2 = "Select from the list of valid values.";
                    }
                }
            }

            //occasion_type3
            if (!string.IsNullOrEmpty(dto.occasion_type3))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.occasion_type3) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.occasion_type3) + "_" + dto.feed_product_type].Contains(dto.occasion_type3))
                    {
                        msgDto.occasion_type3 = "Select from the list of valid values.";
                    }
                }
            }

            //occasion_type4
            if (!string.IsNullOrEmpty(dto.occasion_type4))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.occasion_type4) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.occasion_type4) + "_" + dto.feed_product_type].Contains(dto.occasion_type4))
                    {
                        msgDto.occasion_type4 = "Select from the list of valid values.";
                    }
                }
            }
            //occasion_type5
            if (!string.IsNullOrEmpty(dto.occasion_type5))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.occasion_type5) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.occasion_type5) + "_" + dto.feed_product_type].Contains(dto.occasion_type5))
                    {
                        msgDto.occasion_type5 = "Select from the list of valid values.";
                    }
                }
            }

            //clasp_type
            if (!string.IsNullOrEmpty(dto.clasp_type))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.clasp_type) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.clasp_type) + "_" + dto.feed_product_type].Contains(dto.clasp_type))
                    {
                        msgDto.clasp_type = "Assign a value from the list on the Valid Values tab.";
                    }
                }
            }

            //stones_creation_method
            if (!string.IsNullOrEmpty(dto.stones_creation_method))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.stones_creation_method) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.stones_creation_method) + "_" + dto.feed_product_type].Contains(dto.stones_creation_method))
                    {
                        msgDto.stones_creation_method = "Assign a value from the list on the Valid Values tab.";
                    }
                }
            }

            //athlete
            if (!string.IsNullOrEmpty(dto.athlete))
            {
                if (!Utilities.IsValidAlphanumericString(dto.athlete, 50))
                {
                    dto.athlete = "An alphanumeric string; 50 characters maximum.";
                }
            }

            //team_name
            if (!string.IsNullOrEmpty(dto.team_name))
            {
                if (!listModelfromExcel[nameof(dto.team_name)].Contains(dto.team_name))
                {
                    msgDto.team_name = "Use the multiple columns  in the Valid Values list. An alphanumeric string; 50 characters maximum.";
                }
            }

            ////stone_shape1
            //if (!string.IsNullOrEmpty(dto.stone_shape1))
            //{

            //    if (listModelfromExcel.ContainsKey(nameof(dto.stone_shape1) + "_" + dto.feed_product_type))
            //    {
            //        if (!listModelfromExcel[nameof(dto.stone_shape1) + "_" + dto.feed_product_type].Contains(dto.stone_shape1))
            //        {
            //            msgDto.stone_shape1 = "Assign a value from the list on the Valid Values tab.";
            //        }
            //    }

            //}
            ////stone_shape2
            //if (!string.IsNullOrEmpty(dto.stone_shape2))
            //{
            //    if (listModelfromExcel.ContainsKey(nameof(dto.stone_shape2) + "_" + dto.feed_product_type))
            //    {
            //        if (!listModelfromExcel[nameof(dto.stone_shape2) + "_" + dto.feed_product_type].Contains(dto.stone_shape2))
            //        {
            //            msgDto.stone_shape2 = "Assign a value from the list on the Valid Values tab.";
            //        }
            //    }
            //}
            ////stone_shape3
            //if (!string.IsNullOrEmpty(dto.stone_shape3))
            //{
            //    if (listModelfromExcel.ContainsKey(nameof(dto.stone_shape3) + "_" + dto.feed_product_type))
            //    {
            //        if (!listModelfromExcel[nameof(dto.stone_shape3) + "_" + dto.feed_product_type].Contains(dto.stone_shape3))
            //        {
            //            msgDto.stone_shape3 = "Assign a value from the list on the Valid Values tab.";
            //        }
            //    }
            //}
            //stone_clarity1
            if (!string.IsNullOrEmpty(dto.stone_clarity1))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.stone_clarity1) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.stone_clarity1) + "_" + dto.feed_product_type].Contains(dto.stone_clarity1))
                    {
                        msgDto.stone_clarity1 = "Assign a value from the list on the Valid Values tab.";
                    }
                }
            }
            //stone_clarity2
            if (!string.IsNullOrEmpty(dto.stone_clarity2))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.stone_clarity2) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.stone_clarity2) + "_" + dto.feed_product_type].Contains(dto.stone_clarity2))
                    {
                        msgDto.stone_clarity2 = "Assign a value from the list on the Valid Values tab.";
                    }
                }
            }
            //stone_clarity3
            if (!string.IsNullOrEmpty(dto.stone_clarity3))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.stone_clarity3) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.stone_clarity3) + "_" + dto.feed_product_type].Contains(dto.stone_clarity3))
                    {
                        msgDto.stone_clarity3 = "Assign a value from the list on the Valid Values tab.";
                    }
                }
            }

            //collection_name
            if (!string.IsNullOrEmpty(dto.collection_name))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.collection_name) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.collection_name) + "_" + dto.feed_product_type].Contains(dto.collection_name))
                    {
                        msgDto.collection_name = "Assign a value from the list on the Valid Values tab.";
                    }
                }
            }


            //chain_length_unit
            if (!string.IsNullOrEmpty(dto.chain_length_unit))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.chain_length_unit) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.chain_length_unit) + "_" + dto.feed_product_type].Contains(dto.chain_length_unit))
                    {
                        msgDto.chain_length_unit = "Assign a value from the list on the Valid Values tab.";
                    }
                }
            }

            ////stone_cut1
            //if (!string.IsNullOrEmpty(dto.stone_cut1))
            //{
            //    if (!listModelfromExcel[nameof(dto.stone_cut1)].Contains(dto.stone_cut1))
            //    {
            //        msgDto.stone_cut1 = "Assign a value from the list on the Valid Values tab.";
            //    }
            //}

            ////stone_cut2
            //if (!string.IsNullOrEmpty(dto.stone_cut2))
            //{
            //    if (!listModelfromExcel[nameof(dto.stone_cut2)].Contains(dto.stone_cut2))
            //    {
            //        msgDto.stone_cut2 = "Assign a value from the list on the Valid Values tab.";
            //    }
            //}

            ////stone_cut3
            //if (!string.IsNullOrEmpty(dto.stone_cut3))
            //{
            //    if (!listModelfromExcel[nameof(dto.stone_cut3)].Contains(dto.stone_cut3))
            //    {
            //        msgDto.stone_cut3 = "Assign a value from the list on the Valid Values tab.";
            //    }
            //}

            //league_name
            if (!string.IsNullOrEmpty(dto.league_name))
            {
                if (!listModelfromExcel[nameof(dto.league_name)].Contains(dto.league_name))
                {
                    msgDto.league_name = "Assign a value from the list on the Valid Values tab.";
                }
            }

            //thesaurus_subject_keywords1
            if (!string.IsNullOrEmpty(dto.thesaurus_subject_keywords1))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.thesaurus_subject_keywords1) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.thesaurus_subject_keywords1) + "_" + dto.feed_product_type].Contains(dto.thesaurus_subject_keywords1))
                    {
                        msgDto.thesaurus_subject_keywords1 = "Assign a value from the list on the Valid Values tab.";
                    }
                }
            }
            //thesaurus_subject_keywords2
            if (!string.IsNullOrEmpty(dto.thesaurus_subject_keywords2))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.thesaurus_subject_keywords2) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.thesaurus_subject_keywords2) + "_" + dto.feed_product_type].Contains(dto.thesaurus_subject_keywords2))
                    {
                        msgDto.thesaurus_subject_keywords2 = "Assign a value from the list on the Valid Values tab.";
                    }
                }
            }
            //thesaurus_subject_keywords3
            if (!string.IsNullOrEmpty(dto.thesaurus_subject_keywords3))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.thesaurus_subject_keywords3) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.thesaurus_subject_keywords3) + "_" + dto.feed_product_type].Contains(dto.thesaurus_subject_keywords3))
                    {
                        msgDto.thesaurus_subject_keywords3 = "Assign a value from the list on the Valid Values tab.";
                    }
                }
            }
            //thesaurus_subject_keywords4
            if (!string.IsNullOrEmpty(dto.thesaurus_subject_keywords4))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.thesaurus_subject_keywords4) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.thesaurus_subject_keywords4) + "_" + dto.feed_product_type].Contains(dto.thesaurus_subject_keywords4))
                    {
                        msgDto.thesaurus_subject_keywords4 = "Assign a value from the list on the Valid Values tab.";
                    }
                }
            }
            //thesaurus_subject_keywords5
            if (!string.IsNullOrEmpty(dto.thesaurus_subject_keywords5))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.thesaurus_subject_keywords5) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.thesaurus_subject_keywords5) + "_" + dto.feed_product_type].Contains(dto.thesaurus_subject_keywords5))
                    {
                        msgDto.thesaurus_subject_keywords5 = "Assign a value from the list on the Valid Values tab.";
                    }
                }
            }

            //chain_length_derived
            if (!string.IsNullOrEmpty(dto.chain_length_derived))
            {
                if (!Utilities.IsValidDecimal(dto.chain_length_derived))
                {
                    msgDto.chain_length_derived = "Specify the length of chain in decimal.";
                }
            }

            //chain_type
            if (!string.IsNullOrEmpty(dto.chain_type))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.chain_type) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.chain_type) + "_" + dto.feed_product_type].Contains(dto.chain_type))
                    {
                        msgDto.chain_type = "Assign a value from the list on the Valid Values tab.";
                    }
                }
            }

            //drop_length_unit
            if (!string.IsNullOrEmpty(dto.drop_length_unit))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.drop_length_unit) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.drop_length_unit) + "_" + dto.feed_product_type].Contains(dto.drop_length_unit))
                    {
                        msgDto.drop_length_unit = "Assign a value from the list on the Valid Values tab.";
                    }
                }
            }

            //item_booking_date
            if (!string.IsNullOrEmpty(dto.item_booking_date))
            {
                if (!Utilities.IsFormatDatetime(dto.item_booking_date))
                {
                    msgDto.item_booking_date = "Provide the date a vendor must receive a PO in order to deliver goods to Amazon by a specified date using YYYY-MM-DD format.";
                }
            }
            if (!string.IsNullOrEmpty(dto.is_autographed))
            {
                if (dto.is_autographed == "False")
                {
                    dto.is_autographed = "No";
                }
                else
                {
                    dto.is_autographed = "Yes";
                }
            }

            //is_autographed
            if (!string.IsNullOrEmpty(dto.is_autographed))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.is_autographed)))
                {

                    if (!listModelfromExcel[nameof(dto.is_autographed)].Contains(dto.is_autographed))
                    {
                        msgDto.is_autographed = "Select true or false.";
                    }
                }
            }

            //back_finding
            if (!string.IsNullOrEmpty(dto.back_finding))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.back_finding) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.back_finding) + "_" + dto.feed_product_type].Contains(dto.back_finding))
                    {
                        msgDto.back_finding = "Assign a value from the list on the Valid Values tab.";
                    }
                }
            }

            //stone_creation_method1
            if (!string.IsNullOrEmpty(dto.stone_creation_method1))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.stone_creation_method1) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.stone_creation_method1) + "_" + dto.feed_product_type].Contains(dto.stone_creation_method1))
                    {
                        msgDto.stone_creation_method1 = "Assign one of the following options natural or synthetic.";
                    }
                }
            }

            //stone_creation_method2
            if (!string.IsNullOrEmpty(dto.stone_creation_method2))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.stone_creation_method2) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.stone_creation_method2) + "_" + dto.feed_product_type].Contains(dto.stone_creation_method2))
                    {
                        msgDto.stone_creation_method2 = "Assign one of the following options natural or synthetic.";
                    }
                }
            }
            //stone_creation_method3
            if (!string.IsNullOrEmpty(dto.stone_creation_method3))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.stone_creation_method3) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.stone_creation_method3) + "_" + dto.feed_product_type].Contains(dto.stone_creation_method3))
                    {
                        msgDto.stone_creation_method3 = "Assign one of the following options natural or synthetic.";
                    }
                }
            }

            //stone_treatment_method1
            if (!string.IsNullOrEmpty(dto.stone_treatment_method1))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.stone_treatment_method1) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.stone_treatment_method1) + "_" + dto.feed_product_type].Contains(dto.stone_treatment_method1))
                    {
                        msgDto.stone_treatment_method1 = "Assign all applicable gemstone treatments from this list.";
                    }
                }
            }
            //stone_treatment_method2
            if (!string.IsNullOrEmpty(dto.stone_treatment_method2))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.stone_treatment_method2) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.stone_treatment_method2) + "_" + dto.feed_product_type].Contains(dto.stone_treatment_method2))
                    {
                        msgDto.stone_treatment_method2 = "Assign all applicable gemstone treatments from this list.";
                    }
                }
            }
            //stone_treatment_method3
            if (!string.IsNullOrEmpty(dto.stone_treatment_method3))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.stone_treatment_method3) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.stone_treatment_method3) + "_" + dto.feed_product_type].Contains(dto.stone_treatment_method3))
                    {
                        msgDto.stone_treatment_method3 = "Assign all applicable gemstone treatments from this list.";
                    }
                }
            }

            //lining_description
            if (!string.IsNullOrEmpty(dto.lining_description))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.lining_description) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.lining_description) + "_" + dto.feed_product_type].Contains(dto.lining_description))
                    {
                        msgDto.lining_description = "Assign all applicable gemstone treatments from this list.";
                    }
                }
            }

            //pattern_name
            if (!string.IsNullOrEmpty(dto.pattern_name))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.pattern_name) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.pattern_name) + "_" + dto.feed_product_type].Contains(dto.pattern_name))
                    {
                        msgDto.pattern_name = "Assign all applicable gemstone treatments from this list.";
                    }
                }
            }

            //wheel_type
            if (!string.IsNullOrEmpty(dto.wheel_type))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.wheel_type) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.wheel_type) + "_" + dto.feed_product_type].Contains(dto.wheel_type))
                    {
                        msgDto.wheel_type = "Assign all applicable gemstone treatments from this list.";
                    }
                }
            }

            //drop_length_unit
            if (!string.IsNullOrEmpty(dto.drop_length_unit))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.drop_length_unit) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.drop_length_unit) + "_" + dto.feed_product_type].Contains(dto.drop_length_unit))
                    {
                        msgDto.drop_length_unit = "Provide the corresponding unit";
                    }
                }
            }

            //band_thickness_unit_of_measure
            if (!string.IsNullOrEmpty(dto.band_thickness_unit_of_measure))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.band_thickness_unit_of_measure) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.band_thickness_unit_of_measure) + "_" + dto.feed_product_type].Contains(dto.band_thickness_unit_of_measure))
                    {
                        msgDto.band_thickness_unit_of_measure = "Provide the corresponding unit";
                    }
                }
            }

            //horsepower
            if (!string.IsNullOrEmpty(dto.horsepower))
            {
                if (!Utilities.ValidateNumber(dto.horsepower, 10, 2))
                {
                    dto.horsepower = "A number with up to 10 digits to the left of the decimal point and 2 digits to the right of the decimal point. ";
                }
            }

            //horsepower_unit_of_measure
            if (!string.IsNullOrEmpty(dto.horsepower_unit_of_measure))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.horsepower_unit_of_measure) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.horsepower_unit_of_measure) + "_" + dto.feed_product_type].Contains(dto.horsepower_unit_of_measure))
                    {
                        msgDto.horsepower_unit_of_measure = "Select a value from the Valid Values worksheet.";
                    }
                }
            }

            //power_source_type
            if (!string.IsNullOrEmpty(dto.power_source_type))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.power_source_type) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.power_source_type) + "_" + dto.feed_product_type].Contains(dto.power_source_type))
                    {
                        msgDto.power_source_type = "Select a value from the Valid Values worksheet.";
                    }
                }
            }

            //voltage_unit_of_measure
            if (!string.IsNullOrEmpty(dto.voltage_unit_of_measure))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.voltage_unit_of_measure) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.voltage_unit_of_measure) + "_" + dto.feed_product_type].Contains(dto.voltage_unit_of_measure))
                    {
                        msgDto.voltage_unit_of_measure = "Select a value from the Valid Values worksheet.";
                    }
                }
            }

            //controller_type
            if (!string.IsNullOrEmpty(dto.controller_type))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.controller_type) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.controller_type) + "_" + dto.feed_product_type].Contains(dto.controller_type))
                    {
                        msgDto.controller_type = "Select a value from the Valid Values worksheet.";
                    }
                }
            }

            if (!string.IsNullOrEmpty(dto.maximum_operating_pressure))
            {
                if (!Utilities.IsValidDecimal(dto.maximum_operating_pressure))
                {
                    dto.maximum_operating_pressure = "A number with up to 2 decimal places.";
                }
            }

            //maximum_operating_pressure_unit_of_measure
            if (!string.IsNullOrEmpty(dto.maximum_operating_pressure_unit_of_measure))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.maximum_operating_pressure_unit_of_measure) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.maximum_operating_pressure_unit_of_measure) + "_" + dto.feed_product_type].Contains(dto.maximum_operating_pressure_unit_of_measure))
                    {
                        msgDto.maximum_operating_pressure_unit_of_measure = "Select a value from the Valid Values worksheet.";
                    }
                }
            }

            //handle_location
            if (!string.IsNullOrEmpty(dto.handle_location))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.handle_location) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.handle_location) + "_" + dto.feed_product_type].Contains(dto.handle_location))
                    {
                        msgDto.handle_location = "Select a value from the Valid Values worksheet.";
                    }
                }
            }

            //light_source_type
            if (!string.IsNullOrEmpty(dto.light_source_type))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.light_source_type) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.light_source_type) + "_" + dto.feed_product_type].Contains(dto.light_source_type))
                    {
                        msgDto.light_source_type = "Select a value from the Valid Values worksheet.";
                    }
                }
            }

            //brightness_unit_of_measure
            if (!string.IsNullOrEmpty(dto.brightness_unit_of_measure))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.brightness_unit_of_measure) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.brightness_unit_of_measure) + "_" + dto.feed_product_type].Contains(dto.brightness_unit_of_measure))
                    {
                        msgDto.brightness_unit_of_measure = "Select a value from the Valid Values worksheet.";
                    }
                }
            }

            //speed_unit_of_measure
            if (!string.IsNullOrEmpty(dto.speed_unit_of_measure))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.speed_unit_of_measure) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.speed_unit_of_measure) + "_" + dto.feed_product_type].Contains(dto.speed_unit_of_measure))
                    {
                        msgDto.speed_unit_of_measure = "Select a value from the Valid Values worksheet.";
                    }
                }
            }

            //item_torque
            if (!string.IsNullOrEmpty(dto.item_torque))
            {
                if (!Utilities.ValidateNumber(dto.item_torque, 10, 2))
                {
                    dto.item_torque = "A number with up to 10 digits to the left of the decimal point and 2 digits to the right of the decimal point. ";
                }
            }

            //item_torque_unit_of_measure
            if (!string.IsNullOrEmpty(dto.item_torque_unit_of_measure))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.item_torque_unit_of_measure) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.item_torque_unit_of_measure) + "_" + dto.feed_product_type].Contains(dto.item_torque_unit_of_measure))
                    {
                        msgDto.item_torque_unit_of_measure = "Select a value from the Valid Values worksheet.";
                    }
                }
            }

            //insulation_resistance_unit_of_measure
            if (!string.IsNullOrEmpty(dto.insulation_resistance_unit_of_measure))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.insulation_resistance_unit_of_measure) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.insulation_resistance_unit_of_measure) + "_" + dto.feed_product_type].Contains(dto.insulation_resistance_unit_of_measure))
                    {
                        msgDto.insulation_resistance_unit_of_measure = "Select a value from the Valid Values worksheet.";
                    }
                }
            }

            //mounting_type
            if (!string.IsNullOrEmpty(dto.mounting_type))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.mounting_type) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.mounting_type) + "_" + dto.feed_product_type].Contains(dto.mounting_type))
                    {
                        msgDto.mounting_type = "Select a value from the Valid Values worksheet.";
                    }
                }
            }
            //oem_equivalent_part_number
            if (!string.IsNullOrEmpty(dto.oem_equivalent_part_number))
            {
                if (!Utilities.IsValidAlphanumericString(dto.oem_equivalent_part_number, 40))
                {
                    msgDto.oem_equivalent_part_number = "An alphanumeric string; 1 character minimum in length and 40 characters maximum in length.";
                }
            }

            //recommended_uses_for_product
            if (!string.IsNullOrEmpty(dto.recommended_uses_for_product))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.recommended_uses_for_product) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.recommended_uses_for_product) + "_" + dto.feed_product_type].Contains(dto.recommended_uses_for_product))
                    {
                        msgDto.recommended_uses_for_product = "Select a value from the Valid Values worksheet.";
                    }
                }
            }

            //shank_type
            if (!string.IsNullOrEmpty(dto.shank_type))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.shank_type) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.shank_type) + "_" + dto.feed_product_type].Contains(dto.shank_type))
                    {
                        msgDto.shank_type = "Select a value from the Valid Values worksheet.";
                    }
                }
            }

            //number_of_drawers
            if (!string.IsNullOrEmpty(dto.number_of_drawers))
            {
                if (!Utilities.IsValidInteger(dto.number_of_drawers))
                {
                    msgDto.number_of_drawers = "A positive whole number.";
                }
            }

            //belt_style
            if (!string.IsNullOrEmpty(dto.belt_style))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.belt_style) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.belt_style) + "_" + dto.feed_product_type].Contains(dto.belt_style))
                    {
                        msgDto.belt_style = "Select a value from the Valid Values worksheet.";
                    }
                }
            }

            //front_style
            if (!string.IsNullOrEmpty(dto.front_style))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.front_style) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.front_style) + "_" + dto.feed_product_type].Contains(dto.front_style))
                    {
                        msgDto.front_style = "Select a value from the Valid Values worksheet.";
                    }
                }
            }

            //country_as_labeled
            if (!string.IsNullOrEmpty(dto.country_as_labeled))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.country_as_labeled) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.country_as_labeled) + "_" + dto.feed_product_type].Contains(dto.country_as_labeled))
                    {
                        msgDto.country_as_labeled = "Select a value from the Valid Values worksheet.";
                    }
                }
            }

            //neck_style
            if (!string.IsNullOrEmpty(dto.neck_style))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.neck_style) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.neck_style) + "_" + dto.feed_product_type].Contains(dto.neck_style))
                    {
                        msgDto.neck_style = "Select a value from the Valid Values worksheet.";
                    }
                }
            }

            //pattern_type
            if (!string.IsNullOrEmpty(dto.pattern_type))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.pattern_type) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.pattern_type) + "_" + dto.feed_product_type].Contains(dto.pattern_type))
                    {
                        msgDto.pattern_type = "Select a value from the Valid Values worksheet.";
                    }
                }
            }

            //pocket_description
            if (!string.IsNullOrEmpty(dto.pocket_description))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.pocket_description) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.pocket_description) + "_" + dto.feed_product_type].Contains(dto.pocket_description))
                    {
                        msgDto.pocket_description = "Select a value from the Valid Values worksheet.";
                    }
                }
            }

            //theme
            if (!string.IsNullOrEmpty(dto.theme))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.theme) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.theme) + "_" + dto.feed_product_type].Contains(dto.theme))
                    {
                        msgDto.theme = "Select a value from the Valid Values worksheet.";
                    }
                }
            }

            //sport_type1
            if (!string.IsNullOrEmpty(dto.sport_type1))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.sport_type1) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.sport_type1) + "_" + dto.feed_product_type].Contains(dto.sport_type1))
                    {
                        msgDto.sport_type1 = "Use the column Sport in the Valid Values list. An alphanumeric string; 50 characters maximum.";
                    }
                }
            }

            //sport_type2
            if (!string.IsNullOrEmpty(dto.sport_type2))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.sport_type2) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.sport_type2) + "_" + dto.feed_product_type].Contains(dto.sport_type2))
                    {
                        msgDto.sport_type2 = "Use the column Sport in the Valid Values list. An alphanumeric string; 50 characters maximum.";
                    }
                }
            }

            //sport_type3
            if (!string.IsNullOrEmpty(dto.sport_type3))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.sport_type3) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.sport_type3) + "_" + dto.feed_product_type].Contains(dto.sport_type3))
                    {
                        msgDto.sport_type3 = "Use the column Sport in the Valid Values list. An alphanumeric string; 50 characters maximum.";
                    }
                }
            }

            //seasons1
            if (!string.IsNullOrEmpty(dto.seasons1))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.seasons1) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.seasons1) + "_" + dto.feed_product_type].Contains(dto.seasons1))
                    {
                        msgDto.seasons1 = "Please see the Valid Values worksheet for the list of accepted values.";
                    }
                }
            }
            //seasons2
            if (!string.IsNullOrEmpty(dto.seasons2))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.seasons2) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.seasons2) + "_" + dto.feed_product_type].Contains(dto.seasons2))
                    {
                        msgDto.seasons2 = "Please see the Valid Values worksheet for the list of accepted values.";
                    }
                }
            }
            //seasons3
            if (!string.IsNullOrEmpty(dto.seasons3))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.seasons3) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.seasons3) + "_" + dto.feed_product_type].Contains(dto.seasons3))
                    {
                        msgDto.seasons3 = "Please see the Valid Values worksheet for the list of accepted values.";
                    }
                }
            }
            //seasons4
            if (!string.IsNullOrEmpty(dto.seasons4))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.seasons4) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.seasons4) + "_" + dto.feed_product_type].Contains(dto.seasons4))
                    {
                        msgDto.seasons4 = "Please see the Valid Values worksheet for the list of accepted values.";
                    }
                }
            }

            //lifestyle
            if (!string.IsNullOrEmpty(dto.lifestyle))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.lifestyle) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.lifestyle) + "_" + dto.feed_product_type].Contains(dto.lifestyle))
                    {
                        msgDto.lifestyle = "Please refer to the BTG.";
                    }
                }
            }

            //fit_type1
            if (!string.IsNullOrEmpty(dto.fit_type1))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.fit_type1) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.fit_type1) + "_" + dto.feed_product_type].Contains(dto.fit_type1))
                    {
                        msgDto.fit_type1 = "Select a value from the Valid Values worksheet.";
                    }
                }
            }

            //fit_type2
            if (!string.IsNullOrEmpty(dto.fit_type2))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.fit_type2) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.fit_type2) + "_" + dto.feed_product_type].Contains(dto.fit_type2))
                    {
                        msgDto.fit_type2 = "Select a value from the Valid Values worksheet.";
                    }
                }
            }
            //fit_type3
            if (!string.IsNullOrEmpty(dto.fit_type3))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.fit_type3) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.fit_type3) + "_" + dto.feed_product_type].Contains(dto.fit_type3))
                    {
                        msgDto.fit_type3 = "Select a value from the Valid Values worksheet.";
                    }
                }
            }
            //fit_type4
            if (!string.IsNullOrEmpty(dto.fit_type4))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.fit_type4) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.fit_type4) + "_" + dto.feed_product_type].Contains(dto.fit_type4))
                    {
                        msgDto.fit_type4 = "Select a value from the Valid Values worksheet.";
                    }
                }
            }
            //fit_type5
            if (!string.IsNullOrEmpty(dto.fit_type5))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.fit_type5) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.fit_type5) + "_" + dto.feed_product_type].Contains(dto.fit_type5))
                    {
                        msgDto.fit_type5 = "Select a value from the Valid Values worksheet.";
                    }
                }
            }
            //weave_type
            if (!string.IsNullOrEmpty(dto.weave_type))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.weave_type) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.weave_type) + "_" + dto.feed_product_type].Contains(dto.weave_type))
                    {
                        msgDto.weave_type = "Provide the item's weave type.";
                    }
                }
            }

            //theme
            if (!string.IsNullOrEmpty(dto.theme))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.theme) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.theme) + "_" + dto.feed_product_type].Contains(dto.theme))
                    {
                        msgDto.theme = "Provide the item's weave type.";
                    }
                }
            }

            //special_size_type
            if (!string.IsNullOrEmpty(dto.special_size_type))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.special_size_type) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.special_size_type) + "_" + dto.feed_product_type].Contains(dto.special_size_type))
                    {
                        msgDto.special_size_type = "Select a value from the Valid Values worksheet.";
                    }
                }
            }

            //thesaurus_attribute_keywords1
            if (!string.IsNullOrEmpty(dto.thesaurus_attribute_keywords1))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.thesaurus_attribute_keywords1) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.thesaurus_attribute_keywords1) + "_" + dto.feed_product_type].Contains(dto.thesaurus_attribute_keywords1))
                    {
                        msgDto.thesaurus_attribute_keywords1 = "Select a value from the Valid Values worksheet.";
                    }
                }
            }

            //thesaurus_attribute_keywords2
            if (!string.IsNullOrEmpty(dto.thesaurus_attribute_keywords2))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.thesaurus_attribute_keywords2) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.thesaurus_attribute_keywords2) + "_" + dto.feed_product_type].Contains(dto.thesaurus_attribute_keywords2))
                    {
                        msgDto.thesaurus_attribute_keywords2 = "Select a value from the Valid Values worksheet.";
                    }
                }
            }

            //thesaurus_attribute_keywords3
            if (!string.IsNullOrEmpty(dto.thesaurus_attribute_keywords3))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.thesaurus_attribute_keywords3) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.thesaurus_attribute_keywords3) + "_" + dto.feed_product_type].Contains(dto.thesaurus_attribute_keywords3))
                    {
                        msgDto.thesaurus_attribute_keywords3 = "Select a value from the Valid Values worksheet.";
                    }
                }
            }

            //thesaurus_attribute_keywords4
            if (!string.IsNullOrEmpty(dto.thesaurus_attribute_keywords4))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.thesaurus_attribute_keywords4) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.thesaurus_attribute_keywords4) + "_" + dto.feed_product_type].Contains(dto.thesaurus_attribute_keywords4))
                    {
                        msgDto.thesaurus_attribute_keywords4 = "Select a value from the Valid Values worksheet.";
                    }
                }
            }
            //thesaurus_attribute_keywords5
            if (!string.IsNullOrEmpty(dto.thesaurus_attribute_keywords5))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.thesaurus_attribute_keywords5) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.thesaurus_attribute_keywords5) + "_" + dto.feed_product_type].Contains(dto.thesaurus_attribute_keywords5))
                    {
                        msgDto.thesaurus_attribute_keywords5 = "Select a value from the Valid Values worksheet.";
                    }
                }
            }

            //catalog_number
            if (!string.IsNullOrEmpty(dto.catalog_number))
            {
                if (!Utilities.IsValidInteger(dto.catalog_number))
                {
                    msgDto.catalog_number = " Positive Integer.";
                }
            }

            //specific_uses_keywords1
            if (!string.IsNullOrEmpty(dto.specific_uses_keywords1))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.specific_uses_keywords1) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.specific_uses_keywords1) + "_" + dto.feed_product_type].Contains(dto.specific_uses_keywords1))
                    {
                        msgDto.specific_uses_keywords1 = "Assign a value from the list on the Valid Values tab.";
                    }
                }
            }
            //specific_uses_keywords2
            if (!string.IsNullOrEmpty(dto.specific_uses_keywords2))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.specific_uses_keywords2) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.specific_uses_keywords2) + "_" + dto.feed_product_type].Contains(dto.specific_uses_keywords2))
                    {
                        msgDto.specific_uses_keywords2 = "Assign a value from the list on the Valid Values tab.";
                    }
                }
            }
            //specific_uses_keywords3
            if (!string.IsNullOrEmpty(dto.specific_uses_keywords3))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.specific_uses_keywords3) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.specific_uses_keywords3) + "_" + dto.feed_product_type].Contains(dto.specific_uses_keywords3))
                    {
                        msgDto.specific_uses_keywords3 = "Assign a value from the list on the Valid Values tab.";
                    }
                }
            }
            //specific_uses_keywords4
            if (!string.IsNullOrEmpty(dto.specific_uses_keywords4))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.specific_uses_keywords4) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.specific_uses_keywords4) + "_" + dto.feed_product_type].Contains(dto.specific_uses_keywords4))
                    {
                        msgDto.specific_uses_keywords4 = "Assign a value from the list on the Valid Values tab.";
                    }
                }
            }
            //specific_uses_keywords5
            if (!string.IsNullOrEmpty(dto.specific_uses_keywords5))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.specific_uses_keywords5) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.specific_uses_keywords5) + "_" + dto.feed_product_type].Contains(dto.specific_uses_keywords5))
                    {
                        msgDto.specific_uses_keywords5 = "Assign a value from the list on the Valid Values tab.";
                    }
                }
            }

            //band_material_type
            if (!string.IsNullOrEmpty(dto.band_material_type))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.band_material_type) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.band_material_type) + "_" + dto.feed_product_type].Contains(dto.band_material_type))
                    {
                        msgDto.band_material_type = "Assign a value from the list on the Valid Values tab.";
                    }
                }
            }
            //case_thickness
            if (!string.IsNullOrEmpty(dto.case_thickness))
            {
                if (!Utilities.ValidateNumber(dto.case_thickness, 10, 2))
                {
                    msgDto.case_thickness = "A number with up to 10 digits allowed to the left of the decimal point and 2 digits allowed to the right of the decimal point. Please do not use commas or dollar signs.";
                }
            }

            //band_color
            if (!string.IsNullOrEmpty(dto.band_color))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.band_color) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.band_color) + "_" + dto.feed_product_type].Contains(dto.band_color))
                    {
                        msgDto.band_color = "Select a value from the Valid Values sheet.";
                    }
                }
            }

            //case_material_type
            if (!string.IsNullOrEmpty(dto.case_material_type))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.case_material_type) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.case_material_type) + "_" + dto.feed_product_type].Contains(dto.case_material_type))
                    {
                        msgDto.case_material_type = "Select a value from the Valid Values sheet.";
                    }
                }
            }

            //water_resistance_depth
            if (!string.IsNullOrEmpty(dto.water_resistance_depth))
            {
                if (!Utilities.ValidateNumber(dto.water_resistance_depth, 10, 2))
                {
                    msgDto.water_resistance_depth = "A number with up to 10 digits allowed to the left of the decimal point and 2 digits allowed to the right of the decimal point. Please do not use commas or dollar signs.";
                }
            }

            //water_resistance_depth_unit_of_measure
            if (!string.IsNullOrEmpty(dto.water_resistance_depth_unit_of_measure))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.water_resistance_depth_unit_of_measure) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.water_resistance_depth_unit_of_measure) + "_" + dto.feed_product_type].Contains(dto.water_resistance_depth_unit_of_measure))
                    {
                        msgDto.water_resistance_depth_unit_of_measure = "Assign a value from the list on the Valid Values tab.";
                    }
                }
            }

            //special_features1
            if (!string.IsNullOrEmpty(dto.special_features1))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.special_features1) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.special_features1) + "_" + dto.feed_product_type].Contains(dto.special_features1))
                    {
                        msgDto.special_features1 = "Assign a value from the list on the Valid Values tab.";
                    }
                }
            }

            //special_features2
            if (!string.IsNullOrEmpty(dto.special_features2))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.special_features2) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.special_features2) + "_" + dto.feed_product_type].Contains(dto.special_features2))
                    {
                        msgDto.special_features2 = "Assign a value from the list on the Valid Values tab.";
                    }
                }
            }

            //special_features3
            if (!string.IsNullOrEmpty(dto.special_features3))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.special_features3) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.special_features3) + "_" + dto.feed_product_type].Contains(dto.special_features3))
                    {
                        msgDto.special_features3 = "Assign a value from the list on the Valid Values tab.";
                    }
                }
            }

            //special_features4
            if (!string.IsNullOrEmpty(dto.special_features4))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.special_features4) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.special_features4) + "_" + dto.feed_product_type].Contains(dto.special_features4))
                    {
                        msgDto.special_features4 = "Assign a value from the list on the Valid Values tab.";
                    }
                }
            }

            //special_features5
            if (!string.IsNullOrEmpty(dto.special_features5))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.special_features5) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.special_features5) + "_" + dto.feed_product_type].Contains(dto.special_features5))
                    {
                        msgDto.special_features5 = "Assign a value from the list on the Valid Values tab.";
                    }
                }
            }

            //case_diameter_unit_of_measure
            if (!string.IsNullOrEmpty(dto.case_diameter_unit_of_measure))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.case_diameter_unit_of_measure) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.case_diameter_unit_of_measure) + "_" + dto.feed_product_type].Contains(dto.case_diameter_unit_of_measure))
                    {
                        msgDto.case_diameter_unit_of_measure = "Assign a value from the list on the Valid Values tab.";
                    }
                }
            }

            //target_audience_base1
            if (!string.IsNullOrEmpty(dto.target_audience_base1))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.target_audience_base1) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.target_audience_base1) + "_" + dto.feed_product_type].Contains(dto.target_audience_base1))
                    {
                        msgDto.target_audience_base1 = "Assign a value from the list on the Valid Values tab.";
                    }
                }
            }

            //target_audience_base2
            if (!string.IsNullOrEmpty(dto.target_audience_base2))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.target_audience_base2) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.target_audience_base2) + "_" + dto.feed_product_type].Contains(dto.target_audience_base2))
                    {
                        msgDto.target_audience_base2 = "Assign a value from the list on the Valid Values tab.";
                    }
                }
            }

            //target_audience_base3
            if (!string.IsNullOrEmpty(dto.target_audience_base3))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.target_audience_base3) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.target_audience_base3) + "_" + dto.feed_product_type].Contains(dto.target_audience_base3))
                    {
                        msgDto.target_audience_base3 = "Assign a value from the list on the Valid Values tab.";
                    }
                }
            }
            //target_audience_base4
            if (!string.IsNullOrEmpty(dto.target_audience_base4))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.target_audience_base4) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.target_audience_base4) + "_" + dto.feed_product_type].Contains(dto.target_audience_base4))
                    {
                        msgDto.target_audience_base4 = "Assign a value from the list on the Valid Values tab.";
                    }
                }
            }
            //target_audience_base5
            if (!string.IsNullOrEmpty(dto.target_audience_base5))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.target_audience_base5) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.target_audience_base5) + "_" + dto.feed_product_type].Contains(dto.target_audience_base5))
                    {
                        msgDto.target_audience_base5 = "Assign a value from the list on the Valid Values tab.";
                    }
                }
            }

            //band_length_unit
            if (!string.IsNullOrEmpty(dto.band_length_unit))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.band_length_unit) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.band_length_unit) + "_" + dto.feed_product_type].Contains(dto.band_length_unit))
                    {
                        msgDto.band_length_unit = "Assign a value from the list on the Valid Values tab.";
                    }
                }
            }

            //pearl_type
            if (!string.IsNullOrEmpty(dto.pearl_type))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.pearl_type) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.pearl_type) + "_" + dto.feed_product_type].Contains(dto.pearl_type))
                    {
                        msgDto.pearl_type = "Assign a value from the list on the Valid Values tab.";
                    }
                }
            }

            //pearl_uniformity
            if (!string.IsNullOrEmpty(dto.pearl_uniformity))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.pearl_uniformity) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.pearl_uniformity) + "_" + dto.feed_product_type].Contains(dto.pearl_uniformity))
                    {
                        msgDto.pearl_uniformity = "Assign a value from the list on the Valid Values tab.";
                    }
                }
            }

            //pearl_shape
            if (!string.IsNullOrEmpty(dto.pearl_shape))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.pearl_shape) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.pearl_shape) + "_" + dto.feed_product_type].Contains(dto.pearl_shape))
                    {
                        msgDto.pearl_shape = "Shape of the Pearl in the Earring";
                    }
                }
            }

            //pearl_surface_blemishes
            if (!string.IsNullOrEmpty(dto.pearl_surface_blemishes))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.pearl_surface_blemishes) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.pearl_surface_blemishes) + "_" + dto.feed_product_type].Contains(dto.pearl_surface_blemishes))
                    {
                        msgDto.pearl_surface_blemishes = "Any surface markings on the Pearl";
                    }
                }
            }

            //water_resistance_level
            if (!string.IsNullOrEmpty(dto.water_resistance_level))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.water_resistance_level) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.water_resistance_level) + "_" + dto.feed_product_type].Contains(dto.water_resistance_level))
                    {
                        msgDto.water_resistance_level = "Assign a value from the list on the Valid Values tab.";
                    }
                }
            }

            //directions
            if (!string.IsNullOrEmpty(dto.directions))
            {
                if (!Utilities.IsValidAlphanumericString(dto.directions, 500))
                {
                    msgDto.directions = "An alphanumeric string; 1 character minimum in length and 500 character maximum in length.";
                }
            }

            //paint_type
            if (!string.IsNullOrEmpty(dto.paint_type))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.paint_type) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.paint_type) + "_" + dto.feed_product_type].Contains(dto.paint_type))
                    {
                        msgDto.paint_type = "Please refer to the BTG.";
                    }
                }
            }

            //number_of_pieces
            if (!string.IsNullOrEmpty(dto.number_of_pieces))
            {
                if (!Utilities.IsValidInteger(dto.number_of_pieces))
                {
                    msgDto.number_of_pieces = " Positive Integer.";
                }
            }

            //is_assembly_required
            if (!string.IsNullOrEmpty(dto.is_assembly_required))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.is_assembly_required) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.is_assembly_required) + "_" + dto.feed_product_type].Contains(dto.is_assembly_required))
                    {
                        msgDto.is_assembly_required = "Select true or false.";
                    }
                }
            }

            //included_components1
            if (!string.IsNullOrEmpty(dto.included_components1))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.included_components1) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.included_components1) + "_" + dto.feed_product_type].Contains(dto.included_components1))
                    {
                        msgDto.included_components1 = "Select in valid value .";
                    }
                }
            }

            //included_components2
            if (!string.IsNullOrEmpty(dto.included_components2))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.included_components2) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.included_components2) + "_" + dto.feed_product_type].Contains(dto.included_components2))
                    {
                        msgDto.included_components2 = "Select in valid value .";
                    }
                }
            }

            //included_components3
            if (!string.IsNullOrEmpty(dto.included_components3))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.included_components3) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.included_components3) + "_" + dto.feed_product_type].Contains(dto.included_components3))
                    {
                        msgDto.included_components3 = "Select in valid value .";
                    }
                }
            }

            //included_components4
            if (!string.IsNullOrEmpty(dto.included_components4))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.included_components4) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.included_components4) + "_" + dto.feed_product_type].Contains(dto.included_components4))
                    {
                        msgDto.included_components4 = "Select in valid value .";
                    }
                }
            }

            //included_components1
            if (!string.IsNullOrEmpty(dto.included_components5))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.included_components5) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.included_components5) + "_" + dto.feed_product_type].Contains(dto.included_components5))
                    {
                        msgDto.included_components5 = "Select in valid value .";
                    }
                }
            }

            //is_resizable
            if (!string.IsNullOrEmpty(dto.is_resizable))
            {
                if (!listModelfromExcel[nameof(dto.is_resizable)].Contains(dto.is_resizable))
                {
                    msgDto.is_resizable = "Select: true or false.";
                }
            }

            //pearl_lustre
            if (!string.IsNullOrEmpty(dto.pearl_lustre))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.pearl_lustre) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.pearl_lustre) + "_" + dto.feed_product_type].Contains(dto.pearl_lustre))
                    {
                        msgDto.pearl_lustre = "Luster of the Pearl in the Earring";
                    }
                }
            }

            //pearl_stringing_method
            if (!string.IsNullOrEmpty(dto.pearl_stringing_method))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.pearl_stringing_method) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.pearl_stringing_method) + "_" + dto.feed_product_type].Contains(dto.pearl_stringing_method))
                    {
                        msgDto.pearl_stringing_method = "Method of Pearl Stringing";
                    }
                }
            }

            //pearl_minimum_color
            if (!string.IsNullOrEmpty(dto.pearl_minimum_color))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.pearl_minimum_color) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.pearl_minimum_color) + "_" + dto.feed_product_type].Contains(dto.pearl_minimum_color))
                    {
                        msgDto.pearl_minimum_color = "Assign a value from the list on the Valid Values tab.";
                    }
                }
            }

            //number_of_pearls
            if (!string.IsNullOrEmpty(dto.number_of_pearls))
            {
                if (!Utilities.IsValidInteger(dto.number_of_pearls))
                {
                    msgDto.number_of_pearls = " Positive Integer.";
                }
            }

            //index_suppressed
            if (!string.IsNullOrEmpty(dto.index_suppressed))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.index_suppressed) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.index_suppressed) + "_" + dto.feed_product_type].Contains(dto.index_suppressed))
                    {
                        msgDto.index_suppressed = "Assign a value from the list on the Valid Values tab.";
                    }
                }
            }

            //number_of_stones
            if (!string.IsNullOrEmpty(dto.number_of_stones))
            {
                if (!Utilities.IsValidInteger(dto.number_of_stones))
                {
                    msgDto.number_of_stones = "Positive Integer";
                }
            }

            //duration_unit_of_measure
            if (!string.IsNullOrEmpty(dto.duration_unit_of_measure))
            {
                if (!listModelfromExcel[nameof(dto.duration_unit_of_measure)].Contains(dto.duration_unit_of_measure))
                {
                    msgDto.duration_unit_of_measure = "Assign a value from the list on the Valid Values tab for the Ring Size attribute.";
                }
            }

            //subject_character
            if (!string.IsNullOrEmpty(dto.subject_character))
            {
                if (!listModelfromExcel[nameof(dto.subject_character)].Contains(dto.subject_character))
                {
                    msgDto.subject_character = "Assign a value from the list on the Valid Values tab for the Ring Size attribute.";
                }
            }

            //ring_sizing_lower_range
            if (!string.IsNullOrEmpty(dto.ring_sizing_lower_range))
            {
                if (!listModelfromExcel[nameof(dto.ring_size)].Contains(dto.ring_sizing_lower_range))
                {
                    msgDto.ring_sizing_lower_range = "Assign a value from the list on the Valid Values tab for the Ring Size attribute.";
                }
            }

            //ring_sizing_upper_range
            if (!string.IsNullOrEmpty(dto.ring_sizing_upper_range))
            {
                if (!listModelfromExcel[nameof(dto.ring_size)].Contains(dto.ring_sizing_upper_range))
                {
                    msgDto.ring_sizing_upper_range = "Assign a value from the list on the Valid Values tab for the Ring Size attribute.";
                }
            }

            //duration_unit_of_measure
            if (!string.IsNullOrEmpty(dto.duration_unit_of_measure))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.duration_unit_of_measure) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.duration_unit_of_measure) + "_" + dto.feed_product_type].Contains(dto.duration_unit_of_measure))
                    {
                        msgDto.duration_unit_of_measure = "Provide the corresponding unit.";
                    }
                }
            }

            //sleeve_type
            if (!string.IsNullOrEmpty(dto.sleeve_type))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.sleeve_type) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.sleeve_type) + "_" + dto.feed_product_type].Contains(dto.sleeve_type))
                    {
                        msgDto.sleeve_type = "Select a value from the Valid Values worksheet.";
                    }
                }
            }

            //fabric_wash
            if (!string.IsNullOrEmpty(dto.fabric_wash))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.fabric_wash) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.fabric_wash) + "_" + dto.feed_product_type].Contains(dto.fabric_wash))
                    {
                        msgDto.fabric_wash = "Select a value from the Valid Values worksheet.";
                    }
                }
            }

            //band_size
            if (!string.IsNullOrEmpty(dto.band_size))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.band_size) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.band_size) + "_" + dto.feed_product_type].Contains(dto.band_size))
                    {
                        msgDto.band_size = "Select a value from the Valid Values worksheet.";
                    }
                }
            }
            //dial_color
            if (!string.IsNullOrEmpty(dto.dial_color))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.dial_color) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.dial_color) + "_" + dto.feed_product_type].Contains(dto.dial_color))
                    {
                        msgDto.dial_color = "Select a value from the Valid Values worksheet.";
                    }
                }
            }

            //bezel_material_type
            if (!string.IsNullOrEmpty(dto.bezel_material_type))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.bezel_material_type) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.bezel_material_type) + "_" + dto.feed_product_type].Contains(dto.bezel_material_type))
                    {
                        msgDto.bezel_material_type = "Select a value from the Valid Values worksheet.";
                    }
                }
            }

            //bezel_function
            if (!string.IsNullOrEmpty(dto.bezel_function))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.bezel_function) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.bezel_function) + "_" + dto.feed_product_type].Contains(dto.bezel_function))
                    {
                        msgDto.bezel_function = "Select a value from the Valid Values worksheet.";
                    }
                }
            }
            //dial_window_material_type
            if (!string.IsNullOrEmpty(dto.dial_window_material_type))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.dial_window_material_type) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.dial_window_material_type) + "_" + dto.feed_product_type].Contains(dto.dial_window_material_type))
                    {
                        msgDto.dial_window_material_type = "Select a value from the Valid Values worksheet.";
                    }
                }
            }

            //calendar_type
            if (!string.IsNullOrEmpty(dto.calendar_type))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.calendar_type) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.calendar_type) + "_" + dto.feed_product_type].Contains(dto.calendar_type))
                    {
                        msgDto.calendar_type = "Select a value from the Valid Values worksheet.";
                    }
                }
            }

            //resale_type
            if (!string.IsNullOrEmpty(dto.resale_type))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.resale_type) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.resale_type) + "_" + dto.feed_product_type].Contains(dto.resale_type))
                    {
                        msgDto.resale_type = "Select a value from the Valid Values worksheet.";
                    }
                }
            }

            //authenticated_by
            if (!string.IsNullOrEmpty(dto.authenticated_by))
            {
                if (!Utilities.IsValidAlphanumericString(dto.authenticated_by, 50))
                {
                    msgDto.authenticated_by = "An alphanumeric string; 1 character minimum in length and 500 character maximum in length.";
                }
            }

            //finish_type1
            if (!string.IsNullOrEmpty(dto.finish_type1))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.finish_type1) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.finish_type1) + "_" + dto.feed_product_type].Contains(dto.finish_type1))
                    {
                        msgDto.finish_type1 = "Free text string,Select a value from the Valid Values worksheet.";
                    }
                }
            }

            //finish_type2
            if (!string.IsNullOrEmpty(dto.finish_type2))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.finish_type2) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.finish_type2) + "_" + dto.feed_product_type].Contains(dto.finish_type2))
                    {
                        msgDto.finish_type2 = "Free text string,Select a value from the Valid Values worksheet.";
                    }
                }
            }
            //fc_shelf_life_unit_of_measure
            if (!string.IsNullOrEmpty(dto.fc_shelf_life_unit_of_measure))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.fc_shelf_life_unit_of_measure) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.fc_shelf_life_unit_of_measure) + "_" + dto.feed_product_type].Contains(dto.fc_shelf_life_unit_of_measure))
                    {
                        msgDto.fc_shelf_life_unit_of_measure = "Select a value from the Valid Values worksheet.";
                    }
                }
            }

            //connectivity_protocol
            if (!string.IsNullOrEmpty(dto.connectivity_protocol))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.connectivity_protocol) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.connectivity_protocol) + "_" + dto.feed_product_type].Contains(dto.connectivity_protocol))
                    {
                        msgDto.connectivity_protocol = "Select a value from the Valid Values worksheet.";
                    }
                }
            }

            //item_pitch_unit_of_measure
            if (!string.IsNullOrEmpty(dto.item_pitch_unit_of_measure))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.item_pitch_unit_of_measure) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.item_pitch_unit_of_measure) + "_" + dto.feed_product_type].Contains(dto.item_pitch_unit_of_measure))
                    {
                        msgDto.item_pitch_unit_of_measure = "Select a value from the Valid Values worksheet.";
                    }
                }
            }

            //energy_guide_annual_operating_cost
            if (!string.IsNullOrEmpty(dto.energy_guide_annual_operating_cost))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.energy_guide_annual_operating_cost) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.energy_guide_annual_operating_cost) + "_" + dto.feed_product_type].Contains(dto.energy_guide_annual_operating_cost))
                    {
                        msgDto.energy_guide_annual_operating_cost = "Select a value from the Valid Values worksheet.";
                    }
                }
            }

            //is_oem_authorized
            if (!string.IsNullOrEmpty(dto.is_oem_authorized))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.is_oem_authorized) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.is_oem_authorized) + "_" + dto.feed_product_type].Contains(dto.is_oem_authorized))
                    {
                        msgDto.is_oem_authorized = "Select a value from the Valid Values worksheet.";
                    }
                }
            }

            //belt_width_unit_of_measure
            if (!string.IsNullOrEmpty(dto.belt_width_unit_of_measure))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.belt_width_unit_of_measure) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.belt_width_unit_of_measure) + "_" + dto.feed_product_type].Contains(dto.belt_width_unit_of_measure))
                    {
                        msgDto.belt_width_unit_of_measure = "Select a value from the Valid Values worksheet.";
                    }
                }
            }

            //map_policy
            if (!string.IsNullOrEmpty(dto.map_policy))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.map_policy) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.map_policy) + "_" + dto.feed_product_type].Contains(dto.map_policy))
                    {
                        msgDto.map_policy = "Select a value from the Valid Values worksheet.";
                    }
                }
            }

            //index_suppressed
            if (!string.IsNullOrEmpty(dto.index_suppressed))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.index_suppressed) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.index_suppressed) + "_" + dto.feed_product_type].Contains(dto.index_suppressed))
                    {
                        msgDto.index_suppressed = "Select a value from the Valid Values worksheet.";
                    }
                }
            }

            //leg_style
            if (!string.IsNullOrEmpty(dto.leg_style))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.leg_style) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.leg_style) + "_" + dto.feed_product_type].Contains(dto.leg_style))
                    {
                        msgDto.leg_style = "Select a value from the Valid Values worksheet.";
                    }
                }
            }
            //strap_type
            if (!string.IsNullOrEmpty(dto.strap_type))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.strap_type) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.strap_type) + "_" + dto.feed_product_type].Contains(dto.strap_type))
                    {
                        msgDto.strap_type = "Select a value from the Valid Values worksheet.";
                    }
                }
            }

            //underwire_type
            if (!string.IsNullOrEmpty(dto.underwire_type))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.underwire_type) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.underwire_type) + "_" + dto.feed_product_type].Contains(dto.underwire_type))
                    {
                        msgDto.underwire_type = "Select a value from the Valid Values worksheet.";
                    }
                }
            }

            //item_display_weight
            if (!string.IsNullOrEmpty(dto.item_display_weight))
            {
                if (!Utilities.ValidateNumber(dto.item_display_weight, 10, 2))
                {
                    dto.item_display_weight = "A number with up to 10 digits to the left of the decimal point and 2 digits to the right of the decimal point. Please do not use commas.";
                }
            }

            //total_diamond_weight
            if (!string.IsNullOrEmpty(dto.total_diamond_weight))
            {
                if (!Utilities.ValidateNumber(dto.total_diamond_weight, 12, 4))
                {
                    msgDto.total_diamond_weight = " A number with up to 12 digits to the left of the decimal point and 4 digits to the right of the decimal point. Please do not use commas.";
                }
            }

            //item_display_diameter
            if (!string.IsNullOrEmpty(dto.item_display_diameter))
            {
                if (!Utilities.ValidateNumber(dto.item_display_diameter, 12, 2))
                {
                    msgDto.item_display_diameter = "A number with up to 12 digits to the left of the decimal point and 2 digits to the right of the decimal point. Please do not use commas.";
                }
            }

            //stone_weight1
            if (!string.IsNullOrEmpty(dto.stone_weight1))
            {
                if (!Utilities.ValidateNumber(dto.stone_weight1, 12, 4))
                {
                    msgDto.stone_weight1 = " A number with up to 12 digits to the left of the decimal point and 4 digits to the right of the decimal point. Please do not use commas.";
                }
            }
            //stone_weight2
            if (!string.IsNullOrEmpty(dto.stone_weight2))
            {
                if (!Utilities.ValidateNumber(dto.stone_weight2, 12, 4))
                {
                    msgDto.stone_weight2 = " A number with up to 12 digits to the left of the decimal point and 4 digits to the right of the decimal point. Please do not use commas.";
                }
            }
            //stone_weight3
            if (!string.IsNullOrEmpty(dto.stone_weight3))
            {
                if (!Utilities.ValidateNumber(dto.stone_weight3, 12, 4))
                {
                    msgDto.stone_weight3 = " A number with up to 12 digits to the left of the decimal point and 4 digits to the right of the decimal point. Please do not use commas.";
                }
            }

            //stone_weight_unit_of_measure1
            if (!string.IsNullOrEmpty(dto.stone_weight_unit_of_measure1))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.stone_weight_unit_of_measure1) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.stone_weight_unit_of_measure1) + "_" + dto.feed_product_type].Contains(dto.stone_weight_unit_of_measure1))
                    {
                        msgDto.stone_weight_unit_of_measure1 = "Provide the corresponding weight unit of the stone.";
                    }
                }
            }
            //stone_weight_unit_of_measure2
            if (!string.IsNullOrEmpty(dto.stone_weight_unit_of_measure2))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.stone_weight_unit_of_measure2) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.stone_weight_unit_of_measure2) + "_" + dto.feed_product_type].Contains(dto.stone_weight_unit_of_measure2))
                    {
                        msgDto.stone_weight_unit_of_measure2 = "Provide the corresponding weight unit of the stone.";
                    }
                }
            }
            //stone_weight_unit_of_measure3
            if (!string.IsNullOrEmpty(dto.stone_weight_unit_of_measure3))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.stone_weight_unit_of_measure3) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.stone_weight_unit_of_measure3) + "_" + dto.feed_product_type].Contains(dto.stone_weight_unit_of_measure3))
                    {
                        msgDto.stone_weight_unit_of_measure3 = "Provide the corresponding weight unit of the stone.";
                    }
                }
            }

            //total_gem_weight
            if (!string.IsNullOrEmpty(dto.total_gem_weight))
            {
                if (!Utilities.ValidateNumber(dto.total_gem_weight, 12, 4))
                {
                    msgDto.total_gem_weight = "A number with up to 12 digits to the left of the decimal point and 4 digits to the right of the decimal point. Please do not use commas.";
                }
            }

            //size_per_pearl
            if (!string.IsNullOrEmpty(dto.size_per_pearl))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.size_per_pearl) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.size_per_pearl) + "_" + dto.feed_product_type].Contains(dto.size_per_pearl))
                    {
                        msgDto.size_per_pearl = "Assign a value from the list on the Valid Values tab.";
                    }
                }
            }


            //item_display_diameter_unit_of_measure
            if (!string.IsNullOrEmpty(dto.item_display_diameter_unit_of_measure))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.item_display_diameter_unit_of_measure) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.item_display_diameter_unit_of_measure) + "_" + dto.feed_product_type].Contains(dto.item_display_diameter_unit_of_measure))
                    {
                        msgDto.item_display_diameter_unit_of_measure = "Accepted units of measure are MM, CM, M, IN, FT.";
                    }
                }
            }

            //item_display_length
            if (!string.IsNullOrEmpty(dto.item_display_length))
            {
                if (!Utilities.ValidateNumber(dto.item_display_length, 10, 2))
                {
                    msgDto.item_display_length = "A number with up to 10 digits to the left of the decimal point and 2 digits to the right of the decimal point. Please do not use commas.";
                }
            }

            //item_display_length_unit_of_measure
            if (!string.IsNullOrEmpty(dto.item_display_length_unit_of_measure))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.item_display_length_unit_of_measure) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.item_display_length_unit_of_measure) + "_" + dto.feed_product_type].Contains(dto.item_display_length_unit_of_measure))
                    {
                        msgDto.item_display_length_unit_of_measure = "Select one of the following options: MM, CM, M, IN, FT";
                    }
                }
            }

            //total_gem_weight_unit_of_measure
            if (!string.IsNullOrEmpty(dto.total_gem_weight_unit_of_measure))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.total_gem_weight_unit_of_measure) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.total_gem_weight_unit_of_measure) + "_" + dto.feed_product_type].Contains(dto.total_gem_weight_unit_of_measure))
                    {
                        msgDto.total_gem_weight_unit_of_measure = "Select one of the following options:  MG, GR, OZ, DWT, CARATS.";
                    }
                }
            }

            //item_display_weight_unit_of_measure
            if (!string.IsNullOrEmpty(dto.item_display_weight_unit_of_measure))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.item_display_weight_unit_of_measure) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.item_display_weight_unit_of_measure) + "_" + dto.feed_product_type].Contains(dto.item_display_weight_unit_of_measure))
                    {
                        msgDto.item_display_weight_unit_of_measure = "Select one of the following options: GR, KG, OZ, LB.";
                    }
                }
            }

            //total_diamond_weight_unit_of_measure
            if (!string.IsNullOrEmpty(dto.total_diamond_weight_unit_of_measure))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.total_diamond_weight_unit_of_measure) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.total_diamond_weight_unit_of_measure) + "_" + dto.feed_product_type].Contains(dto.total_diamond_weight_unit_of_measure))
                    {
                        msgDto.total_diamond_weight_unit_of_measure = "Select one of the following options:  MG, GR, OZ, DWT, CARATS.";
                    }
                }
            }

            //band_width
            if (!string.IsNullOrEmpty(dto.band_width))
            {
                if (!Utilities.ValidateNumber(dto.band_width, 10, 2))
                {
                    msgDto.band_width = "A number with up to 10 digits to the left of the decimal point and 2 digits to the right of the decimal point. Please do not use commas.";
                }
            }

            //band_width_unit_of_measure
            if (!string.IsNullOrEmpty(dto.band_width_unit_of_measure))
            {
                if (!listModelfromExcel[nameof(dto.band_width_unit_of_measure)].Contains(dto.band_width_unit_of_measure))
                {
                    msgDto.band_width_unit_of_measure = "Select the corresponding unit.";
                }
            }

            //item_display_width
            if (!string.IsNullOrEmpty(dto.item_display_width))
            {
                if (!Utilities.ValidateNumber(dto.item_display_width, 12, 2))
                {
                    msgDto.item_display_width = "A number with up to 12 digits to the left of the decimal point and 2 digits to the right of the decimal point. Please do not use commas.";
                }
            }
            //website_shipping_weight
            if (!string.IsNullOrEmpty(dto.website_shipping_weight))
            {
                if (!Utilities.ValidateNumber(dto.website_shipping_weight, 10, 2))
                {
                    msgDto.website_shipping_weight = "A number with up to 10 digits to the left of the decimal point and 2 digits to the right of the decimal point. Please do not use commas.";
                }
            }
            //website_shipping_weight_unit_of_measure
            if (!string.IsNullOrEmpty(dto.website_shipping_weight_unit_of_measure))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.website_shipping_weight_unit_of_measure) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.website_shipping_weight_unit_of_measure) + "_" + dto.feed_product_type].Contains(dto.website_shipping_weight_unit_of_measure))
                    {
                        msgDto.website_shipping_weight_unit_of_measure = "Select the shipping unit of measure for the weight of this product.";
                    }
                }
            }

            //maximum_size_unit_of_measure
            if (!string.IsNullOrEmpty(dto.maximum_size_unit_of_measure))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.maximum_size_unit_of_measure) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.maximum_size_unit_of_measure) + "_" + dto.feed_product_type].Contains(dto.maximum_size_unit_of_measure))
                    {
                        msgDto.maximum_size_unit_of_measure = "Select the shipping unit of measure for the weight of this product.";
                    }
                }
            }

            //line_size_unit_of_measure
            if (!string.IsNullOrEmpty(dto.line_size_unit_of_measure))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.line_size_unit_of_measure) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.line_size_unit_of_measure) + "_" + dto.feed_product_type].Contains(dto.line_size_unit_of_measure))
                    {
                        msgDto.line_size_unit_of_measure = "Select the shipping unit of measure for the weight of this product.";
                    }
                }
            }

            //stone_dimensions_unit_of_measure1
            if (!string.IsNullOrEmpty(dto.stone_dimensions_unit_of_measure1))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.stone_dimensions_unit_of_measure1) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.stone_dimensions_unit_of_measure1) + "_" + dto.feed_product_type].Contains(dto.stone_dimensions_unit_of_measure1))
                    {
                        msgDto.stone_dimensions_unit_of_measure1 = "Select the unit of measure for the diameter, length, width, and height of the product.";
                    }
                }
            }
            //stone_dimensions_unit_of_measure2
            if (!string.IsNullOrEmpty(dto.stone_dimensions_unit_of_measure2))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.stone_dimensions_unit_of_measure2) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.stone_dimensions_unit_of_measure2) + "_" + dto.feed_product_type].Contains(dto.stone_dimensions_unit_of_measure2))
                    {
                        msgDto.stone_dimensions_unit_of_measure2 = "Select the unit of measure for the diameter, length, width, and height of the product.";
                    }
                }
            }
            //stone_dimensions_unit_of_measure3
            if (!string.IsNullOrEmpty(dto.stone_dimensions_unit_of_measure3))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.stone_dimensions_unit_of_measure3) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.stone_dimensions_unit_of_measure3) + "_" + dto.feed_product_type].Contains(dto.stone_dimensions_unit_of_measure3))
                    {
                        msgDto.stone_dimensions_unit_of_measure3 = "Select the unit of measure for the diameter, length, width, and height of the product.";
                    }
                }
            }

            //item_display_height_unit_of_measure
            if (!string.IsNullOrEmpty(dto.item_display_height_unit_of_measure))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.item_display_height_unit_of_measure) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.item_display_height_unit_of_measure) + "_" + dto.feed_product_type].Contains(dto.item_display_height_unit_of_measure))
                    {
                        msgDto.item_display_height_unit_of_measure = "Indicate the unit of measure for the item.  If PeakHeight is populated, you must populate PeakHeightUnitOfMeasure.";
                    }
                }
            }
            //item_display_width_unit_of_measure
            if (!string.IsNullOrEmpty(dto.item_display_width_unit_of_measure))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.item_display_width_unit_of_measure) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.item_display_width_unit_of_measure) + "_" + dto.feed_product_type].Contains(dto.item_display_width_unit_of_measure))
                    {
                        msgDto.item_display_width_unit_of_measure = "Indicate the unit of measure for the item.  If PeakHeight is populated, you must populate PeakHeightUnitOfMeasure.";
                    }
                }
            }
            //item_display_height
            if (!string.IsNullOrEmpty(dto.item_display_height))
            {
                if (!Utilities.IsValidDecimal(dto.item_display_height))
                {
                    msgDto.item_display_height = "A number that can include 2 decimal places (e.g., 10.15 or 145.00).";
                }
            }

            //stone_width_unit_of_measure1
            if (!string.IsNullOrEmpty(dto.stone_width_unit_of_measure1))
            {
                if (!listModelfromExcel[nameof(dto.stone_width_unit_of_measure1)].Contains(dto.stone_width_unit_of_measure1))
                {
                    msgDto.stone_width_unit_of_measure1 = "Select the corresponding unit.";
                }
            }
            //stone_width_unit_of_measure2
            if (!string.IsNullOrEmpty(dto.stone_width_unit_of_measure2))
            {
                if (!listModelfromExcel[nameof(dto.stone_width_unit_of_measure2)].Contains(dto.stone_width_unit_of_measure2))
                {
                    msgDto.stone_width_unit_of_measure2 = "Select the corresponding unit.";
                }
            }
            //stone_width_unit_of_measure3
            if (!string.IsNullOrEmpty(dto.stone_width_unit_of_measure3))
            {
                if (!listModelfromExcel[nameof(dto.stone_width_unit_of_measure3)].Contains(dto.stone_width_unit_of_measure3))
                {
                    msgDto.stone_width_unit_of_measure3 = "Select the corresponding unit.";
                }
            }


            //measurement_system
            if (!string.IsNullOrEmpty(dto.measurement_system))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.measurement_system) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.measurement_system) + "_" + dto.feed_product_type].Contains(dto.measurement_system))
                    {
                        msgDto.measurement_system = "Select a value from the Valid Values worksheet.";
                    }
                }
            }
            //maximum_weight_recommendation_unit_of_measure
            if (!string.IsNullOrEmpty(dto.maximum_weight_recommendation_unit_of_measure))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.maximum_weight_recommendation_unit_of_measure) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.maximum_weight_recommendation_unit_of_measure) + "_" + dto.feed_product_type].Contains(dto.maximum_weight_recommendation_unit_of_measure))
                    {
                        msgDto.maximum_weight_recommendation_unit_of_measure = "Select a value from the Valid Values worksheet.";
                    }
                }
            }

            //stone_length_unit_of_measure1
            if (!string.IsNullOrEmpty(dto.stone_length_unit_of_measure1))
            {
                if (!listModelfromExcel[nameof(dto.stone_length_unit_of_measure1)].Contains(dto.stone_length_unit_of_measure1))
                {
                    msgDto.stone_length_unit_of_measure1 = "Select the corresponding unit.";
                }
            }
            //stone_length_unit_of_measure2
            if (!string.IsNullOrEmpty(dto.stone_length_unit_of_measure2))
            {
                if (!listModelfromExcel[nameof(dto.stone_length_unit_of_measure2)].Contains(dto.stone_length_unit_of_measure2))
                {
                    msgDto.stone_length_unit_of_measure2 = "Select the corresponding unit.";
                }
            }
            //stone_length_unit_of_measure3
            if (!string.IsNullOrEmpty(dto.stone_length_unit_of_measure3))
            {
                if (!listModelfromExcel[nameof(dto.stone_length_unit_of_measure3)].Contains(dto.stone_length_unit_of_measure3))
                {
                    msgDto.stone_length_unit_of_measure3 = "Select the corresponding unit.";
                }
            }

            //total_metal_weight
            if (!string.IsNullOrEmpty(dto.total_metal_weight))
            {
                if (!Utilities.ValidateNumber(dto.total_metal_weight, 12, 4))
                {
                    msgDto.total_metal_weight = "A number with up to 12 digits to the left of the decimal point and 4 digits to the right of the decimal point. Please do not use commas.";
                }
            }

            //total_metal_weight_unit_of_measure
            if (!string.IsNullOrEmpty(dto.total_metal_weight_unit_of_measure))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.total_metal_weight_unit_of_measure) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.total_metal_weight_unit_of_measure) + "_" + dto.feed_product_type].Contains(dto.total_metal_weight_unit_of_measure))
                    {
                        msgDto.total_metal_weight_unit_of_measure = "Select from the list of valid values.";
                    }
                }
            }

            //item_thickness_unit_of_measure
            if (!string.IsNullOrEmpty(dto.item_thickness_unit_of_measure))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.item_thickness_unit_of_measure) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.item_thickness_unit_of_measure) + "_" + dto.feed_product_type].Contains(dto.item_thickness_unit_of_measure))
                    {
                        msgDto.item_thickness_unit_of_measure = "Select from the list of valid values.";
                    }
                }
            }

            //band_size_num_unit_of_measure
            if (!string.IsNullOrEmpty(dto.band_size_num_unit_of_measure))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.band_size_num_unit_of_measure) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.band_size_num_unit_of_measure) + "_" + dto.feed_product_type].Contains(dto.band_size_num_unit_of_measure))
                    {
                        msgDto.band_size_num_unit_of_measure = "Select from the list of valid values.";
                    }
                }
            }

            //waist_size_unit_of_measure
            if (!string.IsNullOrEmpty(dto.waist_size_unit_of_measure))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.waist_size_unit_of_measure) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.waist_size_unit_of_measure) + "_" + dto.feed_product_type].Contains(dto.waist_size_unit_of_measure))
                    {
                        msgDto.waist_size_unit_of_measure = "Select from the list of valid values.";
                    }
                }
            }

            //item_shape
            if (!string.IsNullOrEmpty(dto.item_shape))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.item_shape) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.item_shape) + "_" + dto.feed_product_type].Contains(dto.item_shape))
                    {
                        msgDto.item_shape = "The shape of the item.";
                    }
                }
            }


            //fulfillment_center_id
            if (!string.IsNullOrEmpty(dto.fulfillment_center_id))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.fulfillment_center_id) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.fulfillment_center_id) + "_" + dto.feed_product_type].Contains(dto.fulfillment_center_id))
                    {
                        msgDto.fulfillment_center_id = "Select one of the following options:  AMAZON_NA, DEFAULT";
                    }
                }
            }



            //regulation_type1
            if (!string.IsNullOrEmpty(dto.regulation_type1))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.regulation_type1) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.regulation_type1) + "_" + dto.feed_product_type].Contains(dto.regulation_type1))
                    {
                        msgDto.regulation_type1 = "Select applicable regulation type";
                    }
                }
            }
            //regulation_type2
            if (!string.IsNullOrEmpty(dto.regulation_type2))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.regulation_type2) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.regulation_type2) + "_" + dto.feed_product_type].Contains(dto.regulation_type2))
                    {
                        msgDto.regulation_type2 = "Select applicable regulation type";
                    }
                }
            }
            //regulation_type3
            if (!string.IsNullOrEmpty(dto.regulation_type3))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.regulation_type3) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.regulation_type3) + "_" + dto.feed_product_type].Contains(dto.regulation_type3))
                    {
                        msgDto.regulation_type3 = "Select applicable regulation type";
                    }
                }
            }
            //regulation_type4
            if (!string.IsNullOrEmpty(dto.regulation_type4))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.regulation_type4) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.regulation_type4) + "_" + dto.feed_product_type].Contains(dto.regulation_type4))
                    {
                        msgDto.regulation_type4 = "Select applicable regulation type";
                    }
                }
            }
            //regulation_type5
            if (!string.IsNullOrEmpty(dto.regulation_type5))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.regulation_type5) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.regulation_type5) + "_" + dto.feed_product_type].Contains(dto.regulation_type5))
                    {
                        msgDto.regulation_type5 = "Select applicable regulation type";
                    }
                }
            }

            //batteries_required  xử lý nếu batteries_required == false thì mọi trường batteries ko required
            if (!string.IsNullOrEmpty(dto.batteries_required))
            {
                var tmp = dto.batteries_required == "FALSE" ? "No" : "Yes";
                if (!listModelfromExcel[nameof(dto.batteries_required)].Contains(tmp))
                {
                    msgDto.batteries_required = "Select: true or false ";
                }
            }

            //are_batteries_included
            if (!string.IsNullOrEmpty(dto.are_batteries_included) && dto.batteries_required == "TRUE")
            {
                if (!listModelfromExcel[nameof(dto.are_batteries_included)].Contains(dto.are_batteries_included))
                {
                    msgDto.are_batteries_included = "Select: true or false ";
                }
            }
            //battery_cell_composition
            if (!string.IsNullOrEmpty(dto.battery_cell_composition) && dto.batteries_required == "TRUE")
            {
                if (!listModelfromExcel[nameof(dto.battery_cell_composition) + "_" + dto.feed_product_type].Contains(dto.battery_cell_composition))
                {
                    msgDto.battery_cell_composition = "Assign a value from the list on the Valid Values tab.";
                }
            }
            //battery_type1
            if (!string.IsNullOrEmpty(dto.battery_type1) && dto.batteries_required == "TRUE")
            {
                if (!listModelfromExcel[nameof(dto.battery_type1) + "_" + dto.feed_product_type].Contains(dto.battery_type1))
                {
                    msgDto.battery_type1 = "Please refer to the Valid Values worksheet.  Only use this when PowerSource is 'battery'.";
                }
            }
            //battery_type2
            if (!string.IsNullOrEmpty(dto.battery_type2) && dto.batteries_required == "TRUE")
            {
                if (!listModelfromExcel[nameof(dto.battery_type2) + "_" + dto.feed_product_type].Contains(dto.battery_type2))
                {
                    msgDto.battery_type2 = "Please refer to the Valid Values worksheet.  Only use this when PowerSource is 'battery'.";
                }
            }
            //battery_type3
            if (!string.IsNullOrEmpty(dto.battery_type3) && dto.batteries_required == "TRUE")
            {
                if (!listModelfromExcel[nameof(dto.battery_type3) + "_" + dto.feed_product_type].Contains(dto.battery_type3))
                {
                    msgDto.battery_type3 = "Please refer to the Valid Values worksheet.  Only use this when PowerSource is 'battery'.";
                }
            }
            //number_of_batteries1
            if (!string.IsNullOrEmpty(dto.number_of_batteries1) && dto.batteries_required == "TRUE")
            {
                if (!Utilities.IsValidInteger(dto.number_of_batteries1) && !(dto.number_of_batteries1.ToInt() >= 1))
                {
                    msgDto.number_of_batteries1 = "Any integer greater than or equal to one";
                }
            }

            //warranty_type
            if (!string.IsNullOrEmpty(dto.warranty_type))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.warranty_type) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.warranty_type) + "_" + dto.feed_product_type].Contains(dto.warranty_type))
                    {
                        msgDto.warranty_type = "Select a value from the Valid Values worksheet.";
                    }
                }
            }
            //legal_compliance_certification_status
            if (!string.IsNullOrEmpty(dto.legal_compliance_certification_status))
            {
                if (!listModelfromExcel[nameof(dto.legal_compliance_certification_status)].Contains(dto.legal_compliance_certification_status))
                {
                    msgDto.legal_compliance_certification_status = "If you have the required certification for compliance, select Compliant; if not, select, Non-compliant, and if not needed, select Exempt.";
                }
            }

            //flash_point
            if (!string.IsNullOrEmpty(dto.flash_point))
            {
                if (!Utilities.ValidateNumber(dto.flash_point, 10, 2))
                {
                    msgDto.flash_point = "A number with up to 10 digits to the left of the decimal point and 2 digits to the right of the decimal point. Please do not use commas.";
                }
            }

            //metal_stamp
            if (!string.IsNullOrEmpty(dto.metal_stamp))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.metal_stamp) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.metal_stamp) + "_" + dto.feed_product_type].Contains(dto.metal_stamp))
                    {
                        msgDto.metal_stamp = "Select from list of valid value.";
                    }
                }
            }

            //ghs_classification_class1
            if (!string.IsNullOrEmpty(dto.ghs_classification_class1))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.ghs_classification_class1) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.ghs_classification_class1) + "_" + dto.feed_product_type].Contains(dto.ghs_classification_class1))
                    {
                        msgDto.ghs_classification_class1 = "Global Harmonized System (GHS) CLP classification system.";
                    }
                }
            }
            //ghs_classification_class2
            if (!string.IsNullOrEmpty(dto.ghs_classification_class2))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.ghs_classification_class2) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.ghs_classification_class2) + "_" + dto.feed_product_type].Contains(dto.ghs_classification_class2))
                    {
                        msgDto.ghs_classification_class2 = "Global Harmonized System (GHS) CLP classification system.";
                    }
                }
            }
            //ghs_classification_class3
            if (!string.IsNullOrEmpty(dto.ghs_classification_class3))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.ghs_classification_class3) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.ghs_classification_class3) + "_" + dto.feed_product_type].Contains(dto.ghs_classification_class3))
                    {
                        msgDto.ghs_classification_class3 = "Global Harmonized System (GHS) CLP classification system.";
                    }
                }
            }
            //california_proposition_65_compliance_type
            if (!string.IsNullOrEmpty(dto.california_proposition_65_compliance_type))
            {
                if (!listModelfromExcel[nameof(dto.california_proposition_65_compliance_type)].Contains(dto.california_proposition_65_compliance_type))
                {
                    msgDto.california_proposition_65_compliance_type = "Select the warning type applicable to your product, if any. You certify that the warning provided satisfies legal requirements and that you’ll remove a warning previously provided only if it is no longer legally required.";
                }
            }

            //california_proposition_65_chemical_names1
            if (!string.IsNullOrEmpty(dto.california_proposition_65_chemical_names1))
            {
                if (!listModelfromExcel[nameof(dto.california_proposition_65_chemical_names1)].Contains(dto.california_proposition_65_chemical_names1))
                {
                    msgDto.california_proposition_65_chemical_names1 = "If you selected the Food, Furniture, or Chemical warning you must indicate a chemical(s). You certify that the chemical(s) satisfies legal requirements and that you’ll remove a chemical previously provided only if it is no longer legally required.";
                }
            }
            //california_proposition_65_chemical_names2
            if (!string.IsNullOrEmpty(dto.california_proposition_65_chemical_names2))
            {
                if (!listModelfromExcel[nameof(dto.california_proposition_65_chemical_names2)].Contains(dto.california_proposition_65_chemical_names2))
                {
                    msgDto.california_proposition_65_chemical_names2 = "If you selected the Food, Furniture, or Chemical warning you must indicate a chemical(s). You certify that the chemical(s) satisfies legal requirements and that you’ll remove a chemical previously provided only if it is no longer legally required.";
                }
            }
            //california_proposition_65_chemical_names3
            if (!string.IsNullOrEmpty(dto.california_proposition_65_chemical_names3))
            {
                if (!listModelfromExcel[nameof(dto.california_proposition_65_chemical_names3)].Contains(dto.california_proposition_65_chemical_names3))
                {
                    msgDto.california_proposition_65_chemical_names3 = "If you selected the Food, Furniture, or Chemical warning you must indicate a chemical(s). You certify that the chemical(s) satisfies legal requirements and that you’ll remove a chemical previously provided only if it is no longer legally required.";
                }
            }
            //california_proposition_65_chemical_names4
            if (!string.IsNullOrEmpty(dto.california_proposition_65_chemical_names4))
            {
                if (!listModelfromExcel[nameof(dto.california_proposition_65_chemical_names4)].Contains(dto.california_proposition_65_chemical_names4))
                {
                    msgDto.california_proposition_65_chemical_names4 = "If you selected the Food, Furniture, or Chemical warning you must indicate a chemical(s). You certify that the chemical(s) satisfies legal requirements and that you’ll remove a chemical previously provided only if it is no longer legally required.";
                }
            }
            //california_proposition_65_chemical_names5
            if (!string.IsNullOrEmpty(dto.california_proposition_65_chemical_names5))
            {
                if (!listModelfromExcel[nameof(dto.california_proposition_65_chemical_names5)].Contains(dto.california_proposition_65_chemical_names5))
                {
                    msgDto.california_proposition_65_chemical_names5 = "If you selected the Food, Furniture, or Chemical warning you must indicate a chemical(s). You certify that the chemical(s) satisfies legal requirements and that you’ll remove a chemical previously provided only if it is no longer legally required.";
                }
            }
            //pesticide_marking_type1
            if (!string.IsNullOrEmpty(dto.pesticide_marking_type1))
            {
                if (!listModelfromExcel[nameof(dto.pesticide_marking_type1)].Contains(dto.pesticide_marking_type1))
                {
                    msgDto.pesticide_marking_type1 = "Provide any pesticide marking on the item or packaging.";
                }

            }
            //pesticide_marking_type2
            if (!string.IsNullOrEmpty(dto.pesticide_marking_type2))
            {
                if (!listModelfromExcel[nameof(dto.pesticide_marking_type2)].Contains(dto.pesticide_marking_type2))
                {
                    msgDto.pesticide_marking_type2 = "Provide any pesticide marking on the item or packaging.";
                }

            }
            //pesticide_marking_type3
            if (!string.IsNullOrEmpty(dto.pesticide_marking_type3))
            {
                if (!listModelfromExcel[nameof(dto.pesticide_marking_type3)].Contains(dto.pesticide_marking_type3))
                {
                    msgDto.pesticide_marking_type3 = "Provide any pesticide marking on the item or packaging.";
                }

            }
            //pesticide_marking_registration_status1
            if (!string.IsNullOrEmpty(dto.pesticide_marking_registration_status1))
            {
                if (!listModelfromExcel[nameof(dto.pesticide_marking_registration_status1)].Contains(dto.pesticide_marking_registration_status1))
                {
                    msgDto.pesticide_marking_registration_status1 = "Provide a status whether the item requires registration and an associated registration number.";
                }

            }

            //pesticide_marking_registration_status2
            if (!string.IsNullOrEmpty(dto.pesticide_marking_registration_status2))
            {
                if (!listModelfromExcel[nameof(dto.pesticide_marking_registration_status2)].Contains(dto.pesticide_marking_registration_status2))
                {
                    msgDto.pesticide_marking_registration_status2 = "Provide a status whether the item requires registration and an associated registration number.";
                }

            }
            //pesticide_marking_registration_status3
            if (!string.IsNullOrEmpty(dto.pesticide_marking_registration_status3))
            {
                if (!listModelfromExcel[nameof(dto.pesticide_marking_registration_status3)].Contains(dto.pesticide_marking_registration_status3))
                {
                    msgDto.pesticide_marking_registration_status3 = "Provide a status whether the item requires registration and an associated registration number.";
                }

            }
            //fcc_radio_frequency_emission_compliance_registration_status
            if (!string.IsNullOrEmpty(dto.fcc_radio_frequency_emission_compliance_registration_status))
            {
                if (!listModelfromExcel[nameof(dto.fcc_radio_frequency_emission_compliance_registration_status)].Contains(dto.fcc_radio_frequency_emission_compliance_registration_status))
                {
                    msgDto.fcc_radio_frequency_emission_compliance_registration_status = "Indicate whether this product is capable of emitting radio frequency energy, and if so, what type of FCC RF equipment authorization this product has.";
                }

            }


            //merchant_release_date
            if (!string.IsNullOrEmpty(dto.merchant_release_date))
            {
                if (!Utilities.IsFormatDatetime(dto.merchant_release_date))
                {
                    msgDto.merchant_release_date = "Date in this format: yyyy-mm-dd";
                }
            }
            //list_price
            if (!string.IsNullOrEmpty(dto.list_price))
            {
                if (!Utilities.ValidateNumber(dto.list_price, 10, 2))
                {
                    msgDto.list_price = "Manufacturer's suggested retail price or list price.";
                }
            }
            //restock_date
            if (!string.IsNullOrEmpty(dto.restock_date))
            {
                if (!Utilities.IsFormatDatetime(dto.restock_date))
                {
                    msgDto.restock_date = "Date in this format: yyyy-mm-dd";
                }
            }

            //fulfillment_latency
            if (!string.IsNullOrEmpty(dto.fulfillment_latency))
            {
                if (!Utilities.IsValidAlphanumericString(dto.fulfillment_latency, 2))
                {
                    msgDto.fulfillment_latency = "A numeric string; 1 character minimum in length and 2 characters maximum in length.";
                }
            }

            //offering_end_date
            if (!string.IsNullOrEmpty(dto.offering_end_date))
            {
                if (!Utilities.IsFormatDatetime(dto.offering_end_date))
                {
                    msgDto.offering_end_date = "Date in this format: yyyy-mm-dd";
                }
            }

            //merchant_shipping_group_name
            if (!string.IsNullOrEmpty(dto.merchant_shipping_group_name))
            {
                if (!listModelfromExcel[nameof(dto.merchant_shipping_group_name)].Contains(dto.merchant_shipping_group_name))
                {
                    msgDto.merchant_shipping_group_name = "The ship configuration group for an offer. The ship configuration group is created and managed by the seller through the ship setting UI.";
                }
            }
            //offering_start_date
            if (!string.IsNullOrEmpty(dto.offering_start_date))
            {
                if (!Utilities.IsFormatDatetime(dto.offering_start_date))
                {
                    msgDto.offering_start_date = "The date you want to start selling this product on the website,Date in this format: yyyy-mm-dd";
                }
            }
            //product_tax_code
            if (!string.IsNullOrEmpty(dto.product_tax_code))
            {
                if (!listModelfromExcel[nameof(dto.product_tax_code)].Contains(dto.product_tax_code))
                {
                    msgDto.product_tax_code = "Enter the product tax code supplied to you by Amazon.com.";
                }
            }

            if (dto.offering_can_be_gift_messaged == "False")
            {
                dto.offering_can_be_gift_messaged = "No";
            }
            else
            {
                dto.offering_can_be_gift_messaged = "Yes";
            }
            //offering_can_be_gift_messaged
            if (!string.IsNullOrEmpty(dto.offering_can_be_gift_messaged))
            {
                if (!listModelfromExcel[nameof(dto.offering_can_be_gift_messaged)].Contains(dto.offering_can_be_gift_messaged))
                {
                    msgDto.offering_can_be_gift_messaged = "Select: true or false.";
                }
            }

            //condition_note
            if (!string.IsNullOrEmpty(dto.condition_note))
            {
                if (!Utilities.IsValidAlphanumericString(dto.condition_note, 1000))
                {
                    dto.condition_note = "A text string with a maximum of 1,000 characters. The Condition Note field doesn’t allow special characters.";
                }
            }


            //sale_price
            if (!string.IsNullOrEmpty(dto.sale_price))
            {
                if (!Utilities.IsValidDecimal(dto.sale_price))
                {
                    msgDto.sale_price = "Up to 18 digits. Commas and currency symbols cannot be used.";
                }
            }
            //sale_from_date
            if (!string.IsNullOrEmpty(dto.sale_from_date))
            {
                if (!Utilities.IsFormatDatetime(dto.sale_from_date))
                {
                    msgDto.sale_from_date = "The date that the sale price will begin to override the product's standard price; the sale price will be displayed after 0:00AM of Sale From Date.";
                }
            }
            //sale_end_date
            if (!string.IsNullOrEmpty(dto.sale_end_date))
            {
                if (!Utilities.IsFormatDatetime(dto.sale_end_date))
                {
                    msgDto.sale_end_date = "The last date that the sale price will override the item's standard price; the product's standard price will be displayed after 0:00AM of Sale End Date.";
                }
            }
            //product_site_launch_date
            if (!string.IsNullOrEmpty(dto.product_site_launch_date))
            {
                if (!Utilities.IsFormatDatetime(dto.product_site_launch_date))
                {
                    msgDto.product_site_launch_date = "This is the date when customers will start viewing this product on Amazon.";
                }
            }
            //business_price
            if (!string.IsNullOrEmpty(dto.business_price))
            {
                if (!Utilities.ValidateNumber(dto.business_price, 10, 2))
                {
                    msgDto.business_price = "The price at which the merchant offers the product to registered business buyers for sale, expressed in U.S. dollars. ";
                }
            }
            //quantity_price_type
            if (!string.IsNullOrEmpty(dto.quantity_price_type))
            {
                if (!listModelfromExcel[nameof(dto.quantity_price_type)].Contains(dto.quantity_price_type))
                {
                    msgDto.quantity_price_type = "The unit of measure the discount will be expressed in. Either Fixed price in U.S. dollars or Percent Off";
                }
            }
            //quantity_lower_bound1
            if (!string.IsNullOrEmpty(dto.quantity_lower_bound1))
            {
                if (!Utilities.IsValidInteger(dto.quantity_lower_bound1))
                {
                    msgDto.quantity_lower_bound1 = "The minimum purchase quantity necessary to receive the associated Fixed price or Percent Off price";
                }
            }
            //quantity_price1
            if (!string.IsNullOrEmpty(dto.quantity_price1))
            {
                if (!Utilities.ValidateNumber(dto.quantity_price1, 10, 2))
                {
                    msgDto.quantity_price1 = "The Fixed price or Percent Off discount at which the merchant offers the product for sale if the buyer is purchasing at least the associated quantity, expressed in U.S. dollars.";
                }
            }
            //progressive_discount_type
            if (!string.IsNullOrEmpty(dto.progressive_discount_type))
            {
                if (!listModelfromExcel[nameof(dto.progressive_discount_type)].Contains(dto.progressive_discount_type))
                {
                    msgDto.progressive_discount_type = "The unit of measure the discount will be expressed in either Fixed price in U.S. dollars or Percent Off.";
                }
            }
            //progressive_discount_lower_bound1
            if (!string.IsNullOrEmpty(dto.progressive_discount_lower_bound1))
            {
                if (!Utilities.IsValidInteger(dto.progressive_discount_lower_bound1))
                {
                    msgDto.progressive_discount_lower_bound1 = "The minimum purchase quantity necessary to receive the associated Fixed price or Percent Off price";
                }
            }
            //progressive_discount_value1
            if (!string.IsNullOrEmpty(dto.progressive_discount_value1))
            {
                if (!Utilities.ValidateNumber(dto.progressive_discount_value1, 10, 2))
                {
                    msgDto.progressive_discount_value1 = "The Fixed price or Percent Off discount at which the merchant offers the product for sale if the buyer is purchasing at least the associated quantity.";
                }
            }
            //national_stock_number
            if (!string.IsNullOrEmpty(dto.national_stock_number))
            {
                if (!Utilities.IsNATOCode(dto.national_stock_number))
                {
                    msgDto.national_stock_number = "A alpha-numeric 13 digit code. Code identifying standard material items of supply as defined by NATO";
                }
            }

            //unspsc_code
            if (!string.IsNullOrEmpty(dto.unspsc_code))
            {
                if (!Utilities.ValidateNumber(dto.unspsc_code, 8, 2))
                {
                    msgDto.unspsc_code = "An 8 digit code A numeric code classifying products according to the UNSPSC system.";
                }
            }

            return msgDto;
        }

        //private async Task UpdateDb(List<ValidValueVsiriusModel> lstUpdate)
        //{
        //    try
        //    {
        //        _context.ValidValueVsiriusModels.RemoveRange(_context.ValidValueVsiriusModels.ToList());

        //        _context.ValidValueVsiriusModels.AddRange(lstUpdate);
        //        await _context.SaveChangesAsync();

        //    }
        //    catch (Exception ex)
        //    {
        //        string err = ex.Message;

        //    }

        //}
    }
}
