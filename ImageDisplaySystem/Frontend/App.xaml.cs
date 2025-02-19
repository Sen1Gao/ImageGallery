using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Frontend.Interfaces;
using Frontend.Services;
using Frontend.ViewModels;
using Frontend.Views;
using System.Configuration;
using System.Data;
using System.Windows;

namespace Frontend
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IWindsorContainer container =new WindsorContainer();

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            RegisterDependencies();

            //Window entry
            var mainWindow = container.Resolve<MainWindow>();
            mainWindow.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            container.Dispose();
            base.OnExit(e);
        }

        /// <summary>
        /// Build dependency tree
        /// </summary>
        private void RegisterDependencies()
        {
            //Register container helper
            container.Register(Component.For<IContainerHelper>().ImplementedBy<ContainerHelper>().LifestyleSingleton());
            //For getting handler of container.
            var helper = container.Resolve<IContainerHelper>();
            helper.Container = container;

            //Register services
            container.Register(
                Component.For<INavigationService>().ImplementedBy<NavigationService>().LifestyleSingleton(),
                Component.For<IHttpCommunication>().ImplementedBy<HttpCommunication>().LifestyleSingleton(),
                Component.For<IStatementManager>().ImplementedBy<StatementManager>().LifestyleSingleton());
            
            //Register view models
            container.Register(
                Component.For<LoginPageViewModel>().LifestyleTransient(),
                Component.For<SignupPageViewModel>().LifestyleTransient(),
                Component.For<ImageBrowseViewModel>().LifestyleTransient()); 

            //Register pages
            container.Register(
                Component.For<LoginPage>().LifestyleTransient(),
                Component.For<SignupPage>().LifestyleTransient(),
                Component.For<ImageBrowsePage>().LifestyleTransient());

            //Register main window
            container.Register(
                Component.For<MainWindowViewModel>().LifestyleSingleton(),
                Component.For<MainWindow>().LifestyleSingleton());
        }
    }

}
