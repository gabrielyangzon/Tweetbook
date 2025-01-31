﻿
using Tweetbook.Domain;

namespace Tweetbook.Services
{
    public interface IPostService
    {
        Task<List<Post?>> GetPostsAsync();

        Task<Post?> GetPostByIdAsync(Guid guid);

        Task<bool> CreatePostAsync(Post? post);

        Task<bool> UpdatePostAsync(Post? postToUpdate);

        Task<bool> DeletePostAsync(Guid postId);
    }
}
