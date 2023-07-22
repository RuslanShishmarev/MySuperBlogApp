using MySuperBlogApp.Data;
using MySuperBlogApp.Models;

using System.Security.Claims;
using System.Text;

namespace MySuperBlogApp.Services
{
    public class UsersService
    {
        private MyAppDataContext _dataContext;
        private NoSQLDataService _noSQLDataService;
        public UsersService(
            MyAppDataContext dataContext, 
            NoSQLDataService noSQLDataService)
        {
            _dataContext = dataContext;
            _noSQLDataService = noSQLDataService;
        }

        public UserModel Create(UserModel userModel)
        {
            var newUser = new User
            {
                Name = userModel.Name,
                Email = userModel.Email,
                Password = userModel.Password,
                Description = userModel.Description,
                Photo = userModel.Photo,
            };

            _dataContext.Users.Add(newUser);
            _dataContext.SaveChanges();

            userModel.Id = newUser.Id;

            return userModel;
        }

        public List<UserModel> Create(List<UserModel> users)
        {
            var usersList = new List<UserModel>();
            foreach (var userModel in users)
            {
                var newUser = new User
                {
                    Name = userModel.Name,
                    Email = userModel.Email,
                    Password = userModel.Password,
                    Description = userModel.Description,
                    Photo = userModel.Photo,
                };

                _dataContext.Users.Add(newUser);
            }
            _dataContext.SaveChanges();

            return users;
        }

        public UserModel Update(UserModel userModel)
        {
            var userToUpdate = _dataContext.Users.FirstOrDefault(x => x.Id == userModel.Id);

            if (userToUpdate == null)
            {
                return null;
            }

            userToUpdate.Name = userModel.Name;
            userToUpdate.Email = userModel.Email;
            userToUpdate.Password = userModel.Password;
            userToUpdate.Description = userModel.Description;
            userToUpdate.Photo = userModel.Photo;

            _dataContext.Users.Update(userToUpdate);
            _dataContext.SaveChanges();

            return userModel;
        }

        public UserModel Update(User userToUpdate, UserModel userModel)
        {
            userToUpdate.Name = userModel.Name;
            userToUpdate.Email = userModel.Email;
            userToUpdate.Password = userModel.Password;
            userToUpdate.Description = userModel.Description;
            userToUpdate.Photo = userModel.Photo;

            _dataContext.Users.Update(userToUpdate);
            _dataContext.SaveChanges();

            return userModel;
        }

        public (string login, string password) GetUserLoginPassFromBasicAuth(HttpRequest request)
        {
            string userName = "";
            string userPass = "";
            string authHeader = request.Headers["Authorization"].ToString();
            if (authHeader != null && authHeader.StartsWith("Basic"))
            {
                string encodedUserNamePass = authHeader.Replace("Basic ", "");
                var encoding = Encoding.GetEncoding("iso-8859-1");

                string[] namePassArray = encoding.GetString(Convert.FromBase64String(encodedUserNamePass)).Split(':');
                userName = namePassArray[0];
                userPass = namePassArray[1];
            }
            return (userName, userPass);
        }

        public (ClaimsIdentity identity, int id)? GetIdentity(string email, string password)
        {
            User? currentUser = GetUserByLogin(email);

            if (currentUser == null || !VerifyHashedPassword(currentUser.Password, password)) return null;

            var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, currentUser.Email),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, "User"),
                };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(
                claims,
                "Token",
                ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);

            return (claimsIdentity, currentUser.Id);
        }

        public User? GetUserByLogin(string email)
        {
            return _dataContext.Users.FirstOrDefault(x => x.Email == email);
        }

        public UserProfile? GetUserProfileById(int userId)
        {
            var user = _dataContext.Users.FirstOrDefault(x => x.Id == userId);

            if (user is null) return null;

            return ToProfile(user);
        }

        public List<UserShortModel> GetUsersByName(string name)
        {
            string nameLower = name.ToLower();
            return _dataContext.Users
                .Where(x => x.Name.ToLower()
                .StartsWith(nameLower))
                .Select(ToShortModel)
                .ToList();
        }

        public void DeleteUser(User user)
        {
            _dataContext.Users.Remove(user);
            _dataContext.SaveChanges();
        }

        public void Subscribe(int from, int to)
        {
            _noSQLDataService.SetUserSubs(from, to);
        }

        private bool VerifyHashedPassword(string password1, string password2)
        {
            return password1 == password2;
        }

        private UserModel ToModel(User user)
        {
            return new UserModel
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Description = user.Description,
                Photo = user.Photo,
            };
        }

        private UserProfile ToProfile(User user)
        {
            var usersSubs = _noSQLDataService.GetUserSubs(user.Id);
            return new UserProfile
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Description = user.Description,
                Photo = user.Photo,
                SubsCount = usersSubs?.Users.Count ?? 0
            };
        }

        private UserShortModel ToShortModel(User user)
        {
            return new UserShortModel
            {
                Id = user.Id,
                Name = user.Name,
                Description = new string(user.Description?.Take(50).ToArray()),
                Photo = user.Photo,
            };
        }
    }
}
