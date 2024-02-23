using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

namespace AzureBlobUpload.Controllers
{
    [Route("/api/upload")]
    [ApiController]
    public class BlobUploadController : ControllerBase
    {
       private const string connectionString = "DefaultEndpointsProtocol=https;AccountName=maufvstorage;AccountKey=sp=racwdli&st=2024-02-23T00:08:28Z&se=2024-02-23T08:08:28Z&spr=https&sv=2022-11-02&sr=c&sig=1Qc6N1od4JXHOFFnq19lkyrsWzXqTeUVm6Fpblm%2FLow%3D==;EndpointSuffix=core.windows.net";
        private const string containerName = "files";

        [HttpPost]
        public async Task<IActionResult> UploadFile([FromForm] IFormFile file)
        {
            try
            {
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = blobClient.GetContainerReference(containerName);

                await container.CreateIfNotExistsAsync();
                await container.SetPermissionsAsync(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });

                CloudBlockBlob blob = container.GetBlockBlobReference(file.FileName);

                using (Stream fileStream = file.OpenReadStream())
                {
                    await blob.UploadFromStreamAsync(fileStream);
                }

                return Ok("File uploaded successfully");
            }
            catch (StorageException ex)
            {
                return StatusCode(500, $"Error uploading file: {ex.Message}");
            }
        }
    }
}
