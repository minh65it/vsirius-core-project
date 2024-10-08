using EbayCoreApi.Dtos;
using EbayCoreApi.Models;

namespace EbayCoreApi.Common.Mappers
{
    public static class CustomMapper
    {
        public static Product TransformPimProductToEbay(PimProductDto item)
        {
            return new Product()
            {
                Sku = item.item_sku,
                Tittle = item.item_name,
                Brand = string.IsNullOrEmpty(item.brand_name) ? "VSirius" : item.brand_name,
                Category = CategoryDictionary.CategoryPIM[item.feed_product_type],
                ProductType = item.feed_product_type.ToCapitalizedCase(),
                ItemStyle = item.item_type_name.Replace("-", " ").ToCapitalizedCase(),
                ItemWeight = $"{item.item_weight} {item.item_weight_unit_of_measure}",
                ChainType = item.chain_type.ToCapitalizedCase(),
                RingSize = item.ring_size,
                Description = item.product_description
                .Replace("</br></br>Customized items can not be returned</br>", ""),
                MainImageUrl = item.main_image_url,
                Image1Url = item.other_image_url1,
                Image2Url = item.other_image_url2,
                Image3Url = item.other_image_url3,
                Image4Url = item.other_image_url4,
                Image5Url = item.other_image_url5,
                Image6Url = item.other_image_url6,
                Image7Url = item.other_image_url7,
                Image8Url = item.other_image_url8,
                Price = decimal.Parse(item.standard_price),
                Origin = item.country_of_origin == "Unknown" ? string.Empty : item.country_of_origin,
                Quantity = int.Parse(item.quantity),
                PrimaryStoneShape = item.stone_shape1 == "Other Shape" ? string.Empty : item.stone_shape1,
                PrimaryStoneSize = string.Empty,
                PrimaryStoneType = MapPrimaryStoneType(item),
                Image9Url = "",
                Note = "",
                Material = item.material_type1,
                ItemColor = item.color_name,
                ItemLength = $"{item.item_display_length} {item.item_length_numeric_unit_of_measure}",
                ItemSize = item.size_name,
                IsDeleted = false,
                CreatedDate = DateTime.Now,
                CreatedBy = string.IsNullOrEmpty(item.createdBy) ? "Admin" : item.createdBy,
                UpdatedDate = DateTime.Now,
                UpdatedBy = string.IsNullOrEmpty(item.createdBy) ? "Admin" : item.createdBy
            };
        }

        private static string MapPrimaryStoneType(PimProductDto item)
        {
            if (string.IsNullOrWhiteSpace(item.gem_type1) || item.gem_type1 == "Not Known")
            {
                return string.Empty;
            }
            else
            {
                return item.gem_type1.Replace("-", " ").ToCapitalizedCase();
            }
        }

        public static string MultipleCategory(string category, int index = 0)
        {
            var arrCategories = category.Split('|');
            if (arrCategories.Count() > index)
            {
                return arrCategories[index];
            }
            else
            {
                return string.Empty;
            }

        }
    }
}