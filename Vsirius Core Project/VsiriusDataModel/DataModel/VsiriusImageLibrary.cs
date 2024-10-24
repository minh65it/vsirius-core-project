namespace VsiriusDataModel.DataModel
{
    public interface IVsiriusImageLibrary
    {
        Guid ImageLibraryId { get; set; }
        string? Title { get; set; }
        int? Position { get; set; }
        string? Description { get; set; }
        int? Status { get; set; }
        string?   CreatedBy { get; set; }
        DateTime? CreatedDate { get; set; }
        string? UpdatedBy { get; set; }
        DateTime? UpdatedDate { get; set; }
    }

    public class VsiriusImageLibrary : IVsiriusImageLibrary
    {
        public Guid ImageLibraryId { get; set; }
        public string? Title { get; set; }
        public int? Position { get; set; }
        public string? Description { get; set; }
        public int? Status { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
