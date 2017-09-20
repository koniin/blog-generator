using System.IO;

namespace blog_generator {
    public class BlogFactory {
        private readonly Settings _settings;
        public BlogFactory(Settings settings) {
            _settings = settings;
        }

        public Blog Create() {
            Blog b = new Blog(_settings.PostsPerPage, _settings.RecentPostCount);
            var years = Directory.GetDirectories("src");
            foreach(string year in years) {
                var y = new DirectoryInfo(year);
                var months = Directory.GetDirectories(year);
                foreach(var month in months) {
                    var m = new DirectoryInfo(month);
                    var posts = Directory.GetFiles(month);
                    foreach(var post in posts) {
                        var markdown = File.ReadAllText(post);
                        var blogPost = b.AddPost(y.Name, m.Name, post, markdown);
                    }
                }
            }
            return b;
        }
    }
}