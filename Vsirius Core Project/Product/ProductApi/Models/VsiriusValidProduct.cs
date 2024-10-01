namespace ProductApi.Models
{
    public class VsiriusValidProduct : VsiriusDataModel.DataModel.IVsiriusValidProduct
    {
        public int FilterId { get; set; }
        public string? FieldName { get; set; }
        public string? LabelName { get; set; }
        public string? Value { get; set; }
        public string? ProductType { get; set; }
        public string? GroupName { get; set; }
        public int? ParentId { get; set; }
        public int? OrderId { get; set; }
    }
}
