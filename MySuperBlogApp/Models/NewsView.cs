namespace MySuperBlogApp.Models
{
    public class NewsView
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public object Image { get; set; }

        public int? LikesCount { get; set; }

        public DateTime? PostDate { get; set; }
    }
}
