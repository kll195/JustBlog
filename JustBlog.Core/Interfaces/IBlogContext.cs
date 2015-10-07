using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JustBlog.Core.Objects;

namespace JustBlog.Core.Interfaces
{
    public interface IBlogContext
    {
         IDbSet<Category> Categories { get; set; }
         IDbSet<Tag> Tags { get; set; }
         IDbSet<Post> Posts { get; set; }
        int SaveChanges();


        
    }
}
