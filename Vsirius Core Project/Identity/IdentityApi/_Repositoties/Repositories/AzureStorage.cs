using Abp.MimeTypes;
using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using IdentityApi._Repositoties.Interfaces;
using IdentityApi.Dtos;
using IdentityApi.Helpers.Utilities;

namespace IdentityApi._Repositoties.Repositories
{
    public class AzureStorage : IAzureStorage

    {
        #region Dependency Injection / Constructor

        private readonly string _storageConnectionString;
        private readonly string _storageContainerName;
        private readonly string _blobFolder;

        public AzureStorage(IConfiguration configuration)
        {
            _storageConnectionString = configuration.GetValue<string>("BlobConnectionString")!;
            _storageContainerName = configuration.GetValue<string>("BlobContainerName")!;
            _blobFolder = configuration.GetValue<string>("BlobFolder")!;
        }

        #endregion

        public async Task<BlobResponseDTO> DeleteAsync(string blobFilename)
        {
            BlobContainerClient client = new(_storageConnectionString, _storageContainerName);

            BlobClient file = client.GetBlobClient(blobFilename);

            try
            {
                // Delete the file
                await file.DeleteAsync();
            }
            catch 
            {
                return new BlobResponseDTO { Error = true, Status = $"File with name {blobFilename} not found." };
            }

            // Return a new BlobResponseDto to the requesting method
            return new BlobResponseDTO { Error = false, Status = $"File: {blobFilename} has been successfully deleted." };

        }

        public async Task<BlobDto> DownloadAsync(string blobFilename)
        {
            // Get a reference to a container named in appsettings.json
            BlobContainerClient client = new(_storageConnectionString, _storageContainerName);

            try
            {
                // Get a reference to the blob uploaded earlier from the API in the container from configuration settings
                BlobClient file = client.GetBlobClient(blobFilename);

                // Check if the file exists in the container
                if (await file.ExistsAsync())
                {
                    var data = await file.OpenReadAsync();
                    Stream blobContent = data;

                    // Download the file details async
                    var content = await file.DownloadContentAsync();

                    // Add data to variables in order to return a BlobDto
                    string name = blobFilename;
                    string contentType = content.Value.Details.ContentType;

                    // Create new BlobDto with blob data from variables
                    return new BlobDto { Content = blobContent, Name = name, ContentType = contentType };
                }
            }
            catch (RequestFailedException ex)
                when (ex.ErrorCode == BlobErrorCode.BlobNotFound)
            {
                // Log error to console
                //_logger.LogError($"File {blobFilename} was not found.");
            }

            // File does not exist, return null and handle that in requesting method
            return null;
        }

        public async Task<List<BlobDto>> ListAsync()
        {
            // Get a reference to a container named in appsettings.json
            BlobContainerClient container = new(_storageConnectionString, _storageContainerName);

            // Create a new list object for 
            List<BlobDto> files = new();

            await foreach (BlobItem file in container.GetBlobsAsync())
            {
                // Add each file retrieved from the storage container to the files list by creating a BlobDto object
                string uri = container.Uri.ToString();
                var name = file.Name;
                var fullUri = $"{uri}/{name}";

                files.Add(new BlobDto
                {
                    Uri = fullUri,
                    Name = name,
                    ContentType = file.Properties.ContentType
                });
            }

            // Return all files to the requesting method
            return files;
        }

        public async Task<BlobResponseDTO> UploadAsync(IFormFile blob)
        {
            // Create new upload response object that we can return to the requesting method
            BlobResponseDTO response = new();

            // Get a reference to a container named in appsettings.json and then create it
            BlobContainerClient container = new(_storageConnectionString, _storageContainerName);

            // Create the container if it does not exist
            await container.CreateIfNotExistsAsync();

            try
            {
                // Get a reference to the blob just uploaded from the API in a container from configuration settings
                BlobClient client = container.GetBlobClient(Path.Combine(_blobFolder, blob.FileName));

                // Open a stream for the file we want to upload
                await using (Stream data = blob.OpenReadStream())
                {
                    // Upload the file async
                    await client.UploadAsync(data, new BlobHttpHeaders
                    {
                        ContentType = GetFileContentType(blob.FileName)
                    });
                }

                // Everything is OK and file got uploaded
                response.Status = $"File {blob.FileName} Uploaded Successfully";
                response.Error = false;
                response.Blob.Uri = client.Uri.AbsoluteUri;
                response.Blob.Name = client.Name;

            }
            // If the file already exists, we catch the exception and do not upload it
            catch (Exception ex)
            {             
                response.Status = $"File with name {blob.FileName}. Err: {ex.Message}";
                response.Error = true;
                return response;
            }

            // Return the BlobUploadResponse object
            return response;
        }

        public async Task<BlobResponseDTO> UploadAsync(string stringBase)
        {
            return await UploadAsync(Common.ConvertStringBase64ToFile(stringBase));
        }

        public async Task<BlobResponseDTO> UploadWithLink(string linkImage, int index, string sku)
        {
            BlobResponseDTO response = new();
            if (string.IsNullOrWhiteSpace(linkImage))
            {
                return response;
            }

            try
            {
                Uri uri = new Uri(linkImage);
                string fileName = $"{index}{Path.GetExtension(uri.PathAndQuery).Split("?")[0]}";
                string url = Path.Combine($"Q-{sku}", fileName);
                var containerClient = new BlobContainerClient(_storageConnectionString, _storageContainerName);
                await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);

                var blob = containerClient.GetBlobClient(url);

                if (!(await blob.ExistsAsync()))
                {
                    byte[] bytesFile;
                    using var client = new HttpClient();
                    var stream = await client.GetStreamAsync(linkImage);
                    var memoryStream = new MemoryStream();
                    await stream.CopyToAsync(memoryStream);

                    try
                    {
                        await blob.UploadAsync(memoryStream, new BlobHttpHeaders
                        {
                            ContentType = GetFileContentType(fileName)
                        });
                    }
                    // If the file already exists, we catch the exception and do not upload it
                    catch (Exception ex)
                    {
                        response.Status = ex.Message;
                        response.Error = true;
                        response.Blob.Uri = blob.Uri.AbsoluteUri;
                        response.Blob.Name = blob.Name;
                        return response;
                    }
                }

                response.Status = $"File {fileName} Uploaded Successfully";
                response.Error = false;
                response.Blob.Uri = blob.Uri.AbsoluteUri;
                response.Blob.Name = blob.Name;
            }
            catch (Exception ex)
            {
                response.Status = ex.Message;
                response.Error = true;
            }

            return response;
        }

        private string GetFileContentType(string fileName)
        {
            string extension = Path.GetExtension(fileName).ToLower();
            MimeTypeMap mimeMap = new();
            return mimeMap.GetMimeType(extension, false);
        }
    }
}
