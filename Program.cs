using System;

namespace blog_generator {
	class Program {
        static void Main(string[] args) {
			if(args.Length == 0) {
				PrintHelp();
				return;
			}
			
			if(args[0] == "-n") {
				string postName = string.Empty;
				for(int i = 1; i < args.Length; i++) {
					if(i != 1)
						postName += " ";
					postName += args[i];
				}
				BlogPostCreator blogPostCreator = new BlogPostCreator();
				blogPostCreator.Create(postName);
			} else if(args[0] == "-g") {
				BlogHtmlGenerator blogHtmlGenerator = new BlogHtmlGenerator();
				blogHtmlGenerator.Generate();
			}
        }
		
		private static void PrintHelp() {
			Console.WriteLine("Usage:");
			Console.WriteLine("-n [POSTNAME]         - Create a new post with POSTNAME as name");
			Console.WriteLine("-g                    - Generate output");
			Console.WriteLine("\n");
		}
		
		private static void CreatePost(string name) {
			
		}
    }
}
