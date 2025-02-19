using Castle.Windsor;
using Frontend.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Frontend.Services
{
    public class NavigationService : INavigationService
    {
        private Frame? frame;
        private bool isSet;

        public NavigationService()
        {
            isSet = false;
        }

        public Frame Frame
        {
            set
            {
                if (isSet == false)
                {
                    frame = value;
                    isSet = true;
                }
                else
                {
                    throw new InvalidOperationException("The frame only can be assigned once because there is only one frame in main window");
                }
            }
        }

        public void NavigateTo(object content)
        {
            if (frame != null && isSet == true)
            {
                while (frame.CanGoBack)
                {
                    frame.RemoveBackEntry();
                }
                frame.Navigate(content);
            }
        }
    }
}
