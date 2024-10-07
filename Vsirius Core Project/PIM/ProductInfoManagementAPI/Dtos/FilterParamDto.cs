namespace PimApi.Dtos
{
    public class FilterParamDto
    {
        public string FieldName { get; set; }
        public string? LabelName { get; set; }
        public List<string> Value { get; set; }
        public string? ProductType { get; set; }
        public string? GroupName { get; set; }
    }
    public class GetAllParmas
    {
        public List<FilterParamDto> FilterParamDto { get; set; }
        public string? Keyword { get; set; }
        public Marketplace? marketplace { get; set; }
    }
}
