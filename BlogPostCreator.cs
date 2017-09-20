using System;
using System.IO;

namespace blog_generator {
    public class BlogPostCreator {
        const string rootDir = "src";
        const string fileExtension = ".md";
        readonly string _templateFile;
        private DateTime _date;

        public BlogPostCreator() {
            _date = DateTime.Now;
            _templateFile = Path.Combine("template", "post.md");
        }

        public void Create(string name) {
            DirectoryInfo dir = CreateOutputDirectory();
            string template = GetTemplate();
            var content = GeneratePost(template, name);
            var fileName = GenerateFileName(name);
            WritePostFile(dir, fileName, content);
        }

        private string GenerateFileName(string name) {
            return name.Trim().Replace(" ", "-");
        }

        private DirectoryInfo CreateOutputDirectory() {
            var year = _date.ToString("yyyy");
            var month = _date.ToString("MM");
            string path = Path.Combine(rootDir, year, month);
            var dir = Directory.CreateDirectory(path);
            return dir;
        }

        private string GetTemplate() {
            string template = File.ReadAllText(_templateFile);
            return template;
        }

        private string GeneratePost(string template, string name) {
            var post = template.Replace("{{date}}", _date.ToString("yyyy-MM-dd HH:mm"));
            return post.Replace("{{topic}}", name);
        }

        private void WritePostFile(DirectoryInfo directory, string fileName, string content) {
            var filePath = Path.Combine(directory.FullName, fileName + _date.ToString("_yyyyMMddHHmm") + fileExtension);
            var file = File.Create(filePath);
            using (StreamWriter sw = new StreamWriter(file)) {            
                sw.WriteLine(content);
            }
        }
    }
}