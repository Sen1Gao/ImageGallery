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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Frontend.Views
{
    /// <summary>
    /// Interaction logic for ImageDetailsPage.xaml
    /// </summary>
    public partial class ImageDetailsPage : Page
    {
        private readonly IWindsorContainer container;
        private readonly ImageDetailsViewModel viewModel;
        public ImageDetailsPage(IContainerHelper containerHelper)
        {
            InitializeComponent();

            container = containerHelper.Container;
            viewModel = container.Resolve<ImageDetailsViewModel>();
            DataContext = viewModel;
        }
    }
}
