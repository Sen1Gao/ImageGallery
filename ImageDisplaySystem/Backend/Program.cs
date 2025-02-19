using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.MsDependencyInjection;
using Microsoft.AspNetCore.Mvc;

namespace Backend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //Bridge local DI to Castle Windsor
            builder.Host.UseServiceProviderFactory(new WindsorServiceProviderFactory());

            // Prevent controller to be registered by local DI
            builder.Services.AddControllers().AddControllersAsServices();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.UseAuthorization();
            app.MapControllers();

            var container = app.Services.GetService<IWindsorContainer>();
            var lifetime = app.Services.GetRequiredService<IHostApplicationLifetime>();
            lifetime.ApplicationStopping.Register(() => container?.Dispose());
            app.Run();
        }
    }
}
