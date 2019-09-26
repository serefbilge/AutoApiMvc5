using System.Collections.Generic;

namespace Application.Configuration.Startup
{
    public interface ICustomConfigProvider
    {
        Dictionary<string, object> GetConfig(CustomConfigProviderContext customConfigProviderContext);
    }
}
