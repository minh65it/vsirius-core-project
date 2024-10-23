namespace IdentityApi.Dtos
{
    public class BlobResponseDTO
    {
        public BlobResponseDTO()
        {
            Blob = new BlobDto();
        }
        public string? Status { get; set; } = string.Empty;
        public bool Error { get; set; }
        public BlobDto Blob { get; set; }
    }

    public class BlobDto
    {
        public string Uri { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;
        public Stream Content { get; set; }
    }
}
