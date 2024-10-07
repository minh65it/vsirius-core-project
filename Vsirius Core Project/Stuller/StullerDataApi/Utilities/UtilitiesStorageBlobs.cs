using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using MongoDB.Driver;
using StullerApiClient.Models;
using StullerDataApi.Models;
using System.IO;

namespace StullerDataApi.Utilities
{
    public interface IUtilitiesStorageBlobs
    {
        public Task<List<Product>> UpdateImages(List<Product> dataProductBackup, List<Product> destination);
    }
    public class UtilitiesStorageBlobs : IUtilitiesStorageBlobs
    {
        private readonly IConfiguration _config;
        private string? conn = "";
        private string? dataBase = "";
        private string? _storageConnectionString = "";
        private string? _storageContainerName = "";
        private readonly IMongoDatabase _database;
        private string? productBackupCollection = "";
        private readonly IMongoCollection<Product> _productBackupCollection;
        public UtilitiesStorageBlobs(IConfiguration config)
        {
            _config = config;
            conn = _config.GetSection("Database:ConnectionString").Value;
            dataBase = _config.GetSection("Database:DatabaseName").Value;
            _storageConnectionString = _config.GetSection("PullStuller:BlobConnectionString").Value;
            _storageContainerName = _config.GetSection("PullStuller:BlobContainerName").Value;
            productBackupCollection = _config.GetSection("PullStuller:productBackupCollection").Value;

            //connection database
            var mongoClient = new MongoClient(conn);
            _database = mongoClient.GetDatabase(dataBase);
            _productBackupCollection = _database.GetCollection<Product>(productBackupCollection);
        }

        public async Task<BlobResponseDto> UploadWithLink(string linkImage, int index, string sku)
        {
            try
            {
                BlobResponseDto response = new();
                if (string.IsNullOrWhiteSpace(linkImage))
                {
                    return response;
                }

                string fileName = $"{index}.jpg";
                string url = Path.Combine($"S-{sku}", fileName);
                var containerClient = new BlobContainerClient(_storageConnectionString, _storageContainerName);
                await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);

                var blob = containerClient.GetBlobClient(url);
                if (!(await blob.ExistsAsync()))
                {
                    using var client = new HttpClient();
                    var stream = await client.GetStreamAsync(linkImage);
                    var memoryStream = new MemoryStream();
                    await stream.CopyToAsync(memoryStream);
                    try
                    {
                        memoryStream.Position = 0;
                        await blob.UploadAsync(memoryStream, new BlobHttpHeaders
                        {
                            ContentType = "image/jpeg"
                        });
                    }
                    // If the file already exists, we catch the exception and do not upload it
                    catch (RequestFailedException ex)
                        when (ex.ErrorCode == BlobErrorCode.BlobAlreadyExists)
                    {
                        //_logger.LogError($"File with name {fileName} already exists in container. Set another name to store the file in the container: '{_storageContainerName}.'");
                        response.Status = $"File with name {fileName} already exists. Please use another name to store your file.";
                        response.Error = true;
                        response.Blob.Uri = blob.Uri.AbsoluteUri;
                        response.Blob.Name = blob.Name;
                        return response;
                    }
                    // If we get an unexpected error, we catch it here and return the error message
                    catch (RequestFailedException ex)
                    {
                        response.Status = $"Unexpected error: {ex.StackTrace}. Check log with StackTrace ID.";
                        response.Error = true;
                        return response;
                    }
                }

                response.Status = $"File {fileName} Uploaded Successfully";
                response.Error = false;
                response.Blob.Uri = blob.Uri.AbsoluteUri;
                response.Blob.Name = blob.Name;
                // Return the BlobUploadResponse object
                return response;
            }
            catch (Exception ex)
            {
                await Common.WriteLogAsync($"get Link from Azure error with : {ex.ToString()} at [{DateTime.Now}]. SKU = {sku}, linkimg = {linkImage}, index = {index}");
                return null;
            }

        }

        public async Task<List<Product>> UpdateImages(List<Product> dataProductBackup, List<Product> destination)
        {
            var bulkOperations = new List<WriteModel<Product>>();

            //cập nhật url theo collection backup cũ
            if (destination.Count > 0 && dataProductBackup.Count > 0)
            {
                foreach (var item in destination)
                {
                    var item2 = await _productBackupCollection.Find(x => x.SKU == item.SKU).FirstOrDefaultAsync();
                    if (item2 != null)
                    {
                        item.Images = item2.Images;
                        item.GroupImages = item2.GroupImages;
                    }
                }
            }

            //cập nhật url theo Azure
            var listSKUUpdateUrl = destination.Select(x => x.SKU).ToList().Except(dataProductBackup.Select(x => x.SKU).ToList()).ToList();
            if (listSKUUpdateUrl.Count > 0)
            {
                var bulkOperationsUrl = new List<WriteModel<Product>>();
                int count = 0;
                foreach (var sku in listSKUUpdateUrl)
                {
                    var item = destination.FirstOrDefault(x => x.SKU == sku);
                    //Images
                    if (item?.Images?.Count > 0)
                    {
                        for (int i = 0; i < item.Images.Count; i++)
                        {
                            if (i >= 10)
                                break;
                            var newURL = await UploadWithLink(item.Images[i].ZoomUrl, i, item.SKU);
                            item.Images[i].FullUrl = newURL?.Blob.Uri;
                            item.Images[i].ZoomUrl = newURL?.Blob.Uri;
                        }
                    }
                    //GroupImages
                    if (item?.GroupImages?.Count > 0)
                    {
                        for (int i = 0; i < item.GroupImages.Count; i++)
                        {
                            if (i >= 10)
                                break;
                            var newURL = await UploadWithLink(item.GroupImages[i].ZoomUrl, 10 + i, item.SKU);
                            item.GroupImages[i].FullUrl = newURL?.Blob.Uri;
                            item.GroupImages[i].ZoomUrl = newURL?.Blob.Uri;
                        }
                    }

                    Console.WriteLine($"Line {count++}");
                }
            }
            return destination;
        }
    }
}
