using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Data.Entity;
using System.Reflection;
using System.Web;
using Autofac;
using Autofac.Builder;
using Autofac.Core;
using Autofac.Integration.Mvc;
using JFP.Core;
using JFP.Core.Caching;
using JFP.Core.Configuration;
using JFP.Core.Config;
using JFP.Core.Data;
using JFP.Core.Fakes;
using JFP.Core.Infrastructure;
using JFP.WEB.Framework.WebContext;
using JFP.Core.Infrastructure.DependencyManagement;
using JFP.Data;
using JFP.Core.Loggin;
using JFP.Services.CatalogoServices;
using JFP.Services.Resources;

namespace JFP.WEB.Framework
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public virtual void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            ////HTTP context and other related stuff
            //builder.Register(c => 
            //    //register FakeHttpContext when HttpContext is not available
            //    HttpContext.Current != null ?
            //    (new HttpContextWrapper(HttpContext.Current) as HttpContextBase) :
            //    (new FakeHttpContext("~/") as HttpContextBase))
            //    .As<HttpContextBase>()
            //    .InstancePerLifetimeScope();
            //builder.Register(c => c.Resolve<HttpContextBase>().Request)
            //    .As<HttpRequestBase>()
            //    .InstancePerLifetimeScope();
            //builder.Register(c => c.Resolve<HttpContextBase>().Response)
            //    .As<HttpResponseBase>()
            //    .InstancePerLifetimeScope();
            //builder.Register(c => c.Resolve<HttpContextBase>().Server)
            //    .As<HttpServerUtilityBase>()
            //    .InstancePerLifetimeScope();
            //builder.Register(c => c.Resolve<HttpContextBase>().Session)
            //    .As<HttpSessionStateBase>()
            //    .InstancePerLifetimeScope();
            
            //controllers
            builder.RegisterControllers(typeFinder.GetAssemblies().ToArray());

            ////data layer
            //var dataSettingsManager = new DataSettingsManager();
            //var dataProviderSettings = dataSettingsManager.LoadSettings();
            //builder.Register(c => dataSettingsManager.LoadSettings()).As<DataSettings>();
            //builder.Register(x => new EfDataProviderManager(x.Resolve<DataSettings>())).As<BaseDataProviderManager>().InstancePerDependency();
            //builder.Register(x => x.Resolve<BaseDataProviderManager>().LoadDataProvider()).As<IDataProvider>().InstancePerDependency();

            //builder.Register<IDbContext>(c => new PJObjectContext(Config.LlaveString("DBPrecedente"))).InstancePerRequest();

           
            //builder.RegisterGeneric(typeof(EfRepository<>)).As(typeof(IRepository<>)).InstancePerLifetimeScope();


            //////cache manager
            //builder.RegisterType<MemoryCacheManager>().As<ICacheManager>().Named<ICacheManager>("LEGIS.PJ_cache_static").SingleInstance();
            //builder.RegisterType<PerRequestCacheManager>().As<ICacheManager>().Named<ICacheManager>("LEGIS.PJ_cache_per_request").InstancePerLifetimeScope();


            //Loggin
            builder.RegisterType<LoggingManager>().As<ILogging>().SingleInstance();


            builder.RegisterType<FileResource>().As<IResource>().Named<IResource>("JFP.FIleResource").SingleInstance();

            builder.RegisterType<HTTPResource>().As<IResource>().Named<IResource>("JFP.HTTPResource").SingleInstance();

            builder.RegisterType<CatalogoServices>().As<ICatalogoServices>().SingleInstance();
           
           

        }

        public int Order
        {
            get { return 0; }
        }
    }


    public class SettingsSource : IRegistrationSource
    {
        static readonly MethodInfo BuildMethod = typeof(SettingsSource).GetMethod(
            "BuildRegistration",
            BindingFlags.Static | BindingFlags.NonPublic);

        public IEnumerable<IComponentRegistration> RegistrationsFor(
                Service service,
                Func<Service, IEnumerable<IComponentRegistration>> registrations)
        {
            var ts = service as TypedService;
            if (ts != null && typeof(ISettings).IsAssignableFrom(ts.ServiceType))
            {
                var buildMethod = BuildMethod.MakeGenericMethod(ts.ServiceType);
                yield return (IComponentRegistration)buildMethod.Invoke(null, null);
            }
        }

        static IComponentRegistration BuildRegistration<TSettings>() where TSettings : ISettings, new()
        {
            return null;            
        }

        public bool IsAdapterForIndividualComponents { get { return false; } }
    }

}
