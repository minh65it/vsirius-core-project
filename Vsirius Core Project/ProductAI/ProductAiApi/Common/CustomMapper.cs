using ProductAiApi.DTOs;
using ProductAiApi.Models;

namespace ProductAiApi.Common
{
    public static class CustomMapper
    {
        public static ProductAiDto? ChangeProductAIToDTO(ProductAI x)
        {
            return new ProductAiDto()
            {
                SKU = x.SKU,
                ProductName = x.ProductName,
                Brand = x.Brand,
                Keywords = x.Keywords,
                BulletPoint1 = x.BulletPoint1,
                BulletPoint2 = x.BulletPoint2,
                BulletPoint3 = x.BulletPoint3,
                BulletPoint4 = x.BulletPoint4,
                BulletPoint5 = x.BulletPoint5,
                BulletPoint6 = x.BulletPoint6,
                BulletPoint7 = x.BulletPoint7,
                BulletPoint8 = x.BulletPoint8,
                BulletPoint9 = x.BulletPoint9,
                IsReview = x.IsReview,
                Note = x.Note,
            };
        }
    }
}
