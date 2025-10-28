using GameMatch.Core.Models;
using GameMatch.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GameMatch.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostsController : ControllerBase
    {
        private readonly PostService _service;

        public PostsController(PostService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePostDto dto)
        {
            var post = await _service.CreatePost(dto.UserId, dto.Caption, dto.ImageUrl);
            return Ok(post);
        }

        [HttpGet("feed")]
        public async Task<IActionResult> GetFeed()
        {
            var feed = await _service.GetFeed();
            return Ok(feed);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserPosts(int userId)
        {
            var posts = await _service.GetUserPosts(userId);
            return Ok(posts);
        }

        public class CreatePostDto
        {
            public int UserId { get; set; }
            public string Caption { get; set; } = string.Empty;
            public string? ImageUrl { get; set; }
        }
    }
}
