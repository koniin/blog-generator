using System;
using System.Collections.Generic;
using System.IO;

namespace blog_generator {
    public class Blog {
        public List<BlogPost> Posts = new List<BlogPost>();
        public void AddPost(string year, string month, string fileName) {
            Posts.Add(new BlogPost(year, month, fileName));
        }

        public void Sort() {
            
        }
    }

    public class BlogPost {
        public string Year { get; }
        public string Month { get; }
        public string FileName { get; }
        public BlogPost(string year, string month, string fileName) {
            Year = year;
            Month = month;
            FileName = fileName;
        }
    }

    public class BlogHtmlGenerator {
        readonly string _template;
        readonly string _output;
        public BlogHtmlGenerator() {
            _template = Path.Combine("template", "template.html");
            _output = Path.Combine("html", "index.html");
        }

        public void Generate() {
            var template = File.ReadAllText(_template);
            var b = GetBlog();
            b.Sort();
            var searchString = "<ul id=\"posts\">";
            var postIndex = template.IndexOf(searchString) + searchString.Length;
            
            foreach(BlogPost post in b.Posts) {
                var postText = File.ReadAllText(post.FileName);
                var postHtml = "<li>" + 
                    CommonMark.CommonMarkConverter.Convert(postText)
                    + "</li>";
                template = template.Insert(postIndex, postHtml);
                postIndex += postHtml.Length;
            }

            if(!Directory.Exists("html"))
                Directory.CreateDirectory("html");
            // if(!File.Exists(_output)) 
            //     File.Create(_output);
			File.WriteAllText(_output, template);
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
                        b.AddPost(y.Name, m.Name, post);
                    }
                }
            }
            return b;
        }
    }
}