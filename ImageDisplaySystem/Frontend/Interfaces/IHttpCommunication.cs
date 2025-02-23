using BasicArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Frontend.Interfaces
{
    public interface IHttpCommunication
    {
        Task<bool> VerifyAsync(SigninInfoArgs args);
        Task<bool> RegisterAsync(SigninInfoArgs args);
        Task<bool> UploadImageAsync(string filePath,  string description, string tag);
        Task<List<ImageCardInfo>> GetImageCardInfos(int page, int pageSize);
        Task<BitmapImage> GetImage(string imageName);
    }
}
