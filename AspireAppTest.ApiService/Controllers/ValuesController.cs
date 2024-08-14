using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Mvc;

namespace AspireAppTest.ApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly BlobServiceClient _blobServiceClient;
        private IConfiguration _configuration;

        public ValuesController(IConfiguration configuration, BlobServiceClient blobServiceClient)
        {
            _configuration = configuration;
            _blobServiceClient = blobServiceClient;
        }

        [HttpGet("time")]
        public async Task<IActionResult> GetTime()
        {
            return base.Ok(DateTime.Now);
        }

        [HttpGet("blob")]
        public async Task<IActionResult> GetBlob()
        {
            var blobContainerClient = _blobServiceClient.GetBlobContainerClient("data");
            var blobClient = blobContainerClient.GetBlobClient("test.txt");
            var response = await blobClient.DownloadAsync();
            using (var streamReader = new StreamReader(response.Value.Content))
            {
                while (!streamReader.EndOfStream)
                {
                    var line = await streamReader.ReadLineAsync();
                    return base.Ok(line);
                }
            }
            return base.NotFound();
        }

        [HttpGet("secret")]
        public async Task<IActionResult> GetSecret(string key)
        {
            string secretValue = _configuration[key];
            return base.Ok(secretValue);
        }
    }
}
