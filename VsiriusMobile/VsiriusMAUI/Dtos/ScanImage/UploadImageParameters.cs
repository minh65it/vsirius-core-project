namespace VsiriusMAUI.Dtos.ScanImage
{
	public class ClaimTextParam
	{
		public DocumentTypeDto? DocumentType { get; set; }
		public List<string>? Notes { get; set; }
		public StreamContent? Image { get; set; }
	}

	//public class ImageTextInfo
	//{
	//	public StreamContent? Image { get; set; }
	//	public string? TextExtract { get; set; }
	//}

	public enum DocumentTypeDto
	{
		None = -1,
		ReturnPackage,
		OrderDocuments,
		ShippedPackage
	}
}
