using Castle.Windsor;
using Frontend.Interfaces;
using Frontend.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Frontend.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainWindowViewModel viewModel;
        private readonly IWindsorContainer container;
        private readonly INavigationService navigationService;
        public MainWindow(IContainerHelper containerHelper, INavigationService navigationService)
        {
            InitializeComponent();

            container = containerHelper.Container;
            viewModel=container.Resolve<MainWindowViewModel>();
            DataContext= viewModel;
            this.navigationService = navigationService;
            this.navigationService.Frame = MainFrame;

            this.Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.navigationService.NavigateTo(container.Resolve<LoginPage>());
        }
    }
}
