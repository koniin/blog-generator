using System;
using System.IO;

namespace blog_generator {
    public class BlogGenerator {
        private readonly HtmlGenerator _htmlGenerator;
        private readonly BlogFactory _blogFactory;
        private readonly string _templateFile;
        private readonly string _indexFile;
        private readonly string _pageFolder;
        private readonly Settings _settings;

        public BlogGenerator(Settings settings) {
            _htmlGenerator = new HtmlGenerator();
            _blogFactory = new BlogFactory(settings);
            _templateFile = Path.Combine(settings.TemplateFolder, "template.html");
            _indexFile = Path.Combine(settings.OutPutFolder, "index.html");
            _pageFolder = Path.Combine(settings.OutPutFolder, "pages");
            _settings = settings;
        }

        public void Generate() {
            if(Directory.Exists(_settings.OutPutFolder))
                Directory.Delete(_settings.OutPutFolder, true);
            Directory.CreateDirectory(_settings.OutPutFolder);

            var template = File.ReadAllText(_templateFile);
            var blog = _blogFactory.Create();
            blog.Sort();

            GenerateIndex(blog, template);
            GeneratePosts(blog, template);
            GeneratePages(blog, template);
        }

        internal void CopyDirectories() {
            if(string.IsNullOrWhiteSpace(_settings.CopyDirectories))
                return;

            var directories = _settings.CopyDirectories.Split(',');
            foreach(var directory in directories) {
                var sourceDir = Path.Combine(_settings.TemplateFolder, directory);
                var outDir = Path.Combine(_settings.OutPutFolder, directory);
                IOExtensions.CopyDirectory(sourceDir, outDir, true);
            }
        }

        private void GenerateIndex(Blog blog, string template) {
            var index = 1;
            var html = _htmlGenerator.IndexFromTemplate(blog, template, index);
			File.WriteAllText(_indexFile, html);
        }

        private void GeneratePosts(Blog blog, string template) {            
            foreach(BlogPost post in blog.Posts) {
                var path = GetPostFilePath(post);
                var dir = Path.GetDirectoryName(path);
                if(!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                var html = _htmlGenerator.PostPageTemplate(blog, post, template);
                html = _htmlGenerator.GenerateTreeMenu(blog, html);
                File.WriteAllText(path, html);
            }
        }

        private void GeneratePages(Blog blog, string template) {
            if(!Directory.Exists(_pageFolder))
                Directory.CreateDirectory(_pageFolder);

            for(int i = 1; i <= blog.PageCount(); i++) {
                var html = _htmlGenerator.PagesFromTemplate(blog, template, i);
			    File.WriteAllText(Path.Combine(_pageFolder, $"{i}.html"), html);
            }
        }

        private string GetPostFilePath(BlogPost post) {
            var fileName = Path.GetFileNameWithoutExtension(post.FileName) + ".html";
            var filePath = Path.Combine(_settings.OutPutFolder, post.Year, post.Month, fileName);
            return filePath;
        }
    }
}