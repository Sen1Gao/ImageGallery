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
    }
}
