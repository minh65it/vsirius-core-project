namespace VsiriusDataModel.DataModel
{
    public interface IVsiriusFilter
    {
        int FilterId { get; set; }
        string? FieldName { get; set; }
        string? LabelName { get; set; }
        string? Value { get; set; }
        string? ProductType { get; set; }
        string? GroupName { get; set; }
        int? ParentId { get; set; }
        int? OrderId { get; set; }
        string? ImageURL { get; set; }
        bool? IsActive { get; set; }
        bool? IsDelete { get; set; }

        string? CreatedBy { get; set; }
        DateTime? CreatedDate { get; set; }
        string? UpdatedBy { get; set; }
        DateTime? UpdatedDate { get; set; }
        string? Title { get; set; }
    }


    public class VsiriusFilter : IVsiriusFilter
    {
        public int FilterId { get; set; }
        public string? FieldName { get; set; }
        public string? LabelName { get; set; }
        public string? Value { get; set; }
        public string? ProductType { get; set; }
        public string? GroupName { get; set; }
        public int? ParentId { get; set; }
        public int? OrderId { get; set; }
        public string? ImageURL { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDelete { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? Title { get; set; }
    }
}
