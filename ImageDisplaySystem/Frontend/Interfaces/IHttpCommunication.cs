using BasicArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frontend.Interfaces
{
    public interface IHttpCommunication
    {
        Task<bool> VerifyAsync(SigninInfoArgs args);
        Task<bool> RegisterAsync(SigninInfoArgs args);
        Task<bool> UploadImageAsync(string filePath,  string description, string tag);
    }
}
