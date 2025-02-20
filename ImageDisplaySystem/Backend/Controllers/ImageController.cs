using Backend.interfaces;
using BasicArgs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IDbManagement dbManagement;
        public ImageController(IDbManagement dbManagement)
        {
            this.dbManagement = dbManagement;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload([FromForm] IFormFile file, [FromForm] string description, [FromForm] string tag)
        {
            if (file == null)
            {
                return Ok(false);
            }
            var imageStoragePath = Environment.CurrentDirectory + "/UploadedImages";
            if (!Path.Exists(imageStoragePath))
            {
                Directory.CreateDirectory(imageStoragePath);
            }
            var fileName = $"{Guid.NewGuid()}.jpg";
            var filePath = Path.Combine(imageStoragePath, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            var result = await dbManagement.UploadImageAsync(fileName, description, tag);
            if (result == false && System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
            return Ok(result);
        }
    }
}
