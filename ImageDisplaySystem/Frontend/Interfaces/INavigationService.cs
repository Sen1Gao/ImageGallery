using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Frontend.Interfaces
{
    public interface INavigationService
    {
        Frame Frame { set; }

        void NavigateTo(object content,bool keepHistory=false);

        void GoBack();
    }
}
