using System;
using System.IO;

namespace blog_generator {
    public class Settings {
        public int PostsPerPage { get; set; }
        public string OutPutFolder { get; set; }
        public string TemplateFolder { get; set; }
        public string CopyDirectories { get; set; }
        public int RecentPostCount { get; set; }

        public static Settings LoadOrDefault() {
            if(File.Exists("settings.config"))
                return Load("settings.config");
            
            return Default();
        }

        public static Settings Load(string file) {
            var settings = new Settings();
            var lines = File.ReadAllLines(file);
            foreach(var line in lines) {
                ReadSetting(line, settings);
            }
            return settings;
        }

        private static void ReadSetting(string line, Settings settings) {
            var values = line.Split('=');
            switch(values[0]) {
                case "PostsPerPage":
                    settings.PostsPerPage = int.Parse(values[1]);
                    break;
                case "OutPutFolder":
                    settings.OutPutFolder = values[1];
                    break;
                case "TemplateFolder":
                    settings.TemplateFolder = values[1];
                    break;
                case "CopyDirectories":
                    settings.CopyDirectories = values[1];
                    break;
                case "RecentPostCount":
                    settings.RecentPostCount = int.Parse(values[1]);
                    break;
            }
        }

        public static Settings Default() {
            return new Settings {
				PostsPerPage = 2,
				OutPutFolder = "html",
				TemplateFolder = "template",
				RecentPostCount = 4,
				//CopyDirectories = "img"
			};
        }
    }
}