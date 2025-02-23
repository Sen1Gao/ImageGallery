using BasicArgs;
using Castle.Windsor;
using Frontend.Arguments;
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
using System.Windows.Media.Imaging;
using System.Xml.Linq;

namespace Frontend.ViewModels
{
    public class ImageDetailsViewModel : INotifyPropertyChanged
    {
        private readonly IStatementManager statementManager;
        private readonly INavigationService navigationService;
        private readonly IWindsorContainer container;
        private readonly IHttpCommunication httpCommunication;

        private readonly ICommand goBackCommand;
        private readonly ICommand saveCommand;
        private readonly ICommand deleteCommand;

        private ImageCard imageCard;
        private bool canEdit = false;
        private BitmapImage image = new BitmapImage();
        private string tag = "";
        private string description = "";
        private string newReview = "";
        private string newRating = "";
        
        public ImageDetailsViewModel(IStatementManager statementManager, INavigationService navigationService,
            IContainerHelper containerHelper, IHttpCommunication httpCommunication)
        {
            this.statementManager = statementManager;
            this.navigationService = navigationService;
            container = containerHelper.Container;
            this.httpCommunication = httpCommunication;

            goBackCommand = new RelayCommand(ExecuteGoBack);
            saveCommand = new RelayCommand(ExecuteSave);
            deleteCommand = new RelayCommand(ExecuteDelete);

            imageCard = this.statementManager.CurrentImageCard;
            canEdit = statementManager.SigninType == SigninType.Admin ? true : false;
            image = imageCard.Image;
            tag = imageCard.Tag;
            description = imageCard.Description;
            DeleteImageButtonVisibility=statementManager.SigninType==SigninType.Admin ? Visibility.Visible : Visibility.Collapsed;
            ReviewInfoList = new ObservableCollection<ReviewInfo>();
            UpdateReview();
        }



        public event PropertyChangedEventHandler? PropertyChanged;

        public ICommand GoBackCommand => goBackCommand;
        public ICommand SaveCommand => saveCommand;
        public ICommand DeleteCommand => deleteCommand;

        public Visibility DeleteImageButtonVisibility { get; set; }

        public ObservableCollection<ReviewInfo> ReviewInfoList { get; set; }

        public bool CanEdit
        {
            get => canEdit;
            set
            {
                canEdit = value;
                RaisePropertyChanged(nameof(CanEdit));
            }
        }
        public BitmapImage Image => image;
        public string Tag
        {
            get => tag;
            set
            {
                App.Current.Dispatcher.Invoke(() =>
                {
                    if (MessageBoxResult.Yes == MessageBox.Show("Confirm to modify?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question))
                    {
                        tag = value;
                        Task.Run(async () =>
                        {
                            var result=await httpCommunication.UpdateImageInfoAsync(new ImageCardInfo() { 
                                ImageId=imageCard.ImageId,
                                Tag= value,
                                Description= Description
                            });
                            App.Current.Dispatcher.Invoke(() =>
                            {
                                if (result == true)
                                {
                                    MessageBox.Show("The tag has been modified.");
                                }
                                else
                                {
                                    MessageBox.Show("Failed to modify the tag.");
                                }
                            });
                        });
                    }
                });
                
                RaisePropertyChanged(nameof(Tag));
            }
        }
        public string Description
        {
            get => description;
            set
            {
                App.Current.Dispatcher.Invoke(() =>
                {
                    if (MessageBoxResult.Yes == MessageBox.Show("Confirm to modify?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question))
                    {
                        description = value;
                        Task.Run(async () =>
                        {
                            var result = await httpCommunication.UpdateImageInfoAsync(new ImageCardInfo()
                            {
                                ImageId = imageCard.ImageId,
                                Tag = Tag,
                                Description = value
                            });
                            App.Current.Dispatcher.Invoke(() =>
                            {
                                if (result == true)
                                {
                                    MessageBox.Show("The description has been modified.");
                                }
                                else
                                {
                                    MessageBox.Show("Failed to modify the description.");
                                }
                            });
                        });
                    }
                });
                
                RaisePropertyChanged(nameof(Description));
            }
        }

        public string NewReview
        {
            get => newReview;
            set
            {
                newReview = value;
                RaisePropertyChanged(nameof(NewReview));
            }
        }
        public string NewRating
        {
            get => newRating;
            set
            {
                newRating = value;
                RaisePropertyChanged(nameof(NewRating));
            }
        }


        private void ExecuteGoBack(object? parameter)
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                navigationService.NavigateTo(container.Resolve<ImageBrowsePage>());
            });
        }

        private void ExecuteSave(object? parameter)
        {
            Task.Run(async () =>
            {
                var result = await httpCommunication.UploadReviewAsync(new ReviewInfo()
                {
                    ImageID = imageCard.ImageId,
                    Rating = int.Parse(NewRating),
                    Review = NewReview
                });
                App.Current.Dispatcher.Invoke(() =>
                {
                    if (result == true)
                    {
                        MessageBox.Show("Your review has been saved.");
                        NewReview = "";
                        NewRating = "";
                        UpdateReview();
                    }
                    else
                    {
                        MessageBox.Show("Failed to save your review.");
                    }
                });
            });
        }

        private void ExecuteDelete(object? parameter)
        {
            Task.Run(async () =>
            {
                var result=await httpCommunication.DeleteImageAsync(imageCard.ImageId);
                App.Current.Dispatcher.Invoke(() =>
                {
                    if (result == true)
                    {
                        MessageBox.Show("This image has been deleted.");
                        navigationService.NavigateTo(container.Resolve<ImageBrowsePage>());
                    }
                    else
                    {
                        MessageBox.Show("Failed to delete.");
                    }
                });
            });
        }

        private void UpdateReview()
        {
            Task.Run(async () =>
            {
                var result = await httpCommunication.GetReviewInfosAsync(imageCard.ImageId);
                if (result != null && result.Count > 0)
                {
                    App.Current.Dispatcher.Invoke(() =>
                    {
                        ReviewInfoList.Clear();
                    });
                    foreach (var item in result)
                    {
                        App.Current.Dispatcher.Invoke(() =>
                        {
                            ReviewInfoList.Add(new ReviewInfo()
                            {
                                ReviewID = item.ReviewID,
                                ImageID = item.ImageID,
                                Rating = item.Rating,
                                Review = item.Review
                            });
                        });
                    }
                }
            });
        }


        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
