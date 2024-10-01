namespace ProductApi.Dtos
{
    public class VsiriusValidProductDto
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

        public DateTime? CreatedDateFrom { get; set; }
        public DateTime? CreatedDateTo { get; set; }
        public DateTime? UpdatedDateFrom { get; set; }
        public DateTime? UpdatedDateTo { get; set; }
    }
}
