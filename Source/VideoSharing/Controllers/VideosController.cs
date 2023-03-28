using BusinesLogic.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VideoSharing.ApiModels;

namespace VideoSharing.Controllers
{

    [Authorize(Roles = "Admin")]
    [Route("[controller]")]
    [ApiController]
    public class VideosController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IVideoService _videoService;

        public VideosController(IVideoService videoService, ILogger<VideoModel> logger)
        {
            _logger = logger;
            _videoService = videoService;
        }

        // GET: api/<VideoController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var videos = await _videoService.GetVideos();
            var videoModels = videos.Select(video => new VideoModel(video)).ToList();
            _logger.LogInformation("Videos Retrieved succesfully");
            return Ok(videoModels);
        }

        // GET api/<VideoController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<VideoModel>> Get(int id)
        {
            var video = await _videoService.GetVideo(id);
            if (video == null)
            {
                _logger.LogInformation("Videos unsuccesfully retrieved");
                return NotFound();
            }
            var videoModel = new VideoModel(video);
            _logger.LogInformation("Videos succesfully retrieved");
            return Ok(videoModel);
        }

        // POST api/<VideoController>
        [HttpPost]
        public async Task<IActionResult> Post([FromForm] ApiModels.VideoUploadModel video)
        {
            var newVideo = await _videoService.AddVideo(video.Title, video.Description, video.File);
            _logger.LogInformation("Video Uploaded succesfully");
            return Ok(newVideo);
        }


        [Obsolete("Unimplemented")]
        // PUT api/<VideoController>/5
        [HttpPut]
        public void Put([FromBody] string value)
        {
        }

        [Obsolete("Unimplemented")]
        // DELETE api/<VideoController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}