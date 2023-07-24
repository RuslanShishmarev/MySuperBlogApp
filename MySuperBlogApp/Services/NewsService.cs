using MySuperBlogApp.Data;
using MySuperBlogApp.Models;

namespace MySuperBlogApp.Services
{
    public class NewsService
    {
        private MyAppDataContext _dataContext;
        private NoSQLDataService _noSQLDataService;
        public NewsService(
            MyAppDataContext dataContext,
            NoSQLDataService noSQLDataService)
        {
            _dataContext = dataContext;
            _noSQLDataService = noSQLDataService;
        }

        public List<NewsView> GetByAuthor(int userId)
        {
            var news = _dataContext.News.Where(x => x.AuthorId == userId)
                .OrderBy(x => x.PostDate)
                .Reverse()
                .Select(ToView)
                .ToList();
            return news;
        }

        public NewsModel Create(NewsModel newsModel, int userId)
        {
            var newNews = new News
            {
                AuthorId = userId,
                Text = newsModel.Text,
                Image = ImageService.GetPhoto(newsModel.Image),
                PostDate = DateTime.Now,
            };

            _dataContext.News.Add(newNews);
            _dataContext.SaveChanges();

            newsModel.Id = newNews.Id;
            newsModel.PostDate = newNews.PostDate;

            return newsModel;
        }

        public List<NewsModel> Create(List<NewsModel> newsModels, int userId)
        {
            foreach (var newsModel in newsModels)
            {
                var newNews = new News
                {
                    AuthorId = userId,
                    Text = newsModel.Text,
                    Image = ImageService.GetPhoto(newsModel.Image),
                    PostDate = DateTime.Now,
                };

                _dataContext.News.Add(newNews);
            }
            _dataContext.SaveChanges();

            return newsModels;
        }

        public NewsView Update(NewsModel newsModel, int userId)
        {
            var newsToUpdate = _dataContext.News
                .FirstOrDefault(x => x.Id == newsModel.Id && x.AuthorId == userId);

            if (newsToUpdate == null)
            {
                return null;
            }

            newsToUpdate.Text = newsModel.Text;
            var photo = ImageService.GetPhoto(newsModel.Image);
            if (!(newsToUpdate.Image?.Length > 10 && photo.Length < 10)) 
                newsToUpdate.Image = photo;

            _dataContext.News.Update(newsToUpdate);
            _dataContext.SaveChanges();

            var newsView = ToView(newsToUpdate);

            return newsView;
        }

        public void Delete(int newsId, int userId)
        {
            var newsToDelete = _dataContext.News
                .FirstOrDefault(x => x.Id == newsId && x.AuthorId == userId);

            if (newsToDelete == null)
            {
                return;
            }

            _dataContext.News.Remove(newsToDelete);
            _dataContext.SaveChanges();
        }

        public List<NewsView> GetNewsForCurrentUser(int userId)
        {
            var subs = _noSQLDataService.GetUserSubs(userId);

            var allNews = new List<NewsView>();

            if (subs is null) return allNews;

            foreach (var sub in subs.Users)
            {
                var allNewsByAuthor = _dataContext.News.Where(x => x.AuthorId == sub.Id);
                allNews.AddRange(allNewsByAuthor.Select(ToView));
            }


            allNews.Sort(new NewsComparer());

            return allNews;
        }

        public void SetLike(int newsId, int userId)
        {
            _noSQLDataService.SetNewsLike(
                from: userId,
                newsId: newsId);
        }

        private NewsView ToView(News news)
        {
            var likes = _noSQLDataService.GetNewsLike(news.Id);
            var newsModel = new NewsView
            {
                Id = news.Id,
                Text = news.Text,
                Image = news.Image,
                PostDate = news.PostDate,
                LikesCount = likes?.Users.Count ?? 0
            };

            return newsModel;
        }
    }

    class NewsComparer : IComparer<NewsView>
    {
        public int Compare(NewsView? x, NewsView? y)
        {
            if (x.PostDate > y.PostDate) return -1;
            if (x.PostDate < y.PostDate) return 1;

            return 0;
        }
    }
}
