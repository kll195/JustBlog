using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework; 
using Rhino.Mocks; 
using JustBlog.Core;
using JustBlog.Core.Interfaces;
using JustBlog.Core.Objects;

namespace JustBlog.Core.Tests
{
    [TestFixture]
    public class BlogRepositoryTests
    {
        private BlogContext MockContext;
        private IQueryable<Post> Posts;
        private IQueryable<Category> Categories;
        private IQueryable<Tag> Tags;
        private IDbSet<Post> SetPosts;
        private IDbSet<Category> SetCategories;
        private IDbSet<Tag> SetTags;

 



        [SetUp]
        public void SetUp()
        {
            MockContext = MockRepository.GenerateMock<BlogContext>();
            SetPosts = MockRepository.GenerateMock<IDbSet<Post>, IQueryable>();
            SetCategories = MockRepository.GenerateMock<IDbSet<Category>, IQueryable>();
            SetTags = MockRepository.GenerateMock<IDbSet<Tag>, IQueryable>();
            #region Sample Posts and Mock Set Up
            Posts = new List<Post>
            {
                new Post
                {
                    //Category = Categories.Single(p => p.Id == 1),  
                    Description = "Hello World Welcomes you!", 
                    Id = 1, 
                    Meta = "helloworld", 
                    Modified = new DateTime(2015, 6, 3), 
                    PostedOn = new DateTime(2015, 6, 1), 
                    Published = true, 
                    ShortDescription = "Hello World is awesome", 
                    //Tags = this.Tags.ToList(), 
                    Title = "Hello World is a great starting place", 
                    UrlSlug = "helloworldpost"
                    
                }, 
                new Post
                {
                    //Category = Categories.Single(p => p.Id == 2),  
                    Description = "GMU Welcomes you!", 
                    Id = 2, 
                    Meta = "gmu", 
                    Modified = new DateTime(2015, 6, 3), 
                    PostedOn = new DateTime(2015, 6, 1), 
                    Published = true, 
                    ShortDescription = "GMU is awesome", 
                    //Tags = this.Tags.Where(t => t.Id == 2).ToList(), 
                    Title = "GMUis a great starting place", 
                    UrlSlug = "gmupost"
                    
                }, 
                 new Post
                {
                   // Category = Categories.Single(p => p.Id == 3),  
                    Description = "Entity Framework is great!", 
                    Id = 3, 
                    Meta = "ef", 
                    Modified = new DateTime(2015, 6, 3), 
                    PostedOn = new DateTime(2015, 6, 1), 
                    Published = true, 
                    ShortDescription = "Entity Framework is awesome", 
                    //Tags = this.Tags.Where(t => t.Id == 1 || t.Id == 3).ToList(), 
                    Title = "Entity Framework is a great starting place", 
                    UrlSlug = "efpost"
                    
                } 
                    
                
            }.AsQueryable();


         
            #endregion

            #region Sample Categories and Mock Set Up
            Categories = new List<Category>
            {
                new Category
                {
                    Description = "Humor",
                    Id = 1,
                    Name = "Humor",
                    //Posts = this.Posts.ToList(),
                    UrlSlug = "humor"
                },
                   new Category
                {
                    Description = "Programming",
                    Id = 2,
                    Name = "Programming",
                    //Posts = this.Posts.ToList(),
                    UrlSlug = "programming"
                },
                   new Category
                {
                    Description = "School",
                    Id = 3,
                    Name = "School",
                    //Posts = this.Posts.ToList(),
                    UrlSlug = "school"
                }
            }.AsQueryable();


            #endregion

            #region Sample Tags and Mock Set Up
            Tags = new List<Tag>
            {
                new Tag
                {
                    Description = "Hello World", 
                    Id = 1,
                    Name = "Hello World", 
                    //Posts = this.Posts.ToList(), 
                    UrlSlug = "helloworld"
                }, 
                 new Tag
                {
                    Description = "MVC", 
                    Id = 2,
                    Name = "MVC", 
                   // Posts = this.Posts.ToList(), 
                    UrlSlug = "MVC"
                },
                 new Tag
                {
                    Description = "Hello World", 
                    Id = 3,
                    Name = "Hello World", 
                    //Posts = this.Posts.ToList(), 
                    UrlSlug = "helloworld"
                },

            }.AsQueryable();

            
           

            #endregion

            #region Setting relationships
            // setting relations for posts
            Random rnd = new Random();
            foreach (var post in Posts)
            {
                //assigns random tags to posts 
                post.Tags = this.Tags.Where(p => p.Id == rnd.Next(1, 4)).ToList(); 
                //assigns random category to post
                post.Category = this.Categories.Single(p => p.Id == 1); 
            }

            //setting relations for tags
            foreach (var tag in Tags)
            {
                //assigns random tags to posts 
                tag.Posts = this.Posts.Where(p => p.Id == rnd.Next(1, 4)).ToList();
            }

            foreach (var category in Categories)
            {
                category.Posts = this.Posts.Where(p => p.Id == rnd.Next(1, 4)).ToList();
            }
            #endregion

            SetPosts.Stub(m => m.Provider).Return(Posts.Provider);
            SetPosts.Stub(m => m.Expression).Return(Posts.Expression);
            SetPosts.Stub(m => m.ElementType).Return(Posts.ElementType);
            SetPosts.Stub(m => m.GetEnumerator()).Return(Posts.GetEnumerator());
            MockContext.Stub(x => x.Posts).PropertyBehavior();

            MockContext.Posts = SetPosts;


            SetCategories.Stub(m => m.Provider).Return(Categories.Provider);
            SetCategories.Stub(m => m.Expression).Return(Categories.Expression);
            SetCategories.Stub(m => m.ElementType).Return(Categories.ElementType);
            SetCategories.Stub(m => m.GetEnumerator()).Return(Categories.GetEnumerator());
            MockContext.Stub(x => x.Categories).PropertyBehavior();

            MockContext.Categories = SetCategories; 


            SetTags.Stub(m => m.Provider).Return(Tags.Provider);
            SetTags.Stub(m => m.Expression).Return(Tags.Expression);
            SetTags.Stub(m => m.ElementType).Return(Tags.ElementType);
            SetTags.Stub(m => m.GetEnumerator()).Return(Tags.GetEnumerator());
            MockContext.Stub(x => x.Tags).PropertyBehavior();

            MockContext.Tags = SetTags; 

        }


        [Test]
        public void Get_all_posts_returns_3()
        {
            var testRepository = new BlogRepository(MockContext);
            IEnumerable<Post> result = testRepository.Posts(0, 3); 
            Assert.That(result.Count(), Is.EqualTo(3));

        }

        [Test]
        public void Category_gets_correct_category()
        {
            string testSlug = "humor";
            var testRepository = new BlogRepository(MockContext);
            Category result = testRepository.Category(testSlug);

            Assert.That(result.Name.Equals("Humor"));
        }

        [Test]
        public void PostForCategory_for_humor_returns_three_posts()
        {
            string testSlug = "humor";
            {
                var testRepo = new BlogRepository(MockContext);
                IList<Post> result = testRepo.PostsForCategory("humor", 0, 5); 

                Assert.That(result.Count, Is.EqualTo(3));
            }
        }

        [Test]
        public void PostForCategory_for_programming_returns_zero()
        {
            var testRepo = new BlogRepository(MockContext);
            IList<Post> result = testRepo.PostsForCategory("programming", 0, 5);

            Assert.That(result.Count, Is.EqualTo(0));
        }
   }

    
}
