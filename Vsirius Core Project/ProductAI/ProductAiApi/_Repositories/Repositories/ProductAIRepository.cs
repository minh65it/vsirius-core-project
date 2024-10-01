using ProductAiApi._Repositories.Interfaces;
using ProductAiApi.DataContext;
using ProductAiApi.Models;

namespace ProductAiApi._Repositories.Repositories
{
    public class ProductAIRepository : Repository<ProductAI>, IProductAIRepository
    {
        public ProductAIRepository(ModifiedProductContext context) : base(context)
        {
        }
    }
}
