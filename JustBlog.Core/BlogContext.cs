using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JustBlog.Core.Interfaces;
using JustBlog.Core.Objects;

namespace JustBlog.Core
{
    public class BlogContext : DbContext, IBlogContext
    {
        public BlogContext()
            : base("name=BlogDBConnectionString")
        {
            
        }
        public virtual IDbSet<Category> Categories { get; set; }
        public virtual IDbSet<Tag> Tags { get; set; }
        public virtual IDbSet<Post> Posts { get; set; } 
 
    
    }
}
