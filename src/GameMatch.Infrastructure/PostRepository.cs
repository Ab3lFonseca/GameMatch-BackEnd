using GameMatch.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameMatch.Infrastructure
{
    public class PostRepository
    {
        private readonly AppDb _db;

        public PostRepository(AppDb db)
        {
            _db = db;
        }

        public async Task<Post> CreateAsync(Post post)
        {
            _db.Posts.Add(post);
            await _db.SaveChangesAsync();
            return post;
        }

        public async Task<IEnumerable<Post>> GetFeedAsync()
        {
            return await _db.Posts
                .Include(p => p.User)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Post>> GetByUserAsync(int userId)
        {
            return await _db.Posts
                .Include(p => p.User)
                .Where(p => p.UserId == userId)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }
    }
}
