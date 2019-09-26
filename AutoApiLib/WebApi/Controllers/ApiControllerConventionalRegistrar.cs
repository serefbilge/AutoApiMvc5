using System.Web.Http;
using Abp.Dependency;
using Castle.MicroKernel.Registration;
using System.Reflection;
using Application.Dependency;

namespace AutoApiLib.WebApi.Controllers
{
    /// <summary>
    /// Registers all Web API Controllers derived from <see cref="System.Web.Http.ApiController"/>.
    /// </summary>
    public class ApiControllerConventionalRegistrar : IConventionalDependencyRegistrar
    {
        public void RegisterAssembly(IConventionalRegistrationContext context)
        {
            context.IocManager.IocContainer.Register(
                Classes.FromAssembly(context.Assembly)
                    .BasedOn<System.Web.Http.ApiController>()
                    .If(type => !type.GetTypeInfo().IsGenericTypeDefinition)
                    .LifestyleTransient()
                );
        }
    }
}
