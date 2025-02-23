using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Frontend.Arguments
{
    public class ImageCard
    {
        public int ImageId { get; set; }
        public string ImageName { get; set; } = "";
        public BitmapImage Image { get; set; } = new BitmapImage();
        public string Description { get; set; } = "";
        public string Tag { get; set; } = "";
    }
}
