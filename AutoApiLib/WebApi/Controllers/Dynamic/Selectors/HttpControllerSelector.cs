using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using Application.Extensions;
using AutoApiLib.WebApi.Controllers.Dynamic;
using AutoApiLib.WebApi.Controllers.Dynamic.Builders;
using AutoApiLib.WebApi.Controllers.Dynamic.Selectors;

namespace AutoApiLib.WebApi.Controllers.Dynamic.Selectors
{
    /// <summary>
    /// This class is used to extend default controller selector to add dynamic api controller creation feature of Abp.
    /// It checks if requested controller is a dynamic api controller, if it is,
    /// returns <see cref="HttpControllerDescriptor"/> to ASP.NET system.
    /// </summary>
    public class HttpControllerSelector : DefaultHttpControllerSelector
    {
        private readonly HttpConfiguration _configuration;
        private readonly DynamicApiControllerManager _dynamicApiControllerManager;

        /// <summary>
        /// Creates a new <see cref="AbpHttpControllerSelector"/> object.
        /// </summary>
        /// <param name="configuration">Http configuration</param>
        /// <param name="dynamicApiControllerManager"></param>
        public HttpControllerSelector(HttpConfiguration configuration, DynamicApiControllerManager dynamicApiControllerManager)
            : base(configuration)
        {
            _configuration = configuration;
            _dynamicApiControllerManager = dynamicApiControllerManager;
        }

        /// <summary>
        /// This method is called by Web API system to select the controller for this request.
        /// </summary>
        /// <param name="request">Request object</param>
        /// <returns>The controller to be used</returns>
        public override HttpControllerDescriptor SelectController(HttpRequestMessage request)
        {
            //Get request and route data
            if (request == null)
            {
                return base.SelectController(null);
            }

            var routeData = request.GetRouteData();
            if (routeData == null)
            {
                return base.SelectController(request);
            }

            //Get serviceNameWithAction from route
            string serviceNameWithAction;
            object tempValue;
            if (!routeData.Values.TryGetValue("serviceNameWithAction", out tempValue))
            {
                return base.SelectController(request);                
            }

            serviceNameWithAction = tempValue.ToString();

            //Normalize serviceNameWithAction
            if (serviceNameWithAction.EndsWith("/"))
            {
                serviceNameWithAction = serviceNameWithAction.Substring(0, serviceNameWithAction.Length - 1);
                routeData.Values["serviceNameWithAction"] = serviceNameWithAction;
            }

            //Get the dynamic controller
            var hasActionName = false;
            var controllerInfo = _dynamicApiControllerManager.FindOrNull(serviceNameWithAction);
            if (controllerInfo == null)
            {
                if (!DynamicApiServiceNameHelper.IsValidServiceNameWithAction(serviceNameWithAction))
                {
                    return base.SelectController(request);
                }
                
                var serviceName = DynamicApiServiceNameHelper.GetServiceNameInServiceNameWithAction(serviceNameWithAction);
                controllerInfo = _dynamicApiControllerManager.FindOrNull(serviceName);
                if (controllerInfo == null)
                {
                    return base.SelectController(request);                    
                }

                hasActionName = true;
            }
            
            //Create the controller descriptor
            var controllerDescriptor = new DynamicHttpControllerDescriptor(_configuration, controllerInfo);
            controllerDescriptor.Properties["__DynamicApiControllerInfo"] = controllerInfo;
            controllerDescriptor.Properties["__DynamicApiHasActionName"] = hasActionName;
            return controllerDescriptor;
        }
    }
}