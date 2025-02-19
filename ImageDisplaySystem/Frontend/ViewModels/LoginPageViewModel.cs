using BasicArgs;
using Castle.Windsor;
using Frontend.Interfaces;
using Frontend.Services;
using Frontend.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Frontend.ViewModels
{
    public class LoginPageViewModel : INotifyPropertyChanged
    {
        private readonly INavigationService navigationService;
        private readonly IWindsorContainer container;
        private readonly IHttpCommunication httpCommunication;
        private readonly IStatementManager statementManager;

        private readonly ICommand signinCommand;
        private readonly ICommand accessCommand;
        private readonly ICommand createAccountCommand;
        private string username = "";
        private string password = "";
        private string message = "";
        private bool isButtonEnable = true;
        public LoginPageViewModel(INavigationService navigationService, IContainerHelper container,
            IHttpCommunication httpCommunication, IStatementManager statementManager)
        {
            this.navigationService = navigationService;
            this.container = container.Container;
            this.httpCommunication = httpCommunication;
            this.statementManager = statementManager;

            signinCommand = new RelayCommand(ExecuteSignin);
            accessCommand = new RelayCommand(ExecuteAccess);
            createAccountCommand = new RelayCommand(ExecuteCreatingAccount);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public ICommand SigninCommand => signinCommand;
        public ICommand CreateAccountCommand => createAccountCommand;
        public ICommand AccessCommand => accessCommand;

        public string Username { set => username = value; }
        public string Password { set => password = value; }
        public string Message
        {
            get => message;
            set
            {
                message = value;
                RaisePropertyChanged(nameof(Message));
            }
        }
        public bool IsButtonEnable
        {
            get => isButtonEnable;
            set
            {
                isButtonEnable = value;
                RaisePropertyChanged(nameof(IsButtonEnable));
            }
        }

        private void ExecuteSignin(object? parameter)
        {
            Message = "";
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                Message = "Username or Password is empty!";
                return;
            }
            IsButtonEnable = false;
            Task.Run(async () =>
            {
                var args = new SigninInfoArgs() { SigninType = SigninType.Admin, Username = username, Password = password };
                var result = await httpCommunication.VerifyAsync(args);
                if (result == true)
                {
                    statementManager.SigninType = args.SigninType;
                    App.Current.Dispatcher.Invoke(() =>
                    {
                        navigationService.NavigateTo(container.Resolve<ImageBrowsePage>());
                    });
                }
                else
                {
                    Message = "Sign in failed! Try again.";
                    IsButtonEnable = true;
                }
            });
        }
        private void ExecuteAccess(object? parameter)
        {
            Message = "";
            IsButtonEnable = false;
            Task.Run(async () =>
            {
                var args = new SigninInfoArgs() { SigninType = SigninType.Guest };
                var result = await httpCommunication.VerifyAsync(args);
                if (result == true)
                {
                    statementManager.SigninType = args.SigninType;
                    App.Current.Dispatcher.Invoke(() =>
                    {
                        navigationService.NavigateTo(container.Resolve<ImageBrowsePage>());
                    });
                }
                else
                {
                    Message = "Access failed! Try again.";
                    IsButtonEnable = true;
                }
            });
        }
        private void ExecuteCreatingAccount(object? parameter)
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                navigationService.NavigateTo(container.Resolve<SignupPage>());
            });
            
        }

        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
