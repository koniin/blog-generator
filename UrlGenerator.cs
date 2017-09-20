using System.IO;

namespace blog_generator {
    public class UrlGenerator {
        public string GetPageUrl(int page) {
            return $"/pages/{page}.html";
        }

        public string GetPostUrl(BlogPost post) {
            return $"/{post.Year}/{post.Month}/{Path.GetFileNameWithoutExtension(post.FileName)}.html";
        }
    }
}