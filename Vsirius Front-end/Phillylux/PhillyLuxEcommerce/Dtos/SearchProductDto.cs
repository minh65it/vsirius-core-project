namespace PhillyLuxEcommerce.Dtos
{
    public class SearchProductDto
    {
        public PaginationDto? pagination { get; set; }
        public List<VsiriusProductDto>? result { get; set; }

    }

}
