using LiteDB;

namespace MySuperBlogApp.Data
{
    public class NoSQLDataService
    {
        private readonly string DBPath = "MySuperBlogApp_NoSQLDB.db";

        private const string SubsCollection = "SubsCollection";
        private const string NewsLikesCollection = "NewsLikesCollection";

        public UserSubs GetUserSubs(int userId)
        {
            using (var db = new LiteDatabase(DBPath))
            {
                var subs = db.GetCollection<UserSubs>(SubsCollection);

                var subsForUser = subs.FindOne(x => x.UserId == userId);

                return subsForUser;
            }
        }

        public UserSubs SetUserSubs(int from, int to)
        {
            using (var db = new LiteDatabase(DBPath))
            {
                var subs = db.GetCollection<UserSubs>(SubsCollection);

                var subsForUser = subs.FindOne(x => x.UserId == from);

                if (subsForUser != null)
                {
                    if (!subsForUser.Users.Contains(to))
                    {
                        subsForUser.Users.Add(to);
                        subs.Update(subsForUser);
                    }
                }
                else
                {
                    var newSubsForUser = new UserSubs
                    {
                        UserId = from,
                        Users = new List<int> { to }
                    };

                    subs.Insert(newSubsForUser);
                    subs.EnsureIndex(x => x.UserId);

                    subsForUser = newSubsForUser;
                }

                return subsForUser;
            }
        }

        public NewsLike GetNewsLike(int newsId)
        {
            using (var db = new LiteDatabase(DBPath))
            {
                var likes = db.GetCollection<NewsLike>(NewsLikesCollection);
                var newsLikes = likes.FindOne(x => x.NewsId == newsId);

                return newsLikes;
            }
        }

        public NewsLike SetNewsLike(int from, int newsId)
        {
            using (var db = new LiteDatabase(DBPath))
            {
                var likes = db.GetCollection<NewsLike>(NewsLikesCollection);

                var newsLikes = likes.FindOne(x => x.NewsId == newsId);

                if (newsLikes != null)
                {
                    if (!newsLikes.Users.Contains(from))
                    {
                        newsLikes.Users.Add(from);
                        likes.Update(newsLikes);
                    }
                }
                else
                {
                    var newSubsForUser = new NewsLike
                    {
                        NewsId = newsId,
                        Users = new List<int> { from }
                    };

                    likes.Insert(newSubsForUser);
                    likes.EnsureIndex(x => x.NewsId);

                    newsLikes = newSubsForUser;
                }

                return newsLikes;
            }
        }
    }
}
