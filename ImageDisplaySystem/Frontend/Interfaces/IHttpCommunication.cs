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
        Task<bool> UploadImageAsync(string filePath, string description, string tag);
        Task<List<ImageCardInfo>> GetImageCardInfosAsync(int page, int pageSize);
        Task<BitmapImage> GetImageAsync(string imageName);
        Task<List<ReviewInfo>> GetReviewInfosAsync(int imageID);
        Task<bool> UploadReviewAsync(ReviewInfo reviewInfo);
        Task<bool> DeleteImageAsync(int imageID,string imageName);
        Task<bool> UpdateImageInfoAsync(ImageCardInfo imageCardInfo);
    }
}
