namespace Backend.interfaces
{
    public interface IDbManagement
    {
        Task<bool> QueryUserAsync(string username,string password);
        Task<bool> RegisterUserAsync(string username, string password);

    }
}
