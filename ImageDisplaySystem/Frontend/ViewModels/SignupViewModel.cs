using BasicArgs;
using Castle.Windsor;
using Frontend.Interfaces;
using Frontend.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Frontend.ViewModels
{
    public class SignupViewModel : INotifyPropertyChanged
    {
        private readonly INavigationService navigationService;
        private readonly IWindsorContainer container;
        private readonly IHttpCommunication httpCommunication;
        private readonly IStatementManager statementManager;

        private readonly ICommand signupCommand;
        private readonly ICommand cancelCommand;
        private string username = "";
        private string password = "";
        private string samePassword = "";
        private string message = "";
        private bool isSignupButtonEnable=true;
        private bool isCancelButtonEnable = true;
        public SignupViewModel(INavigationService navigationService, IContainerHelper container,
            IHttpCommunication httpCommunication, IStatementManager statementManager)
        {
            this.navigationService = navigationService;
            this.container = container.Container;
            this.httpCommunication = httpCommunication;
            this.statementManager = statementManager;

            signupCommand =new RelayCommand(ExecuteSignup);
            cancelCommand = new RelayCommand(ExecuteCanceling);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public ICommand CancelCommand => cancelCommand;
        public ICommand SignupCommand => signupCommand;

        public string Username { set => username = value; }
        public string Password { set => password = value; }
        public string SamePassword { set => samePassword = value; }
        public string Message
        {
            get => message;
            set
            {
                message = value;
                OnPropertyChanged(nameof(Message));
            }
        }
        public bool IsSignupButtonEnable
        {
            get => isSignupButtonEnable;
            set
            {
                isSignupButtonEnable = value;
                OnPropertyChanged(nameof(IsSignupButtonEnable));
            }
        }
        public bool IsCancelButtonEnable
        {
            get => isCancelButtonEnable;
            set
            {
                isCancelButtonEnable = value;
                OnPropertyChanged(nameof(IsCancelButtonEnable));
            }
        }
        private void ExecuteSignup(object parameter)
        {
            Message = "";
            if (string.IsNullOrEmpty(username)|| string.IsNullOrEmpty(password) || string.IsNullOrEmpty(samePassword))
            {
                Message = "All Information must be typed to text boxes!";
                return;
            }
            if(password!= samePassword)
            {
                Message = "You gave two different passwords!";
                return;
            }
            IsSignupButtonEnable=false;
            IsCancelButtonEnable=false;
            Task.Run(async () =>
            {
                var args = new SigninInfoArgs() { SigninType = SigninType.Admin, Username = username, Password = password };
                var result = await httpCommunication.RegisterAsync(args);
                if (result==true) 
                {
                    statementManager.SigninType = args.SigninType;
                    App.Current.Dispatcher.Invoke(() =>
                    {
                        navigationService.NavigateTo(container.Resolve<ImageBrowsePage>());
                    });
                }
                else
                {
                    Message = "Register failed! Try again.";
                    IsSignupButtonEnable = true;
                    IsCancelButtonEnable = true;
                }
            });
        }
        private void ExecuteCanceling(object parameter)
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                navigationService.NavigateTo(container.Resolve<LoginPage>());
            });
            
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
