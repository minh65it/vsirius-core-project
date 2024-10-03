using PhillyLuxEcommerce.Dtos;

namespace PhillyLuxEcommerce.Dtos.Params
{
    public class SearchParam
    {
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
        public string? keyWord { get; set; }
        public List<FilterDto>? filters { get; set; }
    }
}
