using System;
using System.IO;

public class BlogPostCreator {
    const string rootDir = "src";
    const string fileExtension = ".md";
    const string templateFile = "templates/post.md";
    public void Create(string name) {
        var now = DateTime.Now;
        DirectoryInfo dir = CreateDirectory(now);

        Console.WriteLine("Read default post markdown template");

        var filePath = Path.Combine(dir.FullName, name + now.ToString("_yyyyMMddHHmm") + fileExtension);
        var file = File.Create(filePath);
        using (StreamWriter sw = new StreamWriter(file)) {
            Console.WriteLine("Add date to template and comments?");
            sw.WriteLine(now.ToString("yyyy-MM-dd HH:mm"));
            sw.WriteLine("haHAA");
        }
    }

    private DirectoryInfo CreateDirectory(DateTime now) {
        var year = now.ToString("yyyy");
        var month = now.ToString("MM");
        string path = Path.Combine(rootDir, year, month);
        var dir = Directory.CreateDirectory(path);
        return dir;
    }
}