using System;

namespace blog_generator {
	class Program {
        static void Main(string[] args) {
			if(args.Length == 0) {
				PrintHelp();
				return;
			}
			
			if(args[0] == "-p") {
				string postName = string.Empty;
				for(int i = 1; i < args.Length; i++) {
					if(i != 1)
						postName += " ";
					postName += args[i];
				}
				BlogPostCreator blogPostCreator = new BlogPostCreator();
				blogPostCreator.Create(postName);
			} else if(args[0] == "-g") {
				BlogGenerator blogGenerator = new BlogGenerator(new Settings {
					PostsPerPage = 2,
					OutPutFolder = "html",
					TemplateFolder = "template",
					RecentPostCount = 4
				});
				blogGenerator.Generate();
			}
        }
		
		private static void PrintHelp() {
			Console.WriteLine("Usage:");
			Console.WriteLine("-p [POSTNAME]         - Create a new post with POSTNAME as name");
			Console.WriteLine("-g                    - Generate output");
			Console.WriteLine("\n");
		}
    }
}
