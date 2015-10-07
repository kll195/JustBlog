using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JustBlog.Core.Interfaces;
using System.Data.Entity;
using JustBlog.Core.Objects;
using Rhino.Mocks;

namespace JustBlog.Core.Tests
{
    public class TestContext : IBlogContext
    {

        public IDbSet<Objects.Category> Categories { get; set; }

        public IDbSet<Objects.Tag> Tags { get; set; }


        public IDbSet<Objects.Post> Posts { get; set; }
 
        public int SaveChanges()
        {
            throw new NotImplementedException();
        }
    }
}
