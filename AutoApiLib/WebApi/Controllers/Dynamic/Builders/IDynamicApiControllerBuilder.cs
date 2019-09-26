﻿using System.Reflection;

namespace AutoApiLib.WebApi.Controllers.Dynamic.Builders
{
    public interface IDynamicApiControllerBuilder
    {
        IApiControllerBuilder<T> For<T>(string serviceName);

        IBatchApiControllerBuilder<T> ForAll<T>(Assembly assembly, string servicePrefix);
    }
}