using ExcludedProductAPI.DTOs;
using ExcludedProductAPI.Models;
using SharpCompress.Common;

namespace ExcludedProductAPI.Helpers
{
    public class CustomMapper
    {
        public static ExcludedProductDTO? ChangeExcludedProductToDTO(ExcludedProduct? entity) {
            if (entity == null) return null;
            return new ExcludedProductDTO()
            {
                ExclusionNote = entity.ExclusionNote,
                ExclusionType = entity.ExclusionType.ToString(),
                Marketplace = entity.Marketplace,
                MarketplaceSKU = entity.MarketplaceSKU,
                SupplierSKU = entity.SupplierSKU,
            };
        }

        public static ExcludedProduct ChangeExcludedDtoToEntity(ExcludedProductDTO dto)
        {
            return new ExcludedProduct()
            {
                SupplierSKU = dto.SupplierSKU,
                MarketplaceSKU = dto.MarketplaceSKU,
                Marketplace = dto.Marketplace,
                ExclusionNote = dto.ExclusionNote,
                ExclusionType = (ExclusionType)Enum.Parse(typeof(ExclusionType), dto.ExclusionType),
                TimeStamp = DateTime.Now,
            };
        }
    }
}
