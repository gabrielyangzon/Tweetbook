using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tweetbook.Contracts.V1;
using Tweetbook.Contracts.V1.Requests;
using Tweetbook.Contracts.V1.Responses;
using Tweetbook.Domain;
using Tweetbook.Migrations;
using Tweetbook.Services;

namespace Tweetbook.Controllers.V1
{
    [ApiController]
    [Authorize]
    public class PostsController : Controller
    {

        private IPostService _postService;

        public PostsController(IPostService postService)
        {
            _postService = postService;
        }

        [HttpGet(ApiRoutes.Posts.GetAll)]
        public async Task<IActionResult> GetAll()
        {
            var allPost = await _postService.GetPostsAsync();
            return Ok(allPost);
        }


        [HttpGet(ApiRoutes.Posts.Get)]
        public async Task<IActionResult> Get(Guid postId)
        {
            var post = await _postService.GetPostByIdAsync(postId);

            if (post == null)
            {
                return NotFound();
            }

            return Ok(post);
        }


        [HttpPost(ApiRoutes.Posts.Create)]
        public IActionResult CreatePost([FromBody] CreatePostRequest postRequest)
        {
            var post = new Post { Id = postRequest.Id, Name = postRequest.Name };

            if (post.Id == Guid.Empty)
            {
                post.Id = Guid.NewGuid();
            }

            bool isAdded = _postService.CreatePostAsync(post).Result;

            if (!isAdded)
            {
                return BadRequest();
            }

            var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
            var locationUri = baseUrl + "/" + ApiRoutes.Posts.Get.Replace("{postId}", post.Id.ToString());


            var response = new PostResponse { Id = post.Id };
            return Created(locationUri, response);
        }


        [HttpPut(ApiRoutes.Posts.Update)]
        public IActionResult Update([FromRoute] Guid postId, [FromBody] UpdatePutRequest updatePutRequest)
        {
            var post = new Post
            {
                Id = postId,
                Name = updatePutRequest.Name
            };

            bool isUpdated = _postService.UpdatePostAsync(post).Result;




            if (!isUpdated)
            {
                return NotFound();
            }

            return Ok();
        }

        [HttpDelete(ApiRoutes.Posts.Delete)]
        public IActionResult Delete([FromRoute] Guid postId)
        {

            var isDeleted = _postService.DeletePostAsync(postId).Result;
            if (!isDeleted)
            {
                return NotFound();
            }


            return Ok();
        }
    }
}
