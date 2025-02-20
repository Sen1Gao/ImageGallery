using BasicArgs;
using Castle.Windsor;
using Frontend.Interfaces;
using Frontend.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Xml.Linq;

namespace Frontend.ViewModels
{

    public class ImageBrowseViewModel : INotifyPropertyChanged
    {
        private readonly IStatementManager statementManager;
        private readonly INavigationService navigationService;
        private readonly IWindsorContainer container;

        private readonly ICommand addImageCommand;

        private Visibility addingImageButtonVisibility;

        
        public ImageBrowseViewModel(IStatementManager statementManager, INavigationService navigationService, 
            IContainerHelper containerHelper)
        {
            this.statementManager = statementManager;
            this.navigationService = navigationService;
            this.container = containerHelper.Container;

            addImageCommand = new RelayCommand(ExecuteAddImage);
            addingImageButtonVisibility = statementManager.SigninType == SigninType.Admin ? Visibility.Visible : Visibility.Collapsed;

        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public ICommand AddImageCommand => addImageCommand;
        public Visibility AddingImageButtonVisibility
        {
            get => addingImageButtonVisibility;
            set
            {
                addingImageButtonVisibility= value;
                RaisePropertyChanged(nameof(AddingImageButtonVisibility));
            }
        }

        private void ExecuteAddImage(object? parameter)
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                navigationService.NavigateTo(container.Resolve<ImageUploadingPage>(),true);
            });
        }

        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
