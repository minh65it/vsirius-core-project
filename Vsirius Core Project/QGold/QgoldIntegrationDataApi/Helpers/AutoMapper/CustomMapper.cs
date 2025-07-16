using QgoldIntegrationDataApi.Dtos.Marketplace;
using QgoldIntegrationDataApi.Dtos.Qgold;
using ultilitiesMapper3 = QgoldIntegrationDataApi.Helpers.Ultilities.UltilitiesMapper3;

namespace QgoldIntegrationDataApi.Helpers.AutoMapper
{
    public static class CustomMapper
    {
        public static VsiriusProduct_DTO3 MapperQgoldToVsiriusV3(QgoldCsvData_DTO qgold)
        {
            VsiriusProduct_DTO3 vsirius = new VsiriusProduct_DTO3();
            try
            {
                var typeProduct = ultilitiesMapper3.GetFeedProductType(qgold.Item_Type, qgold.CategoriesText);
                //if( qgold.Item == "QCM1420")
                //{
                //    var a = 1;
                //}
                #region Required
                //vsirius.feed_product_type = ultilitiesMapper3.GetFeedProductTypeAttribute(qgold.Item_Type, qgold.CategoriesText);
                vsirius.feed_product_type = typeProduct;
                vsirius.item_sku = "Q-" + qgold.Item;
                if(qgold.ListOfSpecsText.Contains("Licensed By", StringComparison.OrdinalIgnoreCase))
                {
                    vsirius.brand_name = null;
                }
                else
                {
                    vsirius.brand_name = "VSIRIUS";
                }

                vsirius.external_product_id = string.IsNullOrWhiteSpace(qgold.UPC) ? string.Empty : qgold.UPC;
                vsirius.external_product_id_type = ultilitiesMapper3.GetValueExternalProductIdType(qgold.UPC);

                vsirius.item_name = ultilitiesMapper3.GenerateItemName(qgold.Description, qgold.Item);
                vsirius.item_length_numeric = ultilitiesMapper3.GetValueLengthNumeric(qgold.Length, qgold.Item_Type, qgold.CategoriesText, false);
                vsirius.item_length_numeric_unit_of_measure = ultilitiesMapper3.GetValueLengthNumeric(qgold.Length, qgold.Item_Type, qgold.CategoriesText, true);
                vsirius.main_image_url = ultilitiesMapper3.GetImage(qgold.Image1Link, vsirius.item_sku, 1);
                vsirius.target_gender = ultilitiesMapper3.GetValueGender(qgold.ListOfSpecsText);
                vsirius.quantity = qgold.Qty_Avail;
                vsirius.standard_price = ultilitiesMapper3.GetValuePrice(qgold.Price);
                var item_type = ultilitiesMapper3.GetItemType(qgold.Item_Type, qgold.CategoriesText, qgold.ListOfSpecsText, qgold.Description);
                vsirius.item_type = string.IsNullOrEmpty(item_type) ? ultilitiesMapper3.GetItemType(qgold.Item_Type, qgold.CategoriesText) : item_type;
                vsirius.metal_type = ultilitiesMapper3.GetMetalType(qgold.Metal_Desc, qgold.Item_Type, qgold.CategoriesText);
                vsirius.material_type1 = ultilitiesMapper3.GetValueMaterial(qgold.Item_Type, qgold.CategoriesText, qgold.ListOfSpecsText);
                vsirius.material_type2 = null;
                vsirius.material_type3 = null;
                vsirius.material_type4 = null;
                vsirius.material_type5 = null;
                vsirius.department_name1 = ultilitiesMapper3.GetValueDepartmentName(qgold.Item_Type, qgold.CategoriesText, qgold.ListOfSpecsText);
                vsirius.gem_type1 = ultilitiesMapper3.GetGemType(qgold.Item_Type, qgold.CategoriesText, qgold.ListOfSpecsText, 1);
                vsirius.gem_type2 = ultilitiesMapper3.GetGemType(qgold.Item_Type, qgold.CategoriesText, qgold.ListOfSpecsText, 2);
                vsirius.gem_type3 = ultilitiesMapper3.GetGemType(qgold.Item_Type, qgold.CategoriesText, qgold.ListOfSpecsText, 3);
                vsirius.color_map = ultilitiesMapper3.GetValueColor(qgold.ListOfSpecsText, qgold.Item_Type, qgold.CategoriesText);
                vsirius.size_name = ultilitiesMapper3.GetSizeName(typeProduct, qgold.Size, qgold.CT_Weight); 
                vsirius.material_composition = "None";
                vsirius.age_range_description = ultilitiesMapper3.GetValueAgeRangeDescription(qgold.Item_Type, qgold.CategoriesText);
                vsirius.manufacturer = "VSirius";
                vsirius.part_number = typeProduct == "fineother" ? "" : "Q-" + qgold.Item;
                vsirius.model = typeProduct == "fineother" ? "" : qgold.Item;
                vsirius.watch_movement_type = ultilitiesMapper3.GetValueWatchMovementType(qgold.Item_Type, qgold.CategoriesText);
                vsirius.target_audience_keywords1 = ultilitiesMapper3.GetValueTargetAudience(qgold.Item_Type, qgold.CategoriesText, 1);
                vsirius.target_audience_keywords2 = ultilitiesMapper3.GetValueTargetAudience(qgold.Item_Type, qgold.CategoriesText, 2);
                vsirius.target_audience_keywords3 = ultilitiesMapper3.GetValueTargetAudience(qgold.Item_Type, qgold.CategoriesText, 3);
                vsirius.target_audience_keywords4 = ultilitiesMapper3.GetValueTargetAudience(qgold.Item_Type, qgold.CategoriesText, 4);
                vsirius.target_audience_keywords5 = ultilitiesMapper3.GetValueTargetAudience(qgold.Item_Type, qgold.CategoriesText, 5);
                vsirius.display_type = ultilitiesMapper3.GetValueDisplayType(qgold.Item_Type, qgold.CategoriesText);
                vsirius.product_description = ultilitiesMapper3.GetProductDescription(qgold.Description, qgold.Size, qgold.ListOfSpecsText);
                vsirius.model_name = qgold.Item;
                vsirius.bullet_point1 = ultilitiesMapper3.GetValueBulletPoint(qgold.ListOfSpecsText, 1);
                vsirius.bullet_point2 = ultilitiesMapper3.GetValueBulletPoint(qgold.ListOfSpecsText, 2);
                vsirius.bullet_point3 = ultilitiesMapper3.GetValueBulletPoint(qgold.ListOfSpecsText, 3);
                vsirius.bullet_point4 = ultilitiesMapper3.GetValueBulletPoint(qgold.ListOfSpecsText, 4);
                vsirius.bullet_point5 = ultilitiesMapper3.GetValueBulletPoint(qgold.ListOfSpecsText, 5);
                vsirius.bullet_point6 = null;
                vsirius.bullet_point7 = null;
                vsirius.bullet_point8 = null;
                vsirius.bullet_point9 = null;
                vsirius.bullet_point10 = null;
                var item_type_name = ultilitiesMapper3.GetValueItemTypeName(qgold.Item_Type, qgold.CategoriesText, qgold.ListOfSpecsText, qgold.Description);
                vsirius.item_type_name = string.IsNullOrEmpty(item_type_name) ? vsirius.item_type : item_type_name;
                vsirius.number_of_boxes = "1";
                vsirius.item_length = ultilitiesMapper3.GetLengthInListOfSpecs(qgold.ListOfSpecsText, false, qgold.Size, qgold.Length);
                vsirius.item_length_unit_of_measure =
                    string.IsNullOrWhiteSpace(vsirius.item_length) ? "" : ultilitiesMapper3.GetValueItemLengthOUM(qgold.ListOfSpecsText);
                vsirius.item_width = ultilitiesMapper3.GetWidthInListOfSpecs(qgold.ListOfSpecsText, false);
                vsirius.item_width_unit_of_measure =
                    string.IsNullOrWhiteSpace(vsirius.item_width) ? "" : ultilitiesMapper3.GetWidthInListOfSpecs(qgold.ListOfSpecsText, true);
                vsirius.item_height = ultilitiesMapper3.GetLengthInListOfSpecs(qgold.ListOfSpecsText, false);
                vsirius.item_height_unit_of_measure =
                    string.IsNullOrWhiteSpace(vsirius.item_height) ? "" : ultilitiesMapper3.GetLengthInListOfSpecs(qgold.ListOfSpecsText, true);
                vsirius.cpsia_cautionary_statement1 = "NoWarningApplicable";
                vsirius.cpsia_cautionary_statement2 = null;
                vsirius.cpsia_cautionary_statement3 = null;
                vsirius.cpsia_cautionary_statement4 = null;
                vsirius.country_of_origin = ultilitiesMapper3.GetValueCountryofOrigin(qgold.Country_Of_Origin);
                vsirius.warranty_description = 
                    "Exceptional customer services. 30 days money back or exchange guarantee. " +
                    "Your satisfaction is our priority. Please feel free to contact us if you have any additional request. " +
                    "We are happy to work with you";
                vsirius.condition_type = "New";
                vsirius.currency = ultilitiesMapper3.GetValueCurrency(qgold.Item_Type, qgold.CategoriesText);
                vsirius.number_of_items = ultilitiesMapper3.GetValueNumberOfItem(qgold.Item_Type, qgold.CategoriesText, qgold.ListOfSpecsText);
                vsirius.is_expiration_dated_product = "No";
                vsirius.setting_type = ultilitiesMapper3.GetSettingType(qgold.Item_Type, qgold.CategoriesText, qgold.ListOfSpecsText); //
                vsirius.color_name = ultilitiesMapper3.GetValueColor(qgold.ListOfSpecsText, qgold.Item_Type, qgold.CategoriesText);
                vsirius.ring_size = ultilitiesMapper3.GetValueSize(qgold.Item_Type, qgold.CategoriesText, qgold.Size);

                vsirius.depth_front_to_back = "1";
                vsirius.depth_front_to_back_unit_of_measure = "Inches";
                vsirius.depth_width_side_to_side = "1";
                vsirius.depth_width_side_to_side_unit_of_measure = "Inches";
                vsirius.depth_height_floor_to_top = "1";
                vsirius.depth_height_floor_to_top_unit_of_measure = "Inches";
                vsirius.package_height = vsirius.item_height;
                vsirius.package_height_unit_of_measure = string.IsNullOrWhiteSpace(vsirius.package_height) ? "" : vsirius.item_height_unit_of_measure;
                vsirius.package_length = vsirius.item_length;
                vsirius.package_length_unit_of_measure = string.IsNullOrWhiteSpace(vsirius.package_length) ? "" : vsirius.item_length_unit_of_measure;
                vsirius.package_width = vsirius.item_width;
                vsirius.package_width_unit_of_measure = string.IsNullOrWhiteSpace(vsirius.package_width) ? "" : vsirius.item_width_unit_of_measure;
                vsirius.package_weight = "28.35";
                vsirius.package_weight_unit_of_measure = "GR";
                vsirius.supplier_declared_dg_hz_regulation1 = "Unknown";
                vsirius.supplier_declared_dg_hz_regulation2 = null;// "Not Applicable";
                vsirius.supplier_declared_dg_hz_regulation3 = null;// "Not Applicable";
                vsirius.supplier_declared_material_regulation1 = "Not Applicable";
                vsirius.supplier_declared_material_regulation2 = null; //"Not Applicable";
                vsirius.supplier_declared_material_regulation3 = null; //"Not Applicable";
                vsirius.lifecycle_supply_type = "Perennial";
                vsirius.map_price = ultilitiesMapper3.GetValuePrice(qgold.Price);
                vsirius.style_name = ultilitiesMapper3.GetValueStyleName(qgold.Item_Type, qgold.CategoriesText, qgold.ListOfSpecsText, qgold.Description);
                vsirius.unit_count = "1";
                vsirius.unit_count_type = "Count";
                vsirius.closure_type = ultilitiesMapper3.GetValueClosure(qgold.Item_Type, qgold.CategoriesText);
                vsirius.care_instructions = ultilitiesMapper3.GetValueCareInstruction(qgold.Item_Type, qgold.CategoriesText);
                vsirius.import_designation = ultilitiesMapper3.GetValueImportDesignation(qgold.Item_Type, qgold.CategoriesText);
                vsirius.fabric_type1 = "None";
                vsirius.fabric_type2 = null;
                vsirius.fabric_type3 = null;
                vsirius.fabric_type4 = null;
                vsirius.fabric_type5 = null;
                vsirius.fabric_type6 = null;
                vsirius.fabric_type7 = null;
                vsirius.fabric_type8 = null;
                vsirius.fabric_type9 = null;
                vsirius.fabric_type10 = null;
                vsirius.generic_keywords = "fine jewelry gift";
                vsirius.outer_material_type1 = ultilitiesMapper3.GetValueOuterMaterialType(qgold.Item_Type, qgold.CategoriesText);
                vsirius.outer_material_type2 = null;
                vsirius.outer_material_type3 = null;
                vsirius.outer_material_type4 = null;
                vsirius.outer_material_type5 = null;
                vsirius.size_map = "Medium";
                #endregion

                #region Images
                vsirius.swatch_image_url = vsirius.main_image_url;
                vsirius.other_image_url1 = ultilitiesMapper3.GetImage(qgold.Image2Link, vsirius.item_sku, 2);
                vsirius.other_image_url2 = ultilitiesMapper3.GetImage(qgold.Image3Link, vsirius.item_sku, 3);
                vsirius.other_image_url3 = ultilitiesMapper3.GetImage(qgold.Image4Link, vsirius.item_sku, 4);
                vsirius.other_image_url4 = ultilitiesMapper3.GetImage(qgold.Image5Link, vsirius.item_sku, 5);
                vsirius.other_image_url5 = ultilitiesMapper3.GetImage(qgold.Image6Link, vsirius.item_sku, 6);
                vsirius.other_image_url6 = ultilitiesMapper3.GetImage(qgold.Image7Link, vsirius.item_sku, 7);
                vsirius.other_image_url7 = ultilitiesMapper3.GetImage(qgold.Image8Link, vsirius.item_sku, 8);
                vsirius.other_image_url8 = ultilitiesMapper3.GetImage(qgold.Image9Link, vsirius.item_sku, 9);
                //vsirius.pt1_image_url = ultilitiesMapper3.GetImage(qgold.Image2Link);
                //vsirius.pt2_image_url = ultilitiesMapper3.GetImage(qgold.Image3Link);
                //vsirius.pt3_image_url = ultilitiesMapper3.GetImage(qgold.Image4Link);
                //vsirius.pt4_image_url = ultilitiesMapper3.GetImage(qgold.Image5Link);
                //vsirius.pt5_image_url = ultilitiesMapper3.GetImage(qgold.Image6Link);
                //vsirius.pt6_image_url = ultilitiesMapper3.GetImage(qgold.Image7Link);
                //vsirius.pt7_image_url = ultilitiesMapper3.GetImage(qgold.Image8Link);
                //vsirius.pt8_image_url = ultilitiesMapper3.GetImage(qgold.Image9Link);
                #endregion

                #region Variation
                //vsirius.relationship_type = ultilitiesMapper3.GetValueRelationshipType(qgold.Item_Type, qgold.CategoriesText);
                vsirius.relationship_type = null;
                //vsirius.variation_theme = ultilitiesMapper3.GetValueVariationTheme(qgold.Item_Type, qgold.CategoriesText);
                vsirius.variation_theme = null;
                vsirius.package_level = "unit";
                vsirius.package_contains_identifier = null;
                vsirius.package_contains_quantity = null;
                //vsirius.parent_sku = "Q-" + qgold.Item;
                vsirius.parent_child = "Child";
                #endregion

                #region Basic
                vsirius.update_delete = null;
                vsirius.certificate_number1 = null; // Số chứng chỉ
                vsirius.certificate_number2 = null;
                vsirius.certificate_number3 = null;
                vsirius.certificate_number4 = null;
                vsirius.certificate_number5 = null;
                vsirius.certificate_number6 = null;
                vsirius.certificate_number7 = null;
                vsirius.certificate_number8 = null;
                vsirius.certificate_number9 = null;
                vsirius.certificate_type1 = null; // Loại chứng chỉ
                vsirius.certificate_type2 = null;
                vsirius.certificate_type3 = null;
                vsirius.certificate_type4 = null;
                vsirius.certificate_type5 = null;
                vsirius.certificate_type6 = null;
                vsirius.certificate_type7 = null;
                vsirius.certificate_type8 = null;
                vsirius.certificate_type9 = null;
                vsirius.grade_rating = null; // đánh giá lớp
                vsirius.production_method = null;  // sản phẩm làm như thế nào
                vsirius.model_year = null;
                vsirius.inner_material_type = null;
                vsirius.gtin_exemption_reason = null;
                #endregion

                #region Discovery
                vsirius.stone_color1 = ultilitiesMapper3.GetValueStoneColor(qgold.ListOfSpecsText, qgold.Item_Type, qgold.CategoriesText, 1);
                vsirius.stone_color2 = ultilitiesMapper3.GetValueStoneColor(qgold.ListOfSpecsText, qgold.Item_Type, qgold.CategoriesText, 2);
                vsirius.stone_color3 = ultilitiesMapper3.GetValueStoneColor(qgold.ListOfSpecsText, qgold.Item_Type, qgold.CategoriesText, 3);
                vsirius.stones_color = ultilitiesMapper3.GetValueStoneColor(qgold.ListOfSpecsText, qgold.Item_Type, qgold.CategoriesText, 0);
                //vsirius.occasion_type1 = ultilitiesMapper3.GetValueOccasion(qgold.Item_Type, qgold.CategoriesText, 1);
                //vsirius.occasion_type2 = null;//ultilitiesMapper3.GetValueOccasion(qgold.Item_Type, qgold.CategoriesText, 2);
                //vsirius.occasion_type3 = null;//ultilitiesMapper3.GetValueOccasion(qgold.Item_Type, qgold.CategoriesText, 3);
                //vsirius.occasion_type4 = null;//ultilitiesMapper3.GetValueOccasion(qgold.Item_Type, qgold.CategoriesText, 4);
                //vsirius.occasion_type5 = null; //ultilitiesMapper3.GetValueOccasion(qgold.Item_Type, qgold.CategoriesText, 5);
                vsirius.occasion_type1 = "Christmas";
                vsirius.occasion_type2 = "Wedding";
                vsirius.occasion_type3 = "Anniversary";
                vsirius.occasion_type4 = "Thanksgiving";
                vsirius.occasion_type5 = "Valentines Day";
                vsirius.clasp_type = ultilitiesMapper3.GetValueClaspType(qgold.Item_Type, qgold.CategoriesText, qgold.ListOfSpecsText);
                vsirius.stones_id = ultilitiesMapper3.GetValueStoneId(qgold.Item_Type, qgold.ListOfSpecsText, qgold.CategoriesText);
                vsirius.stones_creation_method = null;
                vsirius.athlete = null;
                vsirius.team_name = null;
                vsirius.stone_shape1 = ultilitiesMapper3.GetValueStoneShapes(qgold.Item_Type, qgold.CategoriesText, qgold.ListOfSpecsText, 1);
                vsirius.stone_shape2 = ultilitiesMapper3.GetValueStoneShapes(qgold.Item_Type, qgold.CategoriesText, qgold.ListOfSpecsText, 2);
                vsirius.stone_shape3 = ultilitiesMapper3.GetValueStoneShapes(qgold.Item_Type, qgold.CategoriesText, qgold.ListOfSpecsText, 3);
                vsirius.stone_clarity1 = ultilitiesMapper3.GetValueStoneClarity(qgold.Item_Type, qgold.CategoriesText, qgold.ListOfSpecsText, 1);
                vsirius.stone_clarity2 = ultilitiesMapper3.GetValueStoneClarity(qgold.Item_Type, qgold.CategoriesText, qgold.ListOfSpecsText, 2);
                vsirius.stone_clarity3 = ultilitiesMapper3.GetValueStoneClarity(qgold.Item_Type, qgold.CategoriesText, qgold.ListOfSpecsText, 3);
                vsirius.collection_name = ultilitiesMapper3.GetValueCollectionName(qgold.Item_Type, qgold.CategoriesText);
                vsirius.metals_metal_weight = null; //string.IsNullOrEmpty(qgold.Weight) ? "1" : qgold.Weight;
                vsirius.metals_metal_weight_unit_of_measure = null;// "GR";
                vsirius.metals_metal_type = null;
                var stones_number_of_stones = ultilitiesMapper3.GetValueNumberStoneOrPearl(qgold.ListOfSpecsText, true, true);
                if (stones_number_of_stones != null && stones_number_of_stones != "0")
                {
                    vsirius.stones_type = null; //ultilitiesMapper3.GetGemType(qgold.ListOfSpecsText, 0);
                }
                vsirius.stones_number_of_stones = null;//stones_number_of_stones == "0" ? null : stones_number_of_stones;
                vsirius.stones_clarity = ultilitiesMapper3.GetValueStoneClarity(qgold.Item_Type, qgold.CategoriesText, qgold.ListOfSpecsText, 0);
                vsirius.stones_treatment_method = null; ultilitiesMapper3.GetValueStoneTreatment(qgold.Item_Type, qgold.CategoriesText, qgold.ListOfSpecsText, 0);
                vsirius.fur_description = null;
                vsirius.metals_id = null;
                vsirius.initial_character = null;// ultilitiesMapper3.GetValueInitialCharacter(qgold.Description, qgold.Item);
                vsirius.metals_metal_stamp = ultilitiesMapper3.GetValueMetalStamp(qgold.Item_Type, qgold.CategoriesText, qgold.ListOfSpecsText, true);
                vsirius.stones_shape = ultilitiesMapper3.GetValueStoneShapes(qgold.Item_Type, qgold.CategoriesText, qgold.ListOfSpecsText, 0);
                vsirius.chain_length_unit = ultilitiesMapper3.GetValueChainLength(qgold.ListOfSpecsText, true);

                vsirius.stones_cut = null;// ultilitiesMapper3.GetValueStoneCuts(qgold.ListOfSpecsText, 0);
                vsirius.stone_cut1 = null;// ultilitiesMapper3.GetValueStoneCuts(qgold.ListOfSpecsText, 1);
                vsirius.stone_cut2 = null;// ultilitiesMapper3.GetValueStoneCuts(qgold.ListOfSpecsText, 2);
                vsirius.stone_cut3 = null;// ultilitiesMapper3.GetValueStoneCuts(qgold.ListOfSpecsText, 3);

                vsirius.league_name = null;
                vsirius.thesaurus_subject_keywords1 = ultilitiesMapper3.GetValueSubjectMatter(qgold.Item_Type, qgold.CategoriesText, 1);
                vsirius.thesaurus_subject_keywords2 = ultilitiesMapper3.GetValueSubjectMatter(qgold.Item_Type, qgold.CategoriesText, 2);
                vsirius.thesaurus_subject_keywords3 = ultilitiesMapper3.GetValueSubjectMatter(qgold.Item_Type, qgold.CategoriesText, 3);
                vsirius.thesaurus_subject_keywords4 = ultilitiesMapper3.GetValueSubjectMatter(qgold.Item_Type, qgold.CategoriesText, 4);
                vsirius.thesaurus_subject_keywords5 = ultilitiesMapper3.GetValueSubjectMatter(qgold.Item_Type, qgold.CategoriesText, 5);
                vsirius.chain_length_derived = ultilitiesMapper3.GetValueChainLength(qgold.ListOfSpecsText, false);
                vsirius.chain_type = ultilitiesMapper3.GetValueChainType(qgold.Item_Type, qgold.CategoriesText, qgold.ListOfSpecsText);
                vsirius.item_booking_date = null;
                vsirius.drop_length_unit = null; //ultilitiesMapper3.GetValueDropLength(qgold.Item_Type, qgold.CategoriesText, qgold.ListOfSpecsText, true);
                vsirius.drop_length = null; //ultilitiesMapper3.GetValueDropLength(qgold.Item_Type, qgold.CategoriesText, qgold.ListOfSpecsText, false);
                vsirius.back_finding = ultilitiesMapper3.GetValueBackingType(qgold.Item_Type, qgold.CategoriesText, qgold.ListOfSpecsText);
                vsirius.is_autographed = null;
                vsirius.sport_type1 = null;
                vsirius.sport_type2 = null;
                vsirius.sport_type3 = null;
                vsirius.seasons1 = null; //"Winter";
                vsirius.seasons2 = null; //"Summer";
                vsirius.seasons3 = null; //"Fall";
                vsirius.seasons4 = null; //"Spring";
                vsirius.lifestyle = null; //"Casual";
                vsirius.fit_type1 = null;
                vsirius.fit_type2 = null;
                vsirius.fit_type3 = null;
                vsirius.fit_type4 = null;
                vsirius.fit_type5 = null;
                vsirius.weave_type = null;
                vsirius.theme = ultilitiesMapper3.GetValueTheme(vsirius.feed_product_type);
                vsirius.special_size_type = null;
                vsirius.thesaurus_attribute_keywords1 = null;
                vsirius.thesaurus_attribute_keywords2 = null;
                vsirius.thesaurus_attribute_keywords3 = null;
                vsirius.thesaurus_attribute_keywords4 = null;
                vsirius.thesaurus_attribute_keywords5 = null;
                vsirius.catalog_number = null;
                vsirius.specific_uses_keywords1 = null; //"anniversary";
                vsirius.specific_uses_keywords2 = null; //"wedding";
                vsirius.specific_uses_keywords3 = null; //"valentines-day";
                vsirius.specific_uses_keywords4 = null; //"birthday";
                vsirius.specific_uses_keywords5 = null; // "engagement";
                vsirius.band_material_type = typeProduct == "watches" ? "chrome-plated-stainless-steel" : null;
                vsirius.case_thickness = null;
                vsirius.case_thickness_unit_of_measure = null;
                vsirius.band_color = null;
                vsirius.case_material_type = ultilitiesMapper3.GetValueCaseMaterialType(qgold.Item_Type, qgold.CategoriesText, qgold.ListOfSpecsText);
                vsirius.water_resistance_depth = ultilitiesMapper3.GetValueWaterResistance(qgold.Item_Type, qgold.CategoriesText, qgold.ListOfSpecsText, false);
                vsirius.water_resistance_depth_unit_of_measure = ultilitiesMapper3.GetValueWaterResistance(qgold.Item_Type, qgold.CategoriesText, qgold.ListOfSpecsText, true);
                vsirius.platinum_keywords1 = null; //"special anniversary gift";
                vsirius.platinum_keywords2 = null; //"jewelry gift for girls";
                vsirius.platinum_keywords3 = null; //"jewelry gift for men";
                vsirius.platinum_keywords4 = null; //"special anniversary gift";
                vsirius.platinum_keywords5 = null; //"christmas jewelry gift";
                vsirius.special_features1 = typeProduct == "watches" ? "timer" : null;
                vsirius.special_features2 = null;
                vsirius.special_features3 = null;
                vsirius.special_features4 = null;
                vsirius.special_features5 = null;
                vsirius.band_length = null;
                vsirius.operating_system = null;
                vsirius.supported_application = null;
                vsirius.target_audience_base1 = null;
                vsirius.target_audience_base2 = null;
                vsirius.target_audience_base3 = null;
                vsirius.target_audience_base4 = null;
                vsirius.target_audience_base5 = null;
                vsirius.band_length_unit = null;
                vsirius.water_resistance_level = null;
                vsirius.directions = null;
                vsirius.paint_type = null;
                vsirius.number_of_pieces = null;
                vsirius.included_components1 = null;
                vsirius.included_components2 = null;
                vsirius.included_components3 = null;
                vsirius.included_components4 = null;
                vsirius.included_components5 = null;
                vsirius.is_resizable = null;
                vsirius.stone_creation_method1 = ultilitiesMapper3.GetValueStoneCreation(qgold.Item_Type, qgold.CategoriesText, qgold.ListOfSpecsText, 1);
                vsirius.stone_creation_method2 = ultilitiesMapper3.GetValueStoneCreation(qgold.Item_Type, qgold.CategoriesText, qgold.ListOfSpecsText, 2);
                vsirius.stone_creation_method3 = ultilitiesMapper3.GetValueStoneCreation(qgold.Item_Type, qgold.CategoriesText, qgold.ListOfSpecsText, 3);
                vsirius.stone_treatment_method1 = ultilitiesMapper3.GetValueStoneTreatment(qgold.Item_Type, qgold.CategoriesText, qgold.ListOfSpecsText, 1);
                vsirius.stone_treatment_method2 = ultilitiesMapper3.GetValueStoneTreatment(qgold.Item_Type, qgold.CategoriesText, qgold.ListOfSpecsText, 2);
                vsirius.stone_treatment_method3 = ultilitiesMapper3.GetValueStoneTreatment(qgold.Item_Type, qgold.CategoriesText, qgold.ListOfSpecsText, 3);
                vsirius.gauge_derived = null;
                vsirius.lining_description = null;
                vsirius.number_of_compartments = null;
                vsirius.number_of_wheels = null;
                vsirius.pattern_name = null;
                vsirius.wheel_type = ultilitiesMapper3.GetValueWheelType(qgold.Item_Type, qgold.CategoriesText);
                vsirius.band_thickness = null;
                vsirius.band_thickness_unit_of_measure = null;

                // jewelrystorage
                //vsirius.horsepower = null;
                //vsirius.horsepower_unit_of_measure = null;
                vsirius.power_source_type = typeProduct == "watches" ? "Battery Powered" : null;
                //vsirius.voltage_unit_of_measure = null;
                //vsirius.controller_type = null;
                //vsirius.maximum_operating_pressure = null;
                //vsirius.maximum_operating_pressure_unit_of_measure = null;
                //vsirius.handle_location = null;
                //vsirius.light_source_type = null;
                //vsirius.brightness_unit_of_measure = null;
                //vsirius.speed_unit_of_measure = null;
                //vsirius.item_torque = null;
                //vsirius.item_torque_unit_of_measure = null;
                //vsirius.insulation_resistance_unit_of_measure = null;
                //vsirius.mounting_type = null;
                //vsirius.oem_equivalent_part_number = null;
                //vsirius.recommended_uses_for_product = null;
                //vsirius.shank_type = null;

                vsirius.number_of_drawers = null;
                vsirius.belt_style = null;
                vsirius.embellishment_feature1 = null;
                vsirius.embellishment_feature2 = null;
                vsirius.embellishment_feature3 = null;
                vsirius.embellishment_feature4 = null;
                vsirius.embellishment_feature5 = null;
                vsirius.style_keywords1 = null;
                vsirius.style_keywords2 = null;
                vsirius.style_keywords3 = null;
                vsirius.compatible_devices = null;
                vsirius.front_style = null;
                vsirius.country_as_labeled = null;
                vsirius.neck_style = null;
                vsirius.pattern_type = null;
                vsirius.pocket_description = null;
                vsirius.shaft_style_type = null;
                #endregion

                #region Product Enrichment
                vsirius.pearl_type = ultilitiesMapper3.GetValuePearlType(qgold.Item_Type, qgold.CategoriesText, qgold.ListOfSpecsText);
                vsirius.legal_compliance_certification_certifying_authority_name = null;
                vsirius.pearl_uniformity = ultilitiesMapper3.GetValuePearlUniformity(qgold.ListOfSpecsText);
                vsirius.pearl_shape = ultilitiesMapper3.GetValuePearlShape(qgold.Item_Type, qgold.CategoriesText, qgold.ListOfSpecsText);
                vsirius.pearl_surface_blemishes = ultilitiesMapper3.GetValuePearlSucface(qgold.Item_Type, qgold.CategoriesText);
                vsirius.pearl_lustre = ultilitiesMapper3.GetValuePearLuster(qgold.Item_Type, qgold.CategoriesText);
                vsirius.legal_compliance_certification_geographic_jurisdiction = null;
                vsirius.flash_point = null;
                vsirius.flash_point_unit_of_measure = null;
                vsirius.pearl_stringing_method = null;
                vsirius.pearl_minimum_color = null;
                vsirius.inscription = null;
                vsirius.number_of_pearls = ultilitiesMapper3.GetValueNumberStoneOrPearl(qgold.ListOfSpecsText, false, false);
                vsirius.number_of_stones = null;// ultilitiesMapper3.GetValueNumberStoneOrPearl(qgold.ListOfSpecsText, true, false);
                vsirius.duration = null;
                vsirius.duration_unit_of_measure = null;
                vsirius.subject_character = null;
                vsirius.sub_brand_name = null;
                vsirius.sleeve_type = null;
                vsirius.fabric_wash = ultilitiesMapper3.GetValueFaricWash(qgold.Item_Type, qgold.CategoriesText);
                vsirius.designer = null;
                vsirius.band_size = ultilitiesMapper3.GetValueBandSize(qgold.Item_Type, qgold.CategoriesText);
                vsirius.dial_color = ultilitiesMapper3.GetValueDialColor(qgold.Item_Type, qgold.CategoriesText, qgold.ListOfSpecsText);
                vsirius.bezel_function = null;
                vsirius.dial_window_material_type = null;
                vsirius.bezel_material_type = null;
                vsirius.calendar_type = null;
                vsirius.resale_type = null;
                vsirius.authenticated_by = null;
                vsirius.finish_type1 = null;
                vsirius.finish_type2 = null;
                vsirius.fc_shelf_life = null;
                vsirius.fc_shelf_life_unit_of_measure = null; // ultilitiesMapper3.GetValueShelfLifeUnit(qgold.Item_Type, qgold.CategoriesText);
                vsirius.ring_sizing_lower_range = null;
                vsirius.ring_sizing_upper_range = null;
                vsirius.golf_club_flex = null;
                vsirius.connectivity_protocol = null;
                vsirius.item_pitch_unit_of_measure = null;
                vsirius.energy_guide_cost_disclosure = null;
                vsirius.is_oem_authorized = null;
                vsirius.energy_guide_annual_operating_cost = null;
                vsirius.belt_width_unit_of_measure = null;
                vsirius.belt_width_string = null;
                vsirius.opacity = null;
                vsirius.opacity_unit_of_measure = null; //"inches";
                vsirius.map_policy = null;
                vsirius.belt_width_derived = null;
                vsirius.index_suppressed = null;
                vsirius.leg_style = null;
                vsirius.strap_type = null;
                vsirius.underwire_type = null;
                #endregion

                #region Dimensions
                vsirius.item_display_weight = string.IsNullOrEmpty(qgold.Weight) ? "1" : qgold.Weight;
                vsirius.item_display_weight_unit_of_measure = ultilitiesMapper3.GetWeightUOM(qgold.ListOfSpecsText, "GR");
                vsirius.total_diamond_weight = null; //ultilitiesMapper3.GetValueTotalDiamondWeight(qgold.ListOfSpecsText, false);
                vsirius.total_diamond_weight_unit_of_measure = null; // ultilitiesMapper3.GetValueTotalDiamondWeight(qgold.ListOfSpecsText, true);
                vsirius.item_display_diameter = string.Empty;
                vsirius.item_display_diameter_unit_of_measure = string.Empty;
                vsirius.stone_weight1 = null;//ultilitiesMapper3.GetValueStoneWeight(qgold.ListOfSpecsText, 1);
                vsirius.stone_weight2 = null;//ultilitiesMapper3.GetValueStoneWeight(qgold.ListOfSpecsText, 2);
                vsirius.stone_weight3 = null;// ultilitiesMapper3.GetValueStoneWeight(qgold.ListOfSpecsText, 3);
                vsirius.stone_weight_unit_of_measure1 = null;// ultilitiesMapper3.GetValueStoneWeightOUM(qgold.ListOfSpecsText, 1);
                vsirius.stone_weight_unit_of_measure2 = null;// ultilitiesMapper3.GetValueStoneWeightOUM(qgold.ListOfSpecsText, 2);
                vsirius.stone_weight_unit_of_measure3 = null;// ultilitiesMapper3.GetValueStoneWeightOUM(qgold.ListOfSpecsText, 3);
                vsirius.total_gem_weight = null;//string.IsNullOrEmpty(qgold.Gem_Weight) ? null : qgold.Gem_Weight;
                vsirius.total_gem_weight_unit_of_measure = string.IsNullOrEmpty(qgold.Gem_Weight) ? null : ultilitiesMapper3.GetValueTotalGemWeight(qgold.Item_Type, qgold.CategoriesText, qgold.ListOfSpecsText);
                //vsirius.size_per_pearl = null;
                vsirius.item_display_length = ultilitiesMapper3.GetLengthInListOfSpecs(qgold.ListOfSpecsText, false);
                vsirius.item_display_length_unit_of_measure =
                    string.IsNullOrWhiteSpace(vsirius.item_display_length) ? "" : ultilitiesMapper3.GetValueItemLengthOUM(qgold.ListOfSpecsText);
                vsirius.band_width = null;
                vsirius.band_width_unit_of_measure = null;
                vsirius.case_diameter = null;
                vsirius.case_diameter_unit_of_measure = null;
                vsirius.item_shape = ultilitiesMapper3.GetValueItemShape(qgold.Item_Type, qgold.CategoriesText, qgold.ListOfSpecsText);
                vsirius.website_shipping_weight = ultilitiesMapper3.GetWebsiteShippingWeight(vsirius.item_display_weight);
                vsirius.website_shipping_weight_unit_of_measure = "GR";
                vsirius.maximum_size = null;
                vsirius.maximum_size_unit_of_measure = null;
                vsirius.line_size = null;
                vsirius.line_size_unit_of_measure = null;
                vsirius.paper_size = null;
                vsirius.paper_size_unit_of_measure = null;
                //vsirius.item_display_height = ultilitiesMapper3.GetLengthInListOfSpecs(qgold.ListOfSpecsText, false);
                vsirius.item_display_height = null;
                vsirius.item_display_height_unit_of_measure = null;
                //vsirius.item_display_width = ultilitiesMapper3.GetWidthInListOfSpecs(qgold.ListOfSpecsText, false);
                vsirius.item_display_width = null;
                vsirius.item_display_width_unit_of_measure = null;
                //vsirius.stone_dimensions_unit_of_measure1 = null;
                //vsirius.stone_dimensions_unit_of_measure2 = null;
                //vsirius.stone_dimensions_unit_of_measure3 = null;
                //vsirius.item_display_width_unit_of_measure = ultilitiesMapper3.GetValueItemDisplayWidthOUM(qgold.Item_Type, qgold.CategoriesText, qgold.ListOfSpecsText);
                //vsirius.total_metal_weight = qgold.Weight;
                vsirius.total_metal_weight = null;
                vsirius.total_metal_weight_unit_of_measure = null;
                //vsirius.total_metal_weight_unit_of_measure = ultilitiesMapper3.GetValueTotalMetalWeightUOM(qgold.Item_Type, qgold.CategoriesText, qgold.ListOfSpecsText);
                vsirius.stone_width1 = null;// ultilitiesMapper3.GetValueStoneOrWidthLength(qgold.ListOfSpecsText, 1, false, false);
                vsirius.stone_width_unit_of_measure1 = null;// ultilitiesMapper3.GetValueStoneOrWidthLength(qgold.ListOfSpecsText, 1, false, true);
                vsirius.stone_width2 = null;// ultilitiesMapper3.GetValueStoneOrWidthLength(qgold.ListOfSpecsText, 2, false, false);
                vsirius.stone_width_unit_of_measure2 = null;//ultilitiesMapper3.GetValueStoneOrWidthLength(qgold.ListOfSpecsText, 2, false, true);
                vsirius.stone_width3 = null;//ultilitiesMapper3.GetValueStoneOrWidthLength(qgold.ListOfSpecsText, 3, false, false);
                vsirius.stone_width_unit_of_measure3 = null;// ultilitiesMapper3.GetValueStoneOrWidthLength(qgold.ListOfSpecsText, 3, false, true);
                vsirius.measurement_system = null;
                vsirius.item_diameter_derived = null;
                vsirius.item_diameter_unit_of_measure = null;
                vsirius.maximum_weight_recommendation = null;
                vsirius.maximum_weight_recommendation_unit_of_measure = null;
                vsirius.stone_length1 = null;//ultilitiesMapper3.GetValueStoneOrWidthLength(qgold.ListOfSpecsText, 1, true, false);
                vsirius.stone_length2 = null;//ultilitiesMapper3.GetValueStoneOrWidthLength(qgold.ListOfSpecsText, 2, true, false);
                vsirius.stone_length3 = null;//ultilitiesMapper3.GetValueStoneOrWidthLength(qgold.ListOfSpecsText, 3, true, false);
                vsirius.stone_length_unit_of_measure1 = null;//ultilitiesMapper3.GetValueStoneOrWidthLength(qgold.ListOfSpecsText, 1, false, true);
                vsirius.stone_length_unit_of_measure2 = null;// ultilitiesMapper3.GetValueStoneOrWidthLength(qgold.ListOfSpecsText, 2, false, true);
                vsirius.stone_length_unit_of_measure3 = null;//ultilitiesMapper3.GetValueStoneOrWidthLength(qgold.ListOfSpecsText, 3, false, true);
                vsirius.stone_height1 = null;//ultilitiesMapper3.GetValueStoneOrWidthLength(qgold.ListOfSpecsText, 1, true, false);
                vsirius.stone_height2 = null;//ultilitiesMapper3.GetValueStoneOrWidthLength(qgold.ListOfSpecsText, 2, true, false);
                vsirius.stone_height3 = null;//ultilitiesMapper3.GetValueStoneOrWidthLength(qgold.ListOfSpecsText, 3, true, false);
                vsirius.item_thickness_unit_of_measure = null;
                vsirius.item_thickness_derived = null;
                vsirius.band_size_num = null;
                vsirius.band_size_num_unit_of_measure = null;
                vsirius.leg_diameter = null;
                vsirius.leg_diameter_unit_of_measure = null;
                vsirius.neck_size = null;
                vsirius.neck_size_unit_of_measure = null;
                vsirius.shoe_width = null;
                vsirius.shoe_width_unit_of_measure = null;
                vsirius.waist_size = null;
                vsirius.waist_size_unit_of_measure = null;
                #endregion

                #region Fulfillment
                vsirius.fulfillment_center_id = null;
                #endregion

                #region Compliance
                vsirius.regulation_type1 = null;
                vsirius.regulation_type2 = null;
                vsirius.regulation_type3 = null;
                vsirius.regulation_type4 = null;
                vsirius.regulation_type5 = null;
                vsirius.regulatory_compliance_certification_value1 = null;
                vsirius.regulatory_compliance_certification_value2 = null;
                vsirius.regulatory_compliance_certification_value3 = null;
                vsirius.regulatory_compliance_certification_value4 = null;
                vsirius.regulatory_compliance_certification_value5 = null;
                vsirius.batteries_required = null;
                //vsirius.are_batteries_included = "FALSE";
                //vsirius.battery_cell_composition = null;
                vsirius.are_batteries_included = null;
                //vsirius.battery_type1 = null;
                //vsirius.battery_type2 = null;
                //vsirius.battery_type3 = null;
                //vsirius.number_of_batteries1 = null;
                //vsirius.number_of_batteries2 = null;
                //vsirius.number_of_batteries3 = null;
                //vsirius.battery_weight = null;
                //vsirius.battery_weight_unit_of_measure = null;
                //vsirius.number_of_lithium_metal_cells = null;
                //vsirius.number_of_lithium_ion_cells = null;
                //vsirius.lithium_battery_packaging = null;
                //vsirius.lithium_battery_energy_content = null;
                //vsirius.lithium_battery_energy_content_unit_of_measure = null;
                //vsirius.lithium_battery_weight = null;
                //vsirius.lithium_battery_weight_unit_of_measure = null;
                //vsirius.hazmat_united_nations_regulatory_id = null;
                //vsirius.safety_data_sheet_url = null;
                vsirius.item_weight = string.IsNullOrEmpty(qgold.Weight) ? "0" : qgold.Weight;
                vsirius.item_weight_unit_of_measure = ultilitiesMapper3.GetWeightUOM(qgold.ListOfSpecsText, "GR");
                //vsirius.item_volume = null;
                //vsirius.item_volume_unit_of_measure = null;
                //vsirius.warranty_type = null;
                //vsirius.legal_compliance_certification_expiration_date = null;                //ngày hết hạn chứng nhận tuân thủ pháp luật
                //vsirius.legal_compliance_certification_regulatory_organization_name = null;   //Tên tổ chức quản lý
                //vsirius.legal_compliance_certification_status = null;                         //Tên tổ chức quản lý
                //vsirius.flash_point = null;
                //vsirius.legal_compliance_certification_date_of_issue = null;                  //chứng nhận tuân thủ pháp luật ngày cấp
                //vsirius.legal_compliance_certification_metadata = null;                       //Giá trị chứng nhận tuân thủ
                //vsirius.legal_compliance_certification_value = null;                          //Chứng nhận tuân thủ pháp luật
                vsirius.metal_stamp = ultilitiesMapper3.GetValueMetalStamp(qgold.Item_Type, qgold.CategoriesText, qgold.ListOfSpecsText, false);
                //vsirius.ghs_classification_class1 = null;                                     //Chữ tượng hình phân loại/GHS
                //vsirius.ghs_classification_class2 = null;
                //vsirius.ghs_classification_class3 = null;
                //vsirius.california_proposition_65_compliance_type = null;                      //Loại Cảnh báo Đề xuất 65 của California
                //vsirius.california_proposition_65_chemical_names1 = null;
                //vsirius.pesticide_marking_type1 = null;                                        //pesticide => Thuốc trừ sâu
                //vsirius.pesticide_marking_type2 = null;
                //vsirius.pesticide_marking_type3 = null;
                //vsirius.pesticide_marking_registration_status1 = null;
                //vsirius.pesticide_marking_registration_status2 = null;
                //vsirius.pesticide_marking_registration_status3 = null;
                //vsirius.pesticide_marking_certification_number1 = null;
                //vsirius.pesticide_marking_certification_number2 = null;
                //vsirius.pesticide_marking_certification_number3 = null;
                //vsirius.fcc_radio_frequency_emission_compliance_registration_status = null;     // radio
                //vsirius.fcc_radio_frequency_emission_compliance_point_of_contact_email = null;
                //vsirius.fcc_radio_frequency_emission_compliance_point_of_contact_phone_number = null;
                //vsirius.fcc_radio_frequency_emission_compliance_point_of_contact_name = null;
                //vsirius.fcc_radio_frequency_emission_compliance_identification_number = null;
                //vsirius.fcc_radio_frequency_emission_compliance_point_of_contact_address = null;
                vsirius.cpsia_cautionary_description = null;
                vsirius.specific_uses_for_product1 = null;
                vsirius.specific_uses_for_product2 = null;
                #endregion

                #region Offer
                vsirius.merchant_release_date = null;  //ngày phát hành
                vsirius.list_price = "0";
                vsirius.restock_date = null;             // ngày bổ sung

                vsirius.fulfillment_latency = null;     // sử dụng khi thời gian sản xuất lớn hơn 2 ngày làm việc
                vsirius.offering_can_be_giftwrapped = null;
                vsirius.item_package_quantity = null;
                vsirius.offering_end_date = null;       // ngày kết thúc ưu đãi
                vsirius.max_order_quantity = null;
                vsirius.merchant_shipping_group_name = null;
                vsirius.offering_start_date = null;
                vsirius.product_tax_code = null;
                vsirius.offering_can_be_gift_messaged = "Yes";//null; // Cung cấp có thể là món quà được nhắn tin
                vsirius.condition_note = null;                    // dành cho các sản phẩm cũ cần note trạng thái sp
                vsirius.sale_price = null;
                vsirius.sale_from_date = null;
                vsirius.sale_end_date = null;
                vsirius.product_site_launch_date = null;         // ngày ra mắt
                vsirius.max_aggregate_ship_quantity = null;
                vsirius.is_discontinued_by_manufacturer = null;
                vsirius.delivery_schedule_group_id = null;
                vsirius.product_expiration_type = null;

                vsirius.image_standard_1 = ultilitiesMapper3.ImageStandard(qgold.Image1Link);
                vsirius.image_standard_2 = ultilitiesMapper3.ImageStandard(qgold.Image2Link);
                vsirius.image_standard_3 = ultilitiesMapper3.ImageStandard(qgold.Image3Link);
                vsirius.image_standard_4 = ultilitiesMapper3.ImageStandard(qgold.Image4Link);
                vsirius.image_standard_5 = ultilitiesMapper3.ImageStandard(qgold.Image5Link);
                #endregion
            }
            catch (Exception ex)
            {
                Common.WriteLog(ex.Message);
                throw;
            }

            return vsirius;
        }
    }
}
