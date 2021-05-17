using Autofac;
using Core.Helpers;
using Repository.Commands.Implementations;
using Repository.Commands.Interfaces;
using Repository.Helpers;
using Repository.Queries.Implementations;
using Repository.Queries.Interfaces;
using Service.BL.Implementations;
using Service.BL.Interfaces;
using System.Net.Http;

namespace Services.Helpers
{
    public class AutofacContainerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof(DapperCommandRepository<>))
             .As(typeof(IDapperCommandRepository<>))
             .InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(ElasticSearchQueryRepository<>))
              .As(typeof(IElasticSearchQueryRepository<>))
              .InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(DapperQueryRepository<>))
             .As(typeof(IDapperQueryRepository<>))
             .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(typeof(IAutoDependencyService).Assembly)
             .AssignableTo<IAutoDependencyService>()
             .As<IAutoDependencyService>()
             .AsImplementedInterfaces().InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(typeof(IAutoDependencyRepository).Assembly)
             .AssignableTo<IAutoDependencyRepository>()
             .As<IAutoDependencyRepository>()
             .AsImplementedInterfaces().InstancePerLifetimeScope(); 
            
            builder.RegisterAssemblyTypes(typeof(IAutoDependencyCore).Assembly)
             .AssignableTo<IAutoDependencyCore>()
             .As<IAutoDependencyCore>()
             .AsImplementedInterfaces().InstancePerLifetimeScope();

            base.Load(builder);
        }
    }
}
