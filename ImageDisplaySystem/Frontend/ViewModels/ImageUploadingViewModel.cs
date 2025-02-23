using Castle.Windsor;
using Frontend.Interfaces;
using Frontend.Services;
using Frontend.Views;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Xml.Linq;

namespace Frontend.ViewModels
{
    public class ImageUploadingViewModel : INotifyPropertyChanged
    {
        private readonly INavigationService navigationService;
        private readonly IHttpCommunication httpCommunication;
        private readonly IWindsorContainer container;

        private readonly ICommand selectImageCommand;
        private readonly ICommand uploadCommand;
        private readonly ICommand cancelCommand;

        private BitmapImage image;
        private string tag;
        private string description;
        private string selectedFilePath;
        private bool isAllEnable;
        public ImageUploadingViewModel(INavigationService navigationService, IHttpCommunication httpCommunication,
            IContainerHelper containerHelper)
        {
            this.navigationService = navigationService;
            this.httpCommunication = httpCommunication;
            container = containerHelper.Container;

            selectImageCommand = new RelayCommand(ExecuteSelectImage);
            uploadCommand = new RelayCommand(ExecuteUpload);
            cancelCommand = new RelayCommand(ExecuteCancel);
            image = new BitmapImage();
            tag = "";
            description = "";
            selectedFilePath = "";
            isAllEnable = true;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public ICommand SelectImageCommand => selectImageCommand;
        public ICommand UploadCommand => uploadCommand;
        public ICommand CancelCommand => cancelCommand;
        public BitmapImage ImageSource
        {
            get => image;
            set
            {
                image = value;
                RaisePropertyChanged(nameof(ImageSource));
            }
        }
        public string Tag { set => tag = value; }
        public string Description { set => description = value; }

        public bool IsAllEnable
        {
            get => isAllEnable;
            set
            {
                isAllEnable = value;
                RaisePropertyChanged(nameof(IsAllEnable));
            }
        }


        private void ExecuteSelectImage(object? parameter)
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                var openFileDialog = new OpenFileDialog()
                {
                    Title = "Image Selecting",
                    Filter = "Image File (*.jpg)|*.jpg",
                    InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                    Multiselect = false
                };
                if (openFileDialog.ShowDialog() == true)
                {
                    selectedFilePath = openFileDialog.FileName;
                    ImageSource = new BitmapImage(new Uri(selectedFilePath, UriKind.Absolute));
                }
            });
        }

        private void ExecuteUpload(object? parameter)
        {
            if (string.IsNullOrEmpty(selectedFilePath) || string.IsNullOrEmpty(tag) || string.IsNullOrEmpty(description))
            {
                App.Current.Dispatcher.Invoke(() =>
                {
                    MessageBox.Show("Please fill all information before uploading!");
                });
                return;
            }
            IsAllEnable = false;
            Task.Run(async () =>
            {
                var result = await httpCommunication.UploadImageAsync(selectedFilePath, description, tag);
                App.Current.Dispatcher.Invoke(() =>
                {
                    if (result == true)
                    {
                        MessageBox.Show("Uploading completed.");
                        navigationService.NavigateTo(container.Resolve<ImageBrowsePage>());
                    }
                    else
                    {
                        MessageBox.Show("Unknown error! Try again!");
                        IsAllEnable = true;
                    }
                });
            });
        }

        private void ExecuteCancel(object? parameter)
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                navigationService.NavigateTo(container.Resolve<ImageBrowsePage>());
            });
        }

        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
