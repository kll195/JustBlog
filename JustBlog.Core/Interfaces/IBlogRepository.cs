using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JustBlog.Core.Objects;

namespace JustBlog.Core.Interfaces
{
    public interface IBlogRepository
    {
        IList<Post> Posts(int pageNo, int pageSize);
        int TotalPosts(bool checkIsPublished = true);

        IList<Post> PostsForCategory(string categorySlug, int pageNo, int pageSize);
        int TotalPostsForCategory(string categorySlug);
        Category Category(string categorySlug);
        IList<Category> Categories();

        IList<Post> PostsForTag(string tagSlug, int pageNo, int pageSize);
        int TotalPostsForTag(string tagSlug);
        Tag Tag(string tagSlug);
        

        IList<Post> PostsForSearch(string search, int pageNo, int pageSize);
        int TotalPostsForSearch(string search);

        Post Post(int year, int month, string titleSlug);

        IList<Tag> Tags();

        IList<Post> Posts(int pageNo, int pageSize, string sortColumn,
                            bool sortByAscending);

        Category Category(int id);
        Tag Tag(int id);

        int AddPost(Post post);
        int AddCategory(Category category);
        int AddTag(Tag tag); 

        
        void EditPost(Post post);
        void EditCategory(Category category);
        void EditTag(Tag tag); 

        void DeletePost(int id);
        void DeleteCategory(int id);
        void DeleteTag(int id); 

    }
}
