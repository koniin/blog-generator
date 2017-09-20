using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace blog_generator {
    public class Blog {
        public List<BlogPost> Posts = new List<BlogPost>();

        const int postsPerPage = 2;

        public BlogPost AddPost(string year, string month, string fileName) {
            var blogPost = new BlogPost(year, month, fileName);
            Posts.Add(blogPost);
            return blogPost;
        }

        public void Sort() {
            Posts = Posts.OrderByDescending(p => p.Year).ThenByDescending(p => p.Month).ToList();
        }

        public IEnumerable<BlogPost> GetPostsForPage(int index) {
            var start = (index - 1) * postsPerPage;
            return Posts.GetRange(start, Math.Min(start + postsPerPage, Posts.Count - start));
        }

        public int PageCount() {
            return (Posts.Count / postsPerPage) + Posts.Count % postsPerPage;
        }
    }

    public class BlogPost {
        public string Year { get; }
        public string Month { get; }
        public string FileName { get; }
        public string Html { get; private set; }
        public BlogPost(string year, string month, string fileName) {
            Year = year;
            Month = month;
            FileName = fileName;
        }

        internal void SetHtml(string html) {
            Html = html;
        }
    }

    public class BlogHtmlGenerator {
        readonly string _template;
        readonly string _indexFile;

        public BlogHtmlGenerator() {
            _template = Path.Combine("template", "template.html");
            _indexFile = Path.Combine("html", "index.html");
        }

        public void Generate() {
            if(Directory.Exists("html"))
                Directory.Delete("html", true);
            
            var template = File.ReadAllText(_template);
            var blog = GetBlog();
            blog.Sort();

            GenerateIndex(blog, template);
            GeneratePosts(blog, template);
            GeneratePages(blog, template);
        }

        private Blog GetBlog() {
            Blog b = new Blog();
            var years = Directory.GetDirectories("src");
            foreach(string year in years) {
                var y = new DirectoryInfo(year);
                var months = Directory.GetDirectories(year);
                foreach(var month in months) {
                    var m = new DirectoryInfo(month);
                    var posts = Directory.GetFiles(month);
                    foreach(var post in posts) {
                        var blogPost = b.AddPost(y.Name, m.Name, post);
                        var postText = File.ReadAllText(blogPost.FileName);
                        blogPost.SetHtml(CommonMark.CommonMarkConverter.Convert(postText));
                    }
                }
            }
            return b;
        }

        private void GenerateIndex(Blog blog, string template) {
            var index = 1;
            template = GeneratePostsHtml(blog, template, index);
            template = GenerateTreeMenu(blog, template);
            template = GeneratePaging(blog, template, index);

            if(!Directory.Exists("html"))
                Directory.CreateDirectory("html");
			File.WriteAllText(_indexFile, template);
        }

        private string GeneratePostsHtml(Blog blog, string template, int page) {
            var posts = "<ul id=\"posts\">";
            foreach(BlogPost post in blog.GetPostsForPage(page)) {
                posts += $"<li>{post.Html}</li>";
            }
            posts += "</ul>";
            var result = template.Replace("{{posts}}", posts);
            return result;
        }

        private string GeneratePaging(Blog blog, string template, int page) {
            string pagingHtml = string.Empty;
            if(page == 1)
                pagingHtml = "<a href='" + GetPageUrl(page + 1) + "'>Next</a>";
            else if(page == blog.PageCount())
                pagingHtml = "<a href='" + GetPageUrl(page - 1) + "'>Previous</a>";
            else {
                pagingHtml = "<a href='" + GetPageUrl(page - 1) + "'>Previous</a> - <a href='" + GetPageUrl(page + 1) + "'>Next</a>";
            }
            return template.Replace("{{paging}}", pagingHtml);
        }

        private void GeneratePages(Blog blog, string inputTemplate) {
            string pagePath = Path.Combine("html", "pages");
            if(!Directory.Exists(pagePath))
                Directory.CreateDirectory(pagePath);

            for(int i = 1; i <= blog.PageCount(); i++) {
                var template = inputTemplate;
                template = GeneratePostsHtml(blog, template, i);
                template = GenerateTreeMenu(blog, template);
                template = GeneratePaging(blog, template, i);

			    File.WriteAllText(GetPageFilePath(i), template);
            }
        }

        private string GetPageUrl(int page) {
            var path = GetPageFilePath(page);
            path = path.Remove(0, 4);
            return path.Replace("\\", "/");
        }

        private string GetPageFilePath(int page) {
            string pagePath = Path.Combine("html", "pages");
            return Path.Combine(pagePath, $"{page}.html");
        }

        private string GetPostFilePath(BlogPost post) {
            var path = Path.Combine("html", post.Year, post.Month);
            var filePath = Path.Combine(path, GetPostFileName(post));
            return filePath;
        }

        private void GeneratePosts(Blog blog, string template) {            
            foreach(BlogPost post in blog.Posts) {
                var path = GetPostFilePath(post);
                var dir = Path.GetDirectoryName(path);
                if(!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                var html = template.Replace("{{posts}}", post.Html);
                html = GenerateTreeMenu(blog, html);
                File.WriteAllText(path, html);
            }
        }

        private string GenerateTreeMenu(Blog blog, string template) {
            return template.Replace("{{tree}}", string.Empty);


            // TODO: Don't know if we need this really

            /* 
            string treeHtml = "<ul>";
            string lastYear = string.Empty, lastMonth = string.Empty;
            foreach(BlogPost b in blog.Posts) {
                if(b.Year != lastYear) {

                }
            }
            treeHtml += "</ul>";
            return template.Replace("{{tree}}", treeHtml);
            */
            /* 
            "<ul>"
                <li>
                    <h3>2017</h3>
                    <ul>
                        <li>
                            <h4>February</h4>
                            <ul>
                                <li>Post 1</li>
                                <li>Post 2</li>
                                <li>Post 3</li>
                            </ul>
                        </li>
                    </ul>
                </li>
            "</ul>"    
            */
        }

        private string GetPostFileName(BlogPost post) {
            var fileName = Path.GetFileNameWithoutExtension(post.FileName) + ".html";
            return fileName;
        }
    }
}