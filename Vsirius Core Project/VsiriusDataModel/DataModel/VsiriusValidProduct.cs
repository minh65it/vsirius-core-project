namespace VsiriusDataModel.DataModel
{
    public interface IVsiriusValidProduct
    {
        int FilterId { get; set; }
        string? FieldName { get; set; }
        string? LabelName { get; set; }
        string? Value { get; set; }
        string? ProductType { get; set; }
        string? GroupName { get; set; }
        int? ParentId { get; set; }
        int? OrderId { get; set; }
    }

    public class VsiriusValidProduct : IVsiriusValidProduct
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
