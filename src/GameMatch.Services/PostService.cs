using GameMatch.Infrastructure;
using GameMatch.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace GameMatch.Services
{
    public class PostService
    {
        private readonly PostRepository _repo;


        public PostService(PostRepository repo)
        {
            _repo = repo;
        }


        public async Task<Post> CreatePost(int userId, string caption, string? imageUrl)
        {
            var post = new Post { UserId = userId, Caption = caption, ImageUrl = imageUrl };
            return await _repo.CreateAsync(post);
        }


        public async Task<IEnumerable<Post>> GetFeed() => await _repo.GetFeedAsync();


        public async Task<IEnumerable<Post>> GetUserPosts(int userId) => await _repo.GetByUserAsync(userId);
    }
}