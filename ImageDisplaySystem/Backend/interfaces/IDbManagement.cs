using BasicArgs;

namespace Backend.interfaces
{
    public interface IDbManagement
    {
        Task<bool> QueryUserAsync(string username, string password);
        Task<bool> RegisterUserAsync(string username, string password);
        Task<bool> UploadImageAsync(string fileName, string description, string tag);
        Task<int> GetTotalNumberOfImagesAsync();
        Task<List<ImageCardInfo>> GetPagedImagesAsync(int offset, int limit);
        Task<bool> UploadReviewAsync(int imageID, string review, int rating);
        Task<List<ReviewInfo>> GetReviewInfoAsync(int imageID);
        Task<bool> DeleteImageAsync(int imageID);
        Task<bool> UpdataImageInfoAsync(int imageID,string tag,string description);
        Task<List<ImageCardInfo>> GetImagesBySearchingAsync(string tag);
    }
}
