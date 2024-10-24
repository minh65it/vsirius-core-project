namespace VsiriusDataModel.DataModel
{
    public interface IVsiriusMenuHeader
    {
        int MenuHeaderId { get; set; }
        string? Title { get; set; }
        string? Description { get; set; }
        int? Position { get; set; }
        int? Status { get; set; }
        int? ParentId { get; set; }
        string? CreatedBy { get; set; }
        DateTime? CreatedDate { get; set; }
        string? UpdatedBy { get; set; }
        DateTime? UpdatedDate { get; set; }
    }

    public class VsiriusMenuHeader : IVsiriusMenuHeader
    {
        public int MenuHeaderId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int? Position { get; set; }
        public int? Status { get; set; }
        public int? ParentId { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
