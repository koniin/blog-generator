using System;
using System.Collections.Generic;
using System.Linq;

namespace blog_generator {
    public class Blog {
        public List<BlogPost> Posts = new List<BlogPost>();
        private readonly int _postsPerPage;
        private readonly int _recentPostCount;
        public Blog(int postsPerPage, int recentPostCount) {
            _postsPerPage = postsPerPage;
            _recentPostCount = recentPostCount;
        }

        public BlogPost AddPost(string year, string month, string fileName, string markDown) {
            var blogPost = new BlogPost(year, month, fileName, markDown);
            Posts.Add(blogPost);
            return blogPost;
        }

        public void Sort() {
            Posts = Posts.OrderByDescending(p => p.Year).ThenByDescending(p => p.Month).ToList();
        }

        public IEnumerable<BlogPost> GetPostsForPage(int index) {
            var start = (index - 1) * _postsPerPage;
            return Posts.GetRange(start, Math.Min(start + _postsPerPage, Posts.Count - start));
        }

        public int PageCount() {
            return (Posts.Count / _postsPerPage) + Posts.Count % _postsPerPage;
        }

        public IEnumerable<BlogPost> GetRecentPosts() {
            return Posts.Take(_recentPostCount);
        }
    }
}