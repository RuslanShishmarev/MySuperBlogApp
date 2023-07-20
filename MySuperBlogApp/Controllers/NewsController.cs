using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySuperBlogApp.Models;
using MySuperBlogApp.Services;

namespace MySuperBlogApp.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class NewsController : ControllerBase
    {
        private NewsService _newsService;
        private UsersService _userService;
        public NewsController(
            NewsService newsService,
            UsersService userService)
        {
            _newsService = newsService;
            _userService = userService;
        }

        [HttpGet("{userId}")]
        public IActionResult GetByAuthor(int userId)
        {
            var news = _newsService.GetByAuthor(userId);
            return Ok(news);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var currentUser = _userService.GetUserByLogin(HttpContext.User.Identity.Name);
            if (currentUser == null)
            {
                return NotFound();
            }
            var news = _newsService.GetNewsForCurrentUser(currentUser.Id);

            return Ok(news);
        }

        [HttpPost]
        public IActionResult Create([FromBody] NewsModel newsModel)
        {
            var currentUser = _userService.GetUserByLogin(HttpContext.User.Identity.Name);
            if (currentUser == null)
            {
                return NotFound();
            }

            var newsModelNew = _newsService.Create(newsModel, currentUser.Id);

            return Ok(newsModelNew);
        }

        [HttpPatch]
        public IActionResult Update([FromBody] NewsModel newsModel)
        {
            var currentUser = _userService.GetUserByLogin(HttpContext.User.Identity.Name);
            if (currentUser == null)
            {
                return NotFound();
            }

            var newsModelNew = _newsService.Update(newsModel, currentUser.Id);

            return Ok(newsModelNew);
        }

        [HttpDelete("{newsId}")]
        public IActionResult Delete(int newsId)
        {
            var currentUser = _userService.GetUserByLogin(HttpContext.User.Identity.Name);
            if (currentUser == null)
            {
                return NotFound();
            }

            _newsService.Delete(newsId, currentUser.Id);

            return Ok();
        }

        [HttpPost("like/{newsId}")]
        public IActionResult SetLike(int newsId)
        {
            var currentUser = _userService.GetUserByLogin(HttpContext.User.Identity.Name);
            if (currentUser == null)
            {
                return NotFound();
            }

            _newsService.SetLike(newsId, currentUser.Id);

            return Ok();
        }
    }
}
