using Microsoft.AspNetCore.Mvc;
using MySuperBlogApp.Services;

namespace MySuperBlogApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private UsersService _userService;

        public UsersController(UsersService userService)
        {
            _userService = userService;
        }

        [HttpGet("{name}")]
        public IActionResult GetUsersByName(string name)
        {
            return Ok(_userService.GetUsersByName(name));
        }

        [HttpPost("subs/{userId}")]
        public IActionResult GetUsersByName(int userId)
        {
            var currentUser = _userService.GetUserByLogin(HttpContext.User.Identity.Name);
            if (currentUser == null)
            {
                return NotFound();
            }

            if (currentUser.Id != userId) _userService.Subscribe(from: currentUser.Id, to: userId);
            else return BadRequest();

            return Ok();
        }
    }
}
