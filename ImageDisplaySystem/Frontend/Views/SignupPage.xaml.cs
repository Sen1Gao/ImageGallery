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
    /// Interaction logic for SignupPage.xaml
    /// </summary>
    public partial class SignupPage : Page
    {
        private readonly IWindsorContainer container;
        private readonly SignupViewModel signupPageViewModel;
        public SignupPage(IContainerHelper containerHelper)
        {
            InitializeComponent();

            container = containerHelper.Container;
            signupPageViewModel = container.Resolve<SignupViewModel>();
            DataContext = signupPageViewModel;

            PasswordBox.LostFocus += PasswordBox_LostFocus;
            SamePasswordBox.LostFocus += SamePasswordBox_LostFocus;
        }

        private void SamePasswordBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(SamePasswordBox.Password))
            {
                signupPageViewModel.SamePassword = SamePasswordBox.Password;
            }
        }

        private void PasswordBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(PasswordBox.Password))
            {
                signupPageViewModel.Password = PasswordBox.Password;
            }
        }
    }
}
