namespace PhillyLuxEcommerce.Dtos
{
    public class FilterDto
    {
        public string? fieldName { get; set; }
        public string? labelName { get; set; }
        public List<string>? value { get; set; }
        public string? productType { get; set; }
        public string? groupName { get; set; }
    }
}
