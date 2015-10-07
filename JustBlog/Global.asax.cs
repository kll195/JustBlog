using JustBlog.Core;
using JustBlog.Core.Interfaces;
using Ninject;
using Ninject.Web.Common;
using System.Data.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using JustBlog.Core.Objects;
using JustBlog.Provider;

namespace JustBlog
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : NinjectHttpApplication
    {
        protected override void OnApplicationStarted()
        {
            //AreaRegistration.RegisterAllAreas();

            //WebApiConfig.Register(GlobalConfiguration.Configuration);
            //FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            base.OnApplicationStarted();
            Database.SetInitializer<BlogContext>(null);
            ModelBinders.Binders.Add(typeof(Post), new PostModelBinder(Kernel));

        }

        protected override IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            kernel.Load(new RepositoryModule());
            kernel.Bind<IBlogRepository>().To<BlogRepository>().InRequestScope();
            kernel.Bind<IAuthProvider>().To<AuthProvider>();
            return kernel; 

        }

    }
}