namespace ProductApi.Dtos
{
    public class VsiriusFilterParamDto
    {
        public string FieldName { get; set; }
        public string? LabelName { get; set; }
        public List<string> Value { get; set; }
        public string? ProductType { get; set; }
        public string? GroupName { get; set; }
    }
}
