using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Azure.Storage.Blobs;
using System.IO;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class FileController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public FileController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> UploadAsync([FromForm] Microsoft.AspNetCore.Http.IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("File is null or empty");

        var connectionString = _configuration.GetConnectionString("AzureStorage:ConnectionString");
        var containerName = _configuration["AzureStorage:ContainerName"];

        var blobServiceClient = new BlobServiceClient(connectionString);
        var blobContainerClient = blobServiceClient.GetBlobContainerClient(containerName);

        var blobName = Path.GetFileName(file.FileName);
        var blobClient = blobContainerClient.GetBlobClient(blobName);

        using (var stream = file.OpenReadStream())
        {
            await blobClient.UploadAsync(stream, true);
        }

        return Ok($"File '{blobName}' uploaded successfully");
    }
}
