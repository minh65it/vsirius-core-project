using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace PackageApi.Dtos.Claim
{
    public class ClaimParam
    {
        /// <summary>
        /// 0: ReturnPackage; 1: OrderDocuments; 3: ShippedPackage
        /// </summary>
        public DocumentTypeDto? DocumentType { get; set; }
        public List<string>? Notes { get; set; }
        public List<IFormFile>? Images { get; set; }
    }
    public enum DocumentTypeDto
    {
        /// <summary>
        /// 0: ReturnPackage
        /// </summary>
        ReturnPackage,
        /// <summary>
        /// 1: OrderDocuments
        /// </summary>
        OrderDocuments,
        /// <summary>
        /// 3: ShippedPackage
        /// </summary>
        ShippedPackage,
    }
}
