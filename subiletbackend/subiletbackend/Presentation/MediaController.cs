using Microsoft.AspNetCore.Mvc;
using SubiletBackend.Application;

namespace SubiletBackend.Presentation
{
    public class FileUpload
    {
        public IFormFile File { get; set; }
    }

    [ApiController]
    [Route("api/admin/media")]
    public class MediaController : ControllerBase
    {
        private readonly IMediaService _mediaService;
        public MediaController(IMediaService mediaService)
        {
            _mediaService = mediaService;
        }

        // POST /api/admin/media/upload
        [HttpPost("upload")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Upload([FromForm] FileUpload fileUpload, [FromQuery] string container = "event-images")
        {
            var url = await _mediaService.UploadImageAsync(fileUpload.File, container);
            return Ok(new { url });
        }

        // DELETE /api/admin/media/delete
        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromQuery] string blobName, [FromQuery] string container = "event-images")
        {
            await _mediaService.DeleteImageAsync(blobName, container);
            return NoContent();
        }
    }
} 