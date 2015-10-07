using Ninject;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JustBlog.Core.Interfaces;
using Ninject.Web.Common;

namespace JustBlog.Core
{
    public class RepositoryModule : NinjectModule
    {

        public override void Load()
        {
            Bind<IBlogContext>().To<BlogContext>().InRequestScope();
        }
    }
}
