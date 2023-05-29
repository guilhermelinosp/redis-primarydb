using Microsoft.AspNetCore.Mvc;
using RedisAPI.Data;
using RedisAPI.Models;

namespace RedisAPI.Controllers
{

    [Route("api/v1/[controller]")]
    [ApiController]
    public class PlatformController : ControllerBase
    {
        private readonly IPlatformRepo _platform;

        public PlatformController(IPlatformRepo platform)
        {
            _platform = platform;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Platform?>> GetAllByPlatforms()
        {
            return Ok(_platform.GetAllByPlatforms());
        }


        [HttpGet("{id}", Name = "GetPlatformById")]
        public ActionResult<Platform> GetPlatformById(string id)
        {
            var platform = _platform.GetPlatformById(id);

            return platform != null ? Ok(platform) : NotFound();
        }


        [HttpPost]
        public ActionResult<Platform> CreatePlatform(Platform? platform)
        {
            if (platform == null)
            {
                return BadRequest();
            }

            _platform.CreatePlatform(platform);

            return CreatedAtAction(nameof(GetPlatformById),
                new { id = platform.Id }, platform);
        }
    }
}
