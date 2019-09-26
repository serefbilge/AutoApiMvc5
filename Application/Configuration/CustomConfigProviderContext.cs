using Application.Dependency;

namespace Application.Configuration
{
    public class CustomConfigProviderContext
    {
        public IScopedIocResolver IocResolver { get; }

        public CustomConfigProviderContext(IScopedIocResolver iocResolver)
        {
            IocResolver = iocResolver;
        }
    }
}