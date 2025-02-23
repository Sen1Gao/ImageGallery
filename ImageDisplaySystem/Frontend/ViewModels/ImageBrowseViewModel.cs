using BasicArgs;
using Castle.Windsor;
using Frontend.Arguments;
using Frontend.Interfaces;
using Frontend.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

    public class ImageBrowseViewModel : INotifyPropertyChanged
    {
        private readonly IStatementManager statementManager;
        private readonly INavigationService navigationService;
        private readonly IWindsorContainer container;
        private readonly IHttpCommunication httpCommunication;

        private readonly ICommand addImageCommand;
        private readonly ICommand goBackCommand;
        private readonly ICommand goNextCommand;
        private readonly ICommand checkDetailCommand;

        private Visibility addingImageButtonVisibility;
        private bool isButtonEnable=true;


        public ImageBrowseViewModel(IStatementManager statementManager, INavigationService navigationService,
            IContainerHelper containerHelper, IHttpCommunication httpCommunication)
        {
            this.statementManager = statementManager;
            this.navigationService = navigationService;
            this.container = containerHelper.Container;
            this.httpCommunication = httpCommunication;

            addImageCommand = new RelayCommand(ExecuteAddImage);
            goBackCommand = new RelayCommand(ExecuteGoBack);
            goNextCommand = new RelayCommand(ExecuteGoNext);
            checkDetailCommand = new RelayCommand(ExecuteCheckDetail);
            addingImageButtonVisibility = statementManager.SigninType == SigninType.Admin ? Visibility.Visible : Visibility.Collapsed;
            ImageCardList = new ObservableCollection<ImageCard>();
            UpdateImageCardList();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public ICommand AddImageCommand => addImageCommand;
        public ICommand GoBackCommand => goBackCommand;
        public ICommand GoNextCommand => goNextCommand;
        public ICommand CheckDetailCommand => checkDetailCommand;
        public Visibility AddingImageButtonVisibility
        {
            get => addingImageButtonVisibility;
            set
            {
                addingImageButtonVisibility = value;
                RaisePropertyChanged(nameof(AddingImageButtonVisibility));
            }
        }
        public ObservableCollection<ImageCard> ImageCardList { get; set; }

        public bool IsButtonEnable
        {
            get => isButtonEnable;
            set
            {
                isButtonEnable = value;
                RaisePropertyChanged(nameof(IsButtonEnable));
            }
        }

        private void ExecuteAddImage(object? parameter)
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                navigationService.NavigateTo(container.Resolve<ImageUploadingPage>());
            });
        }

        private void ExecuteGoBack(object? parameter)
        {
            UpdateImageCardList(-1);
        }

        private void ExecuteGoNext(object? parameter)
        {
            UpdateImageCardList(1);
        }

        private void ExecuteCheckDetail(object? parameter)
        {
            var imageCard = parameter as ImageCard;
            if (imageCard != null)
            {
                statementManager.CurrentImageCard = imageCard;
                App.Current.Dispatcher.Invoke(() =>
                {
                    navigationService.NavigateTo(container.Resolve<ImageDetailsPage>());
                });
            }
        }

        private void UpdateImageCardList(int direction=0)
        {
            IsButtonEnable=false;
            Task.Run(async () => {
                if (direction > 0)
                {
                    statementManager.Page += 1;
                }
                if (direction < 0)
                {
                    statementManager.Page -= 1;
                }
                var result = await httpCommunication.GetImageCardInfosAsync(statementManager.Page, statementManager.PageSize);
                if (result != null && result.Count > 0)
                {
                    App.Current.Dispatcher.Invoke(() => {
                        ImageCardList.Clear();
                    });
                    foreach (var item in result)
                    {
                        var image = await httpCommunication.GetImageAsync(item.ImageURL);
                        if (image == null)
                        {
                            continue;
                        }
                        App.Current.Dispatcher.Invoke(() => {
                            ImageCardList.Add(new ImageCard()
                            {
                                ImageId = item.ImageId,
                                Image = image,
                                Tag = item.Tag,
                                Description = item.Description
                            });
                        });
                    }
                }
                else
                {
                    if (direction > 0) 
                    {
                        statementManager.Page -= 1;
                        App.Current.Dispatcher.Invoke(() => {
                            MessageBox.Show("You have been arrived at the last page!");
                        });
                    }
                    if (direction < 0)
                    {
                        statementManager.Page += 1;
                        App.Current.Dispatcher.Invoke(() => {
                            MessageBox.Show("You have been arrived at the first page!");
                        });
                    }
                }
                IsButtonEnable=true;
            });
        }

        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
