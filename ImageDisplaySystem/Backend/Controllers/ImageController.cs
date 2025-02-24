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

        [HttpGet("getImageInfo")]
        public async Task<IActionResult> GetImageInfo(int page, int pageSize)
        {
            var imageCardInfos = new List<ImageCardInfo>();
            if (page < 1 || pageSize < 1)
            {
                return Ok(imageCardInfos);
            }
            int startPos = (page - 1) * pageSize;
            int imageNumber = await dbManagement.GetTotalNumberOfImagesAsync();
            if (imageNumber == 0 || startPos >= imageNumber)
            {
                return Ok(imageCardInfos);
            }
            imageCardInfos = await dbManagement.GetPagedImagesAsync(startPos, pageSize);
            return Ok(imageCardInfos);
        }

        [HttpGet("getImage")]
        public async Task<IActionResult> GetImage(string imageName)
        {
            var imagePath = Environment.CurrentDirectory + "/UploadedImages" + $"/{imageName}";
            if (!Path.Exists(imagePath))
            {
                return NotFound();
            }
            var imageBytes = await System.IO.File.ReadAllBytesAsync(imagePath);
            return File(imageBytes, "image/jpeg");
        }

        [HttpGet("getReviews")]
        public async Task<IActionResult> GetReviews(int imageID)
        {
            var reviewInfos = new List<ReviewInfo>();
            reviewInfos = await dbManagement.GetReviewInfoAsync(imageID);
            return Ok(reviewInfos);
        }

        [HttpPost("uploadReview")]
        public async Task<IActionResult> UploadReview([FromBody] ReviewInfo reviewInfo)
        {
            var result = await dbManagement.UploadReviewAsync(reviewInfo.ImageID, reviewInfo.Review, reviewInfo.Rating);
            return Ok(result);
        }

        [HttpDelete("deleteImage/{imageID}/{imageName}")]
        public async Task<IActionResult> DeleteImage(int imageID,string imageName)
        {
            var result = await dbManagement.DeleteImageAsync(imageID);
            var imagePath = Environment.CurrentDirectory + "/UploadedImages" + $"/{imageName}";
            if (Path.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }
            return Ok(result);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateImageInfo([FromBody] ImageCardInfo imageCardInfo)
        {
            var result = await dbManagement.UpdataImageInfoAsync(imageCardInfo.ImageId, imageCardInfo.Tag, imageCardInfo.Description);
            return Ok(result);
        }

        [HttpGet("search")]
        public async Task<IActionResult> GetImageBySearch(string tag)
        {
            var result = await dbManagement.GetImagesBySearchingAsync(tag);
            return Ok(result);
        }
    }
}
