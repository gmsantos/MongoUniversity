using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using MongoDB.Driver;
using M101DotNet.WebApp.Models;
using M101DotNet.WebApp.Models.Home;
using MongoDB.Bson;
using System.Linq.Expressions;

namespace M101DotNet.WebApp.Controllers
{
    public class HomeController : Controller
    {
        public async Task<ActionResult> Index()
        {
            var blogContext = new BlogContext();
            // XXX WORK HERE
            // find the most recent 10 posts and order them
            // from newest to oldest

            var sort = Builders<Post>.Sort.Ascending("CreatedAtUtc");
            var recentPosts = await blogContext.Posts.Find("{}").Limit(10).Sort(sort).ToListAsync();

            var model = new IndexModel
            {
                RecentPosts = recentPosts
            };

            return View(model);
        }

        [HttpGet]
        public ActionResult NewPost()
        {
            return View(new NewPostModel());
        }

        [HttpPost]
        public async Task<ActionResult> NewPost(NewPostModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var blogContext = new BlogContext();
            // XXX WORK HERE
            // Insert the post into the posts collection
            var post = new Post{
                Author = this.User.Identity.Name,
                Title = model.Title,
                Content = model.Content,
                Tags = model.Tags.Split(',').Select(p => p.Trim()).ToList(),
                CreatedAtUtc = DateTime.UtcNow
            };

            await blogContext.Posts.InsertOneAsync(post);

            return RedirectToAction("Post", new { id = post.Id });
        }

        [HttpGet]
        public async Task<ActionResult> Post(string id)
        {
            var blogContext = new BlogContext();

            // XXX WORK HERE
            // Find the post with the given identifier

            var filter = Builders<Post>.Filter.Eq("Id", id);
            var post = await blogContext.Posts.Find(filter).SingleOrDefaultAsync();

            if (post == null)
            {
                return RedirectToAction("Index");
            }

            var model = new PostModel
            {
                Post = post
            };

            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> Posts(string tag = null)
        {
            var blogContext = new BlogContext();

            // XXX WORK HERE
            // Find all the posts with the given tag if it exists.
            // Otherwise, return all the posts.
            // Each of these results should be in descending order.

            var filter = Builders<Post>.Filter.Eq("Tags", tag);
            var sort = Builders<Post>.Sort.Descending("CreatedAtUtc");
            var posts = await blogContext.Posts.Find(filter).Sort(sort).ToListAsync();

            if (posts == null)
            {
                posts = await blogContext.Posts.Find("{}").Sort(sort).ToListAsync();
            }

            return View(posts);
        }

        [HttpPost]
        public async Task<ActionResult> NewComment(NewCommentModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Post", new { id = model.PostId });
            }

            var blogContext = new BlogContext();
            // XXX WORK HERE
            // add a comment to the post identified by model.PostId.
            // you can get the author from "this.User.Identity.Name"

            var filter = Builders<Post>.Filter.Eq("Id", model.PostId);
            var post = await blogContext.Posts.Find(filter).SingleAsync();

            var comment = new Comment
            {
                Author = this.User.Identity.Name,
                Content = model.Content,
                CreatedAtUtc = DateTime.UtcNow
            };

            var update = Builders<Post>.Update.Push("Comments", comment);
            await blogContext.Posts.UpdateOneAsync(filter, update);

            return RedirectToAction("Post", new { id = model.PostId });
        }
    }
}