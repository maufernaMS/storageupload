using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

namespace YourNamespace.Controllers
{
    [Route("api/Upload")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private const string blobContainerName = "maufvstorage;
        private const string storageConnectionString = "DefaultEndpointsProtocol=https;AccountName=maufvstorage;AccountKey=wsqFtaHDFPjqEQK7h0z7EEXd9NfHETn6kMOioOSmQMx1Qo9lBCfjcRUG0kYt9d1iG8BSzR7hPH7y+AStM9JwuA==";

        [HttpPost]
        public async Task<IActionResult> Post(IFormFile file)
        {
            BlobServiceClient blobServiceClient = new BlobServiceClient(storageConnectionString);
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(blobContainerName);

            // Create the container if it does not exist.
            await containerClient.CreateIfNotExistsAsync();

            // Get a reference to a blob
            BlobClient blobClient = containerClient.GetBlobClient(file.FileName);

            // Open the file and upload its data
            using (Stream fileStream = file.OpenReadStream())
            {
                await blobClient.UploadAsync(fileStream, true);
            }

            return Ok();
        }
    }