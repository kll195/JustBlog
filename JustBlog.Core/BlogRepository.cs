using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using JustBlog.Core.Interfaces;
using JustBlog.Core.Objects;

namespace JustBlog.Core
{
    public class BlogRepository : IBlogRepository
    {
        private readonly IBlogContext _context; 
        public BlogRepository(IBlogContext newContext)
        {
            _context = newContext; 
        }

       

        public IList<Objects.Post> Posts(int pageNo, int pageSize)
        {
            var posts = _context.Posts
                .Where(p => p.Published)
                .OrderByDescending(p => p.PostedOn)
                .Skip(pageNo*pageSize)
                .Take(pageSize)
                .Include(p => p.Category)
                .ToList();

            var postIds = posts.Select(p => p.Id).ToList();


            return _context.Posts
                .Where(p => postIds.Contains(p.Id))
                .OrderByDescending(p => p.PostedOn)
                .Include(p => p.Tags)
                .ToList(); 
        }

        public int TotalPosts(bool checkIsPublished = true)
        {
            return _context.Posts
           .Where(p => checkIsPublished || p.Published == true)
           .Count();
        }
        public IList<Post> PostsForCategory(string categorySlug, int pageNo, int pageSize)
        {
            var posts = _context.Posts
                                .Where(p => p.Published && p.Category.UrlSlug.Equals(categorySlug))
                                .OrderByDescending(p => p.PostedOn)
                                .Skip(pageNo * pageSize)
                                .Take(pageSize)
                                .Include(p => p.Category)
                                .ToList();

            var postIds = posts.Select(p => p.Id).ToList();

            return _context.Posts
                          .Where(p => postIds.Contains(p.Id))
                          .OrderByDescending(p => p.PostedOn)
                          .Include(p => p.Tags)
                          .ToList();
        }

        public int TotalPostsForCategory(string categorySlug)
        {
            var posts = _context.Posts
                .Where(p => p.Published && p.Category.UrlSlug.Equals(categorySlug));

            return posts.Count(); 
        }

        public Category Category(string categorySlug)
        {
            return _context.Categories
                        .FirstOrDefault(t => t.UrlSlug.Equals(categorySlug));
        }
        public IList<Post> PostsForTag(string tagSlug, int pageNo, int pageSize)
        {
            var posts = _context.Posts
                              .Where(p => p.Published && p.Tags.Any(t => t.UrlSlug.Equals(tagSlug)))
                              .OrderByDescending(p => p.PostedOn)
                              .Skip(pageNo * pageSize)
                              .Take(pageSize)
                              .Include(p => p.Category)
                              .ToList();

            var postIds = posts.Select(p => p.Id).ToList();

            return _context.Posts
                          .Where(p => postIds.Contains(p.Id))
                          .OrderByDescending(p => p.PostedOn)
                          .Include(p => p.Tags)
                          .ToList();
        }

        public int TotalPostsForTag(string tagSlug)
        {
            var query = _context.Posts
                .Where(p => p.Published && p.Tags.Any(t => t.UrlSlug.Equals(tagSlug)));

            return query.Count(); 
        }

        public Tag Tag(string tagSlug)
        {
            return _context.Tags
                        .FirstOrDefault(t => t.UrlSlug.Equals(tagSlug));
        }

        public IList<Category> Categories()
        {
            return _context.Categories.OrderBy(p => p.Name).ToList();
        }


        public IList<Post> PostsForSearch(string search, int pageNo, int pageSize)
        {
            var posts = _context.Posts
                                  .Where(p => p.Published && (p.Title.Contains(search) || p.Category.Name.Equals(search) || p.Tags.Any(t => t.Name.Equals(search))))
                                  .OrderByDescending(p => p.PostedOn)
                                  .Skip(pageNo * pageSize)
                                  .Take(pageSize)
                                  .Include(p => p.Category)
                                  .ToList();

            var postIds = posts.Select(p => p.Id).ToList();

            return _context.Posts
                  .Where(p => postIds.Contains(p.Id))
                  .OrderByDescending(p => p.PostedOn)
                  .Include(p => p.Tags)
                  .ToList();
        }

        public int TotalPostsForSearch(string search)
        {
            var query = _context.Posts
                .Where(
                    p =>
                        p.Published &&
                        (p.Title.Contains(search) || p.Category.Name.Equals(search) ||
                         p.Tags.Any(t => t.Name.Equals(search))));

            return query.Count(); 

        }


        public Post Post(int year, int month, string titleSlug)
        {
            var posts = _context.Posts
                .Where(p => p.PostedOn.Year == year && p.PostedOn.Month == month && p.UrlSlug.Equals(titleSlug))
                .Include(p => p.Category)
                .Include(p => p.Tags);

            return posts.Single(); // There might be cases where more than one is returned. 

        }

        public IList<Tag> Tags()
        {
            return _context.Tags.OrderBy(p => p.Name).ToList();

        }


        public IList<Post> Posts(int pageNo, int pageSize, string sortColumn,
                                    bool sortByAscending)
        {
            IList<Post> posts;
            IList<int> postIds;

            switch (sortColumn)
            {
                case "Title":
                    if (sortByAscending)
                    {
                        posts = _context.Posts
                                        .OrderBy(p => p.Title)
                                        .Skip(pageNo * pageSize)
                                        .Take(pageSize)
                                        .Include(p => p.Category)
                                        .ToList();

                        postIds = posts.Select(p => p.Id).ToList();

                        posts = _context.Posts
                                          .Where(p => postIds.Contains(p.Id))
                                          .OrderBy(p => p.Title)
                                          .Include(p => p.Tags)
                                          .ToList();
                    }
                    else
                    {
                        posts = _context.Posts
                                        .OrderByDescending(p => p.Title)
                                        .Skip(pageNo * pageSize)
                                        .Take(pageSize)
                                        .Include(p => p.Category)
                                        .ToList();

                        postIds = posts.Select(p => p.Id).ToList();

                        posts = _context.Posts
                                          .Where(p => postIds.Contains(p.Id))
                                          .OrderByDescending(p => p.Title)
                                          .Include(p => p.Tags)
                                          .ToList();
                    }
                    break;
                case "Published":
                    if (sortByAscending)
                    {
                        posts = _context.Posts
                                        .OrderBy(p => p.Published)
                                        .Skip(pageNo * pageSize)
                                        .Take(pageSize)
                                        .Include(p => p.Category)
                                        .ToList();

                        postIds = posts.Select(p => p.Id).ToList();

                        posts = _context.Posts
                                          .Where(p => postIds.Contains(p.Id))
                                          .OrderBy(p => p.Published)
                                          .Include(p => p.Tags)
                                          .ToList();
                    }
                    else
                    {
                        posts = _context.Posts
                                        .OrderByDescending(p => p.Published)
                                        .Skip(pageNo * pageSize)
                                        .Take(pageSize)
                                        .Include(p => p.Category)
                                        .ToList();

                        postIds = posts.Select(p => p.Id).ToList();

                        posts = _context.Posts
                                          .Where(p => postIds.Contains(p.Id))
                                          .OrderByDescending(p => p.Published)
                                          .Include(p => p.Tags)
                                          .ToList();
                    }
                    break;
                case "PostedOn":
                    if (sortByAscending)
                    {
                        posts = _context.Posts
                                        .OrderBy(p => p.PostedOn)
                                        .Skip(pageNo * pageSize)
                                        .Take(pageSize)
                                        .Include(p => p.Category)
                                        .ToList();

                        postIds = posts.Select(p => p.Id).ToList();

                        posts = _context.Posts
                                          .Where(p => postIds.Contains(p.Id))
                                          .OrderBy(p => p.PostedOn)
                                          .Include(p => p.Tags)
                                          .ToList();
                    }
                    else
                    {
                        posts = _context.Posts
                                        .OrderByDescending(p => p.PostedOn)
                                        .Skip(pageNo * pageSize)
                                        .Take(pageSize)
                                        .Include(p => p.Category)
                                        .ToList();

                        postIds = posts.Select(p => p.Id).ToList();

                        posts = _context.Posts
                                        .Where(p => postIds.Contains(p.Id))
                                        .OrderByDescending(p => p.PostedOn)
                                        .Include(p => p.Tags)
                                        .ToList();
                    }
                    break;
                case "Modified":
                    if (sortByAscending)
                    {
                        posts = _context.Posts
                                        .OrderBy(p => p.Modified)
                                        .Skip(pageNo * pageSize)
                                        .Take(pageSize)
                                        .Include(p => p.Category)
                                        .ToList();

                        postIds = posts.Select(p => p.Id).ToList();

                        posts = _context.Posts
                                          .Where(p => postIds.Contains(p.Id))
                                          .OrderBy(p => p.Modified)
                                          .Include(p => p.Tags)
                                          .ToList();
                    }
                    else
                    {
                        posts = _context.Posts
                                        .OrderByDescending(p => p.Modified)
                                        .Skip(pageNo * pageSize)
                                        .Take(pageSize)
                                        .Include(p => p.Category)
                                        .ToList();

                        postIds = posts.Select(p => p.Id).ToList();

                        posts = _context.Posts
                                          .Where(p => postIds.Contains(p.Id))
                                          .OrderByDescending(p => p.Modified)
                                          .Include(p => p.Tags)
                                          .ToList();
                    }
                    break;
                case "Category":
                    if (sortByAscending)
                    {
                        posts = _context.Posts
                                        .OrderBy(p => p.Category.Name)
                                        .Skip(pageNo * pageSize)
                                        .Take(pageSize)
                                        .Include(p => p.Category)
                                        .ToList();

                        postIds = posts.Select(p => p.Id).ToList();

                        posts = _context.Posts
                                          .Where(p => postIds.Contains(p.Id))
                                          .OrderBy(p => p.Category.Name)
                                          .Include(p => p.Tags)
                                          .ToList();
                    }
                    else
                    {
                        posts = _context.Posts
                                        .OrderByDescending(p => p.Category.Name)
                                        .Skip(pageNo * pageSize)
                                        .Take(pageSize)
                                        .Include(p => p.Category)
                                        .ToList();

                        postIds = posts.Select(p => p.Id).ToList();

                        posts = _context.Posts
                                          .Where(p => postIds.Contains(p.Id))
                                          .OrderByDescending(p => p.Category.Name)
                                          .Include(p => p.Tags)
                                          .ToList();
                    }
                    break;
                default:
                    posts = _context.Posts
                                    .OrderByDescending(p => p.PostedOn)
                                    .Skip(pageNo * pageSize)
                                    .Take(pageSize)
                                    .Include(p => p.Category)
                                    .ToList();

                    postIds = posts.Select(p => p.Id).ToList();

                    posts = _context.Posts
                                      .Where(p => postIds.Contains(p.Id))
                                      .OrderByDescending(p => p.PostedOn)
                                      .Include(p => p.Tags)
                                      .ToList();
                    break;
            }

            return posts;
        }

        public int AddPost(Post post)
        {
            _context.Posts.Add(post);
            _context.SaveChanges();
                return post.Id;
            
        }


        public Category Category(int id)
        {
            return _context.Categories.FirstOrDefault(t => t.Id == id);
        }

        public Tag Tag(int id)
        {
            return _context.Tags.FirstOrDefault(t => t.Id == id);
        }

        public void EditPost(Post post)
        {




           

            var entry = _context.Posts.Find(post.Id);

            
            entry.Category = post.Category;
            entry.Description = post.Description;
            entry.ShortDescription = post.ShortDescription;
            entry.Meta = post.Meta;
            entry.Published = post.Published;
            entry.Title = post.Title;
            entry.UrlSlug = post.UrlSlug;
            entry.Modified = post.Modified; 
            _context.Tags.Load();

            foreach (var tag in post.Tags.ToList())
            {
                if (!entry.Tags.Contains(tag))
                {
                    entry.Tags.Add(tag); 
                }
            }

            foreach (var tag in entry.Tags.ToList())
            {
                if (!post.Tags.Contains(tag))
                {
                    entry.Tags.Remove(tag); 
                }
            }

           
            
            _context.SaveChanges();
        }
        public void DeletePost(int id)
        {

            var post = _context.Posts.FirstOrDefault(t => t.Id == id);
            _context.Posts.Remove(post);
                _context.SaveChanges();
                
            
        }


        public int AddCategory(Category category)
        {
            _context.Categories.Add(category);
            _context.SaveChanges();

            return category.Id; 
        }

        public void EditCategory(Category category)
        {
            var entry = _context.Categories.Find(category.Id);

            entry.Description = category.Description;
            entry.UrlSlug = category.UrlSlug;
            entry.Name = category.Name;

            _context.SaveChanges(); 

        }

        public void DeleteCategory(int id)
        {
            var entry = _context.Categories.Find(id);

            _context.Categories.Remove(entry);
            _context.SaveChanges(); 
        }


        public int AddTag(Tag tag)
        {
            _context.Tags.Add(tag);
            _context.SaveChanges();

            return tag.Id; 
        }

        public void EditTag(Tag tag)
        {
            var entry = _context.Tags.Find(tag.Id);

            entry.Name = tag.Name;
            entry.Description = tag.Description;
            entry.UrlSlug = tag.UrlSlug;

            _context.SaveChanges(); 
        }

        public void DeleteTag(int id)
        {
            var entry = _context.Tags.Find(id);
            _context.Tags.Remove(entry);
            _context.SaveChanges(); 
        }
    }
}
