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

                var subsForUser = subs.FindOne(x => x.Id == userId);

                return subsForUser;
            }
        }

        public UserSubs SetUserSubs(int from, int to)
        {
            using (var db = new LiteDatabase(DBPath))
            {
                var subs = db.GetCollection<UserSubs>(SubsCollection);

                var subsForUser = subs.FindOne(x => x.Id == from);

                var sub = new UserSub
                {
                    Id = to,
                    Date = DateTime.Now,
                };


                if (subsForUser != null)
                {
                    if (!subsForUser.Users.Select(x => x.Id).Contains(to))
                    {                     
                        subsForUser.Users.Add(sub);
                        subs.Update(subsForUser);
                    }
                }
                else
                {
                    var newSubsForUser = new UserSubs
                    {
                        Id = from,
                        Users = new List<UserSub> { sub }
                    };

                    subs.Insert(newSubsForUser);
                    subs.EnsureIndex(x => x.Id);

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
