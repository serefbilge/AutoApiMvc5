using Application.Configuration.Startup;
using AutoApiLib.WebApi.Configuration;

namespace AutoApiLib.Configuration.Startup
{
    /// <summary>
    /// Defines extension methods to <see cref="IModuleConfigurations"/> to allow to configure Abp.Web.Api module.
    /// </summary>
    public static class AbpWebApiConfigurationExtensions
    {
        /// <summary>
        /// Used to configure Abp.Web.Api module.
        /// </summary>
        public static IWebApiConfiguration AbpWebApi(this IModuleConfigurations configurations)
        {
            return configurations.AbpConfiguration.Get<IWebApiConfiguration>();
        }
    }
}