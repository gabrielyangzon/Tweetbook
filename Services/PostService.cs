using Microsoft.EntityFrameworkCore;
using Tweetbook.Domain;
using Tweetbook.Migrations;


namespace Tweetbook.Services
{
    public class PostService : IPostService
    {
        //private readonly List<Post> _posts;
        private readonly DataContext _dataContext;

        public PostService(DataContext dataContext)
        {
            _dataContext = dataContext;


            if (!dataContext.Posts.Any()) return;
            
            for (var i = 0; i < 5; i++)
            {
                _dataContext.Posts.Add(new Post
                {
                    Id = Guid.NewGuid(),
                    Name = $"Post name {i}"
                });
            }

            _dataContext.SaveChanges();
        }
        public async Task<Post?> GetPostByIdAsync(Guid postId)
        {
            return await _dataContext.Posts.AsNoTracking().SingleOrDefaultAsync(p => p!.Id == postId);
          
        }

        public async Task<List<Post?>> GetPostsAsync()
        {
            return await _dataContext.Posts.AsNoTracking().ToListAsync();
        }

        public async Task<bool> CreatePostAsync(Post? post)
        {
                await _dataContext.Posts.AddAsync(post);
                var result = await _dataContext.SaveChangesAsync();

                return result > 0;
        }

        public async Task<bool> UpdatePostAsync(Post? postToUpdate)
        {

            var existing = await GetPostByIdAsync(postToUpdate!.Id);

            if (existing == null)
            {
                return false;
            }
            
            _dataContext.Posts.Entry(postToUpdate).State = EntityState.Modified;

            _dataContext.Posts.Update(postToUpdate);
            var result = await _dataContext.SaveChangesAsync();

            return result > 0;
            
        }

        public async Task<bool> DeletePostAsync(Guid postId)
        {
            var existing = GetPostByIdAsync(postId).Result;

            if (existing == null)
            {
                return false;
            }

            _dataContext.Remove(existing);
            var result = await _dataContext.SaveChangesAsync();

            return result > 0;
        }
    }
}
