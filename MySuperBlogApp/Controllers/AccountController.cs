using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

using MySuperBlogApp.Data;
using MySuperBlogApp.Models;
using MySuperBlogApp.Services;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MySuperBlogApp.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private UsersService _userService;
        public AccountController(UsersService userService) 
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var currentUserEmail = HttpContext.User.Identity.Name;
            var currentUser = _userService.GetUserByLogin(currentUserEmail);

            if (currentUser is null)
            {
                return NotFound();
            }

            var profile = _userService.ToProfile(currentUser);
            return Ok(profile);
        }

        [HttpPost]
        [AllowAnonymous]
        public object Create([FromBody] UserModel user)
        {
            // add user to DB
            var newUser = _userService.Create(user);

            return Ok(newUser);
        }

        [HttpPatch]
        public object Update(UserModel user)
        {
            // check current user from request with user model
            var currentUserEmail = HttpContext.User.Identity.Name;
            var currentUser = _userService.GetUserByLogin(currentUserEmail);

            if (currentUser != null && currentUser.Id != user.Id)
            {
                return BadRequest();
            }

            // update user in DB
            _userService.Update(currentUser, user);

            return Ok(user);
        }

        [HttpDelete]
        public IActionResult Delete()
        {
            var currentUserEmail = HttpContext.User.Identity.Name;
            var currentUser = _userService.GetUserByLogin(currentUserEmail);
            _userService.DeleteUser(currentUser);

            return Ok();
        }

        [HttpPost("token")]
        [AllowAnonymous]
        public object GetToken()
        {
            // get user data from DB
            var userData = _userService.GetUserLoginPassFromBasicAuth(Request);
            // get identity
            (ClaimsIdentity claims, int id)? identity = _userService.GetIdentity(userData.login, userData.password);

            if (identity is null) return NotFound("login or password is not correct");

            // create jwt token 
            var now = DateTime.UtcNow;
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: identity?.claims.Claims,
                    expires: now.AddMinutes(AuthOptions.LIFETIME),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            // return token


            var tokenModel = new AuthToken(
                minutes: AuthOptions.LIFETIME,
                accessToken: encodedJwt,
                userName: userData.login,
                userId: identity.Value.id);

            return Ok(tokenModel);
        }
    }
}
