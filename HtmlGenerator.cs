using System;

namespace blog_generator {
    public class HtmlGenerator {
        private readonly UrlGenerator _urlGenerator;
        public HtmlGenerator() {
            _urlGenerator = new UrlGenerator();
        }

        public string IndexFromTemplate(Blog blog, string fromTemplate, int page) {
            var result = FromTemplate(blog, fromTemplate, page);
            result = SetTitle(result, string.Empty);
            result = GenerateRecentPosts(blog, result);
            return result;
        }

        public string PagesFromTemplate(Blog blog, string fromTemplate, int page) {
            var result = FromTemplate(blog, fromTemplate, page);
            result = SetTitle(result, $"Posts page {page} - ");
            result = GenerateRecentPosts(blog, result);
            return result;
        }

        public string PostPageTemplate(Blog blog, BlogPost post, string template) {
            var postHtml = GeneratePostHtml(post);
            var result = template.Replace("{{posts}}", postHtml);
            result = SetTitle(result, post.Title + " - ");
            result = GenerateRecentPosts(blog, result);
            result = GeneratePagingHtml(blog, result, 0);
            return result;
        }

        private string FromTemplate(Blog blog, string fromTemplate, int page) {
            var result = fromTemplate;
            result = GeneratePostHtmlList(blog, result, page);
            result = GenerateTreeMenu(blog, result);
            result = GeneratePagingHtml(blog, result, page);
            return result;
        }

        private string GenerateRecentPosts(Blog blog, string fromTemplate) {
            var recentPosts = blog.GetRecentPosts();
            var posts = "<ul id=\"recent-posts\">";
            foreach(BlogPost post in recentPosts) {
                var link = $"<a href='{_urlGenerator.GetPostUrl(post)}'>{post.Title}</a>";
                posts += $"<li>{link}</li>";
            }
            posts += "</ul>";
            return fromTemplate.Replace("{{recent_posts}}", posts);
        }

        private string SetTitle(string template, string title) {
            return template.Replace("{{titleprefix}}", title);
        }

        private string GeneratePostHtmlList(Blog blog, string template, int page) {
            var posts = "<ul id=\"posts\">";
            foreach(BlogPost post in blog.GetPostsForPage(page)) {
                var postHtml = GeneratePostHtml(post);
                posts += $"<li>{postHtml}</li>";
            }
            posts += "</ul>";
            var result = template.Replace("{{posts}}", posts);
            return result;
        }

        private string GeneratePostHtml(BlogPost post) {
            var postHtml = post.MarkDown;
            postHtml = postHtml.Replace("{{post_url}}", _urlGenerator.GetPostUrl(post));
            postHtml = CommonMark.CommonMarkConverter.Convert(postHtml);
            return postHtml;
        }

        private string GeneratePagingHtml(Blog blog, string template, int page) {
            string pagingHtml = string.Empty;
            if(page == 1)
                pagingHtml = "<a href='" + _urlGenerator.GetPageUrl(page + 1) + "'>Next</a>";
            else if(page == blog.PageCount())
                pagingHtml = "<a href='" + _urlGenerator.GetPageUrl(page - 1) + "'>Previous</a>";
            else if(page > 0) {
                pagingHtml = "<a href='" + _urlGenerator.GetPageUrl(page - 1) + "'>Previous</a> - <a href='" + _urlGenerator.GetPageUrl(page + 1) + "'>Next</a>";
            }
            return template.Replace("{{paging}}", pagingHtml);
        }

        public string GenerateTreeMenu(Blog blog, string template) {
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
    }
}