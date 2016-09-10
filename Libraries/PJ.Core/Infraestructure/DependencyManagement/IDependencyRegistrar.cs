using Autofac;

namespace JFP.Core.Infrastructure.DependencyManagement
{
    public interface IDependencyRegistrar
    {
        void Register(ContainerBuilder builder, ITypeFinder typeFinder);

        int Order { get; }
    }
}
