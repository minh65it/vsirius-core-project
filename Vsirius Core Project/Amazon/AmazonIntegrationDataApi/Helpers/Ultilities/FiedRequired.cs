using AmazonIntegrationDataApi.Dtos;

namespace AmazonIntegrationDataApi.Helpers.Ultilities
{
    public class FiedRequired
    {
        public static AmazonJewelryDataFeedItemV3_Dto FiedRequireds(Dictionary<string, List<object>> listModelfromExcel, AmazonJewelryDataFeedItemV3_Dto dto)
        {
            AmazonJewelryDataFeedItemV3_Dto msgDto = new();
            //feed_product_type
            if (!string.IsNullOrEmpty(dto.feed_product_type))
            {
                if (!listModelfromExcel[nameof(dto.feed_product_type)].Contains(dto.feed_product_type))
                {
                    msgDto.feed_product_type = MessageErr.Exists;
                }
            }
            else
            {
                msgDto.feed_product_type = "Not Null";
            }
            //brand_name
            if (!string.IsNullOrEmpty(dto.brand_name))
            {
                if (!listModelfromExcel[nameof(dto.brand_name)].Contains(dto.brand_name.ToUpper()))
                {
                    msgDto.brand_name = MessageErr.Exists;
                }
            }
            else
            {
                msgDto.brand_name = "Not Null";
            }

            ////external_product_id_type
            //if (!string.IsNullOrEmpty(dto.external_product_id_type))
            //{
            //    if (listModelfromExcel.ContainsKey(nameof(dto.external_product_id_type) + "_" + dto.feed_product_type))
            //    {
            //        if (!listModelfromExcel[nameof(dto.external_product_id_type) + "_" + dto.feed_product_type].Contains(dto.external_product_id_type))
            //        {
            //            msgDto.external_product_id_type = "Select one of the following options:UPC,EAN,GCID,GTIN";
            //        }
            //    }
            //}
            ////external_product_id 
            //if (!string.IsNullOrEmpty(dto.external_product_id))
            //{
            //    switch (dto.external_product_id_type)
            //    {
            //        case "UPC":
            //            if (!(Utilities.IsValidUPC(dto.external_product_id)))
            //            {
            //                msgDto.external_product_id = "Any valid GCID, UPC, or EAN.";
            //            }
            //            break;
            //        case "EAN":
            //            if (!(Utilities.IsValidEAN(dto.external_product_id)))
            //            {
            //                msgDto.external_product_id = "Any valid GCID, UPC, or EAN.";
            //            }
            //            break;
            //        case "GCID":
            //            if (!(Utilities.IsValidGCID(dto.external_product_id)))
            //            {
            //                msgDto.external_product_id = "Any valid GCID, UPC, or EAN.";
            //            }
            //            break;
            //        case "":
            //                msgDto.external_product_id = "Any valid GCID, UPC, or EAN.";
            //            break;
            //        default:

            //            break;
            //    }
            //}

            if (!string.IsNullOrEmpty(dto.item_name))
            {
                if (!Utilities.IsValidLength(dto.item_name, 250))
                {
                    msgDto.item_name = string.Format(MessageErr.MaxLength, 250);
                }
            }

            //main_image_url
            if (!string.IsNullOrEmpty(dto.main_image_url))
            {
                if (!Utilities.IsValidImage(dto.main_image_url))
                {
                    msgDto.main_image_url = "The URL where a main image of the product is located.  It's important that this is supplied for all products.";
                }
            }

            //target_gender
            if (!string.IsNullOrEmpty(dto.target_gender))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.target_gender) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.target_gender) + "_" + dto.feed_product_type].Contains(dto.target_gender))
                    {
                        msgDto.target_gender = "Provide the target gender for the product";
                    }
                }
            }

            //quantity
            if (!string.IsNullOrEmpty(dto.quantity))
            {
                if (!Utilities.IsValidInteger(dto.quantity))
                {
                    msgDto.quantity = "A whole number.";
                }
            }

            //standard_price
            if (!string.IsNullOrEmpty(dto.standard_price))
            {
                if (!Utilities.isValidNotUseCurrency(dto.standard_price))
                {
                    msgDto.standard_price = "Up to 18 digits. Commas and currency symbols cannot be used.";
                }
            }

            //item_type
            if (!string.IsNullOrEmpty(dto.item_type))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.item_type) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.item_type) + "_" + dto.feed_product_type].Contains(dto.item_type))
                    {
                        msgDto.item_type = "Select an item type value from the BTG";
                    }
                }
            }

            //metal_type
            if (!string.IsNullOrEmpty(dto.metal_type))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.metal_type) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.metal_type) + "_" + dto.feed_product_type].Contains(dto.metal_type))
                    {
                        msgDto.item_type = "Assign a value from the list on the Valid Values tab for this attribute. Select ONLY from the values on that list, and enter one value exactly as shown.";
                    }
                }
            }
            //material_type1
            if (!string.IsNullOrEmpty(dto.material_type1))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.material_type1) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.material_type1) + "_" + dto.feed_product_type].Contains(dto.material_type1))
                    {
                        msgDto.material_type1 = "See valid values tab for specific options.";
                    }
                }
            }

            //material_type2
            if (!string.IsNullOrEmpty(dto.material_type2))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.material_type2) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.material_type2) + "_" + dto.feed_product_type].Contains(dto.material_type2))
                    {
                        msgDto.material_type2 = "See valid values tab for specific options.";
                    }
                }
            }

            //material_type3
            if (!string.IsNullOrEmpty(dto.material_type3))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.material_type3) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.material_type3) + "_" + dto.feed_product_type].Contains(dto.material_type3))
                    {
                        msgDto.material_type3 = "See valid values tab for specific options.";
                    }
                }
            }
            //material_type4
            if (!string.IsNullOrEmpty(dto.material_type4))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.material_type4) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.material_type4) + "_" + dto.feed_product_type].Contains(dto.material_type4))
                    {
                        msgDto.material_type4 = "See valid values tab for specific options.";
                    }
                }
            }
            //material_type5
            if (!string.IsNullOrEmpty(dto.material_type5))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.material_type5) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.material_type5) + "_" + dto.feed_product_type].Contains(dto.material_type5))
                    {
                        msgDto.material_type5 = "See valid values tab for specific options.";
                    }
                }
            }

            //department_name1
            if (!string.IsNullOrEmpty(dto.department_name1))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.department_name1) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.department_name1) + "_" + dto.feed_product_type].Contains(dto.department_name1))
                    {
                        msgDto.department_name1 = "Please select a value from the Valid Values tab.";
                    }
                }
            }

            //department_name2
            if (!string.IsNullOrEmpty(dto.department_name2))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.department_name2) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.department_name2) + "_" + dto.feed_product_type].Contains(dto.department_name2))
                    {
                        msgDto.department_name2 = "Please select a value from the Valid Values tab.";
                    }
                }
            }

            //department_name3
            if (!string.IsNullOrEmpty(dto.department_name3))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.department_name3) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.department_name3) + "_" + dto.feed_product_type].Contains(dto.department_name3))
                    {
                        msgDto.department_name3 = "Please select a value from the Valid Values tab.";
                    }
                }
            }

            //department_name4
            if (!string.IsNullOrEmpty(dto.department_name4))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.department_name4) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.department_name4) + "_" + dto.feed_product_type].Contains(dto.department_name4))
                    {
                        msgDto.department_name4 = "Please select a value from the Valid Values tab.";
                    }
                }
            }

            //department_name5
            if (!string.IsNullOrEmpty(dto.department_name5))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.department_name5) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.department_name5) + "_" + dto.feed_product_type].Contains(dto.department_name5))
                    {
                        msgDto.department_name5 = "Please select a value from the Valid Values tab.";
                    }
                }
            }

            //gem_type1
            //if (!string.IsNullOrEmpty(dto.gem_type1))
            //{
            //    if (listModelfromExcel.ContainsKey(nameof(dto.gem_type1) + "_" + dto.feed_product_type))
            //    {
            //        if (!listModelfromExcel[nameof(dto.gem_type1) + "_" + dto.feed_product_type].Contains(dto.gem_type1))
            //        {
            //            msgDto.gem_type1 = "Assign a value from the list on the Valid Values tab.";
            //        }
            //    }
            //}
            ////gem_type2
            //if (!string.IsNullOrEmpty(dto.gem_type2))
            //{
            //    if (listModelfromExcel.ContainsKey(nameof(dto.gem_type2) + "_" + dto.feed_product_type))
            //    {
            //        if (!listModelfromExcel[nameof(dto.gem_type2) + "_" + dto.feed_product_type].Contains(dto.gem_type2))
            //        {
            //            msgDto.gem_type2 = "Assign a value from the list on the Valid Values tab.";
            //        }
            //    }
            //}
            ////gem_type3
            //if (!string.IsNullOrEmpty(dto.gem_type3))
            //{
            //    if (listModelfromExcel.ContainsKey(nameof(dto.gem_type3) + "_" + dto.feed_product_type))
            //    {
            //        if (!listModelfromExcel[nameof(dto.gem_type3) + "_" + dto.feed_product_type].Contains(dto.gem_type3))
            //        {
            //            msgDto.gem_type3 = "Assign a value from the list on the Valid Values tab.";
            //        }
            //    }
            //}

            //color_map
            if (!string.IsNullOrEmpty(dto.color_map))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.color_map) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.color_map) + "_" + dto.feed_product_type].Contains(dto.color_map))
                    {
                        msgDto.color_map = "Please see the Valid Values worksheet for the list of accepted values.";
                    }
                }
            }


            //bullet_point1
            if (!string.IsNullOrEmpty(dto.bullet_point1))
            {
                if (!Utilities.isValidBulletPoint(dto.bullet_point1))
                {
                    msgDto.bullet_point1 = "We recommend four or five clear and concise bullet points; keeping to less than 1,000 characters total. " +
                        "Bullet points should focus on the features of your product, such as dimensions, materials, colours, " +
                        "ingredients, warnings, and benefits. Type 1 High ASCII characters such as ®, ©, ™, or other special characters are not supported.";
                }
            }

            //bullet_point2
            if (!string.IsNullOrEmpty(dto.bullet_point2))
            {
                if (!Utilities.isValidBulletPoint(dto.bullet_point2))
                {
                    msgDto.bullet_point2 = "We recommend four or five clear and concise bullet points; keeping to less than 1,000 characters total. " +
                        "Bullet points should focus on the features of your product, such as dimensions, materials, colours, " +
                        "ingredients, warnings, and benefits. Type 1 High ASCII characters such as ®, ©, ™, or other special characters are not supported.";
                }
            }
            //bullet_point3
            if (!string.IsNullOrEmpty(dto.bullet_point3))
            {
                if (!Utilities.isValidBulletPoint(dto.bullet_point3))
                {
                    msgDto.bullet_point3 = "We recommend four or five clear and concise bullet points; keeping to less than 1,000 characters total. " +
                        "Bullet points should focus on the features of your product, such as dimensions, materials, colours, " +
                        "ingredients, warnings, and benefits. Type 1 High ASCII characters such as ®, ©, ™, or other special characters are not supported.";
                }
            }
            //bullet_point4
            if (!string.IsNullOrEmpty(dto.bullet_point4))
            {
                if (!Utilities.isValidBulletPoint(dto.bullet_point4))
                {
                    msgDto.bullet_point4 = "We recommend four or five clear and concise bullet points; keeping to less than 1,000 characters total. " +
                        "Bullet points should focus on the features of your product, such as dimensions, materials, colours, " +
                        "ingredients, warnings, and benefits. Type 1 High ASCII characters such as ®, ©, ™, or other special characters are not supported.";
                }
            }
            //bullet_point5
            if (!string.IsNullOrEmpty(dto.bullet_point5))
            {
                if (!Utilities.isValidBulletPoint(dto.bullet_point5))
                {
                    msgDto.bullet_point5 = "We recommend four or five clear and concise bullet points; keeping to less than 1,000 characters total. " +
                        "Bullet points should focus on the features of your product, such as dimensions, materials, colours, " +
                        "ingredients, warnings, and benefits. Type 1 High ASCII characters such as ®, ©, ™, or other special characters are not supported.";
                }
            }

            //bullet_point6
            if (!string.IsNullOrEmpty(dto.bullet_point6))
            {
                if (!Utilities.isValidBulletPoint(dto.bullet_point6))
                {
                    msgDto.bullet_point6 = "We recommend four or five clear and concise bullet points; keeping to less than 1,000 characters total. " +
                        "Bullet points should focus on the features of your product, such as dimensions, materials, colours, " +
                        "ingredients, warnings, and benefits. Type 1 High ASCII characters such as ®, ©, ™, or other special characters are not supported.";
                }
            }
            //bullet_point7
            if (!string.IsNullOrEmpty(dto.bullet_point7))
            {
                if (!Utilities.isValidBulletPoint(dto.bullet_point7))
                {
                    msgDto.bullet_point7 = "We recommend four or five clear and concise bullet points; keeping to less than 1,000 characters total. " +
                        "Bullet points should focus on the features of your product, such as dimensions, materials, colours, " +
                        "ingredients, warnings, and benefits. Type 1 High ASCII characters such as ®, ©, ™, or other special characters are not supported.";
                }
            }

            //bullet_point8
            if (!string.IsNullOrEmpty(dto.bullet_point8))
            {
                if (!Utilities.isValidBulletPoint(dto.bullet_point8))
                {
                    msgDto.bullet_point8 = "We recommend four or five clear and concise bullet points; keeping to less than 1,000 characters total. " +
                        "Bullet points should focus on the features of your product, such as dimensions, materials, colours, " +
                        "ingredients, warnings, and benefits. Type 1 High ASCII characters such as ®, ©, ™, or other special characters are not supported.";
                }
            }
            //bullet_point9
            if (!string.IsNullOrEmpty(dto.bullet_point9))
            {
                if (!Utilities.isValidBulletPoint(dto.bullet_point9))
                {
                    msgDto.bullet_point9 = "We recommend four or five clear and concise bullet points; keeping to less than 1,000 characters total. " +
                        "Bullet points should focus on the features of your product, such as dimensions, materials, colours, " +
                        "ingredients, warnings, and benefits. Type 1 High ASCII characters such as ®, ©, ™, or other special characters are not supported.";
                }
            }

            //bullet_point10
            if (!string.IsNullOrEmpty(dto.bullet_point10))
            {
                if (!Utilities.isValidBulletPoint(dto.bullet_point10))
                {
                    msgDto.bullet_point10 = "We recommend four or five clear and concise bullet points; keeping to less than 1,000 characters total. " +
                        "Bullet points should focus on the features of your product, such as dimensions, materials, colours, " +
                        "ingredients, warnings, and benefits. Type 1 High ASCII characters such as ®, ©, ™, or other special characters are not supported.";
                }
            }
            //item_type_name
            if (!string.IsNullOrEmpty(dto.item_type_name))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.item_type_name) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.item_type_name) + "_" + dto.feed_product_type].Contains(dto.item_type_name))
                    {
                        msgDto.item_type_name = "Select an item type value from the BTG";
                    }
                }
            }

            //number_of_boxes
            if (!string.IsNullOrEmpty(dto.number_of_boxes))
            {
                if (!Utilities.IsValidInteger(dto.number_of_boxes))
                {
                    msgDto.number_of_boxes = "Not Is Number";
                }
            }

            //item_length_unit_of_measure
            if (!string.IsNullOrEmpty(dto.item_length_unit_of_measure))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.item_length_unit_of_measure) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.item_length_unit_of_measure) + "_" + dto.feed_product_type].Contains(dto.item_length_unit_of_measure))
                    {
                        msgDto.item_length_unit_of_measure = "Select one of the following options:  MM  CM  M  IN  FT";
                    }
                }
            }

            //item_width
            if (!string.IsNullOrEmpty(dto.item_width))
            {
                if (!Utilities.ValidateNumber(dto.item_width, 10, 2))
                {
                    msgDto.item_width = " A number with up to 10 digits to the left of the decimal point and 2 digits to the right of the decimal point. Please do not use commas.";
                }
            }
            //item_length
            if (!string.IsNullOrEmpty(dto.item_length))
            {
                if (!Utilities.ValidateNumber(dto.item_length, 10, 2))
                {
                    msgDto.item_length = " A number with up to 10 digits to the left of the decimal point and 2 digits to the right of the decimal point. Please do not use commas.";
                }
            }
            //item_height
            if (!string.IsNullOrEmpty(dto.item_height))
            {
                if (!Utilities.ValidateNumber(dto.item_height, 10, 2))
                {
                    msgDto.item_height = " A number with up to 10 digits to the left of the decimal point and 2 digits to the right of the decimal point. Please do not use commas.";
                }
            }

            //item_width_unit_of_measure
            if (!string.IsNullOrEmpty(dto.item_width_unit_of_measure))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.item_width_unit_of_measure) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.item_width_unit_of_measure) + "_" + dto.feed_product_type].Contains(dto.item_width_unit_of_measure))
                    {
                        msgDto.item_width_unit_of_measure = "Select from the list of valid values.";
                    }
                }
            }

            //item_height_unit_of_measure 
            if (!string.IsNullOrEmpty(dto.item_height_unit_of_measure))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.item_height_unit_of_measure) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.item_height_unit_of_measure) + "_" + dto.feed_product_type].Contains(dto.item_height_unit_of_measure))
                    {
                        msgDto.item_height_unit_of_measure = "Select from the list of valid values.";
                    }
                }
            }


            //supplier_declared_dg_hz_regulation1
            if (!string.IsNullOrEmpty(dto.supplier_declared_dg_hz_regulation1))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.supplier_declared_dg_hz_regulation1) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.supplier_declared_dg_hz_regulation1) + "_" + dto.feed_product_type].Contains(dto.supplier_declared_dg_hz_regulation1))
                    {
                        msgDto.supplier_declared_dg_hz_regulation1 = "Please select the applicable response from the dropdown.";
                    }
                }
            }
            //supplier_declared_dg_hz_regulation2
            if (!string.IsNullOrEmpty(dto.supplier_declared_dg_hz_regulation2))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.supplier_declared_dg_hz_regulation2) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.supplier_declared_dg_hz_regulation2) + "_" + dto.feed_product_type].Contains(dto.supplier_declared_dg_hz_regulation2))
                    {
                        msgDto.supplier_declared_dg_hz_regulation2 = "Please select the applicable response from the dropdown.";
                    }
                }
            }
            //supplier_declared_dg_hz_regulation3
            if (!string.IsNullOrEmpty(dto.supplier_declared_dg_hz_regulation3))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.supplier_declared_dg_hz_regulation3) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.supplier_declared_dg_hz_regulation3) + "_" + dto.feed_product_type].Contains(dto.supplier_declared_dg_hz_regulation3))
                    {
                        msgDto.supplier_declared_dg_hz_regulation3 = "Please select the applicable response from the dropdown.";
                    }
                }
            }
            //supplier_declared_dg_hz_regulation4
            if (!string.IsNullOrEmpty(dto.supplier_declared_dg_hz_regulation4))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.supplier_declared_dg_hz_regulation4) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.supplier_declared_dg_hz_regulation4) + "_" + dto.feed_product_type].Contains(dto.supplier_declared_dg_hz_regulation4))
                    {
                        msgDto.supplier_declared_dg_hz_regulation4 = "Please select the applicable response from the dropdown.";
                    }
                }
            }
            //supplier_declared_dg_hz_regulation5
            if (!string.IsNullOrEmpty(dto.supplier_declared_dg_hz_regulation5))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.supplier_declared_dg_hz_regulation5) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.supplier_declared_dg_hz_regulation5) + "_" + dto.feed_product_type].Contains(dto.supplier_declared_dg_hz_regulation5))
                    {
                        msgDto.supplier_declared_dg_hz_regulation5 = "Please select the applicable response from the dropdown.";
                    }
                }
            }

            //color_name
            if (!string.IsNullOrEmpty(dto.color_name))
            {
                if (!Utilities.IsValidAlphanumericString(dto.color_name, 50))
                {
                    msgDto.color_name = "An alphanumeric string, 1 character minimum in length and 50 characters maximum in length.";
                }
            }

            //item_type_name
            if (!string.IsNullOrEmpty(dto.item_type_name))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.item_type_name) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.item_type_name) + "_" + dto.feed_product_type].Contains(dto.item_type_name))
                    {
                        msgDto.item_type_name = "Select a value from the list of Valid Values.";
                    }
                }
            }

            //size_name
            if (!string.IsNullOrEmpty(dto.size_name))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.size_name) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.size_name) + "_" + dto.feed_product_type].Contains(dto.size_name))
                    {
                        msgDto.size_name = "Select a value from the list of Valid Values.";
                    }
                }
            }

            //watch_movement_type
            if (!string.IsNullOrEmpty(dto.watch_movement_type))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.watch_movement_type) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.watch_movement_type) + "_" + dto.feed_product_type].Contains(dto.watch_movement_type))
                    {
                        msgDto.watch_movement_type = "Select a value from the list of Valid Values.";
                    }
                }
            }

            //target_audience_keywords1
            if (!string.IsNullOrEmpty(dto.target_audience_keywords1))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.target_audience_keywords1) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.target_audience_keywords1) + "_" + dto.feed_product_type].Contains(dto.target_audience_keywords1))
                    {
                        msgDto.target_audience_keywords1 = "Select a value from the list of Valid Values.";
                    }
                }
            }

            //target_audience_keywords2
            if (!string.IsNullOrEmpty(dto.target_audience_keywords2))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.target_audience_keywords2) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.target_audience_keywords2) + "_" + dto.feed_product_type].Contains(dto.target_audience_keywords2))
                    {
                        msgDto.target_audience_keywords2 = "Select a value from the list of Valid Values.";
                    }
                }
            }

            //target_audience_keywords3
            if (!string.IsNullOrEmpty(dto.target_audience_keywords3))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.target_audience_keywords3) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.target_audience_keywords3) + "_" + dto.feed_product_type].Contains(dto.target_audience_keywords3))
                    {
                        msgDto.target_audience_keywords3 = "Select a value from the list of Valid Values.";
                    }
                }
            }

            //target_audience_keywords4
            if (!string.IsNullOrEmpty(dto.target_audience_keywords4))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.target_audience_keywords4) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.target_audience_keywords4) + "_" + dto.feed_product_type].Contains(dto.target_audience_keywords4))
                    {
                        msgDto.target_audience_keywords4 = "Select a value from the list of Valid Values.";
                    }
                }
            }

            //target_audience_keywords5
            if (!string.IsNullOrEmpty(dto.target_audience_keywords5))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.target_audience_keywords5) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.target_audience_keywords5) + "_" + dto.feed_product_type].Contains(dto.target_audience_keywords5))
                    {
                        msgDto.target_audience_keywords5 = "Select a value from the list of Valid Values.";
                    }
                }
            }

            //display_type
            if (!string.IsNullOrEmpty(dto.display_type))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.display_type) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.display_type) + "_" + dto.feed_product_type].Contains(dto.display_type))
                    {
                        msgDto.display_type = "Select a value from the list of Valid Values.";
                    }
                }
            }

            //country_of_origin
            if (!string.IsNullOrEmpty(dto.country_of_origin))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.country_of_origin) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.country_of_origin) + "_" + dto.feed_product_type].Contains(dto.country_of_origin))
                    {
                        msgDto.country_of_origin = "Select the option that indicates the country/region where the product originates from.";
                    }
                }
            }
            //cpsia_cautionary_statement1
            if (!string.IsNullOrEmpty(dto.cpsia_cautionary_statement1))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.cpsia_cautionary_statement1) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.cpsia_cautionary_statement1) + "_" + dto.feed_product_type].Contains(dto.cpsia_cautionary_statement1))
                    {
                        msgDto.cpsia_cautionary_statement1 = "Assign a value from the list on the Valid Values tab.";
                    }
                }
            }
            //cpsia_cautionary_statement2
            if (!string.IsNullOrEmpty(dto.cpsia_cautionary_statement2))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.cpsia_cautionary_statement2) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.cpsia_cautionary_statement2) + "_" + dto.feed_product_type].Contains(dto.cpsia_cautionary_statement2))
                    {
                        msgDto.cpsia_cautionary_statement2 = "Assign a value from the list on the Valid Values tab.";
                    }
                }
            }
            //cpsia_cautionary_statement3
            if (!string.IsNullOrEmpty(dto.cpsia_cautionary_statement3))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.cpsia_cautionary_statement3) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.cpsia_cautionary_statement3) + "_" + dto.feed_product_type].Contains(dto.cpsia_cautionary_statement3))
                    {
                        msgDto.cpsia_cautionary_statement3 = "Assign a value from the list on the Valid Values tab.";
                    }
                }
            }
            //cpsia_cautionary_statement4
            if (!string.IsNullOrEmpty(dto.cpsia_cautionary_statement4))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.cpsia_cautionary_statement4) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.cpsia_cautionary_statement4) + "_" + dto.feed_product_type].Contains(dto.cpsia_cautionary_statement4))
                    {
                        msgDto.cpsia_cautionary_statement4 = "Assign a value from the list on the Valid Values tab.";
                    }
                }
            }



            //condition_type
            if (!string.IsNullOrEmpty(dto.condition_type))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.condition_type) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.condition_type) + "_" + dto.feed_product_type].Contains(dto.condition_type))
                    {
                        msgDto.condition_type = "Select from the list of valid values.";
                    }
                }
            }

            //currency
            if (!string.IsNullOrEmpty(dto.currency))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.currency) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.currency) + "_" + dto.feed_product_type].Contains(dto.currency))
                    {
                        msgDto.currency = "Assign a value from the list on the Valid Values tab.";
                    }
                }
            }

            //number_of_items
            if (!string.IsNullOrEmpty(dto.number_of_items))
            {
                if (!Utilities.IsValidInteger(dto.number_of_items))
                {
                    msgDto.number_of_items = "The number of items that are included in the product";
                }
            }

            if (!string.IsNullOrEmpty(dto.is_expiration_dated_product))
            {
                if (!listModelfromExcel[nameof(dto.is_expiration_dated_product)].Contains(dto.is_expiration_dated_product))
                {
                    msgDto.is_expiration_dated_product = "Select from the list of valid values.";
                }
            }

            //setting_type
            if (!string.IsNullOrEmpty(dto.setting_type))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.setting_type) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.setting_type) + "_" + dto.feed_product_type].Contains(dto.setting_type))
                    {
                        msgDto.setting_type = "Assign a value from the list on the Valid Values tab.";
                    }
                }
            }

            //ring_size
            if (!string.IsNullOrEmpty(dto.ring_size))
            {
                try
                {
                    if (listModelfromExcel.ContainsKey(nameof(dto.ring_size) + "_" + dto.feed_product_type))
                    {
                        if (!listModelfromExcel[nameof(dto.ring_size) + "_" + dto.feed_product_type].Contains(dto.ring_size))
                        {
                            msgDto.ring_size = "Assign a value from the list on the Valid Values tab.";
                        }
                    }
                }
                catch
                {
                    if (dto.ring_size != "Adjustable")
                    {
                        msgDto.ring_size = "Assign a value from the list on the Valid Values tab.";
                    }
                }

            }

            //depth_front_to_back
            if (!string.IsNullOrEmpty(dto.depth_front_to_back))
            {
                if (!Utilities.ValidateNumber(dto.depth_front_to_back, 18, 2))
                {
                    msgDto.depth_front_to_back = "A number with up to 18 digits allowed to the left of the decimal point and 2 digits to the right of the decimal point. Please do not use commas or currency symbols.";
                }
            }

            //depth_front_to_back_unit_of_measure
            if (!string.IsNullOrEmpty(dto.depth_front_to_back_unit_of_measure))
            {
                if (!listModelfromExcel[nameof(dto.depth_front_to_back_unit_of_measure)].Contains(dto.depth_front_to_back_unit_of_measure))
                {
                    msgDto.depth_front_to_back_unit_of_measure = "Select the unit of measure for the item depth.";
                }
            }

            //depth_width_side_to_side
            if (!string.IsNullOrEmpty(dto.depth_width_side_to_side_unit_of_measure))
            {
                if (!listModelfromExcel[nameof(dto.depth_width_side_to_side_unit_of_measure)].Contains(dto.depth_width_side_to_side_unit_of_measure))
                {
                    msgDto.depth_width_side_to_side_unit_of_measure = "Select the unit of measure for the item depth.";
                }
            }

            //depth_height_floor_to_top
            if (!string.IsNullOrEmpty(dto.depth_height_floor_to_top))
            {
                if (!Utilities.ValidateNumber(dto.depth_height_floor_to_top, 18, 2))
                {
                    msgDto.depth_height_floor_to_top = "A number with up to 18 digits allowed to the left of the decimal point and 2 digits to the right of the decimal point. Please do not use commas or currency symbols.";
                }
            }

            //depth_height_floor_to_top_unit_of_measure
            if (!string.IsNullOrEmpty(dto.depth_height_floor_to_top_unit_of_measure))
            {
                if (!listModelfromExcel[nameof(dto.depth_height_floor_to_top_unit_of_measure)].Contains(dto.depth_height_floor_to_top_unit_of_measure))
                {
                    msgDto.depth_height_floor_to_top_unit_of_measure = "Select the unit of measure for the item depth.";
                }
            }

            //package_height
            if (!string.IsNullOrEmpty(dto.package_height))
            {
                if (!Utilities.ValidateNumber(dto.package_height, 10, 2))
                {
                    msgDto.package_height = "Provide the package height as a numeric value.";
                }
            }
            //package_weight_unit_of_measure
            //if (!string.IsNullOrEmpty(dto.package_weight_unit_of_measure))
            //{
            //    if (listModelfromExcel.ContainsKey(nameof(dto.package_weight_unit_of_measure) + "_" + dto.feed_product_type))
            //    {
            //        if (!listModelfromExcel[nameof(dto.package_weight_unit_of_measure) + "_" + dto.feed_product_type].Contains(dto.package_weight_unit_of_measure))
            //        {
            //            msgDto.package_weight_unit_of_measure = "Select one of the following options:  OZ LB GR KG";
            //        }
            //    }
            //}

            //package_length
            if (!string.IsNullOrEmpty(dto.package_length))
            {
                if (!Utilities.ValidateNumber(dto.package_length, 10, 2))
                {
                    msgDto.package_length = "Provide the package length as a numeric value.";
                }
            }

            //package_length_unit_of_measure
            //if (!string.IsNullOrEmpty(dto.package_length_unit_of_measure))
            //{
            //    if (listModelfromExcel.ContainsKey(nameof(dto.package_length_unit_of_measure) + "_" + dto.feed_product_type))
            //    {
            //        if (!listModelfromExcel[nameof(dto.package_length_unit_of_measure) + "_" + dto.feed_product_type].Contains(dto.package_length_unit_of_measure))
            //        {
            //            msgDto.package_length_unit_of_measure = "Select from the list of valid values";
            //        }
            //    }
            //}
            //package_height_unit_of_measure
            //if (!string.IsNullOrEmpty(dto.package_height_unit_of_measure))
            //{
            //    if (listModelfromExcel.ContainsKey(nameof(dto.package_height_unit_of_measure) + "_" + dto.feed_product_type))
            //    {
            //        if (!listModelfromExcel[nameof(dto.package_height_unit_of_measure) + "_" + dto.feed_product_type].Contains(dto.package_height_unit_of_measure))
            //        {
            //            msgDto.package_height_unit_of_measure = "Select from the list of valid values";
            //        }
            //    }
            //}
            //package_width
            if (!string.IsNullOrEmpty(dto.package_width))
            {
                if (!Utilities.ValidateNumber(dto.package_width, 10, 2))
                {
                    msgDto.package_width = "Provide the package width as a numeric value.";
                }
            }
            //package_width_unit_of_measure
            //if (!string.IsNullOrEmpty(dto.package_width_unit_of_measure))
            //{
            //    if (listModelfromExcel.ContainsKey(nameof(dto.package_width_unit_of_measure) + "_" + dto.feed_product_type))
            //    {
            //        if (!listModelfromExcel[nameof(dto.package_width_unit_of_measure) + "_" + dto.feed_product_type].Contains(dto.package_width_unit_of_measure))
            //        {
            //            msgDto.package_width_unit_of_measure = "Select from the list of valid values";
            //        }
            //    }
            //}
            //package_weight
            if (!string.IsNullOrEmpty(dto.package_weight))
            {
                if (!Utilities.ValidateNumber(dto.package_weight, 10, 2))
                {
                    msgDto.package_weight = "This attribute represents the weight of the item plus the packaging. If your item is shipped to the customer in multiple packages, enter the dimensions of the heaviest package";
                }
            }

            //supplier_declared_material_regulation1
            if (!string.IsNullOrEmpty(dto.supplier_declared_material_regulation1))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.supplier_declared_material_regulation1) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.supplier_declared_material_regulation1) + "_" + dto.feed_product_type].Contains(dto.supplier_declared_material_regulation1))
                    {
                        msgDto.supplier_declared_material_regulation1 = "Does your product contain any of the following materials/fabrics subject to regulation: Bamboo, Wool, and/or Fur? Please select all that apply. If none apply, select not applicable.";
                    }
                }
            }
            //supplier_declared_material_regulation2
            if (!string.IsNullOrEmpty(dto.supplier_declared_material_regulation2))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.supplier_declared_material_regulation2) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.supplier_declared_material_regulation2) + "_" + dto.feed_product_type].Contains(dto.supplier_declared_material_regulation2))
                    {
                        msgDto.supplier_declared_material_regulation2 = "Does your product contain any of the following materials/fabrics subject to regulation: Bamboo, Wool, and/or Fur? Please select all that apply. If none apply, select not applicable.";
                    }
                }
            }

            //supplier_declared_material_regulation3
            if (!string.IsNullOrEmpty(dto.supplier_declared_material_regulation3))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.supplier_declared_material_regulation3) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.supplier_declared_material_regulation3) + "_" + dto.feed_product_type].Contains(dto.supplier_declared_material_regulation3))
                    {
                        msgDto.supplier_declared_material_regulation3 = "Does your product contain any of the following materials/fabrics subject to regulation: Bamboo, Wool, and/or Fur? Please select all that apply. If none apply, select not applicable.";
                    }
                }
            }

            //lifecycle_supply_type
            if (!string.IsNullOrEmpty(dto.lifecycle_supply_type))
            {
                if (!listModelfromExcel[nameof(dto.lifecycle_supply_type)].Contains(dto.lifecycle_supply_type))
                {
                    msgDto.lifecycle_supply_type = "Please see the Valid Values worksheet for the list of accepted values.";
                }
            }

            //map_price
            if (!string.IsNullOrEmpty(dto.map_price))
            {
                if (!Utilities.ValidateNumber(dto.map_price, 18, 2))
                {
                    msgDto.map_price = "A number with up to 18 digits allowed to the left of the decimal point and 2 digits to the right of the decimal point. Please do not use commas or currency symbols.";
                }
            }

            //style_name
            if (!string.IsNullOrEmpty(dto.style_name))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.style_name) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.style_name) + "_" + dto.feed_product_type].Contains(dto.style_name))
                    {
                        msgDto.style_name = "Select from the list of valid values.";
                    }
                }
            }

            //unit_count
            if (!string.IsNullOrEmpty(dto.unit_count))
            {
                if (!Utilities.ValidateNumber(dto.unit_count, 18, 2))
                {
                    msgDto.unit_count = "A number with up to 18 digits allowed to the left of the decimal point and 2 digits to the right of the decimal point. Please do not use commas or currency symbols.";
                }
            }
            //unit_count_type
            if (!string.IsNullOrEmpty(dto.unit_count_type))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.unit_count_type) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.unit_count_type) + "_" + dto.feed_product_type].Contains(dto.unit_count_type))
                    {
                        msgDto.unit_count_type = "Please see the Valid Values worksheet for the list of accepted values.";
                    }
                }
            }

            //closure_type
            if (!string.IsNullOrEmpty(dto.closure_type))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.closure_type) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.closure_type) + "_" + dto.feed_product_type].Contains(dto.closure_type))
                    {
                        msgDto.closure_type = "Assign a value from the list on the Valid Values tab.";
                    }
                }
            }

            //care_instructions
            if (!string.IsNullOrEmpty(dto.care_instructions))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.care_instructions) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.care_instructions) + "_" + dto.feed_product_type].Contains(dto.care_instructions))
                    {
                        msgDto.care_instructions = "Assign a value from the list on the Valid Values tab.";
                    }
                }
            }
            //fabric_type1
            if (!string.IsNullOrEmpty(dto.fabric_type1))
            {
                if (!Utilities.IsValidAlphanumericString(dto.fabric_type1, 50))
                {
                    dto.fabric_type1 = "An alphanumeric text string; 1 character minimum and 50 characters maximum.";
                }
            }
            //fabric_type2
            if (!string.IsNullOrEmpty(dto.fabric_type2))
            {
                if (!Utilities.IsValidAlphanumericString(dto.fabric_type2, 50))
                {
                    dto.fabric_type2 = "An alphanumeric text string; 1 character minimum and 50 characters maximum.";
                }
            }
            //fabric_type3
            if (!string.IsNullOrEmpty(dto.fabric_type3))
            {
                if (!Utilities.IsValidAlphanumericString(dto.fabric_type3, 50))
                {
                    dto.fabric_type3 = "An alphanumeric text string; 1 character minimum and 50 characters maximum.";
                }
            }
            //fabric_type4
            if (!string.IsNullOrEmpty(dto.fabric_type4))
            {
                if (!Utilities.IsValidAlphanumericString(dto.fabric_type4, 50))
                {
                    dto.fabric_type4 = "An alphanumeric text string; 1 character minimum and 50 characters maximum.";
                }
            }
            //fabric_type5
            if (!string.IsNullOrEmpty(dto.fabric_type5))
            {
                if (!Utilities.IsValidAlphanumericString(dto.fabric_type5, 50))
                {
                    dto.fabric_type5 = "An alphanumeric text string; 1 character minimum and 50 characters maximum.";
                }
            }
            //fabric_type6
            if (!string.IsNullOrEmpty(dto.fabric_type6))
            {
                if (!Utilities.IsValidAlphanumericString(dto.fabric_type6, 50))
                {
                    dto.fabric_type6 = "An alphanumeric text string; 1 character minimum and 50 characters maximum.";
                }
            }
            //fabric_type7
            if (!string.IsNullOrEmpty(dto.fabric_type7))
            {
                if (!Utilities.IsValidAlphanumericString(dto.fabric_type7, 50))
                {
                    dto.fabric_type7 = "An alphanumeric text string; 1 character minimum and 50 characters maximum.";
                }
            }
            //fabric_type8
            if (!string.IsNullOrEmpty(dto.fabric_type8))
            {
                if (!Utilities.IsValidAlphanumericString(dto.fabric_type8, 50))
                {
                    dto.fabric_type8 = "An alphanumeric text string; 1 character minimum and 50 characters maximum.";
                }
            }
            //fabric_type9
            if (!string.IsNullOrEmpty(dto.fabric_type9))
            {
                if (!Utilities.IsValidAlphanumericString(dto.fabric_type9, 50))
                {
                    dto.fabric_type9 = "An alphanumeric text string; 1 character minimum and 50 characters maximum.";
                }
            }
            //fabric_type10
            if (!string.IsNullOrEmpty(dto.fabric_type10))
            {
                if (!Utilities.IsValidAlphanumericString(dto.fabric_type10, 50))
                {
                    dto.fabric_type10 = "An alphanumeric text string; 1 character minimum and 50 characters maximum.";
                }
            }

            //import_designation
            if (!string.IsNullOrEmpty(dto.import_designation))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.import_designation) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.import_designation) + "_" + dto.feed_product_type].Contains(dto.import_designation))
                    {
                        msgDto.import_designation = "Please see the Valid Values worksheet for the list of accepted values.";
                    }
                }
            }

            //outer_material_type1
            if (!string.IsNullOrEmpty(dto.outer_material_type1))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.outer_material_type1) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.outer_material_type1) + "_" + dto.feed_product_type].Contains(dto.outer_material_type1))
                    {
                        msgDto.outer_material_type1 = "Please see the Valid Values worksheet for the list of accepted values.";
                    }
                }
            }
            //outer_material_type2
            if (!string.IsNullOrEmpty(dto.outer_material_type2))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.outer_material_type2) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.outer_material_type2) + "_" + dto.feed_product_type].Contains(dto.outer_material_type2))
                    {
                        msgDto.outer_material_type2 = "Please see the Valid Values worksheet for the list of accepted values.";
                    }
                }
            }

            //outer_material_type3
            if (!string.IsNullOrEmpty(dto.outer_material_type3))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.outer_material_type3) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.outer_material_type3) + "_" + dto.feed_product_type].Contains(dto.outer_material_type3))
                    {
                        msgDto.outer_material_type3 = "Please see the Valid Values worksheet for the list of accepted values.";
                    }
                }
            }
            //outer_material_type4
            if (!string.IsNullOrEmpty(dto.outer_material_type4))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.outer_material_type4) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.outer_material_type4) + "_" + dto.feed_product_type].Contains(dto.outer_material_type4))
                    {
                        msgDto.outer_material_type4 = "Please see the Valid Values worksheet for the list of accepted values.";
                    }
                }
            }
            //outer_material_type5
            if (!string.IsNullOrEmpty(dto.outer_material_type5))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.outer_material_type5) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.outer_material_type5) + "_" + dto.feed_product_type].Contains(dto.outer_material_type5))
                    {
                        msgDto.outer_material_type5 = "Please see the Valid Values worksheet for the list of accepted values.";
                    }
                }
            }
            //size_map
            if (!string.IsNullOrEmpty(dto.size_map))
            {
                if (listModelfromExcel.ContainsKey(nameof(dto.size_map) + "_" + dto.feed_product_type))
                {
                    if (!listModelfromExcel[nameof(dto.size_map) + "_" + dto.feed_product_type].Contains(dto.size_map))
                    {
                        msgDto.size_map = "Please see the Valid Values worksheet for the list of accepted values.";
                    }
                }
            }
            return msgDto;
        }
    }
}
