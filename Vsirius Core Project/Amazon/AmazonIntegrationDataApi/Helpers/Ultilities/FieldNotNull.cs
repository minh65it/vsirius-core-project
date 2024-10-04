using AmazonIntegrationDataApi.Dtos;

namespace AmazonIntegrationDataApi.Helpers.Ultilities
{
    public class FieldNotNull
    {
        public static AmazonJewelryDataFeedItemV3_Dto FieldNotNulls(AmazonJewelryDataFeedItemV3_Dto dto)
        {
            if (string.IsNullOrEmpty(dto.feed_product_type))
            {
                dto.feed_product_type = null;
            }

            if (string.IsNullOrEmpty(dto.item_sku))
            {
                dto.item_sku = null;
            }

            if (string.IsNullOrEmpty(dto.brand_name))
            {
                dto.brand_name = null;
            }

            if (string.IsNullOrEmpty(dto.item_name))
            {
                dto.item_name = null;
            }
            if (string.IsNullOrEmpty(dto.item_length_numeric))
            {
                dto.item_length_numeric = null;
            }
            if (string.IsNullOrEmpty(dto.item_length_numeric_unit_of_measure))
            {
                dto.item_length_numeric_unit_of_measure = null;
            }
            if (string.IsNullOrEmpty(dto.main_image_url))
            {
                dto.main_image_url = null;
            }
            if (string.IsNullOrEmpty(dto.quantity))
            {
                dto.quantity = null;
            }
            if (string.IsNullOrEmpty(dto.standard_price))
            {
                dto.standard_price = null;
            }

            if (string.IsNullOrEmpty(dto.item_type))
            {
                dto.item_type = null;
            }
            if (string.IsNullOrEmpty(dto.metal_type))
            {
                dto.metal_type = null;
            }
            if (string.IsNullOrEmpty(dto.material_type1))
            {
                dto.material_type1 = null;
            }
            if (string.IsNullOrEmpty(dto.material_type2))
            {
                dto.material_type2 = null;
            }
            if (string.IsNullOrEmpty(dto.material_type3))
            {
                dto.material_type3 = null;
            }
            if (string.IsNullOrEmpty(dto.material_type4))
            {
                dto.material_type4 = null;
            }
            if (string.IsNullOrEmpty(dto.material_type5))
            {
                dto.material_type5 = null;
            }
            if (string.IsNullOrEmpty(dto.department_name1))
            {
                dto.department_name1 = null;
            }
            if (string.IsNullOrEmpty(dto.department_name2))
            {
                dto.department_name2 = null;
            }
            if (string.IsNullOrEmpty(dto.department_name3))
            {
                dto.department_name3 = null;
            }
            if (string.IsNullOrEmpty(dto.department_name4))
            {
                dto.department_name4 = null;
            }
            if (string.IsNullOrEmpty(dto.department_name5))
            {
                dto.department_name5 = null;
            }
            if (string.IsNullOrEmpty(dto.gem_type1))
            {
                dto.gem_type1 = null;
            }


            if (string.IsNullOrEmpty(dto.product_description))
            {
                dto.product_description = "Your product description should be the marketing pitch for your product that includes a concise but friendly overview of your product, what it is used for, and what makes it special.";
            }

            if (string.IsNullOrEmpty(dto.generic_keywords))
            {
                dto.generic_keywords = "In order for customers to find your products on Amazon, it’s important to provide Search Terms they might use when searching for what they want to buy. Separate words with spaces, don't use brand names, don't use product SKUs.";
            }
            if (string.IsNullOrEmpty(dto.bullet_point1))
            {
                dto.bullet_point1 = null;
            }
            if (string.IsNullOrEmpty(dto.bullet_point2))
            {
                dto.bullet_point2 = null;
            }
            if (string.IsNullOrEmpty(dto.bullet_point3))
            {
                dto.bullet_point3 = null;
            }
            if (string.IsNullOrEmpty(dto.bullet_point4))
            {
                dto.bullet_point4 = null;
            }
            if (string.IsNullOrEmpty(dto.bullet_point5))
            {
                dto.bullet_point5 = null;
            }
            if (string.IsNullOrEmpty(dto.bullet_point6))
            {
                dto.bullet_point6 = null;
            }
            if (string.IsNullOrEmpty(dto.bullet_point7))
            {
                dto.bullet_point7 = null;
            }
            if (string.IsNullOrEmpty(dto.bullet_point8))
            {
                dto.bullet_point8 = null;
            }
            if (string.IsNullOrEmpty(dto.bullet_point9))
            {
                dto.bullet_point9 = null;
            }
            if (string.IsNullOrEmpty(dto.bullet_point10))
            {
                dto.bullet_point10 = null;
            }
            if (string.IsNullOrEmpty(dto.supplier_declared_dg_hz_regulation1))
            {
                dto.supplier_declared_dg_hz_regulation1 = null;
            }
            if (string.IsNullOrEmpty(dto.supplier_declared_dg_hz_regulation2))
            {
                dto.supplier_declared_dg_hz_regulation2 = null;
            }
            if (string.IsNullOrEmpty(dto.supplier_declared_dg_hz_regulation3))
            {
                dto.supplier_declared_dg_hz_regulation3 = null;
            }
            if (string.IsNullOrEmpty(dto.supplier_declared_dg_hz_regulation4))
            {
                dto.supplier_declared_dg_hz_regulation4 = null;
            }
            if (string.IsNullOrEmpty(dto.supplier_declared_dg_hz_regulation5))
            {
                dto.supplier_declared_dg_hz_regulation5 = null;
            }
            if (string.IsNullOrEmpty(dto.color_name))
            {
                dto.color_name = null;
            }
            if (string.IsNullOrEmpty(dto.part_number))
            {
                dto.part_number = null;
            }
            if (string.IsNullOrEmpty(dto.item_type_name))
            {
                dto.item_type_name = null;
            }
            if (string.IsNullOrEmpty(dto.size_name))
            {
                dto.size_name = null;
            }
            if (string.IsNullOrEmpty(dto.country_of_origin))
            {
                dto.country_of_origin = null;
            }
            if (string.IsNullOrEmpty(dto.cpsia_cautionary_statement1))
            {
                dto.cpsia_cautionary_statement1 = null;
            }
            if (string.IsNullOrEmpty(dto.cpsia_cautionary_statement2))
            {
                dto.cpsia_cautionary_statement2 = null;
            }
            if (string.IsNullOrEmpty(dto.cpsia_cautionary_statement3))
            {
                dto.cpsia_cautionary_statement3 = null;
            }
            if (string.IsNullOrEmpty(dto.cpsia_cautionary_statement4))
            {
                dto.cpsia_cautionary_statement4 = null;
            }
            if (string.IsNullOrEmpty(dto.supplier_declared_material_regulation1))
            {
                dto.supplier_declared_material_regulation1 = null;
            }
            if (string.IsNullOrEmpty(dto.supplier_declared_material_regulation2))
            {
                dto.supplier_declared_material_regulation2 = null;
            }
            if (string.IsNullOrEmpty(dto.supplier_declared_material_regulation3))
            {
                dto.supplier_declared_material_regulation3 = null;
            }
            if (string.IsNullOrEmpty(dto.map_price))
            {
                dto.map_price = null;
            }
            if (string.IsNullOrEmpty(dto.currency))
            {
                dto.currency = null;
            }
            if (string.IsNullOrEmpty(dto.condition_type))
            {
                dto.condition_type = null;
            }
            if (string.IsNullOrEmpty(dto.ring_size))
            {
                dto.ring_size = null;
            }
            if (string.IsNullOrEmpty(dto.color_map))
            {
                dto.color_map = null;
            }
            if (string.IsNullOrEmpty(dto.batteries_required))
            {
                dto.batteries_required = null;
            }
            if (string.IsNullOrEmpty(dto.are_batteries_included))
            {
                dto.are_batteries_included = null;
            }
            if (string.IsNullOrEmpty(dto.battery_cell_composition))
            {
                dto.battery_cell_composition = null;
            }
            if (string.IsNullOrEmpty(dto.battery_type1))
            {
                dto.battery_type1 = null;
            }
            if (string.IsNullOrEmpty(dto.battery_type2))
            {
                dto.battery_type2 = null;
            }
            if (string.IsNullOrEmpty(dto.battery_type3))
            {
                dto.battery_type3 = null;
            }
            if (string.IsNullOrEmpty(dto.number_of_batteries1))
            {
                dto.number_of_batteries1 = null;
            }
            if (string.IsNullOrEmpty(dto.number_of_batteries2))
            {
                dto.number_of_batteries2 = null;
            }
            if (string.IsNullOrEmpty(dto.number_of_batteries3))
            {
                dto.number_of_batteries3 = null;
            }
            if (string.IsNullOrEmpty(dto.battery_weight))
            {
                dto.battery_weight = null;
            }
            if (string.IsNullOrEmpty(dto.battery_weight_unit_of_measure))
            {
                dto.battery_weight_unit_of_measure = null;
            }
            if (string.IsNullOrEmpty(dto.number_of_lithium_metal_cells))
            {
                dto.number_of_lithium_metal_cells = null;
            }
            if (string.IsNullOrEmpty(dto.number_of_lithium_ion_cells))
            {
                dto.number_of_lithium_ion_cells = null;
            }
            if (string.IsNullOrEmpty(dto.lithium_battery_packaging))
            {
                dto.lithium_battery_packaging = null;
            }
            if (string.IsNullOrEmpty(dto.lithium_battery_energy_content))
            {
                dto.lithium_battery_energy_content = null;
            }
            if (string.IsNullOrEmpty(dto.lithium_battery_energy_content_unit_of_measure))
            {
                dto.lithium_battery_energy_content_unit_of_measure = null;
            }
            if (string.IsNullOrEmpty(dto.lithium_battery_weight))
            {
                dto.lithium_battery_weight = null;
            }
            if (string.IsNullOrEmpty(dto.lithium_battery_weight_unit_of_measure))
            {
                dto.lithium_battery_weight_unit_of_measure = null;
            }
            if (string.IsNullOrEmpty(dto.supplier_declared_dg_hz_regulation1))
            {
                dto.supplier_declared_dg_hz_regulation1 = null;
            }
            if (string.IsNullOrEmpty(dto.supplier_declared_dg_hz_regulation2))
            {
                dto.supplier_declared_dg_hz_regulation2 = null;
            }
            if (string.IsNullOrEmpty(dto.supplier_declared_dg_hz_regulation3))
            {
                dto.supplier_declared_dg_hz_regulation3 = null;
            }
            if (string.IsNullOrEmpty(dto.supplier_declared_dg_hz_regulation4))
            {
                dto.supplier_declared_dg_hz_regulation4 = null;
            }
            if (string.IsNullOrEmpty(dto.supplier_declared_dg_hz_regulation5))
            {
                dto.supplier_declared_dg_hz_regulation5 = null;
            }
            //if (string.IsNullOrEmpty(dto.hazmat_united_nations_regulatory_id))
            //{
            //    dto.hazmat_united_nations_regulatory_id = null;
            //}

            if (string.IsNullOrEmpty(dto.depth_front_to_back))
            {
                dto.depth_front_to_back = null;
            }

            if (string.IsNullOrEmpty(dto.depth_front_to_back_unit_of_measure))
            {
                dto.depth_front_to_back_unit_of_measure = null;
            }
            if (string.IsNullOrEmpty(dto.depth_width_side_to_side))
            {
                dto.depth_width_side_to_side = null;
            }
            if (string.IsNullOrEmpty(dto.depth_width_side_to_side_unit_of_measure))
            {
                dto.depth_width_side_to_side_unit_of_measure = null;
            }
            if (string.IsNullOrEmpty(dto.depth_height_floor_to_top))
            {
                dto.depth_height_floor_to_top = null;
            }
            if (string.IsNullOrEmpty(dto.depth_height_floor_to_top_unit_of_measure))
            {
                dto.depth_height_floor_to_top_unit_of_measure = null;
            }
            if (string.IsNullOrEmpty(dto.package_height))
            {
                dto.package_height = null;
            }
            if (string.IsNullOrEmpty(dto.package_width))
            {
                dto.package_width = null;
            }
            if (string.IsNullOrEmpty(dto.package_length))
            {
                dto.package_length = null;
            }

            if (string.IsNullOrEmpty(dto.package_length_unit_of_measure))
            {
                dto.package_length_unit_of_measure = null;
            }
            if (string.IsNullOrEmpty(dto.package_weight))
            {
                dto.package_weight = null;
            }
            if (string.IsNullOrEmpty(dto.package_weight_unit_of_measure))
            {
                dto.package_weight_unit_of_measure = null;
            }
            if (string.IsNullOrEmpty(dto.package_height_unit_of_measure))
            {
                dto.package_height_unit_of_measure = null;
            }
            if (string.IsNullOrEmpty(dto.package_width_unit_of_measure))
            {
                dto.package_width_unit_of_measure = null;
            }

            if (string.IsNullOrEmpty(dto.lifecycle_supply_type))
            {
                dto.lifecycle_supply_type = null;
            }
            if (string.IsNullOrEmpty(dto.map_price))
            {
                dto.map_price = null;
            }
            if (string.IsNullOrEmpty(dto.style_name))
            {
                dto.style_name = null;
            }
            if (string.IsNullOrEmpty(dto.unit_count))
            {
                dto.unit_count = null;
            }
            if (string.IsNullOrEmpty(dto.unit_count_type))
            {
                dto.unit_count_type = null;
            }
            if (string.IsNullOrEmpty(dto.closure_type))
            {
                dto.closure_type = null;
            }
            if (string.IsNullOrEmpty(dto.care_instructions))
            {
                dto.care_instructions = null;
            }
            if (string.IsNullOrEmpty(dto.fabric_type1))
            {
                dto.fabric_type1 = null;
            }
            if (string.IsNullOrEmpty(dto.fabric_type2))
            {
                dto.fabric_type2 = null;
            }
            if (string.IsNullOrEmpty(dto.fabric_type3))
            {
                dto.fabric_type3 = null;
            }
            if (string.IsNullOrEmpty(dto.fabric_type4))
            {
                dto.fabric_type4 = null;
            }
            if (string.IsNullOrEmpty(dto.fabric_type5))
            {
                dto.fabric_type5 = null;
            }
            if (string.IsNullOrEmpty(dto.fabric_type6))
            {
                dto.fabric_type6 = null;
            }
            if (string.IsNullOrEmpty(dto.fabric_type7))
            {
                dto.fabric_type7 = null;
            }
            if (string.IsNullOrEmpty(dto.fabric_type8))
            {
                dto.fabric_type8 = null;
            }
            if (string.IsNullOrEmpty(dto.fabric_type9))
            {
                dto.fabric_type9 = null;
            }
            if (string.IsNullOrEmpty(dto.fabric_type10))
            {
                dto.fabric_type10 = null;
            }
            if (string.IsNullOrEmpty(dto.import_designation))
            {
                dto.import_designation = null;
            }
            if (string.IsNullOrEmpty(dto.generic_keywords))
            {
                dto.generic_keywords = null;
            }
            if (string.IsNullOrEmpty(dto.outer_material_type1))
            {
                dto.outer_material_type1 = null;
            }
            if (string.IsNullOrEmpty(dto.outer_material_type2))
            {
                dto.outer_material_type2 = null;
            }
            if (string.IsNullOrEmpty(dto.outer_material_type3))
            {
                dto.outer_material_type3 = null;
            }
            if (string.IsNullOrEmpty(dto.outer_material_type4))
            {
                dto.outer_material_type4 = null;
            }
            if (string.IsNullOrEmpty(dto.outer_material_type5))
            {
                dto.outer_material_type5 = null;
            }
            if (string.IsNullOrEmpty(dto.material_composition))
            {
                dto.material_composition = null;
            }
            if (string.IsNullOrEmpty(dto.size_map))
            {
                dto.size_map = null;
            }
            if (string.IsNullOrEmpty(dto.model))
            {
                dto.model = null;
            }
            return dto;
        }
    }
}
