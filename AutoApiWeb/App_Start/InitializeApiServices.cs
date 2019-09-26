using Application.Dependency;
using System.Linq;
using AutoApiLib.WebApi.Controllers;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.Http.Controllers;
using System.Web.Http.Description;
using AutoApiLib.WebApi.Controllers.Dynamic.Selectors;
using AutoApiLib.WebApi.Configuration;
using AutoApiLib.WebApi.Controllers.Dynamic;
using Castle.MicroKernel.Registration;
using System;
using System.Reflection;
using Application.Services;
using Application.Web.ProxyScripting.Configuration;
using AutoApiLib.WebApi.Controllers.Dynamic.Builders;
using AutoApiLib.WebApi.Controllers.ApiExplorer;

namespace AutoApiWeb.App_Start
{
    public class InitializeApiServices
    {
        static void InitializeWebApi(IocManager iocManager)
        {
            var httpConfiguration = iocManager.Resolve<IWebApiConfiguration>().HttpConfiguration;
            InitializeAspNetServices(httpConfiguration, iocManager);
            InitializeRoutes(httpConfiguration);

            foreach (var controllerInfo in iocManager.Resolve<DynamicApiControllerManager>().GetAll())
            {
                iocManager.IocContainer.Register(
                    Component.For(controllerInfo.InterceptorType).LifestyleTransient(),
                    Component.For(controllerInfo.ApiControllerType)
                        .Proxy.AdditionalInterfaces(controllerInfo.ServiceInterfaceType)
                        .Interceptors(controllerInfo.InterceptorType)
                        .LifestyleTransient()
                    );

                //LogHelper.Logger.DebugFormat("Dynamic web api controller is created for type '{0}' with service name '{1}'.", controllerInfo.ServiceInterfaceType.FullName, controllerInfo.ServiceName);
            }
        }
        static void InitializeAspNetServices(HttpConfiguration httpConfiguration, IocManager iocManager)
        {
            httpConfiguration.Services.Replace(typeof(IHttpControllerSelector), new HttpControllerSelector(httpConfiguration, iocManager.Resolve<DynamicApiControllerManager>()));
            httpConfiguration.Services.Replace(typeof(IHttpActionSelector), new AutoApiLib.WebApi.Controllers.Dynamic.Selectors.ApiControllerActionSelector(iocManager.Resolve<IWebApiConfiguration>()));
            httpConfiguration.Services.Replace(typeof(IHttpControllerActivator), new ApiControllerActivator(iocManager));
            httpConfiguration.Services.Replace(typeof(IApiExplorer), iocManager.Resolve<MyApiExplorer>());
        }
        static void InitializeRoutes(HttpConfiguration httpConfiguration)
        {
            //Dynamic Web APIs

            httpConfiguration.Routes.MapHttpRoute(
                name: "MyDynamicWebApi",                
                routeTemplate: "api/services/{*serviceNameWithAction}"
                );
        }
        static void CreateWebApi(IocManager iocManager)
        {
            var assembly = Assembly.Load("Application");
            var iocResolver = iocManager.Resolve<IIocResolver>();
            var dynamicBuilder = new DynamicApiControllerBuilder(iocResolver);

            //Assembly.GetExecutingAssembly() 
            dynamicBuilder
                .ForAll<IApplicationService>(assembly, "myapp")
                
                .ForMethods(builder =>
                {
                    //if (builder.Method.IsDefined(typeof(MyIgnoreApiAttribute)))
                    //{
                    //    builder.DontCreate = true;
                    //}
                })
                .WithConventionalVerbs()
                .WithProxyScripts(false)
                .Build();
        }

        public static void Start()
        {
            var iocManager = new IocManager();

            iocManager.IocContainer.Register(
                   Component.For<ITestAppService>().ImplementedBy<TestAppService>(),
                   Component.For<IApiProxyScriptingConfiguration>().ImplementedBy<ApiProxyScriptingConfiguration>(),
                   Component.For<IDynamicApiControllerBuilder>().ImplementedBy<DynamicApiControllerBuilder>(),
                   Component.For<DynamicApiControllerManager>().ImplementedBy<DynamicApiControllerManager>()
            );

            iocManager.AddConventionalRegistrar(new ApiControllerConventionalRegistrar());
            iocManager.Register<IWebApiConfiguration, WebApiConfiguration>();
            iocManager.Register<IApiExplorer, MyApiExplorer>();

            try
            {
                CreateWebApi(iocManager);
                InitializeWebApi(iocManager);                               
            }
            catch (Exception exc)
            {
                throw;
            }
        }
    }
}