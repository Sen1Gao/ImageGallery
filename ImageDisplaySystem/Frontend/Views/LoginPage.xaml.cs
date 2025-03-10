﻿using Castle.Windsor;
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
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        private readonly IWindsorContainer container;
        private readonly SigninViewModel viewModel;

        public LoginPage(IContainerHelper containerHelper)
        {
            InitializeComponent();

            container = containerHelper.Container;
            viewModel = container.Resolve<SigninViewModel>();
            DataContext = viewModel;

            PasswordBox.LostFocus += PasswordBox_LostFocus;
        }

        private void PasswordBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(PasswordBox.Password))
            {
                viewModel.Password = PasswordBox.Password;
            }
        }
    }
}
