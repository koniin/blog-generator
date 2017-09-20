using System.IO;

namespace blog_generator {
    public class BlogPost {
        public string Year { get; }
        public string Month { get; }
        public string FileName { get; }
        public string MarkDown { get; }
        public string Title { 
            get {
                var fileName = Path.GetFileNameWithoutExtension(FileName);
                fileName = fileName.Remove(fileName.LastIndexOf("_"));
                return fileName.Replace("-", " ");
            }
        }

        public BlogPost(string year, string month, string fileName, string markDown) {
            Year = year;
            Month = month;
            FileName = fileName;
            MarkDown = markDown;
        }
    }
}