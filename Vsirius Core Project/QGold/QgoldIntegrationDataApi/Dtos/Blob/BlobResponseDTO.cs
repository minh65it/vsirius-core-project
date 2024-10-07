namespace QgoldIntegrationDataApi.Dtos.Blob
{
    public class BlobResponseDTO
    {
        public BlobResponseDTO()
        {
            Blob = new BlobDto();
        }
        public string Status { get; set; }
        public bool Error { get; set; }
        public BlobDto Blob { get; set; }
    }
}
