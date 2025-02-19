using Backend.interfaces;
using Backend.Services;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.MsDependencyInjection;
using Microsoft.AspNetCore.Mvc;

namespace Backend
{


    public class WindsorServiceProviderFactory : IServiceProviderFactory<IWindsorContainer>
    {
        public IWindsorContainer CreateBuilder(IServiceCollection services)
        {
            var container = new WindsorContainer();

            container.Register(
                Component.For<IDbManagement>().ImplementedBy<DatabaseManagement>().LifestyleSingleton());

            container.Register(Classes.FromThisAssembly().BasedOn<ControllerBase>().LifestyleTransient());

            WindsorRegistrationHelper.CreateServiceProvider(container, services);
            return container;
        }

        public IServiceProvider CreateServiceProvider(IWindsorContainer container)
        {
            return container.Resolve<IServiceProvider>();
        }
    }
}
