using Microsoft.AspNetCore.Mvc;
using Tweetbook.Contracts.V1;
using Tweetbook.Contracts.V1.Requests;
using Tweetbook.Contracts.V1.Responses;
using Tweetbook.Domain;
using Tweetbook.Services;

namespace Tweetbook.Controllers.V1
{
    public class IdentityController : Controller
    {
        private readonly IIdentityService _identityService;

        public IdentityController(IIdentityService identityService)
        {
            _identityService = identityService;
        }


        [HttpPost(ApiRoutes.Identity.Register)]
        public async Task<IActionResult> Register([FromBody] UserRegistrationRequest request)
        {

            var newUser = new User
            {
                Id = Guid.NewGuid(),
                Email = request.Email,
                Password = request.Password
            };

            await _identityService.RegisterAsync(newUser);

            return Ok();
        }

        [HttpPost(ApiRoutes.Identity.Login)]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest request)
        {
           var isAuth = await _identityService.LoginAsync(request.Email, request.Password);

           if (!isAuth)
           {
               var authFail = new AuthFailedResponse
               {

                   Errors = new[] { "User not found" }
               };

               return NotFound(authFail);
           }


           var res = new AuthSuccessResponse
           {
               Token = "12345"
           };
           
            return Ok(res);
        }
    }
}
